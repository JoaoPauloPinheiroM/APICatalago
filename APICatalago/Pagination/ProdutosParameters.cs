namespace APICatalago.Pagination;

public class ProdutosParameters
{
    //Esta propiedade é para o filtro de pesquisa
    private const int maxPageSize = 50;

    //Esta propiedade é para o filtro de pesquisa
    public int PageNumber { get; set; } = 1;

    //Valor padrão para o filtro de pesquisa
    private int _pageSize = 10;

    public int PageSize
    {
        get => _pageSize;
        //Se o valor for maior que o maximo, ele vai pegar o maximo
        set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
    }
}