using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace NBG.Visitor.Storage.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "visitor",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visitor", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "visit",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    planned_visit_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    visit_start = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    visit_end = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    visitor_id = table.Column<int>(type: "integer", nullable: false),
                    contact_person = table.Column<string>(type: "text", nullable: true),
                    company_label = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    uuid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visit", x => x.id);
                    table.ForeignKey(
                        name: "FK_visit_visitor_visitor_id",
                        column: x => x.visitor_id,
                        principalTable: "visitor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_visit_visitor_id",
                table: "visit",
                column: "visitor_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "visit");

            migrationBuilder.DropTable(
                name: "visitor");
        }
    }
}
