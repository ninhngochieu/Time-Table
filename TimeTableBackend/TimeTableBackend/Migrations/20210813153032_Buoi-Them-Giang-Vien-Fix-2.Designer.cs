﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeTableBackend.Models;

namespace TimeTableBackend.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20210813153032_Buoi-Them-Giang-Vien-Fix-2")]
    partial class BuoiThemGiangVienFix2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("TimeTableBackend.Models.Buoi", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BatDauLuc")
                        .HasColumnType("INTEGER");

                    b.Property<string>("GiangVien")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NhomMonHocId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Phong")
                        .HasColumnType("TEXT");

                    b.Property<int>("SoTiet")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TietBatDau")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("NhomMonHocId");

                    b.ToTable("Buois");
                });

            modelBuilder.Entity("TimeTableBackend.Models.MonHoc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("MaMonHoc")
                        .HasColumnType("TEXT");

                    b.Property<int?>("NienKhoaId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SoTinChi")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Ten")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NienKhoaId");

                    b.ToTable("MonHocs");
                });

            modelBuilder.Entity("TimeTableBackend.Models.NhomMonHoc", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MonHocId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NMH")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MonHocId");

                    b.ToTable("NhomMonHocs");
                });

            modelBuilder.Entity("TimeTableBackend.Models.NienKhoa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("HocKy")
                        .HasColumnType("TEXT");

                    b.Property<string>("NamHoc")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("NienKhoas");
                });

            modelBuilder.Entity("TimeTableBackend.Models.Buoi", b =>
                {
                    b.HasOne("TimeTableBackend.Models.NhomMonHoc", null)
                        .WithMany("Buois")
                        .HasForeignKey("NhomMonHocId");
                });

            modelBuilder.Entity("TimeTableBackend.Models.MonHoc", b =>
                {
                    b.HasOne("TimeTableBackend.Models.NienKhoa", null)
                        .WithMany("MonHocs")
                        .HasForeignKey("NienKhoaId");
                });

            modelBuilder.Entity("TimeTableBackend.Models.NhomMonHoc", b =>
                {
                    b.HasOne("TimeTableBackend.Models.MonHoc", null)
                        .WithMany("NhomMonHoc")
                        .HasForeignKey("MonHocId");
                });

            modelBuilder.Entity("TimeTableBackend.Models.MonHoc", b =>
                {
                    b.Navigation("NhomMonHoc");
                });

            modelBuilder.Entity("TimeTableBackend.Models.NhomMonHoc", b =>
                {
                    b.Navigation("Buois");
                });

            modelBuilder.Entity("TimeTableBackend.Models.NienKhoa", b =>
                {
                    b.Navigation("MonHocs");
                });
#pragma warning restore 612, 618
        }
    }
}