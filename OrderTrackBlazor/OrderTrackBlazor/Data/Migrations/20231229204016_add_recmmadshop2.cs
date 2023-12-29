using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_recmmadshop2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RecommendShopId2",
                table: "OrderTrackOrderItems",
                type: "bigint",
                nullable: true);

            

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackOrderItems_OrderTrackShops_RecommendShopId2",
                table: "OrderTrackOrderItems",
                column: "RecommendShopId2",
                principalTable: "OrderTrackShops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackOrderItems_OrderTrackShops_RecommendShopId2",
                table: "OrderTrackOrderItems");

            

            migrationBuilder.DropColumn(
                name: "RecommendShopId2",
                table: "OrderTrackOrderItems");
        }
    }
}
