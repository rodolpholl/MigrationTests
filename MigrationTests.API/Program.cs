using Microsoft.EntityFrameworkCore;
using MigrationTests.API.ContextDb;

namespace MigrationTests.API
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Configure DbContext
            builder.Services.AddDbContext<MigrationTestDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapPost("/api/database/migrate", async (MigrationTestDbContext dbContext) =>
            {
                try
                {
                    await dbContext.Database.MigrateAsync();
                    return Results.Ok(new { message = "Migração executada com sucesso!" });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = $"Erro ao executar migração: {ex.Message}" });
                }
            })
            .WithName("ExecuteMigration")
            .WithOpenApi();

            app.Run();
        }
    }
}
