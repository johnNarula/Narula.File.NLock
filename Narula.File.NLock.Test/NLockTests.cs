using System;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using OtpNet;
using QRCoder;
using Xunit;
using Narula.File.NLock;
using Narula.File.NLock.Services;
using Narula.File.NLock.Utilities;
using Narula.File.NLock.Models;

namespace Narula.File.NLock.Test
{
    public class NLockTests : IDisposable
    {
        private readonly string _tempDir;

        public NLockTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "NarulaFileNLockTests", Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            try
            {
                if (System.IO.Directory.Exists(_tempDir))
                    System.IO.Directory.Delete(_tempDir, true);
            }
            catch
            {
                // best-effort cleanup
            }
        }

        [Fact]
        public void CryptoService_GenerateSalt_ReturnsRequestedSize()
        {
            var salt = CryptoService.GenerateSalt(16);
            Assert.NotNull(salt);
            Assert.Equal(16, salt.Length);
        }

        [Fact]
        public void CryptoService_Hash_Then_Verify_Hmac_And_EncryptDecrypt_Works()
        {
            var password = "TestPassword!";
            var salt = CryptoService.GenerateSalt(16);
            var passwordHash = CryptoService.HashPassword(password, salt);
            Assert.Equal(AppConstants.PasswordHashSize, passwordHash.Length);

            var key = CryptoService.DeriveKey(password, salt);
            Assert.Equal(32, key.Length);

            var plain = Encoding.UTF8.GetBytes("hello world");
            var (cipher, iv) = CryptoService.EncryptWithIv(plain, key);
            Assert.NotNull(cipher);
            Assert.NotNull(iv);

            var decrypted = CryptoService.DecryptWithIv(cipher, key, iv);
            Assert.Equal(plain, decrypted);

            var hmac = CryptoService.ComputeHmac(plain, key);
            Assert.True(CryptoService.VerifyHmac(plain, key, hmac));
        }

        [Fact]
        public void TOTPService_GenerateAnd_QR_Create()
        {
            var secret = TOTPService.GenerateTotpSecretBase32();
            Assert.False(string.IsNullOrWhiteSpace(secret));

            var secretBytes = Base32Encoding.ToBytes(secret);
            var totp = new Totp(secretBytes, step: 30);
            var code = totp.ComputeTotp();
            Assert.True(TOTPService.ValidateAuthCode(secret, code));

            var uri = TOTPService.CreateTotpUri(secret, "UnitTestIssuer", "user@example.com");
            Assert.Contains("otpauth://", uri);

            var pngBytes = TOTPService.CreatePngQrCodeBytes(uri, 5);
            Assert.NotNull(pngBytes);
            Assert.True(pngBytes.Length > 0);

            var dataUri = TOTPService.CreateQrCodePngBase64Uri(uri, 5);
            Assert.StartsWith("data:image/png;base64,", dataUri);
        }

        [Fact]
        public void SecureUtils_ClearBytes_And_StringConversions()
        {
            var arr = Encoding.UTF8.GetBytes("secret");
            SecureUtils.ClearBytes(arr);
            Assert.True(arr.All(b => b == 0));

            var s = "mypassword";
            var secure = SecureUtils.StringToSecureString(s);
            var back = SecureUtils.SecureStringToString(secure);
            Assert.Equal(s, back);

            string test = "clearme";
            SecureUtils.SecureClearString(ref test);
            Assert.Null(test);
        }

