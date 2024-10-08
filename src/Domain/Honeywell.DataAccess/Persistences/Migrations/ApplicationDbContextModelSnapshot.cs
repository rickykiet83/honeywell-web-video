﻿// <auto-generated />
using System;
using Honeywell.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Honeywell.DataAccess.Persistences.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Honeywell.Models.VideoFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<decimal>("FileSizeInMb")
                        .HasPrecision(18, 2)
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateTime>("UploadedOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FileName")
                        .IsUnique();

                    b.ToTable("VideoFiles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FileName = "big_buck_bunny.mp4",
                            FilePath = "videos/big_buck_bunny.mp4",
                            FileSizeInMb = 5510872m,
                            FileType = "video/mp4",
                            UploadedOn = new DateTime(2024, 9, 28, 7, 35, 51, 233, DateTimeKind.Utc).AddTicks(150)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
