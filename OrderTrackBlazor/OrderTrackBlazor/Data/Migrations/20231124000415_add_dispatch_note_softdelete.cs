using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_dispatch_note_softdelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "IncomeDate",
                table: "OrderTrackDispatchRecords",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OrderTrackDispatchRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SoftDeleteUntil",
                table: "OrderTrackDispatchRecords",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncomeDate",
                table: "OrderTrackDispatchRecords");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OrderTrackDispatchRecords");

            migrationBuilder.DropColumn(
                name: "SoftDeleteUntil",
                table: "OrderTrackDispatchRecords");
        }
    }
}
