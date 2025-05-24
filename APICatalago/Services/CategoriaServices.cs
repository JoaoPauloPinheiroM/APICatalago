using APICatalago.Models;
using APICatalago.Pagination;
using APICatalago.Repositories.Interfaces;

namespace APICatalago.Services
{
    public class CategoriaServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriaServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            var categorias = _unitOfWork.CategoriaRepository.GetAll();
            if (!categorias.Any())
                throw new ArgumentNullException("Nenhuma categoria encontrada!");
            return categorias;
        }

        public PagedList<Categoria> GetCategorias(CategoriaParameters categoriaParameters)
        {
            var categorias = _unitOfWork.CategoriaRepository.GetCategorias(categoriaParameters);
            if (!categorias.Any())
                throw new ArgumentNullException("Nenhuma categoria encontrada!");
            return categorias;
        }

        public Categoria GetCategoria(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
                throw new ArgumentNullException("Categoria não encontrada!");
            return categoria;
        }

        public PagedList<Categoria> GetCategoriasFiltroNome(CategoriasFiltroNome categoriasParams)
        {
            var categorias = _unitOfWork.CategoriaRepository.GetCategoriasFiltroNome(categoriasParams);
            if (!categorias.Any())
                throw new ArgumentNullException("Nenhuma categoria encontrada!");
            return categorias;
        }

        public Categoria Create(Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));
            var categortiaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
            _unitOfWork.Commit();
            return categortiaCriada;
        }

        public Categoria Update(Categoria categoria)
        {
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));
            return _unitOfWork.CategoriaRepository.Update(categoria);
        }

        public Categoria Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
                throw new ArgumentNullException(nameof(categoria));
            return _unitOfWork.CategoriaRepository.Delete(categoria);
        }
    }
}