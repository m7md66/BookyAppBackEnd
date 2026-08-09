using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class upfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("e0765cba-c1d7-4815-8070-1a017712b465"));

            migrationBuilder.AddColumn<string>(
                name: "ContentFileUrl",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "b441045a-fc0d-4e30-b9a6-acf913ca7a6f");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "e557439e-5a33-4188-983b-4e2a7c8b419b", new DateTime(2026, 8, 1, 14, 38, 23, 844, DateTimeKind.Local).AddTicks(784) });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("b6718dca-50d1-461b-a7fe-ea4660c3d1ea"), null, null, "Fantasy", null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("b6718dca-50d1-461b-a7fe-ea4660c3d1ea"));

            migrationBuilder.DropColumn(
                name: "ContentFileUrl",
                table: "Books");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "b3e3c643-30db-4b19-9c1a-43091f0b3682");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "faf8ea17-b7e1-4f6f-b9c8-2793b16f22e1", new DateTime(2026, 8, 1, 11, 59, 57, 32, DateTimeKind.Local).AddTicks(9617) });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("e0765cba-c1d7-4815-8070-1a017712b465"), null, null, "Fantasy", null, null });
        }
    }
}
