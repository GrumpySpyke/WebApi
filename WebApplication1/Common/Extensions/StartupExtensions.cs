using WebApplication1.Common.Contracts;
using WebApplication1.Common.Entity;
using WebApplication1.Repository.Services;
using WebApplication1.Repository.Services.Contracts;

namespace WebApplication1.Common.Extensions
{
    public static class StartupExtensions
    {
        public static void ConfigureOptions(this IServiceCollection services, IConfigurationRoot configuration)
        {
            IConfigurationSection dbSecretSection = configuration.GetSection("DbSecret");
            services.Configure<DbSecret>(dbSecretSection);
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<ISecretManager, FileSecretManager>();

            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IPaymentService, PaymentService>();

            
        }
    }
}
