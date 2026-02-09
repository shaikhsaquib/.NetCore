using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Dtos;
using StudentManagement.Services;

namespace StudentManagement.Controllers;

[ApiController]
[Authorize]
[Route("api/students")]
public sealed class StudentsController : ControllerBase
{
    private readonly IStudentService _service;

    public StudentsController(IStudentService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IReadOnlyCollection<StudentDto>> GetAll()
    {
        return Ok(_service.GetAll());
    }

    [HttpGet("{id:guid}")]
    public ActionResult<StudentDto> GetById(Guid id)
    {
        var student = _service.GetById(id);
        if (student is null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    [HttpPost]
    public ActionResult<StudentDto> Create(StudentCreateDto dto)
    {
        try
        {
            var created = _service.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, StudentUpdateDto dto)
    {
        try
        {
            var updated = _service.Update(id, dto);
            return updated ? NoContent() : NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var deleted = _service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
