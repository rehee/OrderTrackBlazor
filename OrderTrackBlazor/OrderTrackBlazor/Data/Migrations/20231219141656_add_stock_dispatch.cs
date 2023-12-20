using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_stock_dispatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "StockDispatchRecordId",
                table: "OrderTrackDispatchItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderTrackStockDispatchs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DispatchDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Income = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackStockDispatchs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackStockDispatchPackages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderTrackStockDispatchId = table.Column<long>(type: "bigint", nullable: true),
                    BriefDiscribtion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discribtion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Confirmed = table.Column<bool>(type: "bit", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackStockDispatchPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackStockDispatchPackages_OrderTrackStockDispatchs_OrderTrackStockDispatchId",
                        column: x => x.OrderTrackStockDispatchId,
                        principalTable: "OrderTrackStockDispatchs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackStockDispatchPackageItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageId = table.Column<long>(type: "bigint", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    OrderItemId = table.Column<long>(type: "bigint", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackStockDispatchPackageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackStockDispatchPackageItems_OrderTrackOrderItems_OrderItemId",
                        column: x => x.OrderItemId,
                        principalTable: "OrderTrackOrderItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderTrackStockDispatchPackageItems_OrderTrackStockDispatchPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "OrderTrackStockDispatchPackages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackStockDispatchRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageId = table.Column<long>(type: "bigint", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackDispatchItems_OrderTrackStockDispatchRecords_StockDispatchRecordId",
                table: "OrderTrackDispatchItems");

            migrationBuilder.DropTable(
                name: "OrderTrackStockDispatchPackageItems");

            migrationBuilder.DropTable(
                name: "OrderTrackStockDispatchRecords");

            migrationBuilder.DropTable(
                name: "OrderTrackStockDispatchPackages");

            migrationBuilder.DropTable(
                name: "OrderTrackStockDispatchs");

            migrationBuilder.DropColumn(
                name: "StockDispatchRecordId",
                table: "OrderTrackDispatchItems");
        }
    }
}
