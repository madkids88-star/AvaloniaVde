using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;

namespace AvaloniaVde.Views;

public partial class MainWindow : Window
{
    public event Action? ContentReady;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        Dispatcher.UIThread.Post(() => ContentReady?.Invoke(), DispatcherPriority.Loaded);
    }

    public async Task ShowContentAsync()
    {
        MainContent.Opacity = 0;
        SplashOverlay.Opacity = 1;

        try
        {
            SplashOverlay.Opacity = 0;
            await Task.Delay(300);
            MainContent.Opacity = 1;
            await Task.Delay(600);
        }
        finally
        {
            SplashOverlay.IsVisible = false;
            MainContent.Opacity = 1;
        }
    }

    public void ForceShowContent()
    {
        MainContent.IsVisible = true;
        MainContent.Opacity = 1;
        SplashOverlay.IsVisible = false;
    }

    public void SetContent(Control view)
    {
        MainContent.Content = view;
    }
}
