using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Cas.SaaS.API.Migrations
{
    /// <inheritdoc />
    public partial class editContextData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("833516c2-5627-4079-ab8e-9daee38ed574"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("b980e4ea-4a3a-4ca8-a82c-a2dcc83444da"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("93847db3-96bc-4788-9929-def3072f3354"), "User", "pmarkelo77@gmail.com", "pmarkelo77", "Павел", "pmarkelo77", null, "79887774433", null, null, 2, "Маркелов" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("93847db3-96bc-4788-9929-def3072f3354"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[,]
                {
                    { new Guid("833516c2-5627-4079-ab8e-9daee38ed574"), "User", "pmarkelo77@gmail.com", "client", "Павел", "client", null, "79887774433", null, null, 0, "Маркелов" },
                    { new Guid("b980e4ea-4a3a-4ca8-a82c-a2dcc83444da"), "User", "pmarkelo77@gmail.com", "pmarkelo77", "Павел", "pmarkelo77", null, "79887774433", null, null, 2, "Маркелов" }
                });
        }
    }
}
