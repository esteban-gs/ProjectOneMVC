using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectOneMVC.Data.Migrations
{
    public partial class navPropsForManyToManyEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchoolUserId1",
                table: "UserClasses",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserClasses_SchoolUserId1",
                table: "UserClasses",
                column: "SchoolUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserClasses_SchoolUsers_SchoolUserId1",
                table: "UserClasses",
                column: "SchoolUserId1",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserClasses_SchoolUsers_SchoolUserId1",
                table: "UserClasses");

            migrationBuilder.DropIndex(
                name: "IX_UserClasses_SchoolUserId1",
                table: "UserClasses");

            migrationBuilder.DropColumn(
                name: "SchoolUserId1",
                table: "UserClasses");
        }
    }
}
