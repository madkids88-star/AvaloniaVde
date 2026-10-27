using System;
using System.IO;

namespace AvaloniaVde.Services.Db;

public sealed class AppPaths : IAppPaths
{
    private const string AppFolderName = "AvaloniaVde";

    public string AppDataDirectory
    {
        get
        {
            var baseDir = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData,
                Environment.SpecialFolderOption.Create);

            if (string.IsNullOrEmpty(baseDir))
                baseDir = Path.GetTempPath();

            return Path.Combine(baseDir, AppFolderName);
        }
    }

    public string DatabaseFilePath(string fileName) => Path.Combine(AppDataDirectory, fileName);
}
