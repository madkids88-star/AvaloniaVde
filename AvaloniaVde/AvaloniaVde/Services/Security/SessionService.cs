using System;
using AvaloniaVde.Models.Users;

namespace AvaloniaVde.Services.Security;

public sealed class SessionService : ISessionService
{
    private User? _currentUser;

    public User? CurrentUser => _currentUser;

    public bool IsAuthenticated => _currentUser != null;

    public event EventHandler? SessionChanged;

    public void SignIn(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        _currentUser = user;
        SessionChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SignOut()
    {
        if (_currentUser == null)
            return;

        _currentUser = null;
        SessionChanged?.Invoke(this, EventArgs.Empty);
    }
}
