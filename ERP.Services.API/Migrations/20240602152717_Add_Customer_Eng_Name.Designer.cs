﻿// <auto-generated />
using System;
using ERP.Services.API.PromServiceDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ERP.Services.API.Migrations
{
    [DbContext(typeof(PromDbContext))]
    [Migration("20240602152717_Add_Customer_Eng_Name")]
    partial class Add_Customer_Eng_Name
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ERP.Services.API.Entities.ApiKeyEntity", b =>
                {
                    b.Property<Guid?>("KeyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("key_id");

                    b.Property<string>("ApiKey")
                        .HasColumnType("text")
                        .HasColumnName("api_key");

                    b.Property<DateTime?>("KeyCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("key_created_date");

                    b.Property<string>("KeyDescription")
                        .HasColumnType("text")
                        .HasColumnName("key_description");

                    b.Property<DateTime?>("KeyExpiredDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("key_expired_date");

                    b.Property<string>("OrgId")
                        .HasColumnType("text")
                        .HasColumnName("org_id");

                    b.Property<string>("RolesList")
                        .HasColumnType("text")
                        .HasColumnName("roles_list");

                    b.HasKey("KeyId");

                    b.HasIndex("ApiKey")
                        .IsUnique();

                    b.HasIndex("OrgId");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.BusinessEntity", b =>
                {
                    b.Property<Guid?>("BusinessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("business_id");

                    b.Property<string>("Alley")
                        .HasColumnType("text")
                        .HasColumnName("alley");

                    b.Property<string>("BrnId")
                        .HasColumnType("text")
                        .HasColumnName("branch_id");

                    b.Property<string>("Building")
                        .HasColumnType("text")
                        .HasColumnName("building");

                    b.Property<DateTime?>("BusinessCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("business_created_date");

                    b.Property<string>("BusinessCustomId")
                        .HasColumnType("text")
                        .HasColumnName("business_custom_id");

                    b.Property<string>("BusinessDescription")
                        .HasColumnType("text")
                        .HasColumnName("business_description");

                    b.Property<string>("BusinessLogo")
                        .HasColumnType("text")
                        .HasColumnName("business_logo");

                    b.Property<string>("BusinessName")
                        .HasColumnType("text")
                        .HasColumnName("business_name");

                    b.Property<string>("BusinessStatus")
                        .HasColumnType("text")
                        .HasColumnName("business_status");

                    b.Property<string>("BusinessType")
                        .HasColumnType("text")
                        .HasColumnName("business_type");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("District")
                        .HasColumnType("text")
                        .HasColumnName("district");

                    b.Property<string>("Floor")
                        .HasColumnType("text")
                        .HasColumnName("floor");

                    b.Property<string>("Moo")
                        .HasColumnType("text")
                        .HasColumnName("moo");

                    b.Property<string>("No")
                        .HasColumnType("text")
                        .HasColumnName("house_no");

                    b.Property<Guid?>("OrgId")
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<string>("PostCode")
                        .HasColumnType("text")
                        .HasColumnName("post_code");

                    b.Property<string>("Provice")
                        .HasColumnType("text")
                        .HasColumnName("provice");

                    b.Property<string>("Road")
                        .HasColumnType("text")
                        .HasColumnName("road");

                    b.Property<string>("RoomNo")
                        .HasColumnType("text")
                        .HasColumnName("room_no");

                    b.Property<string>("SubDistrict")
                        .HasColumnType("text")
                        .HasColumnName("sub_district");

                    b.Property<string>("TaxId")
                        .HasColumnType("text")
                        .HasColumnName("tax_id");

                    b.Property<string>("Village")
                        .HasColumnType("text")
                        .HasColumnName("village");

                    b.Property<string>("Website")
                        .HasColumnType("text")
                        .HasColumnName("website");

                    b.HasKey("BusinessId");

                    b.HasIndex("BusinessCustomId")
                        .IsUnique();

                    b.ToTable("Businesses");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.CustomerEntity", b =>
                {
                    b.Property<Guid?>("CusId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("cus_id");

                    b.Property<string>("Alley")
                        .HasColumnType("text")
                        .HasColumnName("alley");

                    b.Property<string>("BrnId")
                        .HasColumnType("text")
                        .HasColumnName("branch_id");

                    b.Property<string>("Building")
                        .HasColumnType("text")
                        .HasColumnName("building");

                    b.Property<Guid?>("BusinessId")
                        .HasColumnType("uuid")
                        .HasColumnName("business_id");

                    b.Property<DateTime?>("CusCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("cus_created_date");

                    b.Property<string>("CusCustomId")
                        .HasColumnType("text")
                        .HasColumnName("cus_custom_id");

                    b.Property<string>("CusDescription")
                        .HasColumnType("text")
                        .HasColumnName("cus_description");

                    b.Property<string>("CusName")
                        .HasColumnType("text")
                        .HasColumnName("cus_name");

                    b.Property<string>("CusNameEng")
                        .HasColumnType("text")
                        .HasColumnName("cus_name_eng");

                    b.Property<string>("CusStatus")
                        .HasColumnType("text")
                        .HasColumnName("cus_status");

                    b.Property<string>("CusType")
                        .HasColumnType("text")
                        .HasColumnName("cus_type");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("District")
                        .HasColumnType("text")
                        .HasColumnName("district");

                    b.Property<string>("Floor")
                        .HasColumnType("text")
                        .HasColumnName("floor");

                    b.Property<string>("Moo")
                        .HasColumnType("text")
                        .HasColumnName("moo");

                    b.Property<string>("No")
                        .HasColumnType("text")
                        .HasColumnName("house_no");

                    b.Property<Guid?>("OrgId")
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<string>("PostCode")
                        .HasColumnType("text")
                        .HasColumnName("post_code");

                    b.Property<string>("Provice")
                        .HasColumnType("text")
                        .HasColumnName("provice");

                    b.Property<string>("Road")
                        .HasColumnType("text")
                        .HasColumnName("road");

                    b.Property<string>("RoomNo")
                        .HasColumnType("text")
                        .HasColumnName("room_no");

                    b.Property<string>("SubDistrict")
                        .HasColumnType("text")
                        .HasColumnName("sub_district");

                    b.Property<string>("TaxId")
                        .HasColumnType("text")
                        .HasColumnName("tax_id");

                    b.Property<string>("Village")
                        .HasColumnType("text")
                        .HasColumnName("village");

                    b.Property<string>("Website")
                        .HasColumnType("text")
                        .HasColumnName("website");

                    b.HasKey("CusId");

                    b.HasIndex("CusCustomId")
                        .IsUnique();

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.DistrictEntity", b =>
                {
                    b.Property<Guid?>("DistrictId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("district_id");

                    b.Property<int?>("DistrictCode")
                        .HasColumnType("integer")
                        .HasColumnName("district_code");

                    b.Property<string>("DistrictNameEn")
                        .HasColumnType("text")
                        .HasColumnName("district_name_en");

                    b.Property<string>("DistrictNameTh")
                        .HasColumnType("text")
                        .HasColumnName("district_name_th");

                    b.Property<string>("PostalCode")
                        .HasColumnType("text")
                        .HasColumnName("postal_code");

                    b.Property<int?>("ProvinceCode")
                        .HasColumnType("integer")
                        .HasColumnName("province_code");

                    b.HasKey("DistrictId");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.OrganizationEntity", b =>
                {
                    b.Property<Guid?>("OrgId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<string>("Alley")
                        .HasColumnType("text")
                        .HasColumnName("alley");

                    b.Property<string>("BrnId")
                        .HasColumnType("text")
                        .HasColumnName("branch_id");

                    b.Property<string>("Building")
                        .HasColumnType("text")
                        .HasColumnName("building");

                    b.Property<string>("BusinessType")
                        .HasColumnType("text")
                        .HasColumnName("business_type");

                    b.Property<string>("DisplayName")
                        .HasColumnType("text")
                        .HasColumnName("display_name");

                    b.Property<string>("District")
                        .HasColumnType("text")
                        .HasColumnName("district");

                    b.Property<string>("Floor")
                        .HasColumnType("text")
                        .HasColumnName("floor");

                    b.Property<string>("Moo")
                        .HasColumnType("text")
                        .HasColumnName("moo");

                    b.Property<string>("No")
                        .HasColumnType("text")
                        .HasColumnName("house_no");

                    b.Property<DateTime?>("OrgCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("org_created_date");

                    b.Property<string>("OrgCustomId")
                        .HasColumnType("text")
                        .HasColumnName("org_custom_id");

                    b.Property<string>("OrgDescription")
                        .HasColumnType("text")
                        .HasColumnName("org_description");

                    b.Property<string>("OrgLogo")
                        .HasColumnType("text")
                        .HasColumnName("org_logo");

                    b.Property<string>("OrgName")
                        .HasColumnType("text")
                        .HasColumnName("org_name");

                    b.Property<string>("OrgStatus")
                        .HasColumnType("text")
                        .HasColumnName("org_status");

                    b.Property<string>("PostCode")
                        .HasColumnType("text")
                        .HasColumnName("post_code");

                    b.Property<string>("Provice")
                        .HasColumnType("text")
                        .HasColumnName("provice");

                    b.Property<string>("Road")
                        .HasColumnType("text")
                        .HasColumnName("road");

                    b.Property<string>("RoomNo")
                        .HasColumnType("text")
                        .HasColumnName("room_no");

                    b.Property<string>("SubDistrict")
                        .HasColumnType("text")
                        .HasColumnName("sub_district");

                    b.Property<string>("TaxId")
                        .HasColumnType("text")
                        .HasColumnName("tax_id");

                    b.Property<string>("Village")
                        .HasColumnType("text")
                        .HasColumnName("village");

                    b.Property<string>("Website")
                        .HasColumnType("text")
                        .HasColumnName("website");

                    b.HasKey("OrgId");

                    b.HasIndex("OrgCustomId")
                        .IsUnique();

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.OrganizationNumberEntity", b =>
                {
                    b.Property<Guid?>("OrgId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<int?>("Allocated")
                        .HasColumnType("integer")
                        .HasColumnName("allocated");

                    b.Property<string>("OrgDate")
                        .HasColumnType("text")
                        .HasColumnName("org_date");

                    b.HasKey("OrgId");

                    b.ToTable("OrganizationNo");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.OrganizationUserEntity", b =>
                {
                    b.Property<Guid?>("OrgUserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("org_user_id");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("FirstNameTh")
                        .HasColumnType("text")
                        .HasColumnName("first_name_th");

                    b.Property<string>("LastnameTh")
                        .HasColumnType("text")
                        .HasColumnName("last_name_th");

                    b.Property<string>("OrgCustomId")
                        .HasColumnType("text")
                        .HasColumnName("org_custom_id");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("TelNo")
                        .HasColumnType("text")
                        .HasColumnName("tel_no");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.Property<string>("email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.HasKey("OrgUserId");

                    b.ToTable("OrganizationsUsers");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.ProvinceEntity", b =>
                {
                    b.Property<Guid?>("ProvinceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("province_id");

                    b.Property<int?>("ProvinceCode")
                        .HasColumnType("integer")
                        .HasColumnName("province_code");

                    b.Property<string>("ProvinceNameEn")
                        .HasColumnType("text")
                        .HasColumnName("province_name_en");

                    b.Property<string>("ProvinceNameTh")
                        .HasColumnType("text")
                        .HasColumnName("province_name_th");

                    b.HasKey("ProvinceId");

                    b.ToTable("Provinces");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.RoleEntity", b =>
                {
                    b.Property<Guid?>("RoleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.Property<DateTime?>("RoleCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("role_created_date");

                    b.Property<string>("RoleDefinition")
                        .HasColumnType("text")
                        .HasColumnName("role_definition");

                    b.Property<string>("RoleDescription")
                        .HasColumnType("text")
                        .HasColumnName("role_description");

                    b.Property<string>("RoleLevel")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_level");

                    b.Property<string>("RoleName")
                        .HasColumnType("text")
                        .HasColumnName("role_name");

                    b.HasKey("RoleId");

                    b.HasIndex("RoleName")
                        .IsUnique();

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.SubDistrictEntity", b =>
                {
                    b.Property<Guid?>("SubDistrictId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("sub_district_id");

                    b.Property<int?>("DistrictCode")
                        .HasColumnType("integer")
                        .HasColumnName("district_code");

                    b.Property<string>("PostalCode")
                        .HasColumnType("text")
                        .HasColumnName("postal_code");

                    b.Property<int?>("ProvinceCode")
                        .HasColumnType("integer")
                        .HasColumnName("province_code");

                    b.Property<int?>("SubDistrictCode")
                        .HasColumnType("integer")
                        .HasColumnName("sub_district_code");

                    b.Property<string>("SubDistrictNameEn")
                        .HasColumnType("text")
                        .HasColumnName("sub_district_name_en");

                    b.Property<string>("SubDistrictNameTh")
                        .HasColumnType("text")
                        .HasColumnName("sub_district_name_th");

                    b.HasKey("SubDistrictId");

                    b.ToTable("SubDistricts");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.UserBusinessEntity", b =>
                {
                    b.Property<Guid?>("UserBusinessId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_business_id");

                    b.Property<Guid?>("BusinessId")
                        .HasColumnType("uuid")
                        .HasColumnName("business_id");

                    b.Property<Guid?>("OrgId")
                        .HasColumnType("uuid")
                        .HasColumnName("org_id");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.HasKey("UserBusinessId");

                    b.HasIndex("UserBusinessId")
                        .IsUnique();

                    b.ToTable("UserBusiness");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.UserEntity", b =>
                {
                    b.Property<Guid?>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<DateTime?>("UserCreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("user_created_date");

                    b.Property<string>("UserEmail")
                        .HasColumnType("text")
                        .HasColumnName("user_email");

                    b.Property<string>("UserName")
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("UserId");

                    b.HasIndex("UserEmail")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ERP.Services.API.Entities.UserSessionEntity", b =>
                {
                    b.Property<Guid>("UserSessionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_session_id");

                    b.Property<string>("SessionStatus")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("session_status");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("token");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("userId");

                    b.HasKey("UserSessionId");

                    b.HasIndex("UserSessionId")
                        .IsUnique();

                    b.ToTable("UserSession");
                });
#pragma warning restore 612, 618
        }
    }
}
