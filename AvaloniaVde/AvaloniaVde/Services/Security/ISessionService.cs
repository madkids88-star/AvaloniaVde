using System;
using AvaloniaVde.Models.Users;

namespace AvaloniaVde.Services.Security;

public interface ISessionService
{
    User? CurrentUser { get; }
    bool IsAuthenticated { get; }
    event EventHandler? SessionChanged;
    void SignIn(User user);
    void SignOut();
}
