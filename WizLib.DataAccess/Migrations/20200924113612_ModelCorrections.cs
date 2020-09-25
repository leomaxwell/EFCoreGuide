using Microsoft.EntityFrameworkCore.Migrations;

namespace WizLib.DataAccess.Migrations
{
    public partial class ModelCorrections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Publisher_id",
                table: "Publishers",
                newName: "Publisher_Id");

            migrationBuilder.RenameColumn(
                name: "Birthdate",
                table: "Authors",
                newName: "BirthDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Publisher_Id",
                table: "Publishers",
                newName: "Publisher_id");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                table: "Authors",
                newName: "Birthdate");
        }
    }
}
