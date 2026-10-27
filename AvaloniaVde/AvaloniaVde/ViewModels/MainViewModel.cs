using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Animation;
using Avalonia.Styling;
using Avalonia.Threading;
using AvaloniaVde.Services.Db;
using AvaloniaVde.Services.Navigation;
using AvaloniaVde.Services.Security;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace AvaloniaVde.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly INavigationService _nav;
    private readonly IScreenFactory _factory;
    private readonly ISessionService _session;
    private readonly IIdleService _idle;
    private readonly DbInitializer _dbInitializer;
    private string _userName = "Гость";
    private string _shiftStatusText = "Смена закрыта";
    private string _statusMessage = "Готово";
    private string _currentDateTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
    private object? _currentPage;
    private bool _isInitialized;
    private IPageTransition _pageTransition = new CrossFade { Duration = TimeSpan.FromMilliseconds(300) };

    public MainViewModel(INavigationService nav, IScreenFactory factory, ISessionService session, IIdleService idle, DbInitializer dbInitializer)
    {
        _nav = nav;
        _factory = factory;
        _session = session;
        _idle = idle;
        _dbInitializer = dbInitializer;

        _session.SessionChanged += OnSessionChanged;
        _idle.Idle += OnIdle;

        _nav.Navigated += OnNavigated;
        _nav.NavigateTo(factory.Create<LoginViewModel>());

        MenuCommand = ReactiveCommand.Create(ExecuteOpenMenu);
        LogoutCommand = ReactiveCommand.Create(ExecuteLogout);

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

    public bool IsAuthenticated => _session.IsAuthenticated;

    public IPageTransition PageTransition
    {
        get => _pageTransition;
        private set => this.RaiseAndSetIfChanged(ref _pageTransition, value);
    }

    public ReactiveCommand<Unit, Unit> MenuCommand { get; }
    public ReactiveCommand<Unit, Unit> LogoutCommand { get; }

    private void ExecuteOpenMenu() => _nav.NavigateTo(_factory.Create<MenuViewModel>());

    private void ExecuteLogout() => _session.SignOut();

    public void NotifyUserActivity() => _idle.NotifyActivity();

    private void OnSessionChanged(object? sender, EventArgs e)
    {
        this.RaisePropertyChanged(nameof(IsAuthenticated));

        var user = _session.CurrentUser;
        if (user == null)
        {
            UserName = "Гость";
            _idle.Stop();
            _nav.ResetTo(_factory.Create<LoginViewModel>());
            return;
        }

        UserName = user.DisplayName;
        _idle.Start();
    }

    private void OnIdle(object? sender, EventArgs e)
    {
        _session.SignOut();
    }

    private void OnNavigated(object? sender, EventArgs e)
    {
        PageTransition = new CrossFade { Duration = TimeSpan.FromMilliseconds(300) };
        CurrentPage = null;

        Dispatcher.UIThread.Post(() =>
        {
            CurrentPage = _nav.CurrentPage;
            this.RaisePropertyChanged(nameof(Title));
            _ = (_nav.CurrentPage as ViewModelBase)?.OnActivatedAsync();
        }, DispatcherPriority.Background);
    }

    public async Task InitializeAsync()
    {
        await _dbInitializer.InitializeAsync();
        IsInitialized = true;
    }
}
