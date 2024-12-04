using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentTimeMangementApp.Migrations
{
    public partial class AddModuleTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersID = table.Column<int>(type: "int", nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    ModuleCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ModuleCredits = table.Column<int>(type: "int", nullable: false),
                    ModuleClassHrs = table.Column<int>(type: "int", nullable: false),
                    Weeks = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    SelfStudyHr = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Modules");
        }
    }
}
