using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs;

public class RegisterModel
{
    [Required(ErrorMessage = "Nome é requerido")]
    public string? UserName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email é requerido")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Senha é requerido")]
    public string? Password { get; set; }
}