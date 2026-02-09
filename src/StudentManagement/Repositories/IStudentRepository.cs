using StudentManagement.Models;

namespace StudentManagement.Repositories;

public interface IStudentRepository
{
    IReadOnlyCollection<Student> GetAll();
    Student? GetById(Guid id);
    Student Add(Student student);
    bool Update(Student student);
    bool Delete(Guid id);
    bool EmailExists(string email, Guid? excludeId = null);
}
