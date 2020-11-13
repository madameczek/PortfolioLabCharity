using Charity.Mvc.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Charity.Mvc.Context
{
    public class CharityDbContext : IdentityDbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public new DbSet<CharityUser> Users { get; set; }
        public new DbSet<IdentityRole> Roles { get; set; }

        public CharityDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "SiteManager",
                    NormalizedName = "SITEMANAGER"
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR"
                });
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "ubrania, które nadają się do ponownego użycia",
                },
                new Category
                {
                    Id = 2,
                    Name = "ubrania, do wyrzucenia",
                },
                new Category
                {
                    Id = 3,
                    Name = "zabawki",
                },
                new Category
                {
                    Id = 4,
                    Name = "książki",
                },
                new Category
                {
                    Id = 5,
                    Name = "inne",
                });
            modelBuilder.Entity<Institution>().HasData(
               new Institution
               {
                   Id = 1,
                   Name = "Fundacja \"Bez domu\"",
                   Desctiption = "Cel i misja: Pomoc dla osób nie posiadających miejsca zamieszkania"
               }, new Institution
               {
                   Id = 2,
                   Name = "Fundacja \"Dla dzieci\"",
                   Desctiption = "Cel i misja: Pomoc osobom znajdującym się w trudnej sytuacji życiowej"
               });
        }
    }
}
