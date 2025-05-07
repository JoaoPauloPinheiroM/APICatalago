using APICatalago.Models;

namespace APICatalago.DTOs.Mappings;

public static class CategoriaMappingExtensions
{
    public static CategoriaDTO? ToDTO(this Categoria entity)
    {
        if (entity == null) return null;

        return new CategoriaDTO
        {
            CategoriaId = entity.CategoriaId,
            Nome = entity.Nome,
            ImgUrl = entity.ImgUrl
        };
    }

    public static IEnumerable<CategoriaDTO> ToDTOList(this IEnumerable<Categoria> entities)
    {
        if (entities == null || !entities.Any())
            return new List<CategoriaDTO>();

        return entities.Select(e => e.ToDTO()!)!;
    }

    public static Categoria? ToEntity(this CategoriaDTO dto)
    {
        if (dto == null) return null;

        return new Categoria
        {
            CategoriaId = dto.CategoriaId,
            Nome = dto.Nome,
            ImgUrl = dto.ImgUrl
        };
    }
}