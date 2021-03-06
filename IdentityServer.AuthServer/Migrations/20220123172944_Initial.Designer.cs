// <auto-generated />
using IdentityServer.AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IdentityServer.AuthServer.Migrations
{
    [DbContext(typeof(CustomDbContext))]
    [Migration("20220123172944_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.22")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("IdentityServer.AuthServer.Models.CustomUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("CustomUsers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "İstanbul",
                            Email = "ofaruksahin@outlook.com.tr",
                            Password = "123",
                            UserName = "ofaruksahin@outlook.com.tr"
                        },
                        new
                        {
                            Id = 2,
                            City = "İstanbul",
                            Email = "harunsahin@outlook.com.tr",
                            Password = "123",
                            UserName = "harunsahin@outlook.com.tr"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
