using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaVde.Services;
using AvaloniaVde.ViewModels;
using AvaloniaVde.Views;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AvaloniaVde;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
#if DEBUG
        this.AttachDeveloperTools();
#endif
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var services = new ServiceCollection();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IScreenFactory, ScreenFactory>();
        services.AddSingleton<MainViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<DosingViewModel>();
        services.AddTransient<ScaleViewModel>();
        var provider = services.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var viewModel = provider.GetRequiredService<MainViewModel>();
            var mainWindow = new MainWindow
            {
                DataContext = viewModel
            };
            desktop.MainWindow = mainWindow;

            var started = false;
            mainWindow.ContentReady += async () =>
            {
                if (started) return;
                started = true;
                await InitializeAsync(viewModel, mainWindow);
            };
        }
        else if (ApplicationLifetime is IActivityApplicationLifetime singleViewFactoryApplicationLifetime)
        {
            singleViewFactoryApplicationLifetime.MainViewFactory = () => new MainView
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            };
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static async System.Threading.Tasks.Task InitializeAsync(MainViewModel viewModel, MainWindow mainWindow)
    {
        try
        {
            await viewModel.InitializeAsync();
            await mainWindow.ShowContentAsync();
        }
        catch
        {
            mainWindow.ForceShowContent();
        }
    }
}
