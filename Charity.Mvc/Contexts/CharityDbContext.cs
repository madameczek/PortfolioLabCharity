using Charity.Mvc.Models.DbModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Charity.Mvc.Contexts
{
    public class CharityDbContext : IdentityDbContext
    {
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<InstitutionModel> Institutions { get; set; }
        public new DbSet<CharityUser> Users { get; set; }
        public new DbSet<IdentityRole> Roles { get; set; }
        public DbSet<DonationModel> Donations { get; set; }
        public DbSet<CategoryDonationModel> CategoryDonation { get; set; }

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
            modelBuilder.Entity<CategoryModel>().HasData(
                new CategoryModel
                {
                    Id = 1,
                    Name = "ubrania, które nadają się do ponownego użycia",
                },
                new CategoryModel
                {
                    Id = 2,
                    Name = "ubrania, do wyrzucenia",
                },
                new CategoryModel
                {
                    Id = 3,
                    Name = "zabawki",
                },
                new CategoryModel
                {
                    Id = 4,
                    Name = "książki",
                },
                new CategoryModel
                {
                    Id = 5,
                    Name = "inne",
                });
            modelBuilder.Entity<InstitutionModel>().HasData(
               new InstitutionModel
               {
                   Id = 1,
                   Name = "Fundacja \"Bez domu\"",
                   Description = "Cel i misja: Pomoc dla osób nie posiadających miejsca zamieszkania"
               }, new InstitutionModel
               {
                   Id = 2,
                   Name = "Fundacja \"Dla dzieci\"",
                   Description = "Cel i misja: Pomoc osobom znajdującym się w trudnej sytuacji życiowej"
               });
        }
    }
}
