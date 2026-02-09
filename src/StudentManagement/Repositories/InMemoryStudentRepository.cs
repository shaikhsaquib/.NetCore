using StudentManagement.Models;

namespace StudentManagement.Repositories;

public sealed class InMemoryStudentRepository : IStudentRepository
{
    private readonly Dictionary<Guid, Student> _students = new();

    public IReadOnlyCollection<Student> GetAll() => _students.Values.ToList();

    public Student? GetById(Guid id) => _students.GetValueOrDefault(id);

    public Student Add(Student student)
    {
        _students[student.Id] = student;
        return student;
    }

    public bool Update(Student student)
    {
        if (!_students.ContainsKey(student.Id))
        {
            return false;
        }

        _students[student.Id] = student;
        return true;
    }

    public bool Delete(Guid id) => _students.Remove(id);

    public bool EmailExists(string email, Guid? excludeId = null)
    {
        return _students.Values.Any(student =>
            student.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
            && student.Id != excludeId);
    }
}
