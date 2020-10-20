﻿// <auto-generated />
using System;
using CarRentalAdministrationService.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarRentalAdministrationService.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20201020121756_initialCreate")]
    partial class initialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9");

            modelBuilder.Entity("CarRentalAdministrationService.Model.CarCategory", b =>
                {
                    b.Property<int>("CarCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BaseDayRentalCost")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .HasColumnType("TEXT");

                    b.Property<int>("KilometerPrice")
                        .HasColumnType("INTEGER");

                    b.HasKey("CarCategoryId");

                    b.ToTable("CarCategories");

                    b.HasData(
                        new
                        {
                            CarCategoryId = 1,
                            BaseDayRentalCost = 100,
                            Category = "Compact",
                            KilometerPrice = 10
                        },
                        new
                        {
                            CarCategoryId = 2,
                            BaseDayRentalCost = 125,
                            Category = "Premium",
                            KilometerPrice = 12
                        },
                        new
                        {
                            CarCategoryId = 3,
                            BaseDayRentalCost = 150,
                            Category = "Minivan",
                            KilometerPrice = 15
                        });
                });

            modelBuilder.Entity("CarRentalAdministrationService.Model.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CarCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CustomerDateOfBirth")
                        .HasColumnType("TEXT");

                    b.HasKey("OrderId");

                    b.HasIndex("CarCategoryId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CarRentalAdministrationService.Model.Order", b =>
                {
                    b.HasOne("CarRentalAdministrationService.Model.CarCategory", "CarCategory")
                        .WithMany()
                        .HasForeignKey("CarCategoryId");
                });
#pragma warning restore 612, 618
        }
    }
}
