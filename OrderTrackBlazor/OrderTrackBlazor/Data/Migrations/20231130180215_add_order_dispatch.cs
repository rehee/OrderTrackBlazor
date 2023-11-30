using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_order_dispatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderProductionId",
                table: "OrderTrackDispatchItems",
                type: "bigint",
                nullable: true);

            

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackDispatchItems_OrderTrackOrderItems_OrderProductionId",
                table: "OrderTrackDispatchItems",
                column: "OrderProductionId",
                principalTable: "OrderTrackOrderItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackDispatchItems_OrderTrackOrderItems_OrderProductionId",
                table: "OrderTrackDispatchItems");

            

            migrationBuilder.DropColumn(
                name: "OrderProductionId",
                table: "OrderTrackDispatchItems");
        }
    }
}
