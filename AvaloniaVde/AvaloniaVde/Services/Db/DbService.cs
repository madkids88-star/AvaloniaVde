using System;
using System.IO;
using AvaloniaVde.Models.SettingsApp;
using LiteDB;

namespace AvaloniaVde.Services.Db;

public sealed class DbService : IDisposable
{
    private readonly LiteDatabase _database;
    private bool _disposed;

    public DbService(IAppPaths paths, DatabaseOptions options)
    {
        Directory.CreateDirectory(paths.AppDataDirectory);

        var connectionString = new ConnectionString
        {
            Filename = paths.DatabaseFilePath(options.FileName),
            Connection = ConnectionType.Shared,
            Upgrade = options.Upgrade,
            ReadOnly = options.ReadOnly
        };

        if (!string.IsNullOrEmpty(options.Password))
            connectionString.Password = options.Password;

        _database = new LiteDatabase(connectionString);
        Mapper = _database.Mapper;
    }

    public BsonMapper Mapper { get; }

    public ILiteCollection<T> GetCollection<T>() => _database.GetCollection<T>();

    public ILiteCollection<T> GetCollection<T>(string name) => _database.GetCollection<T>(name);

    public bool BeginTrans() => _database.BeginTrans();

    public bool Commit() => _database.Commit();

    public bool Rollback() => _database.Rollback();

    #region Users
    #endregion

    #region Recipes
    #endregion

    #region ShiftTasks
    #endregion

    #region Settings
    #endregion

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _database.Dispose();
    }
}
