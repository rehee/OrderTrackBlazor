using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_package_related_entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PackageQuantity",
                table: "OrderTrackDispatchItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderTrackPackageSizes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackPackageSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackPackages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    SizeId = table.Column<long>(type: "bigint", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BriefDiscribtion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discribtion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Confirmed = table.Column<bool>(type: "bit", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackPackages_OrderTrackOrders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "OrderTrackOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderTrackPackages_OrderTrackPackageSizes_SizeId",
                        column: x => x.SizeId,
                        principalTable: "OrderTrackPackageSizes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackDispatchPackages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageId = table.Column<long>(type: "bigint", nullable: true),
                    RecordId = table.Column<long>(type: "bigint", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackDispatchPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackDispatchPackages_OrderTrackDispatchRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "OrderTrackDispatchRecords",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderTrackDispatchPackages_OrderTrackPackages_PackageId",
                        column: x => x.PackageId,
                        principalTable: "OrderTrackPackages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderTrackPackageItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageId = table.Column<long>(type: "bigint", nullable: true),
                    ProductionId = table.Column<long>(type: "bigint", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    OrderTrackPackageId = table.Column<long>(type: "bigint", nullable: true),
                    TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderTrackPackageItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderTrackPackageItems_OrderTrackPackageItems_PackageId",
                        column: x => x.PackageId,
                        principalTable: "OrderTrackPackageItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderTrackPackageItems_OrderTrackPackages_OrderTrackPackageId",
                        column: x => x.OrderTrackPackageId,
                        principalTable: "OrderTrackPackages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderTrackPackageItems_OrderTrackProductions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "OrderTrackProductions",
                        principalColumn: "Id");
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderTrackDispatchPackages");

            migrationBuilder.DropTable(
                name: "OrderTrackPackageItems");

            migrationBuilder.DropTable(
                name: "OrderTrackPackages");

            migrationBuilder.DropTable(
                name: "OrderTrackPackageSizes");

            migrationBuilder.DropColumn(
                name: "PackageQuantity",
                table: "OrderTrackDispatchItems");
        }
    }
}
