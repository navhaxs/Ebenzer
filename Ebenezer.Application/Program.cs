using Avalonia;
using System;
using System.Threading;

namespace Ebenezer.Application;

class Program
{
    private static Mutex _mutex = null;
    
    const string appName = "Ebenezer.Application";
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        bool createdNew;
        _mutex = new Mutex(true, appName, out createdNew);

        if (!createdNew)
        {
            // app is already running, allow only a single instance
            return;
        }
        
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
}