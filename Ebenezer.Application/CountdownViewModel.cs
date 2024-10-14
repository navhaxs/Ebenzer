using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Timers;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;

namespace Ebenezer.Application;

public class CountdownViewModel : ReactiveObject
{
    public event EventHandler OnCloseDialog;

    private Timer? aTimer;
    
    private int _countdown = 30;

    public int Countdown
    {
        get => _countdown;
        set => this.RaiseAndSetIfChanged(ref _countdown, value);
    }

    private bool _isPaused = false;

    public bool IsPaused
    {
        get => _isPaused;
        set => this.RaiseAndSetIfChanged(ref _isPaused, value);
    }

    private readonly ObservableAsPropertyHelper<string> _pauseText;
    public string PauseText => _pauseText.Value;

    private IDisposable? _subscription;

    public CountdownViewModel()
    {
        _pauseText = this.WhenAnyValue(vm => vm.IsPaused, (bool b) => b ? "..." : "")
            .ToProperty(this, vm => vm.PauseText);

        if (Design.IsDesignMode)
            return;

        var interval = Observable.Interval(TimeSpan.FromSeconds(1), RxApp.TaskpoolScheduler)
            .Subscribe(_ =>
            {
                if (Countdown > 0 && !IsPaused)
                {
                    Countdown--;
                }
                else if (Countdown == 0)
                {
                    // do the thing
                    if (!Debugger.IsAttached)
                    {
                        var process = new Process
                        {
                            StartInfo =
                            {
                                FileName = "shutdown.exe",
                                Arguments = $"/s /t 10 /f",
                                CreateNoWindow = true,
                                UseShellExecute = false
                            }
                        };
                        process.Start();
                    }

                    IsPaused = true;

                    OnCloseDialog?.Invoke(this, EventArgs.Empty);
                }
            });

        _subscription = interval;

        aTimer = new Timer();
        aTimer.Elapsed += (object source, ElapsedEventArgs e) =>
        {
            uint x = IdleTimeDetect.GetIdleTime();
            if (x > 1_000)
            {
                if (IsPaused)
                {
                    // resume from paused state
                    Countdown = 30;
                }

                IsPaused = false;
            }
            else
            {
                IsPaused = true;
            }
        };
        aTimer.Interval = 100; // 100ms
        aTimer.Enabled = true;
    }
    
    public void CancelCountdown()
    {
        aTimer?.Stop();
        _subscription?.Dispose();
        _subscription = null;
    }
}