using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ebenezer.Data;
using GrpcDotNetNamedPipes;

namespace Ebenezer.Application;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            if (!Design.IsDesignMode)
            {
                if (Debugger.IsAttached)
                {
                    desktop.MainWindow = new MainWindow();
                }
                else
                {
                    desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
                    var server = new NamedPipeServer(Constants.PipeName);
                    Greeter.BindService(server.ServiceBinder, new GreeterService(desktop));
                    server.Start();
                }
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}