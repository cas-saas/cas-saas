using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cas.SaaS.API.Migrations
{
    /// <inheritdoc />
    public partial class addHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("6c77bda2-37de-45a7-9324-502313b6595c"), "User", "pmarkelo77@gmail.com", "", "Павел", "", null, "79887774433", null, null, 2, "Маркелов" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6c77bda2-37de-45a7-9324-502313b6595c"));
        }
    }
}
