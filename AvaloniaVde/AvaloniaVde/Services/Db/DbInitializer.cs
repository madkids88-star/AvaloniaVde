using System.Linq;
using System.Threading.Tasks;
using AvaloniaVde.Models.Users;
using AvaloniaVde.Services.Db.Repositories;

namespace AvaloniaVde.Services.Db;

public sealed class DbInitializer
{
    private readonly IUserRepository _users;

    public DbInitializer(IUserRepository users)
    {
        _users = users;
    }

    public Task InitializeAsync() => Task.Run(Initialize);

    public void Initialize()
    {
        SeedDefaultUsers();
    }

    private void SeedDefaultUsers()
    {
        if (_users.Count() > 0)
            return;

        foreach (var user in DefaultUsers.Create())
            _users.Insert(user);
    }
}
