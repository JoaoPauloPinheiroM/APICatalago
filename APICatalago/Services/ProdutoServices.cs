using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using X.PagedList;

namespace APICatalago.Services;

public class ProdutoServices
{
    private readonly IUnitOfWork _unitOfWork;

    public ProdutoServices(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
    {
        return await _unitOfWork.ProdutoRepository.GetProdutosPorCategoriaAsync(id);
    }

    public async Task<IEnumerable<Produto>> GetProdutosAsync()
    {
        var produto = await _unitOfWork.ProdutoRepository.GetAllAsync();
        if (!produto.Any())
            throw new ArgumentNullException("Nenhum produto encontrado!");
        return produto;
    }

    public async Task<Produto?> GetProduto(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetAsync(c => c.ProdutoId == id);
        if (produto is null)
            throw new ArgumentNullException("Produto não encontrado!");
        return produto;
    }

    public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetProdutosAsync(produtosParameters);
        if (!produtos.Any())
            throw new ArgumentNullException("Nenhum produto encontrado!");
        return produtos;
    }

    public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroparameters)
    {
        var produtos = await _unitOfWork.ProdutoRepository.GetProdutosFiltroPrecoAsync(produtosFiltroparameters);
        if (!produtos.Any())
            throw new ArgumentNullException("Nenhum produto encontrado com os filtros informados!");
        return produtos;
    }

    public async Task<Produto> CreateAsync(Produto produto)
    {
        if (produto is null)
            throw new ArgumentNullException(nameof(produto));
        var produtoCriado = _unitOfWork.ProdutoRepository.Create(produto);
        await _unitOfWork.CommitAsync();
        return produtoCriado;
    }

    public async Task<Produto> UpdateAsync(Produto produto)
    {
        if (produto is null)
            throw new ArgumentNullException(nameof(produto));
        var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
        await _unitOfWork.CommitAsync();
        return produtoAtualizado;
    }

    public async Task<Produto> DeleteAsync(int id)
    {
        var produto = await _unitOfWork.ProdutoRepository.GetAsync(c => c.ProdutoId == id);
        if (produto is null)
            throw new KeyNotFoundException($"Produto com ID {id} não encontrado");
        return _unitOfWork.ProdutoRepository.Delete(produto);
    }
}