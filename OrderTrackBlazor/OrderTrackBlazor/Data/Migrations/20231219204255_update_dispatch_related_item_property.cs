using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class update_dispatch_related_item_property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackStockDispatchPackageItems_OrderTrackOrderItems_OrderItemId",
                table: "OrderTrackStockDispatchPackageItems");

            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "OrderTrackStockDispatchPackageItems",
                newName: "ProductionId");

            

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "OrderTrackStockDispatchPackages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PackageSizeId",
                table: "OrderTrackStockDispatchPackages",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PackageWeight",
                table: "OrderTrackStockDispatchPackages",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackStockDispatchPackageItems_OrderTrackProductions_ProductionId",
                table: "OrderTrackStockDispatchPackageItems",
                column: "ProductionId",
                principalTable: "OrderTrackProductions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackStockDispatchPackages_OrderTrackPackageSizes_PackageSizeId",
                table: "OrderTrackStockDispatchPackages",
                column: "PackageSizeId",
                principalTable: "OrderTrackPackageSizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackStockDispatchPackageItems_OrderTrackProductions_ProductionId",
                table: "OrderTrackStockDispatchPackageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackStockDispatchPackages_OrderTrackPackageSizes_PackageSizeId",
                table: "OrderTrackStockDispatchPackages");


            migrationBuilder.DropColumn(
                name: "Number",
                table: "OrderTrackStockDispatchPackages");

            migrationBuilder.DropColumn(
                name: "PackageSizeId",
                table: "OrderTrackStockDispatchPackages");

            migrationBuilder.DropColumn(
                name: "PackageWeight",
                table: "OrderTrackStockDispatchPackages");

            migrationBuilder.RenameColumn(
                name: "ProductionId",
                table: "OrderTrackStockDispatchPackageItems",
                newName: "OrderItemId");


            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackStockDispatchPackageItems_OrderTrackOrderItems_OrderItemId",
                table: "OrderTrackStockDispatchPackageItems",
                column: "OrderItemId",
                principalTable: "OrderTrackOrderItems",
                principalColumn: "Id");
        }
    }
}
