using ReactiveUI;

namespace AvaloniaVde.ViewModels;

public abstract class ViewModelBase : ReactiveObject
{
    public virtual string Title => string.Empty;

    protected void RefreshTitle() => this.RaisePropertyChanged(nameof(Title));
}
