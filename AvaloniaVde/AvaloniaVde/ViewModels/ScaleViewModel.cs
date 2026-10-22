using System.Reactive;
using AvaloniaVde.Services;
using ReactiveUI;

namespace AvaloniaVde.ViewModels;

public class ScaleViewModel : ViewModelBase
{
    public override string Title => "Взвешивание";

    private readonly INavigationService _nav;

    public ScaleViewModel(INavigationService nav)
    {
        _nav = nav;
        BackCommand = ReactiveCommand.Create(() => { _nav.GoBack(); });
    }

    public ReactiveCommand<Unit, Unit> BackCommand { get; }
}
