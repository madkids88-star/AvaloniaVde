using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using AvaloniaVde.Models.Users;
using AvaloniaVde.Services.Navigation;
using AvaloniaVde.Services.Db.Repositories;
using AvaloniaVde.Services.Security;
using ReactiveUI;
using ReactiveUI.Avalonia;

namespace AvaloniaVde.ViewModels;

public class LoginViewModel : ViewModelBase, IDisposable
{
    public override string Title => "Вход в систему";

    private User? _selectedUser;
    private string _password = string.Empty;
    private string _message = string.Empty;
    private bool _isSlaveMode;
    private string _selectedShift = "Смена 1";
    private bool _isShiftOpen;
    private string _shiftButtonText = "Открыть смену";
    private string _shiftStatusText = "Смена закрыта";

    public ObservableCollection<User> Users { get; } = new();

    public ObservableCollection<string> Shifts { get; } = new() { "Смена 1", "Смена 2", "Смена 3" };

    private readonly INavigationService _nav;
    private readonly IScreenFactory _factory;
    private readonly IUserRepository _users;
    private readonly ISessionService _session;
    private readonly SerialDisposable _messageReset = new();

    public LoginViewModel(INavigationService nav, IScreenFactory factory, IUserRepository users, ISessionService session)
    {
        _nav = nav;
        _factory = factory;
        _users = users;
        _session = session;

        LoginCommand = ReactiveCommand.Create(ExecuteLogin);
        ToggleShiftCommand = ReactiveCommand.Create(ExecuteToggleShift);
        NavigateToScalesCommand = ReactiveCommand.Create(ExecuteNavigateToScales);
    }

    public async Task LoadAsync()
    {
        var users = await Task.Run(() => _users.GetAll().ToList());

        await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
        {
            Users.Clear();
            foreach (var user in users)
                Users.Add(user);
            SelectedUser = Users.FirstOrDefault();
        });
    }

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedUser, value);
            Message = string.Empty;
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            this.RaiseAndSetIfChanged(ref _password, value);
            Message = string.Empty;
        }
    }

    public string Message
    {
        get => _message;
        set
        {
            this.RaiseAndSetIfChanged(ref _message, value);
            _messageReset.Disposable = null;
            if (!string.IsNullOrEmpty(value))
            {
                _messageReset.Disposable = Observable
                    .Timer(TimeSpan.FromSeconds(2))
                    .ObserveOn(AvaloniaScheduler.Instance)
                    .Subscribe(_ => Message = string.Empty);
            }
        }
    }

    public bool IsSlaveMode
    {
        get => _isSlaveMode;
        set => this.RaiseAndSetIfChanged(ref _isSlaveMode, value);
    }

    public string SelectedShift
    {
        get => _selectedShift;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedShift, value);
            Message = string.Empty;
        }
    }

    public bool IsShiftOpen
    {
        get => _isShiftOpen;
        set => this.RaiseAndSetIfChanged(ref _isShiftOpen, value);
    }

    public string ShiftButtonText
    {
        get => _shiftButtonText;
        set => this.RaiseAndSetIfChanged(ref _shiftButtonText, value);
    }

    public string ShiftStatusText
    {
        get => _shiftStatusText;
        set => this.RaiseAndSetIfChanged(ref _shiftStatusText, value);
    }

    public ReactiveCommand<Unit, Unit> LoginCommand { get; }
    public ReactiveCommand<Unit, Unit> ToggleShiftCommand { get; }
    public ReactiveCommand<Unit, Unit> NavigateToScalesCommand { get; }

    private void ExecuteLogin()
    {
        if (IsSlaveMode)
        {
            NavigateToDosing();
            return;
        }

        if (SelectedUser == null)
        {
            Message = "Выберите пользователя";
            return;
        }

        if (string.IsNullOrEmpty(Password))
        {
            Message = "Введите пароль!";
            return;
        }

        if (SelectedUser.Password == Password)
        {
            _session.SignIn(SelectedUser);
            Password = string.Empty;
            NavigateToDosing();
        }
        else
        {
            Password = string.Empty;
            Message = "Неверный пароль!";
        }
    }

    private void NavigateToDosing() => _nav.NavigateTo(_factory.Create<DosingViewModel>());

    private void ExecuteToggleShift()
    {
        IsShiftOpen = !IsShiftOpen;
        if (IsShiftOpen)
        {
            ShiftButtonText = "Закрыть смену";
            ShiftStatusText = $"{SelectedShift} открыта";
        }
        else
        {
            ShiftButtonText = "Открыть смену";
            ShiftStatusText = "Смена закрыта";
        }
    }

    public override Task OnActivatedAsync() => LoadAsync();

    public void Dispose() => _messageReset.Dispose();

    private void ExecuteNavigateToScales()
    {
        _nav.NavigateTo(_factory.Create<ScaleViewModel>());
    }
}
