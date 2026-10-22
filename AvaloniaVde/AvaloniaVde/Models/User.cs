namespace AvaloniaVde.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int Privilege { get; set; }

    public string DisplayName => $"{UserName} ({GetPrivilegeName()})";

    public string GetPrivilegeName()
    {
        return Privilege switch
        {
            10 => "Администратор",
            20 => "Сервис",
            30 => "Технолог",
            40 => "Оператор",
            _ => $"Неизвестно ({Privilege})"
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
