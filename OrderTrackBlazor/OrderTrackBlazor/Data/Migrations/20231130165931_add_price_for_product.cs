using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_price_for_product : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "OrderTrackProductions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OrderPrice",
                table: "OrderTrackOrderItems",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DispatchPrice",
                table: "OrderTrackDispatchItems",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "OrderTrackProductions");

            migrationBuilder.DropColumn(
                name: "OrderPrice",
                table: "OrderTrackOrderItems");

            migrationBuilder.DropColumn(
                name: "DispatchPrice",
                table: "OrderTrackDispatchItems");
        }
    }
}
