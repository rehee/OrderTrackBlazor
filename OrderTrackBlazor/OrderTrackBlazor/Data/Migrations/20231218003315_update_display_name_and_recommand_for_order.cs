using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    /// <inheritdoc />
    public partial class update_display_name_and_recommand_for_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShopName",
                table: "OrderTrackShops",
                newName: "Name");

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackPurchaseRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderTrackPurchaseRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackPurchaseItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderTrackPurchaseItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackProductions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderTrackPackages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackPackageItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderTrackPackageItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderTrackOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackOrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderTrackOrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OrderTrackOrderItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RecommendShopId",
                table: "OrderTrackOrderItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackDispatchRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderTrackDispatchRecords",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackDispatchPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderTrackDispatchPackages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "OrderTrackDispatchItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "OrderTrackDispatchItems",
                type: "nvarchar(max)",
                nullable: true);

            

            migrationBuilder.AddForeignKey(
                name: "FK_OrderTrackOrderItems_OrderTrackShops_RecommendShopId",
                table: "OrderTrackOrderItems",
                column: "RecommendShopId",
                principalTable: "OrderTrackShops",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderTrackOrderItems_OrderTrackShops_RecommendShopId",
                table: "OrderTrackOrderItems");

            

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackPurchaseRecords");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderTrackPurchaseRecords");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackPurchaseItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderTrackPurchaseItems");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackProductions");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackPackages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderTrackPackages");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackPackageItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderTrackPackageItems");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackOrders");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderTrackOrders");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackOrderItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderTrackOrderItems");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OrderTrackOrderItems");

            migrationBuilder.DropColumn(
                name: "RecommendShopId",
                table: "OrderTrackOrderItems");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackDispatchRecords");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderTrackDispatchRecords");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackDispatchPackages");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderTrackDispatchPackages");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "OrderTrackDispatchItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "OrderTrackDispatchItems");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "OrderTrackShops",
                newName: "ShopName");
        }
    }
}
