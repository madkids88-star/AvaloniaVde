using System;
using System.IO;
using Android.App;
using AvaloniaVde.Services.Db;

namespace AvaloniaVde.Android;

public sealed class AndroidAppPaths : IAppPaths
{
    public string AppDataDirectory
    {
        get
        {
            var filesDir = Application.Context?.FilesDir?.AbsolutePath;
            return string.IsNullOrEmpty(filesDir) ? Path.GetTempPath() : filesDir;
        }
    }

    public string DatabaseFilePath(string fileName) => Path.Combine(AppDataDirectory, fileName);
}
