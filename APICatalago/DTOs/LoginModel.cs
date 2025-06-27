using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs;

public class LoginModel
{
    [Required(ErrorMessage = "Nome é requerido")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Senha é requerido")]
    public string? Password { get; set; }
}