        [Fact]
        public void SecureUtils_SanitizeExceptionMessage_MapsCommonExceptions()
        {
            Assert.Equal("Access denied. Please check file permissions.",
                new UnauthorizedAccessException().SanitizeExceptionMessage());
            Assert.Equal("File not found.",
                new FileNotFoundException().SanitizeExceptionMessage());
            Assert.Equal("Directory not found.",
                new DirectoryNotFoundException().SanitizeExceptionMessage());
            Assert.Equal("File operation failed. Please check if the file is in use.",
                new IOException().SanitizeExceptionMessage());
            Assert.Equal("Required parameter is missing.",
                new ArgumentNullException().SanitizeExceptionMessage());
            Assert.Equal("Invalid input provided.",
                new ArgumentException().SanitizeExceptionMessage());
            Assert.Equal("Operation not supported.",
                new NotSupportedException().SanitizeExceptionMessage());
            Assert.Equal("Security violation detected.",
                new System.Security.SecurityException().SanitizeExceptionMessage());

            var unknown = new Exception("boom");
            Assert.Equal("An unexpected error occurred", unknown.SanitizeExceptionMessage());
        }

        [Fact]
        public void ObfuscationUtils_ObfuscatedConstantsAndCompare()
        {
            Assert.Equal(600_000, ObfuscationUtils.GetPbkdf2Iterations());
            Assert.Equal(16, ObfuscationUtils.GetSaltSize());
            Assert.Equal(32, ObfuscationUtils.GetHashSize());

            var a = new byte[] { 1, 2, 3 };
            var b = new byte[] { 1, 2, 3 };
            var c = new byte[] { 1, 2, 4 };
            Assert.True(ObfuscationUtils.SecureCompare(a, b));
            Assert.False(ObfuscationUtils.SecureCompare(a, c));
        }

        [Fact]
        public void ObfuscationUtils_DeriveKeyObfuscated_IsDeterministic()
        {
            var salt = CryptoService.GenerateSalt(16);
            var key1 = ObfuscationUtils.DeriveKeyObfuscated("pass", salt);
            var key2 = ObfuscationUtils.DeriveKeyObfuscated("pass", salt);
            Assert.Equal(key1, key2);
            Assert.Equal(32, key1.Length);
        }

        [Fact]
        public void FileUtility_DirectoryHelpers_And_SaveFile_ReadFileBytes()
        {
            var destFile = Path.Combine(_tempDir, "subdir", "file.bin");
            var data = Encoding.UTF8.GetBytes("payload");

            var saved = data.SaveFile(destFile);
            Assert.True(saved);
            Assert.True(System.IO.File.Exists(destFile));
            Assert.Equal(data, System.IO.File.ReadAllBytes(destFile));

            var dir = Path.Combine(_tempDir, "another");
            Assert.True(FileUtility.EnsureDirectoryExists(dir));
            Assert.True(System.IO.Directory.Exists(dir));

            var pathWithDir = Path.Combine(_tempDir, "x", "y", "z.txt");
            Assert.True(FileUtility.EnsureDirectoryOfFileExists(pathWithDir));
            Assert.True(System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(pathWithDir)));

            var initial = "name.txt";
            var p1 = Path.Combine(_tempDir, initial);
            System.IO.File.WriteAllText(p1, "one");
            var unique = FileUtility.GetUniqueTargetFilenameInDirectory(_tempDir, initial);
            Assert.NotEqual(initial, unique);
            Assert.StartsWith("name(", unique);

