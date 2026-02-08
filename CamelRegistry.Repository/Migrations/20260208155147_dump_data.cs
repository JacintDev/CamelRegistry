using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CamelRegistry.Repository.Migrations
{
    /// <inheritdoc />
    public partial class dump_data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Camels",
                columns: new[] { "Id", "Color", "HumpCount", "LastFed", "Name" },
                values: new object[,]
                {
                    { new Guid("43004233-b9bf-487c-ba55-37b8ac938392"), "Brown", 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Default Camel2" },
                    { new Guid("69e78efa-c4dd-4379-bfa3-65a7db97996f"), "Brown", 2, new DateTime(2026, 2, 8, 16, 51, 46, 653, DateTimeKind.Local).AddTicks(9135), "Default Camel" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Camels",
                keyColumn: "Id",
                keyValue: new Guid("43004233-b9bf-487c-ba55-37b8ac938392"));

            migrationBuilder.DeleteData(
                table: "Camels",
                keyColumn: "Id",
                keyValue: new Guid("69e78efa-c4dd-4379-bfa3-65a7db97996f"));
        }
    }
}
