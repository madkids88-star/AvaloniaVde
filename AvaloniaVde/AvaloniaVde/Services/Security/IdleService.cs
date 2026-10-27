using System;
using Avalonia.Threading;

namespace AvaloniaVde.Services.Security;

public sealed class IdleService : IIdleService
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan CheckInterval = TimeSpan.FromSeconds(1);

    private readonly object _sync = new();
    private DispatcherTimer? _timer;
    private DateTime _lastActivityUtc = DateTime.UtcNow;
    private bool _idleRaised;

    public TimeSpan IdleTimeout { get; set; } = DefaultTimeout;

    public bool IsRunning => _timer != null;

    public event EventHandler? Idle;

    public void Start()
    {
        lock (_sync)
        {
            if (_timer != null)
                return;

            _lastActivityUtc = DateTime.UtcNow;
            _idleRaised = false;

            _timer = new DispatcherTimer { Interval = CheckInterval };
            _timer.Tick += OnTick;
            _timer.Start();
        }
    }

    public void Stop()
    {
        lock (_sync)
        {
            if (_timer == null)
                return;

            _timer.Stop();
            _timer.Tick -= OnTick;
            _timer = null;
            _idleRaised = false;
        }
    }

    public void NotifyActivity()
    {
        lock (_sync)
        {
            _lastActivityUtc = DateTime.UtcNow;
            _idleRaised = false;
        }
    }

    private void OnTick(object? sender, EventArgs e)
    {
        bool raise;

        lock (_sync)
        {
            if (_idleRaised)
                return;

            if (DateTime.UtcNow - _lastActivityUtc < IdleTimeout)
                return;

            _idleRaised = true;
            raise = true;
        }

        if (raise)
            Idle?.Invoke(this, EventArgs.Empty);
    }
}
