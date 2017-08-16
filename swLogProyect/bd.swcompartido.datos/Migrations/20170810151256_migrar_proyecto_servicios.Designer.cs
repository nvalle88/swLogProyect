using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using bd.swLogProyect.datos;

namespace bd.swLogProyect.datos.Migrations
{
    [DbContext(typeof(SwLogDbContext))]
    [Migration("20170810151256_migrar_proyecto_servicios")]
    partial class migrar_proyecto_servicios
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("bd.swLogProyect.entidades.LogCategory", b =>
                {
                    b.Property<int>("LogCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("ParameterValue")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.HasKey("LogCategoryId");

                    b.ToTable("LogCategories");
                });

            modelBuilder.Entity("bd.swLogProyect.entidades.LogEntry", b =>
                {
                    b.Property<int>("LogEntryId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationName")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("ExceptionTrace")
                        .HasMaxLength(4096);

                    b.Property<int>("LogCategoryId");

                    b.Property<DateTime>("LogDate");

                    b.Property<int>("LogLevelId");

                    b.Property<string>("MachineIP")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("MachineName")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("ObjEntityId")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.HasKey("LogEntryId");

                    b.HasIndex("LogCategoryId");

                    b.HasIndex("LogLevelId");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("bd.swLogProyect.entidades.LogLevel", b =>
                {
                    b.Property<int>("LogLevelId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Code");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1024);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(5);

                    b.HasKey("LogLevelId");

                    b.ToTable("LogLevels");
                });

            modelBuilder.Entity("bd.swLogProyect.entidades.LogEntry", b =>
                {
                    b.HasOne("bd.swLogProyect.entidades.LogCategory", "LogCategory")
                        .WithMany("LogEntries")
                        .HasForeignKey("LogCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("bd.swLogProyect.entidades.LogLevel", "LogLevel")
                        .WithMany("LogEntries")
                        .HasForeignKey("LogLevelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
