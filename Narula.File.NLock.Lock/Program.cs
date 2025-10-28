namespace Narula.File.NLock.Lock;

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
		//args = Directory.GetFiles(@"C:\Users\inter\OneDrive\Downloads\N256test");
		Application.Run(new LockForm(args));
	}
}