using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class add_package_related_entity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackPackageItems_OrderTrackPackageItems_PackageId",
                table: "OrderTrackPackageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackPackageItems_OrderTrackPackages_OrderTrackPackageId",
                table: "OrderTrackPackageItems");

            

            migrationBuilder.DropColumn(
                name: "OrderTrackPackageId",
                table: "OrderTrackPackageItems");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackPackageItems_OrderTrackPackages_PackageId",
                table: "OrderTrackPackageItems",
                column: "PackageId",
                principalTable: "OrderTrackPackages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackPackageItems_OrderTrackPackages_PackageId",
                table: "OrderTrackPackageItems");

            migrationBuilder.AddColumn<long>(
                name: "OrderTrackPackageId",
                table: "OrderTrackPackageItems",
                type: "bigint",
                nullable: true);

            

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackPackageItems_OrderTrackPackageItems_PackageId",
                table: "OrderTrackPackageItems",
                column: "PackageId",
                principalTable: "OrderTrackPackageItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackPackageItems_OrderTrackPackages_OrderTrackPackageId",
                table: "OrderTrackPackageItems",
                column: "OrderTrackPackageId",
                principalTable: "OrderTrackPackages",
                principalColumn: "Id");
        }
    }
}
