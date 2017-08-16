using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace bd.swLogProyect.datos.Migrations
{
    public partial class migrar_proyecto_servicios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogCategories",
                columns: table => new
                {
                    LogCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(maxLength: 1024, nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    ParameterValue = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogCategories", x => x.LogCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "LogLevels",
                columns: table => new
                {
                    LogLevelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Code = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    ShortName = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogLevels", x => x.LogLevelId);
                });

            migrationBuilder.CreateTable(
                name: "LogEntries",
                columns: table => new
                {
                    LogEntryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApplicationName = table.Column<string>(maxLength: 1024, nullable: false),
                    ExceptionTrace = table.Column<string>(maxLength: 4096, nullable: true),
                    LogCategoryId = table.Column<int>(nullable: false),
                    LogDate = table.Column<DateTime>(nullable: false),
                    LogLevelId = table.Column<int>(nullable: false),
                    MachineIP = table.Column<string>(maxLength: 1024, nullable: false),
                    MachineName = table.Column<string>(maxLength: 1024, nullable: false),
                    Message = table.Column<string>(maxLength: 1024, nullable: false),
                    ObjEntityId = table.Column<string>(maxLength: 1024, nullable: false),
                    UserName = table.Column<string>(maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.LogEntryId);
                    table.ForeignKey(
                        name: "FK_LogEntries_LogCategories_LogCategoryId",
                        column: x => x.LogCategoryId,
                        principalTable: "LogCategories",
                        principalColumn: "LogCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LogEntries_LogLevels_LogLevelId",
                        column: x => x.LogLevelId,
                        principalTable: "LogLevels",
                        principalColumn: "LogLevelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_LogCategoryId",
                table: "LogEntries",
                column: "LogCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_LogLevelId",
                table: "LogEntries",
                column: "LogLevelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogEntries");

            migrationBuilder.DropTable(
                name: "LogCategories");

            migrationBuilder.DropTable(
                name: "LogLevels");
        }
    }
}
