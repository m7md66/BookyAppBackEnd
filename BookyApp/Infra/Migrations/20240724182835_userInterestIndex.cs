using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class userInterestIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserInterests_InterestId",
                table: "UserInterests");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("f9cad203-5770-44fe-8bcb-3a31416d4b19"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "c1bdf91b-de31-4454-a794-23678c1a7d69");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "c4dfeb75-123c-463e-b36e-de3fafb1d305", new DateTime(2024, 7, 24, 21, 28, 33, 529, DateTimeKind.Local).AddTicks(3424) });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("df57139b-1566-4fb6-935c-0f03d3c71555"), null, null, "Fantasy", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_UserInterests_InterestId_UserId",
                table: "UserInterests",
                columns: new[] { "InterestId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserInterests_InterestId_UserId",
                table: "UserInterests");

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("df57139b-1566-4fb6-935c-0f03d3c71555"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "ad26d9bd-559b-4d0d-b561-f7edda9bd101");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "83480938-6491-4b8a-a3f8-3ad64a2f566e", new DateTime(2024, 7, 24, 16, 5, 49, 690, DateTimeKind.Local).AddTicks(9435) });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name", "UpdatedBy", "UpdatedDate" },
                values: new object[] { new Guid("f9cad203-5770-44fe-8bcb-3a31416d4b19"), null, null, "Fantasy", null, null });

            migrationBuilder.CreateIndex(
                name: "IX_UserInterests_InterestId",
                table: "UserInterests",
                column: "InterestId");
        }
    }
}
