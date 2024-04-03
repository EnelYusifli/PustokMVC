using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PustokTemp.Migrations
{
    /// <inheritdoc />
    public partial class CheckoutBooksTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutBook_Checkouts_CheckoutId",
                table: "CheckoutBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckoutBook",
                table: "CheckoutBook");

            migrationBuilder.RenameTable(
                name: "CheckoutBook",
                newName: "CheckoutBooks");

            migrationBuilder.RenameIndex(
                name: "IX_CheckoutBook_CheckoutId",
                table: "CheckoutBooks",
                newName: "IX_CheckoutBooks_CheckoutId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckoutBooks",
                table: "CheckoutBooks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutBooks_Checkouts_CheckoutId",
                table: "CheckoutBooks",
                column: "CheckoutId",
                principalTable: "Checkouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutBooks_Checkouts_CheckoutId",
                table: "CheckoutBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CheckoutBooks",
                table: "CheckoutBooks");

            migrationBuilder.RenameTable(
                name: "CheckoutBooks",
                newName: "CheckoutBook");

            migrationBuilder.RenameIndex(
                name: "IX_CheckoutBooks_CheckoutId",
                table: "CheckoutBook",
                newName: "IX_CheckoutBook_CheckoutId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CheckoutBook",
                table: "CheckoutBook",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutBook_Checkouts_CheckoutId",
                table: "CheckoutBook",
                column: "CheckoutId",
                principalTable: "Checkouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
