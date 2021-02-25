using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFApp.Migrations
{
    public partial class Test : Migration
    {
protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.CreateTable(
        name: "cart",
        columns: table => new
        {
            id = table.Column<Guid>(type: "uuid", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("pk_cart", x => x.id);
        });

    migrationBuilder.CreateTable(
        name: "CartItem",
        columns: table => new
        {
            item_id = table.Column<Guid>(type: "uuid", nullable: false),
            cart_id = table.Column<Guid>(type: "uuid", nullable: false),
            quantity = table.Column<int>(type: "integer", nullable: false)
        },
        constraints: table =>
        {
            table.PrimaryKey("pk_cart_item", x => new { x.cart_id, x.item_id });
            table.ForeignKey(
                name: "fk_cart_item_cart_cart_id",
                column: x => x.cart_id,
                principalTable: "cart",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        });
}

protected override void Down(MigrationBuilder migrationBuilder)
{
    migrationBuilder.DropTable(
        name: "CartItem");

    migrationBuilder.DropTable(
        name: "cart");
}
    }
}
