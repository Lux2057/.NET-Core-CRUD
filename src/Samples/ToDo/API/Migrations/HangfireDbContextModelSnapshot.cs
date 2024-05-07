﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Samples.ToDo.API;

#nullable disable

namespace Samples.ToDo.API.Migrations
{
    [DbContext(typeof(HangfireDbContext))]
    partial class HangfireDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireCounter", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("ExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ExpireAt");

                    b.HasIndex("Key", "Value");

                    b.ToTable("HangfireCounter");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireHash", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Field")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("ExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Key", "Field");

                    b.HasIndex("ExpireAt");

                    b.ToTable("HangfireHash");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireJob", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("InvocationData")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long?>("StateId")
                        .HasColumnType("bigint");

                    b.Property<string>("StateName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("ExpireAt");

                    b.HasIndex("StateId");

                    b.HasIndex("StateName");

                    b.ToTable("HangfireJob");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireJobParameter", b =>
                {
                    b.Property<long>("JobId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("JobId", "Name");

                    b.ToTable("HangfireJobParameter");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireList", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("Key", "Position");

                    b.HasIndex("ExpireAt");

                    b.ToTable("HangfireList");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireLock", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime>("AcquiredAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("HangfireLock");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireQueuedJob", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("FetchedAt")
                        .IsConcurrencyToken()
                        .HasColumnType("timestamp with time zone");

                    b.Property<long>("JobId")
                        .HasColumnType("bigint");

                    b.Property<string>("Queue")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("Queue", "FetchedAt");

                    b.ToTable("HangfireQueuedJob");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireServer", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime>("Heartbeat")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Queues")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("WorkerCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Heartbeat");

                    b.ToTable("HangfireServer");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireSet", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Value")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("ExpireAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double>("Score")
                        .HasColumnType("double precision");

                    b.HasKey("Key", "Value");

                    b.HasIndex("ExpireAt");

                    b.HasIndex("Key", "Score");

                    b.ToTable("HangfireSet");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<long>("JobId")
                        .HasColumnType("bigint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.ToTable("HangfireState");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireJob", b =>
                {
                    b.HasOne("Hangfire.EntityFrameworkCore.HangfireState", "State")
                        .WithMany()
                        .HasForeignKey("StateId");

                    b.Navigation("State");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireJobParameter", b =>
                {
                    b.HasOne("Hangfire.EntityFrameworkCore.HangfireJob", "Job")
                        .WithMany("Parameters")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireQueuedJob", b =>
                {
                    b.HasOne("Hangfire.EntityFrameworkCore.HangfireJob", "Job")
                        .WithMany("QueuedJobs")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireState", b =>
                {
                    b.HasOne("Hangfire.EntityFrameworkCore.HangfireJob", "Job")
                        .WithMany("States")
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");
                });

            modelBuilder.Entity("Hangfire.EntityFrameworkCore.HangfireJob", b =>
                {
                    b.Navigation("Parameters");

                    b.Navigation("QueuedJobs");

                    b.Navigation("States");
                });
#pragma warning restore 612, 618
        }
    }
}
