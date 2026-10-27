using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using AvaloniaVde.Services.Security;
using AvaloniaVde.ViewModels;

namespace AvaloniaVde.Views;

public partial class MainWindow : Window
{
    public event Action? ContentReady;

    private bool _contentShown;

    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        AddHandler(PointerMovedEvent, OnUserActivity, Avalonia.Interactivity.RoutingStrategies.Tunnel);
        AddHandler(PointerPressedEvent, OnUserActivity, Avalonia.Interactivity.RoutingStrategies.Tunnel);
        AddHandler(KeyDownEvent, OnUserActivity, Avalonia.Interactivity.RoutingStrategies.Tunnel);

        if (MainContent.Content is MainView view)
        {
            view.Ready += OnContentReady;
            _ = FallbackAsync();
        }
        else
        {
            Dispatcher.UIThread.Post(() => ContentReady?.Invoke(), DispatcherPriority.Loaded);
        }
    }

    private void OnContentReady()
    {
        if (_contentShown) return;
        _contentShown = true;
        Dispatcher.UIThread.Post(() => ContentReady?.Invoke(), DispatcherPriority.Loaded);
    }

    private async System.Threading.Tasks.Task FallbackAsync()
    {
        await System.Threading.Tasks.Task.Delay(5000);
        if (!_contentShown)
            ForceShowContent();
    }

    private void OnUserActivity(object? sender, EventArgs e)
    {
        if (DataContext is MainViewModel vm)
            vm.NotifyUserActivity();
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
