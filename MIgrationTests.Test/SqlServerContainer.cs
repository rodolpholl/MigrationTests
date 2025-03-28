using Testcontainers.MsSql;

namespace MIgrationTests.Test
{
    public class SqlServerContainer : IAsyncDisposable
    {
        private readonly MsSqlContainer _container;

        public SqlServerContainer()
        {
            _container = new MsSqlBuilder()
                .WithPassword("Teste@2025")
                .Build();
        }

        public string GetConnectionString()
        {
            return _container.GetConnectionString();
        }

        public async Task InitializeAsync()
        {
            await _container.StartAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _container.DisposeAsync();
        }
    }
}