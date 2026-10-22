using System;
using AvaloniaVde.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaVde.Services;

public interface IScreenFactory
{
    T Create<T>() where T : ViewModelBase;
}

public class ScreenFactory : IScreenFactory
{
    private readonly IServiceProvider _provider;

    public ScreenFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public T Create<T>() where T : ViewModelBase => _provider.GetRequiredService<T>();
}
