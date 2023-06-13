using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cas.SaaS.API.Migrations
{
    /// <inheritdoc />
    public partial class editContextBrigade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e450ce4e-d686-4cdf-9d35-89c09e767781"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Brigades",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Brigades",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Brigades",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("8b90832d-01a1-4e36-a68f-21b6b05610a9"), "User", "pmarkelo77@gmail.com", "pmarkelo77", "Павел", "pmarkelo77", null, "79887774433", null, null, 2, "Маркелов" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8b90832d-01a1-4e36-a68f-21b6b05610a9"));

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Brigades");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("e450ce4e-d686-4cdf-9d35-89c09e767781"), "User", "pmarkelo77@gmail.com", "pmarkelo77", "Павел", "pmarkelo77", null, "79887774433", null, null, 2, "Маркелов" });
        }
    }
}
