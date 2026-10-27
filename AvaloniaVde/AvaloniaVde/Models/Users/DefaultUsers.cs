using System.Collections.Generic;

namespace AvaloniaVde.Models.Users;

public static class DefaultUsers
{
    public static IReadOnlyList<User> Create() => new List<User>
    {
        new() { UserName = "Administrator", Password = "111111", Privilege = UserPrivileges.Administrator, IsDefault = true },
        new() { UserName = "Service", Password = "7832", Privilege = UserPrivileges.Service, IsDefault = true },
        new() { UserName = "Tehnolog", Password = "555", Privilege = UserPrivileges.Technolog, IsDefault = true },
        new() { UserName = "Operator", Password = "111", Privilege = UserPrivileges.Operator, IsDefault = true }
    };
}
