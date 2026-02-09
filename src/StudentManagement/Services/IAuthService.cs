using StudentManagement.Dtos;

namespace StudentManagement.Services;

public interface IAuthService
{
    AuthResponseDto Signup(SignupDto dto);
    AuthResponseDto Login(LoginDto dto);
}
