using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MigrationTests.API;
using MigrationTests.API.ContextDb;
using MIgrationTests.Test.Extensions;
using System.Net.Http.Json;

namespace MIgrationTests.Test
{
    public class MigrationTest : IAsyncLifetime
    {
        private readonly Containers.SqlServerContainer _sqlContainer = new Containers.SqlServerContainer();
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        public MigrationTest()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithSqlContainer(_sqlContainer);
        }

        public async Task InitializeAsync()
        {
            await _sqlContainer.InitializeAsync();
            _client = _factory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            await _sqlContainer.DisposeAsync();
            await _factory.DisposeAsync();
        }

        [Fact]
        public async Task DeveExecutarMigracaoComSucesso()
        {
            // Arrange & Act
            var response = await _client.PostAsync("/api/database/migrate", null);

            // Assert
            Assert.True(response.IsSuccessStatusCode);

            var content = await response.Content.ReadFromJsonAsync<dynamic>();
            Assert.NotNull(content);

            // Verifica se o banco foi realmente criado tentando acessar o contexto
            using var scope = _factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MigrationTestDbContext>();

            // Verifica se o banco existe
            Assert.True(await dbContext.Database.CanConnectAsync());

            // Verifica se as tabelas foram criadas
            var tables = await dbContext.Database.SqlQuery<string>($@"
                SELECT TABLE_NAME 
                FROM INFORMATION_SCHEMA.TABLES 
                WHERE TABLE_TYPE = 'BASE TABLE'").ToListAsync();

            Assert.NotEmpty(tables);
        }
    }
}