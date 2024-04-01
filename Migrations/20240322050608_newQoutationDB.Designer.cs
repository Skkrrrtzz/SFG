﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SFG.Data;

#nullable disable

namespace SFG.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240322050608_newQoutationDB")]
    partial class newQoutationDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
