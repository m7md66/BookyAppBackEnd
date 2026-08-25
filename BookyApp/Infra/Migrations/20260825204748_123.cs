using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class _123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "1fd7392a-aa81-48c5-89cb-273cfd49ea78");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "e06d0a3b-18a5-40d0-8fcf-1aaad79d52a6", new DateTime(2026, 8, 25, 23, 47, 45, 784, DateTimeKind.Local).AddTicks(7873) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "4dd9f99f-c9dd-43a7-b918-ce374bace673");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "401db0bc-dbfe-4f58-b1e5-2c834ae7fcbc", new DateTime(2026, 8, 23, 16, 30, 3, 999, DateTimeKind.Local).AddTicks(3518) });
        }
    }
}
