﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SavingsInvestment.Data;

#nullable disable

namespace SavingsInvestment.Migrations
{
    [DbContext(typeof(SavingsInvestmentContext))]
    [Migration("20241107174649_first")]
    partial class first
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SavingsInvestment.Models.Investment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CurrentValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InvestmentDetailsId")
                        .HasColumnType("int");

                    b.Property<string>("InvestmentType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Investments");
                });

            modelBuilder.Entity("SavingsInvestment.Models.InvestmentDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("CurrentPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("InvestmentId")
                        .HasColumnType("int");

                    b.Property<decimal>("PurchasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("ReturnPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Returns")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("InvestmentId")
                        .IsUnique();

                    b.ToTable("InvestmentDetails");
                });

            modelBuilder.Entity("SavingsInvestment.Models.SavingsGoal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("CurrentAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("GoalName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Priority")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TargetAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("TargetDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SavingsGoals");
                });

            modelBuilder.Entity("SavingsInvestment.Models.InvestmentDetails", b =>
                {
                    b.HasOne("SavingsInvestment.Models.Investment", "Investment")
                        .WithOne("Details")
                        .HasForeignKey("SavingsInvestment.Models.InvestmentDetails", "InvestmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Investment");
                });

            modelBuilder.Entity("SavingsInvestment.Models.Investment", b =>
                {
                    b.Navigation("Details")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
