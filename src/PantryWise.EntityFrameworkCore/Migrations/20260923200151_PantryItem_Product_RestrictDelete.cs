using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PantryWise.Migrations
{
    /// <inheritdoc />
    public partial class PantryItem_Product_RestrictDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPantryItems_AppProducts_ProductId",
                table: "AppPantryItems");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPantryItems_AppProducts_ProductId",
                table: "AppPantryItems",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppPantryItems_AppProducts_ProductId",
                table: "AppPantryItems");

            migrationBuilder.AddForeignKey(
                name: "FK_AppPantryItems_AppProducts_ProductId",
                table: "AppPantryItems",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
