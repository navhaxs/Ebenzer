using System;
using System.Runtime.InteropServices;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;

namespace Ebenezer.Application;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new CountdownViewModel();
        
        if (Design.IsDesignMode)
            return;

        this.Position = new PixelPoint(0, 0);
        this.Width = this.Screens.Primary.WorkingArea.Width / this.Screens.Primary.Scaling;

       
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e) => Close();


}