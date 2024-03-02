using System.Diagnostics;

namespace Ebenezer;

public static class RunCommandModule
{
    public static void LaunchProgram(string programFullPath, bool killExistingApp = false, string? arguments = null)
    {
        if (killExistingApp)
        {
            var processName = Path.GetFileName(programFullPath);
            Process.Start("taskkill", $"/F /IM {processName}");
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