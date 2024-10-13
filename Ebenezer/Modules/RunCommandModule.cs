using System.Diagnostics;

namespace Ebenezer;

public static class RunCommandModule
{
    public static async void LaunchProgram(string programFullPath, bool killExistingApp = false,
        string? arguments = null)
    {
        try
        {
            if (killExistingApp)
            {
                await KillProgram(programFullPath);
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
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public static async Task KillProgram(string processName)
    {
        var proc = Process.Start("taskkill", $"/F /IM {processName}");
        await proc.WaitForExitAsync();
        await Task.Delay(1000);
    }
}