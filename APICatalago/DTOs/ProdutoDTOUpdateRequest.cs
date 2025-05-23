﻿using System.ComponentModel.DataAnnotations;

namespace APICatalago.DTOs;

public class ProdutoDTOUpdateRequest : IValidatableObject
{
    [Range(1, 9999, ErrorMessage = "O campo {0} deve ter entre {1} e {2}.")]
    public float Estoque { get; set; }

    public DateTime DataCadastro { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (DataCadastro <= DateTime.Now)
        {
            yield return new ValidationResult("A data deve ser maior que a data atual.",
                new[] { nameof(DataCadastro) });
        }
    }
}