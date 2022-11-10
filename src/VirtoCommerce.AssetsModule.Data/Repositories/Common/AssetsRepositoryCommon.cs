using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.AssetsModule.Data.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.AssetsModule.Data.Repositories.Common
{
    public class AssetsRepositoryCommon<T> : DbContextRepositoryBase<T>, IAssetsRepository where T : DbContextWithTriggers
    {
        public AssetsRepositoryCommon(T dbContext) : base(dbContext)
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
