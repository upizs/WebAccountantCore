using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAccountantApp.Migrations
{
    public partial class ChangedDateKeeperName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DateKeeper",
                table: "DateKeeper");

            migrationBuilder.RenameTable(
                name: "DateKeeper",
                newName: "DateKeeper");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DateKeeper",
                table: "DateKeeper",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DateKeeper",
                table: "DateKeeper");

            migrationBuilder.RenameTable(
                name: "DateKeeper",
                newName: "DateKeeper");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DateKeeper",
                table: "DateKeeper",
                column: "Id");
        }
    }
}
