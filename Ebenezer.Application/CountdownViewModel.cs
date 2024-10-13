using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Timers;
using Avalonia.Controls;
using ReactiveUI;

namespace Ebenezer.Application;

public class CountdownViewModel : ReactiveObject
{
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

    public CountdownViewModel()
    {
        _pauseText = this.WhenAnyValue(vm => vm.IsPaused, (bool b) => b ? "(paused)" : "")
            .ToProperty(this, vm => vm.PauseText);

        if (Design.IsDesignMode)
            return;
        
        Observable.Interval(TimeSpan.FromSeconds(1), RxApp.TaskpoolScheduler)
            .Subscribe(_ =>
            {
                if (Countdown > 0 && !IsPaused)
                {
                    Countdown--;
                }
                else if (Countdown == 0)
                {
                    // do the thing
                    var process = new Process
                    {
                        StartInfo =
                        {
                            FileName = "shutdown.exe",
                            Arguments= $"/s /t 10 /f",
                        }
                    };

                    process.Start();
                    
                    IsPaused = true;
                }
            });
        
        var aTimer = new Timer();
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

}