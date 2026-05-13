using System.ComponentModel.DataAnnotations;

namespace GadgetShop.Models;

public sealed class LoginRequest
{
    [Required] public string UserName { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public sealed class LoginResponse
{
    public long UserId { get; set; }
    public string? FullName { get; set; }
    public string? RoleCode { get; set; }
    public string? RoleName { get; set; }
    public string? AccessToken { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }
    public string? UserName { get; set; }
}

public sealed class RefreshTokenRequest
{
    [Required] public string AccessToken { get; set; } = string.Empty;
    [Required] public string RefreshToken { get; set; } = string.Empty;
}

public sealed class AuthTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime RefreshTokenExpiryTime { get; set; }
    public DateTime ExpiresAtUtc { get; set; }
}

public sealed class CustomerRegisterRequest
{
    [Required, StringLength(100)] public string UserName { get; set; } = string.Empty;
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Phone] public string? PhoneNumber { get; set; }
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
}

public sealed class CustomerProfileUpdateRequest
{
    [Required, StringLength(100)] public string FullName { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Phone] public string? PhoneNumber { get; set; }
}

public sealed class ChangePasswordRequest
{
    [Required] public string CurrentPassword { get; set; } = string.Empty;
    [Required, MinLength(6)] public string NewPassword { get; set; } = string.Empty;
    [Compare(nameof(NewPassword))] public string ConfirmNewPassword { get; set; } = string.Empty;
}

public sealed class ForgotPasswordRequest
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
}

public sealed class CustomerProfileDto
{
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? CreatedAt { get; set; }
}
