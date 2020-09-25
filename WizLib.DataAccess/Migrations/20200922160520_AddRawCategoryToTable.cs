using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib.DataAccess.Migrations
{
    public partial class AddRawCategoryToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO tbl_category values('Cat 1')");
            migrationBuilder.Sql($"INSERT INTO tbl_category values('Cat 2')");
            migrationBuilder.Sql($"INSERT INTO tbl_category values('Cat 3')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
