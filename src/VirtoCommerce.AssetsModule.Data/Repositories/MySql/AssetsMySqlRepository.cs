using VirtoCommerce.AssetsModule.Data.Repositories.Common;

namespace VirtoCommerce.AssetsModule.Data.Repositories.MySql
{
    public class AssetsMySqlRepository : AssetsRepositoryCommon<AssetsMySqlDbContext>
    {
        public AssetsMySqlRepository(AssetsMySqlDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
