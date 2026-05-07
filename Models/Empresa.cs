namespace Enterprise.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string? Nuit { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? LogoPath { get; set; }
        public string? ContaBancaria { get; set; }
        public string? Banco { get; set; }
        public string MoedaSimbolo { get; set; } = "MT";
    }
}