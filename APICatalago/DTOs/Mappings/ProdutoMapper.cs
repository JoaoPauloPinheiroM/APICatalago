using APICatalago.Models;
using AutoMapper;

namespace APICatalago.DTOs.Mappings;

public class ProdutoMapper : Profile
{
    public ProdutoMapper()
    {
        CreateMap<Produto, ProdutoDTO>().ReverseMap();
        CreateMap<Categoria, CategoriaDTO>().ReverseMap();
    }
}