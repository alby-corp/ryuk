namespace Ryuk.Options;

using System.ComponentModel.DataAnnotations;

public class CompanyOptions
{
    [Required] public AuthOptions Auth { get; set; } = null!;
}

public class AuthOptions
{
    [Required, MinLength(7)] public string Origin { get; set; } = null!;
    [Required, MinLength(5)] public string Username { get; set; } = null!;
    [Required, MinLength(7)] public string Password { get; set; } = null!;
}