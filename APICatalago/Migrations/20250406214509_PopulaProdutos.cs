using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaProdutos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImgUrl, Estoque, DataCadastro, CategoriaId) VALUES ('Coca-Cola', 'Refrigerante de Cola', 5.00, 'coca-cola.jpg', 100, NOW(), 1)");
            mb.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImgUrl, Estoque, DataCadastro, CategoriaId) VALUES ('X-Burguer', 'Sanduíche com carne e queijo', 15.00, 'x-burguer.jpg', 50, NOW(), 2)");
            mb.Sql("INSERT INTO Produtos (Nome, Descricao, Preco, ImgUrl, Estoque, DataCadastro, CategoriaId) VALUES ('Pudim', 'Sobremesa de leite condensado', 8.00, 'pudim.jpg', 30, NOW(), 3)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Produtos where Nome in ('Coca-Cola', 'X-Burguer', 'Pudim')");
        }
    }
}