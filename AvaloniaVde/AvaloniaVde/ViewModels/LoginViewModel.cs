using System.Collections.ObjectModel;
using System.Reactive;
using AvaloniaVde.Models;
using AvaloniaVde.Services;
using ReactiveUI;

namespace AvaloniaVde.ViewModels;

public class LoginViewModel : ViewModelBase
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

    public ObservableCollection<User> Users { get; } = new()
    {
        new User { Id = 1, UserName = "Administrator", Password = "111111", Privilege = UserPrivileges.Administrator },
        new User { Id = 2, UserName = "Service", Password = "7832", Privilege = UserPrivileges.Service },
        new User { Id = 3, UserName = "Tehnolog", Password = "555", Privilege = UserPrivileges.Technolog },
        new User { Id = 4, UserName = "Operator", Password = "111", Privilege = UserPrivileges.Operator },
    };

    public ObservableCollection<string> Shifts { get; } = new() { "Смена 1", "Смена 2", "Смена 3" };

    private readonly INavigationService _nav;
    private readonly IScreenFactory _factory;

    public LoginViewModel(INavigationService nav, IScreenFactory factory)
    {
        _nav = nav;
        _factory = factory;
        SelectedUser = Users[0];

        LoginCommand = ReactiveCommand.Create(ExecuteLogin);
        ToggleShiftCommand = ReactiveCommand.Create(ExecuteToggleShift);
        NavigateToScalesCommand = ReactiveCommand.Create(ExecuteNavigateToScales);
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
        set => this.RaiseAndSetIfChanged(ref _message, value);
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
            Message = "Вход выполнен (режим Slave)";
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
            Message = $"Пользователь {SelectedUser.UserName} авторизован";
            NavigateToDosing();
        }
        else
        {
            Message = "Неверный пароль!";
            Password = string.Empty;
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
            Message = $"{SelectedShift} открыта";
        }
        else
        {
            ShiftButtonText = "Открыть смену";
            ShiftStatusText = "Смена закрыта";
            Message = "Смена закрыта";
        }
    }

    private void ExecuteNavigateToScales()
    {
        _nav.NavigateTo(_factory.Create<ScaleViewModel>());
    }
}