            var smallFile = Path.Combine(_tempDir, "small.txt");
            System.IO.File.WriteAllText(smallFile, "small");
            Assert.True(FileUtility.ValidateFileSize(smallFile, out long size));
            Assert.True(size > 0);
            var read = FileUtility.ReadFileBytesSafely(smallFile);
            Assert.NotNull(read);
            Assert.Equal(System.IO.File.ReadAllBytes(smallFile), read);
        }

        [Fact]
        public void FileUtility_WriteAndReadEncryptedFile_WithHmac()
        {
            var outFile = Path.Combine(_tempDir, "encfile.nlock");

            var salt = CryptoService.GenerateSalt(16);
            var passwordHash = CryptoService.HashPassword("pw", salt);
            var iv1 = CryptoService.GenerateSalt(16);
            var iv2 = CryptoService.GenerateSalt(16);
            var encryptedFile = Encoding.UTF8.GetBytes("filebytes");
            var encryptedTotp = Encoding.UTF8.GetBytes("totpbytes");
            var masterKey = CryptoService.DeriveKey("pw", salt);
            var hmacData = new byte[salt.Length + passwordHash.Length + iv1.Length + encryptedFile.Length + iv2.Length + encryptedTotp.Length];
            var offset = 0;
            Buffer.BlockCopy(salt, 0, hmacData, offset, salt.Length); offset += salt.Length;
            Buffer.BlockCopy(passwordHash, 0, hmacData, offset, passwordHash.Length); offset += passwordHash.Length;
            Buffer.BlockCopy(iv1, 0, hmacData, offset, iv1.Length); offset += iv1.Length;
            Buffer.BlockCopy(encryptedFile, 0, hmacData, offset, encryptedFile.Length); offset += encryptedFile.Length;
            Buffer.BlockCopy(iv2, 0, hmacData, offset, iv2.Length); offset += iv2.Length;
            Buffer.BlockCopy(encryptedTotp, 0, hmacData, offset, encryptedTotp.Length);
            var hmac = CryptoService.ComputeHmac(hmacData, masterKey);

            FileUtility.WriteEncryptedFile(outFile, salt, passwordHash, encryptedFile, encryptedTotp, iv1, iv2, hmac);

            var ok = FileUtility.ReadEncryptedFile(outFile, out var rsalt, out var rpasswordHash, out var rencryptedFile, out var rencryptedTotp, out var riv1, out var riv2, out var rhmac);
            Assert.True(ok);
            Assert.Equal(salt, rsalt);
            Assert.Equal(passwordHash, rpasswordHash);
            Assert.Equal(encryptedFile, rencryptedFile);
            Assert.Equal(encryptedTotp, rencryptedTotp);
            Assert.Equal(iv1, riv1);
            Assert.Equal(iv2, riv2);
            Assert.Equal(hmac, rhmac);
        }

        [Fact]
        public void NLockInfo_Password_SetGet_Dispose()
        {
            var info = new NLockInfo();
            info.SetPassword("secret123");
            var pw = info.GetPasswordString();
            Assert.Equal("secret123", pw);
            info.Dispose();
        }

        [Fact]
        public void TryLock_Then_TryUnlock_EndToEnd()
        {
            var source = Path.Combine(_tempDir, "plain.txt");
            var locked = Path.Combine(_tempDir, "plain.txt" + AppConstants.Extension);
            var unlocked = Path.Combine(_tempDir, "plain_unlocked.txt");

            var content = Encoding.UTF8.GetBytes("the quick brown fox");
            System.IO.File.WriteAllBytes(source, content);

            var totpSecret = TOTPService.GenerateTotpSecretBase32();
            var nlock = new NLockInfo
            {
                SourceFile = source,
                DestinationFile = locked,
                TotpSecretCode = totpSecret
            };
            nlock.SetPassword("MyTestPassword123!");

            var lockResult = nlock.TryLock();
            Assert.Equal(NLockProcessResultCode.Success, lockResult.ResultCode);
            Assert.True(System.IO.File.Exists(locked));

            var seed = Base32Encoding.ToBytes(totpSecret);
            var totp = new Totp(seed, step: 30);
            var code = totp.ComputeTotp();

            var unlockInfo = new NLockInfo
            {
                SourceFile = locked,
                DestinationFile = unlocked,
                TotpAuthCode = code
            };
            unlockInfo.SetPassword("MyTestPassword123!");
            var unlockResult = unlockInfo.TryUnlock();
            Assert.Equal(NLockProcessResultCode.Success, unlockResult.ResultCode);
            Assert.True(System.IO.File.Exists(unlocked));
            var outBytes = System.IO.File.ReadAllBytes(unlocked);
            Assert.Equal(content, outBytes);
        }
    }
}