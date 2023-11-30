using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_order_package : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PackageNumber",
                table: "OrderTrackDispatchRecords",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PackageNumber",
                table: "OrderTrackDispatchRecords");
        }
    }
}
