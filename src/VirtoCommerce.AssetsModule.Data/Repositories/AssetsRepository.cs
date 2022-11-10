using VirtoCommerce.AssetsModule.Data.Repositories.Common;

namespace VirtoCommerce.AssetsModule.Data.Repositories
{
    public class AssetsRepository : AssetsRepositoryCommon<AssetsDbContext>
    {
        public AssetsRepository(AssetsDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
