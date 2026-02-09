using StudentManagement.Models;

namespace StudentManagement.Repositories;

public sealed class InMemoryUserRepository : IUserRepository
{
    private readonly Dictionary<string, User> _users = new(StringComparer.OrdinalIgnoreCase);

    public User? GetByEmail(string email) => _users.GetValueOrDefault(email);

    public User Add(User user)
    {
        _users[user.Email] = user;
        return user;
    }

    public bool EmailExists(string email) => _users.ContainsKey(email);
}
