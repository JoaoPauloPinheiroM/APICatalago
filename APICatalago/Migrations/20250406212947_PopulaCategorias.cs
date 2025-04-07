using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalago.Migrations
{
    /// <inheritdoc />
    public partial class PopulaCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias (Nome, ImgUrl) Values('Bebidas','bebidas.jpg')");
            mb.Sql("Insert into Categorias (Nome, ImgUrl) Values('Lanches','Lanches.jpg')");
            mb.Sql("Insert into Categorias (Nome, ImgUrl) Values('Sobremesas','Sobremesas.jpg')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias where Nome in ('Bebidas','Lanches','Sobremesas')");
        }
    }
}