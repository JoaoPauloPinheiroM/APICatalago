namespace APICatalago.Pagination
{
    public class ProdutosFiltroPreco : QueryStringParamers
    {
        public decimal? Preco { get; set; }
        public string? PrecoCriterio { get; set; } // "maior", "menor", "igual"
    }
}