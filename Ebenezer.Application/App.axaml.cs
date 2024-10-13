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
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            if (!Design.IsDesignMode)
            {
                var server = new NamedPipeServer(Constants.PipeName);
                Greeter.BindService(server.ServiceBinder, new GreeterService(desktop));
                server.Start();
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}