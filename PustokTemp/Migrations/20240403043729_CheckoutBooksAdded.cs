using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PustokTemp.Migrations
{
    /// <inheritdoc />
    public partial class CheckoutBooksAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasketItems_Checkouts_CheckoutId",
                table: "BasketItems");

            migrationBuilder.DropIndex(
                name: "IX_BasketItems_CheckoutId",
                table: "BasketItems");

            migrationBuilder.DropColumn(
                name: "CheckoutId",
                table: "BasketItems");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Checkouts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "CheckoutBook",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CheckoutId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    IsDeactive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutBook", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CheckoutBook_Checkouts_CheckoutId",
                        column: x => x.CheckoutId,
                        principalTable: "Checkouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutBook_CheckoutId",
                table: "CheckoutBook",
                column: "CheckoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckoutBook");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Checkouts");

            migrationBuilder.AddColumn<int>(
                name: "CheckoutId",
                table: "BasketItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasketItems_CheckoutId",
                table: "BasketItems",
                column: "CheckoutId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasketItems_Checkouts_CheckoutId",
                table: "BasketItems",
                column: "CheckoutId",
                principalTable: "Checkouts",
                principalColumn: "Id");
        }
    }
}
