namespace APICatalago.Pagination
{
    public abstract class QueryStringParamers
    {
        private const int maxPageSize = 50;

        public int PageNumber { get; set; } = 1;

        private int _pageSize = maxPageSize;

        public int PageSize
        {
            get => _pageSize;
            //usei esta propriedade para limitar o tamanho da paginação
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}