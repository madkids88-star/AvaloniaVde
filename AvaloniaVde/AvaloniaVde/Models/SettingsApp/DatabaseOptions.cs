namespace AvaloniaVde.Models.SettingsApp;

public class DatabaseOptions
{
    public const string SectionName = "Database";

    public string FileName { get; set; } = "avaloniavde.db";
    public bool AutoCreate { get; set; } = true;
    public string Password { get; set; } = string.Empty;
    public bool Upgrade { get; set; } = true;
    public bool ReadOnly { get; set; }
}
