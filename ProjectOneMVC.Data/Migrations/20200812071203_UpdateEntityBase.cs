using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectOneMVC.Data.Migrations
{
    public partial class UpdateEntityBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserClasses",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserClasses");
        }
    }
}
