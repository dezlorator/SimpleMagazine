using Microsoft.EntityFrameworkCore.Migrations;

namespace PetStore.Migrations
{
    public partial class CommentsFixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ProductExtendeds_ProductExtendedID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Products_ProductID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductExtendeds_Products_ProductID",
                table: "ProductExtendeds");

            migrationBuilder.DropIndex(
                name: "IX_Comments_ProductID",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductExtendeds",
                table: "ProductExtendeds");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "ProductExtendeds",
                newName: "ProductsExtended");

            migrationBuilder.RenameIndex(
                name: "IX_ProductExtendeds_ProductID",
                table: "ProductsExtended",
                newName: "IX_ProductsExtended_ProductID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductsExtended",
                table: "ProductsExtended",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ProductsExtended_ProductExtendedID",
                table: "Comments",
                column: "ProductExtendedID",
                principalTable: "ProductsExtended",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsExtended_Products_ProductID",
                table: "ProductsExtended",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_ProductsExtended_ProductExtendedID",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsExtended_Products_ProductID",
                table: "ProductsExtended");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductsExtended",
                table: "ProductsExtended");

            migrationBuilder.RenameTable(
                name: "ProductsExtended",
                newName: "ProductExtendeds");

            migrationBuilder.RenameIndex(
                name: "IX_ProductsExtended_ProductID",
                table: "ProductExtendeds",
                newName: "IX_ProductExtendeds_ProductID");

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductExtendeds",
                table: "ProductExtendeds",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProductID",
                table: "Comments",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_ProductExtendeds_ProductExtendedID",
                table: "Comments",
                column: "ProductExtendedID",
                principalTable: "ProductExtendeds",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Products_ProductID",
                table: "Comments",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductExtendeds_Products_ProductID",
                table: "ProductExtendeds",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
