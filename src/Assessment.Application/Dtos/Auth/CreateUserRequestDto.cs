using System.ComponentModel.DataAnnotations;

namespace Assessment.Application.Dtos.Auth;

public class CreateUserRequestDto
{
    [Required, EmailAddress]
    public string Email { get; set; } = default!;

    [Required, MinLength(3), MaxLength(50)]
    public string UserName { get; set; } = default!;

    [Required, MinLength(6), MaxLength(100)]
    public string Password { get; set; } = default!;

    public bool IsAdmin { get; set; } = false;
}

