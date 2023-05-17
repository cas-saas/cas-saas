using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cas.SaaS.API.Migrations
{
    /// <inheritdoc />
    public partial class editHasData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("6c77bda2-37de-45a7-9324-502313b6595c"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("d53287d2-6df1-494b-ac0c-4a84435b8ab2"), "User", "pmarkelo77@gmail.com", "pmarkelo77", "Павел", "pmarkelo77", null, "79887774433", null, null, 2, "Маркелов" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d53287d2-6df1-494b-ac0c-4a84435b8ab2"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("6c77bda2-37de-45a7-9324-502313b6595c"), "User", "pmarkelo77@gmail.com", "", "Павел", "", null, "79887774433", null, null, 2, "Маркелов" });
        }
    }
}
