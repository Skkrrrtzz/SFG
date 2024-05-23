﻿// <auto-generated />
using System;
using DMD_Prototype.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DMD_Prototype.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230925055404_removetechname")]
    partial class removetechname
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DMD_Prototype.Models.AccountModel", b =>
                {
                    b.Property<int>("AccID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccID"));

                    b.Property<string>("AccName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Dom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sec")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccID");

                    b.ToTable("AccountDb");
                });

            modelBuilder.Entity("DMD_Prototype.Models.MTIModel", b =>
                {
                    b.Property<int>("MTIID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MTIID"));

                    b.Property<string>("AssemblyDesc")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssemblyPN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("DocType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ObsoleteStat")
                        .HasColumnType("bit");

                    b.Property<string>("OriginatorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Product")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RevNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MTIID");

                    b.ToTable("MTIDb");
                });

            modelBuilder.Entity("DMD_Prototype.Models.PauseWorkModel", b =>
                {
                    b.Property<int>("PWID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PWID"));

                    b.Property<DateTime>("PauseDT")
                        .HasColumnType("datetime2");

                    b.Property<string>("PauseReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RestartDT")
                        .HasColumnType("datetime2");

                    b.Property<string>("SessionID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PWID");

                    b.ToTable("PauseWorkDb");
                });

            modelBuilder.Entity("DMD_Prototype.Models.StartWorkModel", b =>
                {
                    b.Property<int>("SWID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SWID"));

                    b.Property<string>("DocNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("FinishDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SessionID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.HasKey("SWID");

                    b.ToTable("StartWorkDb");
                });
#pragma warning restore 612, 618
        }
    }
}
