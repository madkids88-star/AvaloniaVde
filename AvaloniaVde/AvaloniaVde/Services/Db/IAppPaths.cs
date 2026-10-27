namespace AvaloniaVde.Services.Db;

public interface IAppPaths
{
    string AppDataDirectory { get; }
    string DatabaseFilePath(string fileName);
}
