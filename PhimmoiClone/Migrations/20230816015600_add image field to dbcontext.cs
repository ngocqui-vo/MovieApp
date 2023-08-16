using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PhimmoiClone.Migrations
{
    public partial class addimagefieldtodbcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Actors",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Actors");
        }
    }
}
