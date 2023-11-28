using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_purcher_orderId_and_totalPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "OrderTrackPurchaseRecords",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "OrderTrackPurchaseRecords",
                type: "decimal(18,2)",
                nullable: true);

            

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackPurchaseRecords_OrderTrackOrders_OrderId",
                table: "OrderTrackPurchaseRecords",
                column: "OrderId",
                principalTable: "OrderTrackOrders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackPurchaseRecords_OrderTrackOrders_OrderId",
                table: "OrderTrackPurchaseRecords");

            

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderTrackPurchaseRecords");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderTrackPurchaseRecords");
        }
    }
}
