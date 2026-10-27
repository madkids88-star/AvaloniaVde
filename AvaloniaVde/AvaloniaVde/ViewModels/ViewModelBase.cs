using System.Threading.Tasks;
using ReactiveUI;

namespace AvaloniaVde.ViewModels;

public abstract class ViewModelBase : ReactiveObject
{
    public virtual string Title => string.Empty;

    public virtual Task OnActivatedAsync() => Task.CompletedTask;

    protected void RefreshTitle() => this.RaisePropertyChanged(nameof(Title));
}
