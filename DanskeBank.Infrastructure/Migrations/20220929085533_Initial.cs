using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DanskeBank.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "company");

            migrationBuilder.CreateTable(
                name: "companies",
                schema: "company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CompanyNumber = table.Column<string>(type: "nchar(10)", fixedLength: true, maxLength: 10, nullable: false),
                    CompanyType = table.Column<int>(type: "int", nullable: false),
                    Market = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "company",
                columns: table => new
                {
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SendingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => new { x.CompanyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Notification_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalSchema: "company",
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notification",
                schema: "company");

            migrationBuilder.DropTable(
                name: "companies",
                schema: "company");
        }
    }
}
