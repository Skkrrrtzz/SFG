﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SFG.Data;

#nullable disable

namespace SFG.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SFG.Models.LastPurchaseInfoModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomerVendorCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerVendorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FGName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ForeignName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("GWRLQty")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ItemDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ItemNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastPurchasedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("LastPurchasedUSDPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RMWHEREUSED")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Unit")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("Id");

                    b.ToTable("LastPurchaseInfo");
                });

            modelBuilder.Entity("SFG.Models.MRPBOMModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Commodity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateModified")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DescriptionTable")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EQPA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Item")
                        .HasColumnType("int");

                    b.Property<int?>("Level")
                        .HasColumnType("int");

                    b.Property<string>("MPN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PartNumberTable")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreparedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Product")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QPA")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Rev")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReviewedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Revision")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("SAPPartNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UOM")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("MRPBOM");
                });

            modelBuilder.Entity("SFG.Models.QoutationModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PartNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Qoutations");
                });

            modelBuilder.Entity("SFG.Models.UsersModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Department")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
