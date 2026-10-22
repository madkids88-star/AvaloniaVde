using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Styling;
using Avalonia.Threading;
using AvaloniaVde.Services;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace AvaloniaVde.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INavigationService _nav;
    private string _userName = "Оператор";
    private string _shiftStatusText = "Смена закрыта";
    private string _statusMessage = "Готово";
    private string _currentDateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
    private object? _currentPage;
    private bool _isInitialized;
    private IPageTransition _pageTransition = new CrossFade { Duration = TimeSpan.FromMilliseconds(300) };

    public MainViewModel(INavigationService nav, IScreenFactory factory)
    {
        _nav = nav;
        _nav.Navigated += OnNavigated;
        _nav.NavigateTo(factory.Create<LoginViewModel>());

        BackCommand = ReactiveCommand.Create(ExecuteGoBack);

        Observable.Interval(TimeSpan.FromSeconds(1))
            .ObserveOn(AvaloniaScheduler.Instance)
            .Subscribe(_ => CurrentDateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
    }

    public override string Title => (CurrentPage as ViewModelBase)?.Title ?? "AvaloniaVde";

    public string UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }

    public string ShiftStatusText
    {
        get => _shiftStatusText;
        set => this.RaiseAndSetIfChanged(ref _shiftStatusText, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => this.RaiseAndSetIfChanged(ref _statusMessage, value);
    }

    public string CurrentDateTime
    {
        get => _currentDateTime;
        set => this.RaiseAndSetIfChanged(ref _currentDateTime, value);
    }

    public object? CurrentPage
    {
        get => _currentPage;
        private set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    public bool IsInitialized
    {
        get => _isInitialized;
        set => this.RaiseAndSetIfChanged(ref _isInitialized, value);
    }

    public IPageTransition PageTransition
    {
        get => _pageTransition;
        private set => this.RaiseAndSetIfChanged(ref _pageTransition, value);
    }

    public ReactiveCommand<System.Reactive.Unit, System.Reactive.Unit> BackCommand { get; }

    private void ExecuteGoBack() => _nav.GoBack();

    private void OnNavigated(object? sender, EventArgs e)
    {
        PageTransition = new CrossFade { Duration = TimeSpan.FromMilliseconds(300) };
        CurrentPage = null;

        Dispatcher.UIThread.Post(() =>
        {
            CurrentPage = _nav.CurrentPage;
            this.RaisePropertyChanged(nameof(Title));
        }, DispatcherPriority.Background);
    }

    public async Task InitializeAsync()
    {
        await Task.Delay(1500);
        IsInitialized = true;
    }
}
