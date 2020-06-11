using Microsoft.EntityFrameworkCore.Migrations;

namespace _3Lab.Migrations
{
    public partial class UserRoleChangedV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanDeleteUsers",
                table: "UserRole",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanDeleteUsers",
                table: "UserRole");
        }
    }
}
