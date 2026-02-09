using StudentManagement.Dtos;
using StudentManagement.Models;
using StudentManagement.Repositories;

namespace StudentManagement.Services;

public sealed class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyCollection<StudentDto> GetAll() =>
        _repository.GetAll().Select(MapToDto).ToList();

    public StudentDto? GetById(Guid id)
    {
        var student = _repository.GetById(id);
        return student is null ? null : MapToDto(student);
    }

    public StudentDto Create(StudentCreateDto dto)
    {
        if (_repository.EmailExists(dto.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var now = DateTimeOffset.UtcNow;
        var student = new Student
        {
            Id = Guid.NewGuid(),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            CreatedAt = now,
            UpdatedAt = now
        };

        _repository.Add(student);
        return MapToDto(student);
    }

    public bool Update(Guid id, StudentUpdateDto dto)
    {
        var student = _repository.GetById(id);
        if (student is null)
        {
            return false;
        }

        if (_repository.EmailExists(dto.Email, id))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        student.FirstName = dto.FirstName;
        student.LastName = dto.LastName;
        student.Email = dto.Email;
        student.DateOfBirth = dto.DateOfBirth;
        student.UpdatedAt = DateTimeOffset.UtcNow;

        return _repository.Update(student);
    }

    public bool Delete(Guid id) => _repository.Delete(id);

    private static StudentDto MapToDto(Student student) => new()
    {
        Id = student.Id,
        FirstName = student.FirstName,
        LastName = student.LastName,
        Email = student.Email,
        DateOfBirth = student.DateOfBirth
    };
}
