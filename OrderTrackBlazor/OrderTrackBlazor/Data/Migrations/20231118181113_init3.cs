using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderTrackOrders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackProductions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackProductions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackShops",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopType = table.Column<int>(type: "int", nullable: false),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackShops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackDispatchRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DispatchDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderTrackOrderId = table.Column<long>(type: "bigint", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackDispatchRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackDispatchRecords_OrderTrackOrders_OrderTrackOrderId",
                        column: x => x.OrderTrackOrderId,
                        principalTable: "OrderTrackOrders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackOrderItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderTrackOrderId = table.Column<long>(type: "bigint", nullable: true),
                    ProductionId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackOrderItems_OrderTrackOrders_OrderTrackOrderId",
                        column: x => x.OrderTrackOrderId,
                        principalTable: "OrderTrackOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderTrackOrderItems_OrderTrackProductions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "OrderTrackProductions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackPurchaseRecords",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShopId = table.Column<long>(type: "bigint", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackPurchaseRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackPurchaseRecords_OrderTrackShops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "OrderTrackShops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackDispatchItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseRecordId = table.Column<long>(type: "bigint", nullable: true),
                    DispatchRecordId = table.Column<long>(type: "bigint", nullable: true),
                    ProductionId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackDispatchItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackDispatchItems_OrderTrackDispatchRecords_DispatchRecordId",
                        column: x => x.DispatchRecordId,
                        principalTable: "OrderTrackDispatchRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderTrackDispatchItems_OrderTrackProductions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "OrderTrackProductions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackPurchaseItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseRecordId = table.Column<long>(type: "bigint", nullable: true),
                    ProductionId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackPurchaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackPurchaseItems_OrderTrackProductions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "OrderTrackProductions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderTrackPurchaseItems_OrderTrackPurchaseRecords_PurchaseRecordId",
                        column: x => x.PurchaseRecordId,
                        principalTable: "OrderTrackPurchaseRecords",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderTrackDispatchItems_DispatchRecordId",
                table: "OrderTrackDispatchItems",
                column: "DispatchRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTrackDispatchItems_ProductionId",
                table: "OrderTrackDispatchItems",
                column: "ProductionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTrackDispatchRecords_OrderTrackOrderId",
                table: "OrderTrackDispatchRecords",
                column: "OrderTrackOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTrackOrderItems_OrderTrackOrderId",
                table: "OrderTrackOrderItems",
                column: "OrderTrackOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTrackOrderItems_ProductionId",
                table: "OrderTrackOrderItems",
                column: "ProductionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTrackPurchaseItems_ProductionId",
                table: "OrderTrackPurchaseItems",
                column: "ProductionId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTrackPurchaseItems_PurchaseRecordId",
                table: "OrderTrackPurchaseItems",
                column: "PurchaseRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderTrackPurchaseRecords_ShopId",
                table: "OrderTrackPurchaseRecords",
                column: "ShopId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderTrackDispatchItems");

            migrationBuilder.DropTable(
                name: "OrderTrackOrderItems");

            migrationBuilder.DropTable(
                name: "OrderTrackPurchaseItems");

            migrationBuilder.DropTable(
                name: "OrderTrackDispatchRecords");

            migrationBuilder.DropTable(
                name: "OrderTrackProductions");

            migrationBuilder.DropTable(
                name: "OrderTrackPurchaseRecords");

            migrationBuilder.DropTable(
                name: "OrderTrackOrders");

            migrationBuilder.DropTable(
                name: "OrderTrackShops");
        }
    }
}
