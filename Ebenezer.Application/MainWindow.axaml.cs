using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Ebenezer.Application;

public partial class MainWindow : Window
{
    private CountdownViewModel vm;
    
    public MainWindow()
    {
        InitializeComponent();

        DataContext = vm = new CountdownViewModel();;

        if (Design.IsDesignMode)
            return;

        vm.OnCloseDialog += (_, _) => { Dispatcher.UIThread.InvokeAsync(() => Close()); };

        Closed += (sender, args) => vm.CancelCountdown();

        Activated += ((sender, args) =>
        {
            if (Screens.Primary is not null)
            {
                Position = Screens.Primary.Bounds.Position;
                Width = Screens.Primary.WorkingArea.Width / Screens.Primary.Scaling;
            }
        });
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e) => Close();
}