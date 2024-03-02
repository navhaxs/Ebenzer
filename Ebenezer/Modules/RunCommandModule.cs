using System.Diagnostics;

namespace Ebenezer;

public static class RunCommandModule
{
    public static async void LaunchProgram(string programFullPath, bool killExistingApp = false, string? arguments = null)
    {
        if (killExistingApp)
        {
            var processName = Path.GetFileName(programFullPath);
            var proc = Process.Start("taskkill", $"/F /IM {processName}");
            await proc.WaitForExitAsync();
            await Task.Delay(1000);
        }

        if (arguments != null)
        {
            Process.Start(programFullPath, arguments); 
        }
        else
        {
            Process.Start(programFullPath); 
        }
    }
}