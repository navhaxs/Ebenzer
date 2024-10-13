using System.Diagnostics;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Ebenezer.Application;

public class GreeterService : Greeter.GreeterBase
{
    private readonly IClassicDesktopStyleApplicationLifetime _desktop;

    public GreeterService(IClassicDesktopStyleApplicationLifetime window)
    {
        _desktop = window;
    }

    public override Task<Empty> SayHello(HelloRequest request,
        ServerCallContext context)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            _desktop.MainWindow?.Close();
            _desktop.MainWindow = new MainWindow();
            _desktop.MainWindow.Show();
        });
        return Task.FromResult(new Empty());
    }
    
    public override Task<Empty> Abort(HelloRequest request,
        ServerCallContext context)
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            _desktop.MainWindow?.Close();
            var process = new Process
            {
                StartInfo =
                {
                    FileName = "shutdown.exe",
                    Arguments= $"/a",
                }
            };

            process.Start();
        });
        return Task.FromResult(new Empty());
    }
}