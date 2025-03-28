
using Microsoft.EntityFrameworkCore;
using MigrationTests.API.Entities;

namespace MigrationTests.API.ContextDb
{
    public class MigrationTestDbContext : DbContext
    {
        public MigrationTestDbContext(DbContextOptions options) : base(options) { }


        protected DbSet<Cliente> Clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cliente>().HasKey(x => x.Id);
            modelBuilder.Entity<Cliente>().Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn();

            modelBuilder.Entity<Cliente>().Property(x => x.Nome).HasMaxLength(150).IsRequired();
            modelBuilder.Entity<Cliente>().Property(x => x.DataNascimento);
            modelBuilder.Entity<Cliente>().Property(x => x.Telefone).HasMaxLength(30);


            base.OnModelCreating(modelBuilder);

        }

    }
}
