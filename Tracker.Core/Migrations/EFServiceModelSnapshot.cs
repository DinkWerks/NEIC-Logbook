﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tracker.Core.Services;

namespace Tracker.Core.Migrations
{
    [DbContext(typeof(EFService))]
    partial class EFServiceModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Tracker.Core.Models.Organization", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressID");

                    b.Property<string>("Email")
                        .HasMaxLength(200);

                    b.Property<string>("NewPEID")
                        .HasMaxLength(10);

                    b.Property<string>("Notes");

                    b.Property<string>("OldPEID")
                        .HasMaxLength(10);

                    b.Property<string>("OrganizationName")
                        .HasMaxLength(200);

                    b.Property<string>("OrganizationStandingName");

                    b.Property<string>("Phone")
                        .HasMaxLength(15);

                    b.Property<string>("Website")
                        .HasMaxLength(200);

                    b.HasKey("ID");

                    b.HasIndex("OrganizationStandingName");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Tracker.Core.Models.Person", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AddressID");

                    b.Property<int?>("AffiliationID");

                    b.Property<string>("DisclosureLevel");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Note");

                    b.Property<string>("Phone1");

                    b.Property<string>("Phone2");

                    b.HasKey("ID");

                    b.HasIndex("AffiliationID");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Tracker.Core.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Acres");

                    b.Property<string>("AdditionalCounties");

                    b.Property<string>("AdditionalRequestors");

                    b.Property<bool>("AreResourcesInProject");

                    b.Property<string>("CheckName")
                        .HasMaxLength(100);

                    b.Property<string>("CheckNumber")
                        .HasMaxLength(50);

                    b.Property<int?>("ClientID");

                    b.Property<DateTime?>("DateBilled");

                    b.Property<DateTime?>("DateEntered");

                    b.Property<DateTime?>("DateOfResponse");

                    b.Property<DateTime?>("DatePaid");

                    b.Property<DateTime?>("DateReceived");

                    b.Property<string>("EncryptionPassword");

                    b.Property<int>("FeeID");

                    b.Property<string>("FeeVersion")
                        .HasMaxLength(50);

                    b.Property<int>("ICEnumeration");

                    b.Property<string>("ICSuffix")
                        .HasMaxLength(3);

                    b.Property<string>("ICTypePrefix")
                        .HasMaxLength(1);

                    b.Property<string>("ICYear")
                        .HasMaxLength(2);

                    b.Property<string>("InvoiceNumber")
                        .HasMaxLength(50);

                    b.Property<bool>("IsMailingSameAsBilling");

                    b.Property<bool>("IsPrePaid");

                    b.Property<bool>("IsReportReceived");

                    b.Property<DateTime?>("LastUpdated");

                    b.Property<decimal>("LinearMiles");

                    b.Property<string>("MainCounty");

                    b.Property<string>("PLSS");

                    b.Property<string>("Processor");

                    b.Property<string>("ProjectName");

                    b.Property<string>("ProjectNumber");

                    b.Property<string>("ProjectType")
                        .HasMaxLength(100);

                    b.Property<string>("Recommendation")
                        .HasMaxLength(1000);

                    b.Property<int?>("RequestorID");

                    b.Property<string>("SpecialDetails");

                    b.Property<string>("Status")
                        .HasMaxLength(100);

                    b.Property<decimal>("TotalFee");

                    b.HasKey("Id");

                    b.HasIndex("ClientID");

                    b.HasIndex("RequestorID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Tracker.Core.Models.Staff", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Staffs");
                });

            modelBuilder.Entity("Tracker.Core.StaticTypes.OrganizationStanding", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Icon");

                    b.Property<int>("Severity");

                    b.HasKey("Name");

                    b.ToTable("OrganizationStandings");
                });

            modelBuilder.Entity("Tracker.Core.Models.Organization", b =>
                {
                    b.HasOne("Tracker.Core.StaticTypes.OrganizationStanding", "OrganizationStanding")
                        .WithMany("Organizations")
                        .HasForeignKey("OrganizationStandingName");

                    b.OwnsOne("Tracker.Core.Models.Address", "Address", b1 =>
                        {
                            b1.Property<int>("OrganizationID")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("AddressLine1");

                            b1.Property<string>("AddressLine2");

                            b1.Property<string>("AddressName");

                            b1.Property<string>("AttentionTo");

                            b1.Property<string>("City");

                            b1.Property<string>("Notes");

                            b1.Property<string>("State");

                            b1.Property<string>("ZIP");

                            b1.HasKey("OrganizationID");

                            b1.ToTable("Organizations");

                            b1.HasOne("Tracker.Core.Models.Organization")
                                .WithOne("Address")
                                .HasForeignKey("Tracker.Core.Models.Address", "OrganizationID")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Tracker.Core.Models.Person", b =>
                {
                    b.HasOne("Tracker.Core.Models.Organization", "Affiliation")
                        .WithMany("Employees")
                        .HasForeignKey("AffiliationID");

                    b.OwnsOne("Tracker.Core.Models.Address", "Address", b1 =>
                        {
                            b1.Property<int>("PersonID")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("AddressLine1");

                            b1.Property<string>("AddressLine2");

                            b1.Property<string>("AddressName");

                            b1.Property<string>("AttentionTo");

                            b1.Property<string>("City");

                            b1.Property<string>("Notes");

                            b1.Property<string>("State");

                            b1.Property<string>("ZIP");

                            b1.HasKey("PersonID");

                            b1.ToTable("People");

                            b1.HasOne("Tracker.Core.Models.Person")
                                .WithOne("Address")
                                .HasForeignKey("Tracker.Core.Models.Address", "PersonID")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("Tracker.Core.Models.Project", b =>
                {
                    b.HasOne("Tracker.Core.Models.Organization", "Client")
                        .WithMany()
                        .HasForeignKey("ClientID");

                    b.HasOne("Tracker.Core.Models.Person", "Requestor")
                        .WithMany()
                        .HasForeignKey("RequestorID");

                    b.OwnsOne("Tracker.Core.Models.Address", "BillingAddress", b1 =>
                        {
                            b1.Property<int>("ProjectId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("AddressLine1");

                            b1.Property<string>("AddressLine2");

                            b1.Property<string>("AddressName");

                            b1.Property<string>("AttentionTo");

                            b1.Property<string>("City");

                            b1.Property<string>("Notes");

                            b1.Property<string>("State");

                            b1.Property<string>("ZIP");

                            b1.HasKey("ProjectId");

                            b1.ToTable("Projects");

                            b1.HasOne("Tracker.Core.Models.Project")
                                .WithOne("BillingAddress")
                                .HasForeignKey("Tracker.Core.Models.Address", "ProjectId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });

                    b.OwnsOne("Tracker.Core.Models.Address", "MailingAddress", b1 =>
                        {
                            b1.Property<int>("ProjectId")
                                .ValueGeneratedOnAdd()
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("AddressLine1");

                            b1.Property<string>("AddressLine2");

                            b1.Property<string>("AddressName");

                            b1.Property<string>("AttentionTo");

                            b1.Property<string>("City");

                            b1.Property<string>("Notes");

                            b1.Property<string>("State");

                            b1.Property<string>("ZIP");

                            b1.HasKey("ProjectId");

                            b1.ToTable("Projects");

                            b1.HasOne("Tracker.Core.Models.Project")
                                .WithOne("MailingAddress")
                                .HasForeignKey("Tracker.Core.Models.Address", "ProjectId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
