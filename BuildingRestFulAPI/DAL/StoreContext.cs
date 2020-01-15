using BuildingRestFulAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.DAL
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountCategory> AccountCategories { get; set; }
        public DbSet<Management> Managements { get; set; }
        public DbSet<ManagementRole> ManagementRoles { get; set; }
        public DbSet<RoleToMenu> RoleToMenus { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AccountTransaction> AccountTransactions { get; set; }
        public DbSet<Agent> Agents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Bank>(entity =>
            //{
            //    entity.Property(c => c.CustomerId)
            //    .HasColumnName("CustomerId")
            //    .HasColumnType<Guid?>("uniqueidentifier")
            //    .
            //});
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(c => c.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

                entity.Property(c => c.Dob)
                .HasColumnName("dob")
                .HasColumnType("datetime");

                entity.Property(c => c.Email)
                .IsRequired()
                .HasColumnName("email")
                .HasMaxLength(110);

                entity.Property(c => c.Fax)
                .IsRequired()
                .HasColumnName("fax")
                .HasMaxLength(50);

                entity.Property(c => c.Firstname)
                .IsRequired()
                .HasColumnName("firstname")
                .HasMaxLength(50);


                entity.Property(c => c.Gender)
                .IsRequired()
                .HasColumnName("gender")
                .HasMaxLength(50);

                entity.Property(c => c.Lastname)
                .IsRequired()
                .HasColumnName("lastname")
                .HasMaxLength(50);


                entity.Property(c => c.MainAddressId)
                .HasColumnName("mainaddressid");


                entity.Property(c => c.NewsLetterOpted)
                .HasColumnName("newsletteropted");


                entity.Property(c => c.Password)
                .IsRequired()
                .HasColumnName("password")
                .IsRequired();


                entity.Property(c => c.Telephone)
                .IsRequired()
                .HasColumnName("telephone")
                .HasMaxLength(50);


            });
        }
    }
}
