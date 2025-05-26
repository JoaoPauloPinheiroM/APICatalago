using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;
using X.PagedList;

namespace APICatalago.Services
{
    public class CategoriaServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasAsync()
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetAllAsync();
            if (!categorias.Any())
                throw new ArgumentNullException("Nenhuma categoria encontrada!");
            return categorias;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriaParameters categoriaParameters)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasAsync(categoriaParameters);
            if (!categorias.Any())
                throw new ArgumentNullException("Nenhuma categoria encontrada!");
            return categorias;
        }

        public async Task<Categoria> GetCategoriaAsync(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            if (categoria is null)
                throw new ArgumentNullException("Categoria não encontrada!");
            return categoria;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriasFiltroNome categoriasParams)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriasParams);
            if (!categorias.Any())
                throw new ArgumentNullException("Nenhuma categoria encontrada!");
            return categorias;
        }

        public async Task<Categoria> CreateAsync(Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));
            var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
            await _unitOfWork.CommitAsync();
            return categoriaCriada;
        }

        public async Task<Categoria> UpdateAsync(Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));
            var resultado = _unitOfWork.CategoriaRepository.Update(categoria);
            await _unitOfWork.CommitAsync();
            return resultado;
        }

        public async Task<Categoria> DeleteAsync(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));
            var resultado = _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.CommitAsync();
            return resultado;
        }
    }
}