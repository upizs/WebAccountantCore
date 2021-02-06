using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WebAccountantApp.Migrations
{
    public partial class ChangeDateInBalanceReportSQL : Migration
    {
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var date = DateTime.Now.AddMonths(-1);

            migrationBuilder.Sql("UPDATE dbo.BalanceReports SET Date = '" + date +
                "' WHERE Date = '01-Feb-21 12:00:00 AM' ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
