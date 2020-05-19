using Microsoft.EntityFrameworkCore.Migrations;

namespace PetStore.Migrations
{
    public partial class ExtendedProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ProductExtendeds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "ProductExtendeds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Manufacturer",
                table: "ProductExtendeds",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginCountry",
                table: "ProductExtendeds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ProductExtendeds");

            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "ProductExtendeds");

            migrationBuilder.DropColumn(
                name: "Manufacturer",
                table: "ProductExtendeds");

            migrationBuilder.DropColumn(
                name: "OriginCountry",
                table: "ProductExtendeds");
        }
    }
}
