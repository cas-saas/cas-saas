using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cas.SaaS.API.Migrations
{
    /// <inheritdoc />
    public partial class editContextModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("12ef437c-5b99-4302-96d7-e2fb3b9a40e3"));

            migrationBuilder.AddColumn<string>(
                name: "NumberDelivery",
                table: "Deliveries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumberBrigade",
                table: "Brigades",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumberApplication",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("e450ce4e-d686-4cdf-9d35-89c09e767781"), "User", "pmarkelo77@gmail.com", "pmarkelo77", "Павел", "pmarkelo77", null, "79887774433", null, null, 2, "Маркелов" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("e450ce4e-d686-4cdf-9d35-89c09e767781"));

            migrationBuilder.DropColumn(
                name: "NumberDelivery",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "NumberBrigade",
                table: "Brigades");

            migrationBuilder.DropColumn(
                name: "NumberApplication",
                table: "Applications");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("12ef437c-5b99-4302-96d7-e2fb3b9a40e3"), "User", "pmarkelo77@gmail.com", "pmarkelo77", "Павел", "pmarkelo77", null, "79887774433", null, null, 2, "Маркелов" });
        }
    }
}
