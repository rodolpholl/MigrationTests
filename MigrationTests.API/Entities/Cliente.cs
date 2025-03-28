namespace MigrationTests.API.Entities
{
    public class Cliente
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Telefone { get; set; }
    }
}
