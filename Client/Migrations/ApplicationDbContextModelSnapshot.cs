﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PetStore.Models;

namespace PetStore.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PetStore.Models.CartLine", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("OrderID");

                    b.Property<int?>("ProductID");

                    b.Property<int>("Quantity");

                    b.HasKey("ID");

                    b.HasIndex("OrderID");

                    b.HasIndex("ProductID");

                    b.ToTable("CartLine");
                });

            modelBuilder.Entity("PetStore.Models.CategoryNode", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CategoryNodeID");

                    b.Property<bool>("IsRoot");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.HasIndex("CategoryNodeID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("PetStore.Models.Comment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Message")
                        .IsRequired();

                    b.Property<int?>("ProductExtendedID");

                    b.Property<int>("Rating");

                    b.Property<DateTime>("Time");

                    b.Property<string>("UserName");

                    b.HasKey("ID");

                    b.HasIndex("ProductExtendedID");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PetStore.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Canceled");

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<DateTime>("Date");

                    b.Property<bool>("GiftWrap");

                    b.Property<string>("House")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Room");

                    b.Property<bool>("Shipped");

                    b.Property<string>("State")
                        .IsRequired();

                    b.Property<string>("Street")
                        .IsRequired();

                    b.Property<string>("UserName");

                    b.Property<int>("Zip");

                    b.HasKey("OrderID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("PetStore.Models.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryID");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("ImageId");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<decimal>("Price");

                    b.HasKey("ID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PetStore.Models.ProductExtended", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image");

                    b.Property<string>("LongDescription");

                    b.Property<string>("Manufacturer");

                    b.Property<string>("OriginCountry");

                    b.Property<int?>("ProductID");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("ProductsExtended");
                });

            modelBuilder.Entity("PetStore.Models.Stock", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ProductID");

                    b.Property<int>("Quantity");

                    b.HasKey("ID");

                    b.HasIndex("ProductID");

                    b.ToTable("StockItems");
                });

            modelBuilder.Entity("PetStore.Models.CartLine", b =>
                {
                    b.HasOne("PetStore.Models.Order")
                        .WithMany("Lines")
                        .HasForeignKey("OrderID");

                    b.HasOne("PetStore.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");
                });

            modelBuilder.Entity("PetStore.Models.CategoryNode", b =>
                {
                    b.HasOne("PetStore.Models.CategoryNode")
                        .WithMany("Children")
                        .HasForeignKey("CategoryNodeID");
                });

            modelBuilder.Entity("PetStore.Models.Comment", b =>
                {
                    b.HasOne("PetStore.Models.ProductExtended")
                        .WithMany("Comments")
                        .HasForeignKey("ProductExtendedID");
                });

            modelBuilder.Entity("PetStore.Models.Product", b =>
                {
                    b.HasOne("PetStore.Models.CategoryNode", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PetStore.Models.ProductExtended", b =>
                {
                    b.HasOne("PetStore.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");
                });

            modelBuilder.Entity("PetStore.Models.Stock", b =>
                {
                    b.HasOne("PetStore.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID");
                });
#pragma warning restore 612, 618
        }
    }
}