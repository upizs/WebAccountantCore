using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAccountantApp.Migrations
{
    public partial class ChangedValuesToDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //changed values from double to decimal because double was adding more decimal points
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "Transactions",
                type: "decimal(20,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "BalanceReports",
                type: "decimal(20,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Transactions",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,2)");

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "BalanceReports",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "Value",
                table: "Accounts",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
