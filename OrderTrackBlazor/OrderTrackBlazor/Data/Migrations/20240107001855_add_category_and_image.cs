using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
  /// <inheritdoc />
  public partial class add_category_and_image : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AddColumn<string>(
          name: "AttachmentId",
          table: "OrderTrackProductions",
          type: "nvarchar(max)",
          nullable: true);

      migrationBuilder.AddColumn<long>(
          name: "CategoryId",
          table: "OrderTrackProductions",
          type: "bigint",
          nullable: true);

      migrationBuilder.CreateTable(
          name: "OrderTrackCategories",
          columns: table => new
          {
            Id = table.Column<long>(type: "bigint", nullable: false)
                  .Annotation("SqlServer:Identity", "1, 1"),
            TenantID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
            CreateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            CreateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            UpdateUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
            Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
            DisplayOrder = table.Column<int>(type: "int", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_OrderTrackCategories", x => x.Id);
          });

      migrationBuilder.AddForeignKey(
          name: "FK_OrderTrackProductions_OrderTrackCategories_CategoryId",
          table: "OrderTrackProductions",
          column: "CategoryId",
          principalTable: "OrderTrackCategories",
          principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropForeignKey(
          name: "FK_OrderTrackProductions_OrderTrackCategories_CategoryId",
          table: "OrderTrackProductions");

      migrationBuilder.DropTable(
          name: "OrderTrackCategories");

      migrationBuilder.DropColumn(
          name: "AttachmentId",
          table: "OrderTrackProductions");

      migrationBuilder.DropColumn(
          name: "CategoryId",
          table: "OrderTrackProductions");
    }
  }
}
