using Microsoft.Extensions.Configuration;

namespace VirtoCommerce.AssetsModule.Web.Extensions
{
	internal static class ConfigurationExtensions
	{
		public static bool UseMySqlProviderForModule(this IConfiguration config, string moduleId)
		{
			// TODO: Read config settings related to the module
			return true;
		}

		public static string GetModuleConnectionString(this IConfiguration configuration, string moduleId)
		{
			return configuration.GetConnectionString(moduleId) ?? configuration.GetConnectionString("VirtoCommerce");
		}
	}
}
