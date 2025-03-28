using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MigrationTests.API;
using MigrationTests.API.ContextDb;

namespace MIgrationTests.Test
{
    public static class WebApplicationFactoryExtensions
    {
        public static WebApplicationFactory<Program> WithSqlContainer(
            this WebApplicationFactory<Program> factory,
            SqlServerContainerFacatory sqlContainer)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove o DbContext registrado
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<MigrationTestDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Adiciona o DbContext com a connection string do container
                    services.AddDbContext<MigrationTestDbContext>(options =>
                        options.UseSqlServer(sqlContainer.GetConnectionString()));
                });
            });
        }
    }
} 