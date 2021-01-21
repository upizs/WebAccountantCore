using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAccountantApp.Migrations
{
    public partial class ModifiedDataModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Failed attempt for a Database first approuch, tried to modify database, but eneded up removing too many migrations
            //therefor ruining my Context Model Snapshot. Will use SQL Server to modify the databse. 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
