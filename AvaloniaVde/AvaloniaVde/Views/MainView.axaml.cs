using Avalonia.Controls;
using AvaloniaVde.ViewModels;

namespace AvaloniaVde.Views;

public partial class MainView : UserControl
{
    private bool _initialized;

    public event System.Action? Ready;

    public MainView()
    {
        InitializeComponent();
        AttachedToVisualTree += OnAttachedToVisualTree;
    }

    private async void OnAttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        if (_initialized || DataContext is not MainViewModel vm)
            return;

        _initialized = true;

        try
        {
            await vm.InitializeAsync();
            Opacity = 1;
        }
        finally
        {
            Ready?.Invoke();
        }
    }
}
