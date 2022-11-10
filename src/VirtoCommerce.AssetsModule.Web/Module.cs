using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using EntityFrameworkCore.Triggers;
using VirtoCommerce.AssetsModule.Core.Assets;
using VirtoCommerce.AssetsModule.Data.Repositories;
using VirtoCommerce.AssetsModule.Data.Repositories.MySql;
using VirtoCommerce.AssetsModule.Data.Services;
using VirtoCommerce.AssetsModule.Web.Extensions;
using VirtoCommerce.AssetsModule.Web.Swagger;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Data.Extensions;

namespace VirtoCommerce.AssetsModule.Web
{
    public class Module : IModule, IHasConfiguration
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public IConfiguration Configuration { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
	        var useMySqlProvider = Configuration.UseMySqlProviderForModule(ModuleInfo.Id);
	        var connectionString = Configuration.GetModuleConnectionString(ModuleInfo.Id);

            serviceCollection.AddSwaggerGen(c =>
            {
                c.OperationFilter<FileUploadOperationFilter>();
            });

            if (useMySqlProvider)
            {
	            serviceCollection.AddDbContext<AssetsMySqlDbContext>(options =>
	            {
		            options.UseMySql(connectionString, new MySqlServerVersion(new Version(5, 7)));
	            });
	            serviceCollection.AddTransient<IAssetsRepository, AssetsMySqlRepository>();
            }
            else
            {
	            serviceCollection.AddDbContext<AssetsDbContext>(options =>
	            {
		           options.UseSqlServer(connectionString);
	            });
	            serviceCollection.AddTransient<IAssetsRepository, AssetsRepository>();
            }

            serviceCollection.AddSingleton<Func<IAssetsRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IAssetsRepository>());
            serviceCollection.AddTransient<ICrudService<AssetEntry>, AssetEntryService>();
            serviceCollection.AddTransient<ISearchService<AssetEntrySearchCriteria, AssetEntrySearchResult, AssetEntry>, AssetEntrySearchService>();

        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
	        using var serviceScope = appBuilder.ApplicationServices.CreateScope();

	        var serviceProvider = serviceScope.ServiceProvider;

	        var useMySqlProvider = Configuration.UseMySqlProviderForModule(ModuleInfo.Id);
            
            using DbContextWithTriggers dbContext = useMySqlProvider ? serviceProvider.GetRequiredService<AssetsMySqlDbContext>() :
	                                                                   serviceProvider.GetRequiredService<AssetsDbContext>();

	        if (!useMySqlProvider)
	        {
		        dbContext.Database.MigrateIfNotApplied("20000000000000_UpdateAssetsV3");
            }
	        
	        dbContext.Database.EnsureCreated();
	        dbContext.Database.Migrate();
        }

        public void Uninstall()
        {
            //Nothing special here
        }


    }
}
