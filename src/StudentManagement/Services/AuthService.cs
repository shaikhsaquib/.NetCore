using Microsoft.IdentityModel.Tokens;
using StudentManagement.Dtos;
using StudentManagement.Models;
using StudentManagement.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace StudentManagement.Services;

public sealed class AuthService : IAuthService
{
    private readonly IUserRepository _repository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository repository, IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
    }

    public AuthResponseDto Signup(SignupDto dto)
    {
        if (_repository.EmailExists(dto.Email))
        {
            throw new InvalidOperationException("Email already exists.");
        }

        var salt = GenerateSalt();
        var hash = HashPassword(dto.Password, salt);
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = Convert.ToBase64String(hash),
            PasswordSalt = Convert.ToBase64String(salt),
            CreatedAt = DateTimeOffset.UtcNow
        };

        _repository.Add(user);
        return CreateToken(user);
    }

    public AuthResponseDto Login(LoginDto dto)
    {
        var user = _repository.GetByEmail(dto.Email);
        if (user is null || !VerifyPassword(dto.Password, user.PasswordSalt, user.PasswordHash))
        {
            throw new InvalidOperationException("Invalid credentials.");
        }

        return CreateToken(user);
    }

    private AuthResponseDto CreateToken(User user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection["Key"];
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];
        var expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out var minutes) ? minutes : 60;

        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException("JWT settings are not configured.");
        }

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expires = DateTimeOffset.UtcNow.AddMinutes(expiryMinutes);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expires.UtcDateTime,
            signingCredentials: credentials);

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            ExpiresAt = expires
        };
    }

    private static byte[] GenerateSalt()
    {
        var salt = new byte[16];
        RandomNumberGenerator.Fill(salt);
        return salt;
    }

    private static byte[] HashPassword(string password, byte[] salt)
    {
        using var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256);
        return deriveBytes.GetBytes(32);
    }

    private static bool VerifyPassword(string password, string saltBase64, string hashBase64)
    {
        var salt = Convert.FromBase64String(saltBase64);
        var hash = Convert.FromBase64String(hashBase64);
        var computed = HashPassword(password, salt);
        return CryptographicOperations.FixedTimeEquals(computed, hash);
    }
}
