using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace VirtoCommerce.AssetsModule.Data.Repositories.MySql
{
    public class DesignTimeMySqlDbContextFactory : IDesignTimeDbContextFactory<AssetsMySqlDbContext>
    {
        public AssetsMySqlDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AssetsMySqlDbContext>();

            builder.UseMySql("server=localhost;user=root;password=virto;database=VirtoCommerce3;", new MySqlServerVersion(new Version(5, 7)));
            return new AssetsMySqlDbContext(builder.Options);
        }
    }
}
