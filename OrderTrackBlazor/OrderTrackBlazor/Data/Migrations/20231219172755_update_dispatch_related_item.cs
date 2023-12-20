using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class update_dispatch_related_item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackDispatchItems_OrderTrackStockDispatchRecords_StockDispatchRecordId",
                table: "OrderTrackDispatchItems");

            migrationBuilder.DropTable(
                name: "OrderTrackStockDispatchRecords");

            migrationBuilder.RenameColumn(
                name: "StockDispatchRecordId",
                table: "OrderTrackDispatchItems",
                newName: "OrderTrackStockDispatchPackageId");

            

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackDispatchItems_OrderTrackStockDispatchPackages_OrderTrackStockDispatchPackageId",
                table: "OrderTrackDispatchItems",
                column: "OrderTrackStockDispatchPackageId",
                principalTable: "OrderTrackStockDispatchPackages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackDispatchItems_OrderTrackStockDispatchPackages_OrderTrackStockDispatchPackageId",
                table: "OrderTrackDispatchItems");

            migrationBuilder.RenameColumn(
                name: "OrderTrackStockDispatchPackageId",
                table: "OrderTrackDispatchItems",
                newName: "StockDispatchRecordId");

            

            migrationBuilder.CreateTable(
                name: "OrderTrackStockDispatchRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageId = table.Column<long>(type: "bigint", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackStockDispatchRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackStockDispatchRecords_OrderTrackStockDispatchPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "OrderTrackStockDispatchPackages",
                        principalColumn: "Id");
                });

           

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackDispatchItems_OrderTrackStockDispatchRecords_StockDispatchRecordId",
                table: "OrderTrackDispatchItems",
                column: "StockDispatchRecordId",
                principalTable: "OrderTrackStockDispatchRecords",
                principalColumn: "Id");
        }
    }
}
