using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.AssetsModule.Data.Model;

namespace VirtoCommerce.AssetsModule.Data.Repositories.MySql
{
    public class AssetsMySqlDbContext : DbContextWithTriggers
    {
        public AssetsMySqlDbContext(DbContextOptions<AssetsMySqlDbContext> options)
            : base(options)
        {
        }

        protected AssetsMySqlDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssetEntryEntity>().ToTable("AssetEntry").HasKey(x => x.Id);
            modelBuilder.Entity<AssetEntryEntity>().Property(x => x.Id).HasMaxLength(128).ValueGeneratedOnAdd();
            modelBuilder.Entity<AssetEntryEntity>().Property(x => x.CreatedBy).HasMaxLength(64);
            modelBuilder.Entity<AssetEntryEntity>().Property(x => x.ModifiedBy).HasMaxLength(64);
            modelBuilder.Entity<AssetEntryEntity>().Property(x => x.RelativeUrl).HasMaxLength(512);
            modelBuilder.Entity<AssetEntryEntity>().Property(x => x.Name).HasMaxLength(256);

            modelBuilder.Entity<AssetEntryEntity>().HasIndex(x => new { x.RelativeUrl, x.Name })
                .IsUnique(false)
                .HasDatabaseName("IX_AssetEntry_RelativeUrl_Name");
            base.OnModelCreating(modelBuilder);
        }

    }


}
