using APICatalago.Models;

namespace APICatalago.DTOs.Mappings;

public static class ProdutoMappingExtensions
{
    public static ProdutoDTO? ToDTO(this Produto entity)
    {
        if (entity == null) return null;

        return new ProdutoDTO
        {
            ProdutoId = entity.ProdutoId,
            Nome = entity.Nome,
            Descricao = entity.Descricao,
            Preco = entity.Preco,
            ImgUrl = entity.ImgUrl,
            CategoriaId = entity.CategoriaId
        };
    }

    public static IEnumerable<ProdutoDTO> ToDTOList(this IEnumerable<Produto> entities)
    {
        if (entities == null || !entities.Any())
            return new List<ProdutoDTO>();

        return entities.Select(e => e.ToDTO()!)!;
    }

    public static Produto? ToEntity(this ProdutoDTO dto)
    {
        if (dto == null) return null;

        return new Produto
        {
            ProdutoId = dto.ProdutoId,
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            ImgUrl = dto.ImgUrl,
            CategoriaId = dto.CategoriaId
        };
    }
}