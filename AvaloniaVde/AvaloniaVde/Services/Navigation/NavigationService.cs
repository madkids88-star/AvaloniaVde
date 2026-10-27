using System;
using AvaloniaVde.ViewModels;

namespace AvaloniaVde.Services.Navigation;

public enum NavigationDirection
{
    Forward,
    Backward
}

public interface INavigationService
{
    ViewModelBase? CurrentPage { get; }
    bool CanGoBack { get; }
    NavigationDirection Direction { get; }
    event EventHandler? Navigated;
    void NavigateTo(ViewModelBase viewModel);
    void ResetTo(ViewModelBase viewModel);
    bool GoBack();
}

public class NavigationService : INavigationService
{
    private readonly System.Collections.Generic.Stack<ViewModelBase> _stack = new();

    public ViewModelBase? CurrentPage { get; private set; }

    public bool CanGoBack => _stack.Count > 0;

    public NavigationDirection Direction { get; private set; } = NavigationDirection.Forward;

    public event EventHandler? Navigated;

    public void NavigateTo(ViewModelBase viewModel)
    {
        if (CurrentPage != null)
            _stack.Push(CurrentPage);

        Direction = NavigationDirection.Forward;
        CurrentPage = viewModel;
        Navigated?.Invoke(this, EventArgs.Empty);
    }

    public void ResetTo(ViewModelBase viewModel)
    {
        _stack.Clear();
        Direction = NavigationDirection.Forward;
        CurrentPage = viewModel;
        Navigated?.Invoke(this, EventArgs.Empty);
    }

    public bool GoBack()
    {
        if (_stack.Count == 0)
            return false;

        Direction = NavigationDirection.Backward;
        CurrentPage = _stack.Pop();
        Navigated?.Invoke(this, EventArgs.Empty);
        return true;
    }
}
