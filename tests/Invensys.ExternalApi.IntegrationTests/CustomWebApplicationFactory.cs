using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Invensys.ExternalApi.PaySpace;
using Invensys.ExternalApi.Sage300;
using Testing.Emulator;

namespace Invensys.ExternalApi.IntegrationTests;

public class CustomWebApplicationFactory() : WebApplicationFactory<Program>
{
   protected override void ConfigureWebHost(IWebHostBuilder builder)
   {
      builder.ConfigureAppConfiguration(config =>
      {
         config.AddJsonFile("appsettings.json").AddEnvironmentVariables();
         config.AddJsonFile("appsettings.development.json").AddEnvironmentVariables();
      });

      builder.ConfigureTestServices(services =>
      {
         var serviceProvider = services.BuildServiceProvider();
         var configuration = serviceProvider.GetRequiredService<IConfiguration>();

         services.AddHttpClient();
         services.AddPaySpaceApiServices(configuration);
         services.AddSage300ApiServices();
      });
   }
}
