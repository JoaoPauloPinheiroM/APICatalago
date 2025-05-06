namespace APICatalago.Repositories.Interfaces;

public interface IUnitOfWork
{
    /**
     * Propriedades para acessar os repositórios, como campos publicos
     * >mais flexivel para uso<
    **/
    IProdutoRepository ProdutoRepository { get; }
    ICategoriaRepository CategoriaRepository { get; }

    //Confirmar todas as alterações feitas no banco de dados
    void Commit();
}