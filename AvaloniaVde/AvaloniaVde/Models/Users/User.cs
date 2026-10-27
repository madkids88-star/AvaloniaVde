namespace AvaloniaVde.Models.Users;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Privilege { get; set; }
    public bool IsDefault { get; set; }

    public string DisplayName => UserName;

    public string GetPrivilegeName()
    {
        return Privilege switch
        {
            10 => "Administrator",
            20 => "Service",
            30 => "Technolog",
            40 => "Operator",
            _ => $"Unknown ({Privilege})"
        };
    }
}

public static class UserPrivileges
{
    public const int Administrator = 10;
    public const int Service = 20;
    public const int Technolog = 30;
    public const int Operator = 40;
}
