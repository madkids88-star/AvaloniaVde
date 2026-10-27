using System.Collections.Generic;
using AvaloniaVde.Models.Users;

namespace AvaloniaVde.Services.Db.Repositories;

public interface IUserRepository
{
    IReadOnlyList<User> GetAll();
    User? FindByName(string userName);
    int Count();
    User Insert(User user);
    bool Update(User user);
    bool Delete(int id);
}
