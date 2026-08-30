using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class _123localization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Genres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1",
                column: "ConcurrencyStamp",
                value: "e39f1d86-dff7-49ec-8d7d-a2939c657efb");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedDate" },
                values: new object[] { "cc0852f6-987d-41b5-8249-5913aac42834", new DateTime(2026, 8, 28, 15, 32, 30, 353, DateTimeKind.Local).AddTicks(2870) });

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("10c7fa4b-31c5-4004-83b8-a51ef4a8c462"),
                column: "NameAr",
                value: "علم نفس");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("12284e84-3f74-4f66-89db-1f31afa378cf"),
                column: "NameAr",
                value: "أعمال");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("419727ee-9e34-42da-a231-fcf748a1d0a1"),
                column: "NameAr",
                value: "سيرة ومذكرات");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d7-dafd-49fd-b423-16defbb56726"),
                column: "NameAr",
                value: "روايات معاصرة");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d9-dafd-49fd-b423-16defbb56720"),
                column: "NameAr",
                value: "ديستوبيا");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d9-dafd-49fd-b423-16defbb56721"),
                column: "NameAr",
                value: "أكشن ومغامرة");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d9-dafd-49fd-b423-16defbb56722"),
                column: "NameAr",
                value: "غموض");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d9-dafd-49fd-b423-16defbb56723"),
                column: "NameAr",
                value: "رعب");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d9-dafd-49fd-b423-16defbb56724"),
                column: "NameAr",
                value: "إثارة وتشويق");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d9-dafd-49fd-b423-16defbb56725"),
                column: "NameAr",
                value: "خيال تاريخي");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d9-dafd-49fd-b423-16defbb56726"),
                column: "NameAr",
                value: "رومانسي");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d9-dafd-49fd-b423-16defbb56736"),
                column: "NameAr",
                value: "خيال علمي");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a41d9-dafd-49fd-b423-16defbb56746"),
                column: "NameAr",
                value: "فانتازيا");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a43d7-dafd-49fd-b423-16defbb56726"),
                column: "NameAr",
                value: "خيال أدبي");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("4e5a44d7-dafd-49fd-b423-16defbb56726"),
                column: "NameAr",
                value: "مجتمع الميم");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("5df1e3a4-b736-4987-87eb-ea7474f81016"),
                column: "NameAr",
                value: "شعر");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("71f9c4aa-1c28-4508-a98b-8c77cd0480ed"),
                column: "NameAr",
                value: "تطوير الذات");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("925403f3-0ad5-455c-9962-6e63572c9cfd"),
                column: "NameAr",
                value: "يافعين");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("963b0835-e87a-44ed-a9b4-f9488e481a96"),
                column: "NameAr",
                value: "جريمة");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("9cbe915e-6092-4f04-8055-bdc787b6ba5d"),
                column: "NameAr",
                value: "فلسفة");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("aef82253-4f0c-4fda-96f0-0ce57c48ab68"),
                column: "NameAr",
                value: "علوم وطبيعة");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ba0ffbb1-0509-4e1c-8d40-15c605b37832"),
                column: "NameAr",
                value: "تاريخ");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ce3b2e9d-e813-4318-aa97-bf4542e7e3d4"),
                column: "NameAr",
                value: "روايات مصوّرة وكوميكس");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("df685395-72d2-4156-83d5-5307b1fc5bf9"),
                column: "NameAr",
                value: "كلاسيكيات");

            migrationBuilder.UpdateData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: new Guid("ed620332-d5bf-4a42-8036-135bf49da63f"),
                column: "NameAr",
                value: "دراما");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Genres");

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
    }
}
