using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.AssetsModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.AssetsModule.Data.Repositories.MySql
{
    public class AssetsMySqlRepository : DbContextRepositoryBase<AssetsMySqlDbContext>, IAssetsRepository
    {
        public AssetsMySqlRepository(AssetsMySqlDbContext dbContext)
            : base(dbContext)
        {
        }
       
        public IQueryable<AssetEntryEntity> AssetEntries => DbContext.Set<AssetEntryEntity>();

        public async Task<ICollection<AssetEntryEntity>> GetAssetsByIdsAsync(IEnumerable<string> ids)
        {
            if (ids.IsNullOrEmpty())
            {
                return new List<AssetEntryEntity>();
            }

            return await AssetEntries.Where(x => ids.Contains(x.Id)).ToListAsync();
        }
    }
}
