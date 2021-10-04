using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace api.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "papaya");

            migrationBuilder.CreateTable(
                name: "insurance",
                schema: "papaya",
                columns: table => new
                {
                    claim_case_id = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: true),
                    hospital_name = table.Column<string>(type: "text", nullable: true),
                    admitted_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    discharged_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    has_fraud = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance", x => x.claim_case_id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_insurance_claim_case_id",
                schema: "papaya",
                table: "insurance",
                column: "claim_case_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_insurance_user_name",
                schema: "papaya",
                table: "insurance",
                column: "user_name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "insurance",
                schema: "papaya");
        }
    }
}
