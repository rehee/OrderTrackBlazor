using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_receipt_image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReceiptImage",
                table: "OrderTrackPurchaseRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiptImage",
                table: "OrderTrackPurchaseRecords");
        }
    }
}
