using System.Reactive;
using AvaloniaVde.Services.Navigation;
using ReactiveUI;

namespace AvaloniaVde.ViewModels;

public class MenuViewModel : ViewModelBase
{
    public override string Title => "Меню";

    private readonly INavigationService _nav;
    private readonly IScreenFactory _factory;

    public MenuViewModel(INavigationService nav, IScreenFactory factory)
    {
        _nav = nav;
        _factory = factory;

        GoDayTaskCommand = ReactiveCommand.Create(() => { });
        GoHomeCommand = ReactiveCommand.Create(ExecuteHome);
        GoScalesCommand = ReactiveCommand.Create(ExecuteScales);
        GoInfoCommand = ReactiveCommand.Create(() => { });
        GoRecipesCommand = ReactiveCommand.Create(() => { });
        GoIngredientsCommand = ReactiveCommand.Create(() => { });
        GoUsersCommand = ReactiveCommand.Create(() => { });
        GoImagesCommand = ReactiveCommand.Create(() => { });
        GoStatisticsCommand = ReactiveCommand.Create(() => { });
        GoCountersCommand = ReactiveCommand.Create(() => { });
        GoUsbCommand = ReactiveCommand.Create(() => { });
        GoSettingsCommand = ReactiveCommand.Create(() => { });
    }

    public ReactiveCommand<Unit, Unit> GoDayTaskCommand { get; }
    public ReactiveCommand<Unit, Unit> GoHomeCommand { get; }
    public ReactiveCommand<Unit, Unit> GoScalesCommand { get; }
    public ReactiveCommand<Unit, Unit> GoInfoCommand { get; }
    public ReactiveCommand<Unit, Unit> GoRecipesCommand { get; }
    public ReactiveCommand<Unit, Unit> GoIngredientsCommand { get; }
    public ReactiveCommand<Unit, Unit> GoUsersCommand { get; }
    public ReactiveCommand<Unit, Unit> GoImagesCommand { get; }
    public ReactiveCommand<Unit, Unit> GoStatisticsCommand { get; }
    public ReactiveCommand<Unit, Unit> GoCountersCommand { get; }
    public ReactiveCommand<Unit, Unit> GoUsbCommand { get; }
    public ReactiveCommand<Unit, Unit> GoSettingsCommand { get; }

    private void ExecuteHome() => _nav.NavigateTo(_factory.Create<DosingViewModel>());

    private void ExecuteScales() => _nav.NavigateTo(_factory.Create<ScaleViewModel>());
}
