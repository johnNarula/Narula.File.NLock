namespace Narula.File.NLock.Unlock;

using System.Linq;
using System.Text;
using System.Text.Json;
using Narula.File.NLock.Models;

internal static class Program
{
	/// <summary>
	///  The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main(string[] args)
	{
		// To customize application configuration such as set high DPI settings or default font,
		// see https://aka.ms/applicationconfiguration.
		ApplicationConfiguration.Initialize();
		
		if (args.Length >= 2 && (args[0] == "-json64" || args[0] == "-json"))
		{
			try
			{
				string json;
				if (args[0] == "-json")
					json = string.Join(" ", args[1..]); // Join all remaining args in case of spaces
				else
					json = Encoding.UTF8.GetString(Convert.FromBase64String(args[1]));

				var launchReq = JsonSerializer.Deserialize<UnlockGuiLaunchRequest>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
				Application.Run(new UnLockForm(launchReq));
				return;
			}
			catch { /* ignore and fallback */ }
		}
		
		Application.Run(new UnLockForm(args));
	}
}