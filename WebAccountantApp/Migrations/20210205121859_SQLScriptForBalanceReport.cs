using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WebAccountantApp.Migrations
{
    public partial class SQLScriptForBalanceReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //had to change the date in database, becasue it recorded the current month as the reports period,
            //because of the old code. Now the code is recording last month as reports period. 
            var date = DateTime.Now.AddMonths(-1);

            migrationBuilder.Sql("UPDATE dbo.BalanceReports SET Date = '" + date +
                "' WHERE Date = '01-Feb-21 12:00:00 AM' ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
