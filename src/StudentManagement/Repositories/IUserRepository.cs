using StudentManagement.Models;

namespace StudentManagement.Repositories;

public interface IUserRepository
{
    User? GetByEmail(string email);
    User Add(User user);
    bool EmailExists(string email);
}
