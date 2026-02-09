namespace StudentManagement.Dtos;

public sealed class AuthResponseDto
{
    public string Token { get; init; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; init; }
}
