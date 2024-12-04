using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentTimeMangementApp.Migrations
{
    public partial class AddModuleHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DisplayValues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsersID = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ModuleName = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedSelfStudyHr = table.Column<int>(type: "int", nullable: false),
                    CaptureHrs = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DisplayValues", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DisplayValues");
        }
    }
}
