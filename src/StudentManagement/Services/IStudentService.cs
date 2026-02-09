using StudentManagement.Dtos;

namespace StudentManagement.Services;

public interface IStudentService
{
    IReadOnlyCollection<StudentDto> GetAll();
    StudentDto? GetById(Guid id);
    StudentDto Create(StudentCreateDto dto);
    bool Update(Guid id, StudentUpdateDto dto);
    bool Delete(Guid id);
}
