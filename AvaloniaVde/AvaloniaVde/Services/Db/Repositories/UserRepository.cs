using System.Collections.Generic;
using System.Linq;
using AvaloniaVde.Models.Users;

namespace AvaloniaVde.Services.Db.Repositories;

public sealed class UserRepository : IUserRepository
{
    private const string CollectionName = "users";
    private readonly DbService _db;

    public UserRepository(DbService db)
    {
        _db = db;
        _db.GetCollection<User>(CollectionName).EnsureIndex(u => u.UserName, unique: true);
    }

    private LiteDB.ILiteCollection<User> Collection => _db.GetCollection<User>(CollectionName);

    public IReadOnlyList<User> GetAll() => Collection.FindAll().ToList();

    public User? FindByName(string userName) =>
        Collection.FindOne(u => u.UserName == userName);

    public int Count() => Collection.Count();

    public User Insert(User user)
    {
        Collection.Insert(user);
        return user;
    }

    public bool Update(User user) => Collection.Update(user);

    public bool Delete(int id) => Collection.Delete(id);
}
