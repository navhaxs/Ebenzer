using System.Diagnostics;

namespace Ebenezer;

public static class RunCommandModule
{
    public static void LaunchProgram(string programFullPath, bool killExistingApp = false)
    {
        if (killExistingApp)
        {
            var processName = Path.GetFileName(programFullPath);
            Process.Start("taskkill", $"/F /IM {processName}");
        }
        
        Process.Start(programFullPath); 
    }
}