using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using APICatalago.Repositories;

namespace APICatalago.Services;

public class ProdutoServices
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IEnumerable<Produto> GetProdutosPorCategoria(int id)
    {
        return _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);
    }

    public IEnumerable<Produto> GetProdutos()
    {
        var produto = _unitOfWork.ProdutoRepository.GetAll();
        if (!produto.Any())
            throw new ArgumentNullException("Nenhum produto encontrado!");
        return produto;
    }

    public Produto? GetProduto(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(c => c.ProdutoId == id);
        if (produto is null)
            throw new ArgumentNullException("Produto não encontrado!");
        return produto;
    }

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutos(produtosParameters);
        if (!produtos.Any())
            throw new ArgumentNullException("Nenhum produto encontrado!");
        return produtos;
    }

    public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroparameters)
    {
        var produtos = _unitOfWork.ProdutoRepository.GetProdutosFiltroPreco(produtosFiltroparameters);
        if (!produtos.Any())
            throw new ArgumentNullException("Nenhum produto encontrado com os filtros informados!");
        return produtos;
    }

    public Produto Create(Produto produto)
    {
        if (produto is null)
            throw new ArgumentNullException(nameof(produto));
        var produtoCriado = _unitOfWork.ProdutoRepository.Create(produto);
        _unitOfWork.Commit();
        return produtoCriado;
    }

    public Produto Update(Produto produto)
    {
        if (produto is null)
            throw new ArgumentNullException(nameof(produto));
        var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
        _unitOfWork.Commit();
        return produtoAtualizado;
    }

    public Produto Delete(int id)
    {
        var produto = _unitOfWork.ProdutoRepository.Get(c => c.ProdutoId == id);
        if (produto is null)
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado");
        return _unitOfWork.ProdutoRepository.Delete(produto);
    }
}