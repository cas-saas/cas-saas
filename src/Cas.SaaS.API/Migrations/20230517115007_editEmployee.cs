using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cas.SaaS.API.Migrations
{
    /// <inheritdoc />
    public partial class editEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("93847db3-96bc-4788-9929-def3072f3354"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("5e6008ef-3d4e-4871-a608-374573869f6d"), "User", "pmarkelo77@gmail.com", "pmarkelo77", "Павел", "pmarkelo77", null, "79887774433", null, null, 2, "Маркелов" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5e6008ef-3d4e-4871-a608-374573869f6d"));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Discriminator", "Email", "Login", "Name", "Password", "Patronymic", "Phone", "RefreshToken", "RefreshTokenExpires", "Role", "Surname" },
                values: new object[] { new Guid("93847db3-96bc-4788-9929-def3072f3354"), "User", "pmarkelo77@gmail.com", "pmarkelo77", "Павел", "pmarkelo77", null, "79887774433", null, null, 2, "Маркелов" });
        }
    }
}
