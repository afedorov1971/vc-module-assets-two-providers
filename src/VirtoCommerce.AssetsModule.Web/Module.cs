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
    public class Module : IModule
    {
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                c.OperationFilter<FileUploadOperationFilter>();
            });

            serviceCollection.AddDbContext<AssetsDbContext>((provider, options) =>
            {
                var configuration = provider.GetRequiredService<IConfiguration>();
                options.UseSqlServer(configuration.GetModuleConnectionString(ModuleInfo.Id));
            });

            serviceCollection.AddDbContext<AssetsMySqlDbContext>((provider, options) =>
            {
	            var configuration = provider.GetRequiredService<IConfiguration>();
	            options.UseMySql(configuration.GetModuleConnectionString(ModuleInfo.Id), new MySqlServerVersion(new Version(5, 7)));
            });


            serviceCollection.AddTransient<IAssetsRepository>(provider =>
            {
	            var configuration = provider.GetRequiredService<IConfiguration>();
	            if (configuration.UseMySqlProviderForModule(ModuleInfo.Id))
	            {
		            return new AssetsMySqlRepository(provider.GetRequiredService<AssetsMySqlDbContext>());
	            }

	            return new AssetsRepository(provider.GetRequiredService<AssetsDbContext>());
            });
            serviceCollection.AddSingleton<Func<IAssetsRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IAssetsRepository>());
            serviceCollection.AddTransient<ICrudService<AssetEntry>, AssetEntryService>();
            serviceCollection.AddTransient<ISearchService<AssetEntrySearchCriteria, AssetEntrySearchResult, AssetEntry>, AssetEntrySearchService>();

        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
	        using var serviceScope = appBuilder.ApplicationServices.CreateScope();

	        var serviceProvider = serviceScope.ServiceProvider;

	        var useMySqlProvider = serviceProvider.GetRequiredService<IConfiguration>().UseMySqlProviderForModule(ModuleInfo.Id);
            
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
