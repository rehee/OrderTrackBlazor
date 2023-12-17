﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderTrackBlazor.Data;

#nullable disable

namespace OrderTrackBlazor.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("OrderTrackBlazor.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackDispatchItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("DispatchPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("DispatchRecordId")
                        .HasColumnType("bigint");

                    b.Property<long?>("OrderProductionId")
                        .HasColumnType("bigint");

                    b.Property<int>("PackageQuantity")
                        .HasColumnType("int");

                    b.Property<long?>("ProductionId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DispatchRecordId");

                    b.HasIndex("OrderProductionId");

                    b.HasIndex("ProductionId");

                    b.ToTable("OrderTrackDispatchItems");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackDispatchPackage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("BriefDiscribtion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discribtion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Number")
                        .HasColumnType("int");

                    b.Property<long?>("PackageId")
                        .HasColumnType("bigint");

                    b.Property<long?>("RecordId")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PackageId");

                    b.HasIndex("RecordId");

                    b.ToTable("OrderTrackDispatchPackages");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackDispatchRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DispatchDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("Income")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("IncomeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("OrderTrackOrderId")
                        .HasColumnType("bigint");

                    b.Property<int?>("PackageNumber")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SoftDeleteUntil")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrderTrackOrderId");

                    b.ToTable("OrderTrackDispatchRecords");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackOrder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ShortNote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderTrackOrders");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackOrderItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("OrderPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("OrderTrackOrderId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ProductionId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrderTrackOrderId");

                    b.HasIndex("ProductionId");

                    b.ToTable("OrderTrackOrderItems");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPackage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("BriefDiscribtion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Confirmed")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discribtion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<long?>("SizeId")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("Weight")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("SizeId");

                    b.ToTable("OrderTrackPackages");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPackageItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<long?>("PackageId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ProductionId")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PackageId");

                    b.HasIndex("ProductionId");

                    b.ToTable("OrderTrackPackageItems");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPackageSize", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderTrackPackageSizes");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackProduction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("OriginalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderTrackProductions");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPurchaseItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ProductionId")
                        .HasColumnType("bigint");

                    b.Property<long?>("PurchaseRecordId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProductionId");

                    b.HasIndex("PurchaseRecordId");

                    b.ToTable("OrderTrackPurchaseItems");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPurchaseRecord", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("OrderId")
                        .HasColumnType("bigint");

                    b.Property<decimal?>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<long?>("ShopId")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ShopId");

                    b.ToTable("OrderTrackPurchaseRecords");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackShop", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShopName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderTrackShops");
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityUserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityUserToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("ReheeCmf.Entities.RoleBasedPermission", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ModuleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizationModuleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizationRoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Permissions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("RoleBasedPermissions");
                });

            modelBuilder.Entity("ReheeCmf.Tenants.TenantEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileAccessToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileAllowedFileType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileAllowedRoles")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("FileAuthRequired")
                        .HasColumnType("bit");

                    b.Property<string>("FileBaseFolder")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("FileCompression")
                        .HasColumnType("bit");

                    b.Property<string>("FileCompressionFileExtensions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("FileMaxFileUploadSize")
                        .HasColumnType("bigint");

                    b.Property<string>("FileServerPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FileServiceType")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LicenceEnd")
                        .HasColumnType("datetime2");

                    b.Property<string>("MainConnectionString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NomolizationTenantSubDomain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReadConnectionStrings")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("TenantID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("TenantLevel")
                        .HasColumnType("int");

                    b.Property<string>("TenantName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TenantSubDomain")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackDispatchItem", b =>
                {
                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackDispatchRecord", "DispatchRecord")
                        .WithMany("Items")
                        .HasForeignKey("DispatchRecordId");

                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackOrderItem", "OrderProduction")
                        .WithMany("DispatchItems")
                        .HasForeignKey("OrderProductionId");

                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackProduction", "Production")
                        .WithMany("DispatchItems")
                        .HasForeignKey("ProductionId");

                    b.Navigation("DispatchRecord");

                    b.Navigation("OrderProduction");

                    b.Navigation("Production");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackDispatchPackage", b =>
                {
                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackPackage", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId");

                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackDispatchRecord", "Record")
                        .WithMany("PackageRecords")
                        .HasForeignKey("RecordId");

                    b.Navigation("Package");

                    b.Navigation("Record");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackDispatchRecord", b =>
                {
                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackOrder", "Order")
                        .WithMany("DispatchRecords")
                        .HasForeignKey("OrderTrackOrderId");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackOrderItem", b =>
                {
                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackOrder", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderTrackOrderId");

                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackProduction", "Production")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductionId");

                    b.Navigation("Order");

                    b.Navigation("Production");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPackage", b =>
                {
                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackOrder", "Order")
                        .WithMany("Packages")
                        .HasForeignKey("OrderId");

                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackPackageSize", "Size")
                        .WithMany()
                        .HasForeignKey("SizeId");

                    b.Navigation("Order");

                    b.Navigation("Size");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPackageItem", b =>
                {
                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackPackage", "Package")
                        .WithMany("Items")
                        .HasForeignKey("PackageId");

                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackProduction", "Production")
                        .WithMany()
                        .HasForeignKey("ProductionId");

                    b.Navigation("Package");

                    b.Navigation("Production");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPurchaseItem", b =>
                {
                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackProduction", "Production")
                        .WithMany("PurchaseItems")
                        .HasForeignKey("ProductionId");

                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackPurchaseRecord", "PurchaseRecord")
                        .WithMany("Items")
                        .HasForeignKey("PurchaseRecordId");

                    b.Navigation("Production");

                    b.Navigation("PurchaseRecord");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPurchaseRecord", b =>
                {
                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackOrder", "Order")
                        .WithMany("PurchaseRecords")
                        .HasForeignKey("OrderId");

                    b.HasOne("OrderTrackBlazor.Entities.OrderTrackShop", "Shop")
                        .WithMany()
                        .HasForeignKey("ShopId");

                    b.Navigation("Order");

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityRoleClaim", b =>
                {
                    b.HasOne("ReheeCmf.ContextModule.Entities.TenantIdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityUserClaim", b =>
                {
                    b.HasOne("OrderTrackBlazor.Data.ApplicationUser", null)
                        .WithMany("Claims")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("OrderTrackBlazor.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityUserLogin", b =>
                {
                    b.HasOne("OrderTrackBlazor.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityUserRole", b =>
                {
                    b.HasOne("ReheeCmf.ContextModule.Entities.TenantIdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OrderTrackBlazor.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ReheeCmf.ContextModule.Entities.TenantIdentityUserToken", b =>
                {
                    b.HasOne("OrderTrackBlazor.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OrderTrackBlazor.Data.ApplicationUser", b =>
                {
                    b.Navigation("Claims");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackDispatchRecord", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("PackageRecords");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackOrder", b =>
                {
                    b.Navigation("DispatchRecords");

                    b.Navigation("Items");

                    b.Navigation("Packages");

                    b.Navigation("PurchaseRecords");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackOrderItem", b =>
                {
                    b.Navigation("DispatchItems");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPackage", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackProduction", b =>
                {
                    b.Navigation("DispatchItems");

                    b.Navigation("OrderItems");

                    b.Navigation("PurchaseItems");
                });

            modelBuilder.Entity("OrderTrackBlazor.Entities.OrderTrackPurchaseRecord", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
