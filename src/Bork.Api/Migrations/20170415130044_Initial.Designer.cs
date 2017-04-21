using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Bork.Api.Data;

namespace Bork.Api.Migrations
{
    [DbContext(typeof(BorkDbContext))]
    [Migration("20170415130044_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Bork.Contracts.BorkRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("Borks");
                });

            modelBuilder.Entity("Bork.Contracts.ReBorkRecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("OriginalBorkId");

                    b.Property<string>("OriginalContent");

                    b.Property<string>("OriginalUserName");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("ReBorks");
                });
        }
    }
}
