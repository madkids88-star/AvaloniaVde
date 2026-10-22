using ReactiveUI;

namespace AvaloniaVde.ViewModels;

public class DosingViewModel : ViewModelBase
{
    public override string Title => "Дозирование";

    private string _greeting = "Welcome to Avalonia!";

    public string Greeting
    {
        get => _greeting;
        set => this.RaiseAndSetIfChanged(ref _greeting, value);
    }
}
