using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniCrm.UI.Migrations
{
    /// <inheritdoc />
    public partial class RoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "Employees",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateDateTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoles", x => new { x.RoleId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_EmployeeRoles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Email", "FirstName", "LastName", "Password", "Salt", "UpdateDateTime" },
                values: new object[] { new Guid("5de9688a-1a13-479a-a20d-601be34ac659"), "ivanov@ya.com", "Иванов", "Иван", "ZoLHLS1KAX6f8UB6UcYDETRbxTf1Td7XlsD1QsWhToo=", new byte[] { 167, 36, 77, 102, 34, 152, 94, 33, 205, 134, 73, 160, 151, 120, 246, 95 }, null });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Name", "NormalizedName", "UpdateDateTime" },
                values: new object[,]
                {
                    { new Guid("2e6ec6b6-d90b-4cc2-b910-3579c480a8ef"), "project_manager", "менеджер проекта", null },
                    { new Guid("51abee2b-11eb-4e5f-99aa-27692b3717b3"), "employee", "сотрудник", null },
                    { new Guid("5e65bd9a-8161-44ad-8175-0aeebcc8a0d1"), "manager", "руководитель", null }
                });

            migrationBuilder.InsertData(
                table: "EmployeeRoles",
                columns: new[] { "EmployeeId", "RoleId" },
                values: new object[] { new Guid("5de9688a-1a13-479a-a20d-601be34ac659"), new Guid("5e65bd9a-8161-44ad-8175-0aeebcc8a0d1") });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoles_EmployeeId",
                table: "EmployeeRoles",
                column: "EmployeeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeRoles");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("5de9688a-1a13-479a-a20d-601be34ac659"));

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Employees");
        }
    }
}
