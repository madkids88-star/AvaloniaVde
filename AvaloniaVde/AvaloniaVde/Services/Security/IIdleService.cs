using System;

namespace AvaloniaVde.Services.Security;

public interface IIdleService
{
    TimeSpan IdleTimeout { get; set; }
    bool IsRunning { get; }
    event EventHandler? Idle;
    void Start();
    void Stop();
    void NotifyActivity();
}
