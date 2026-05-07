namespace Enterprise.Models
{
    public class ItemDocumento
    {
        public int Id { get; set; }
        public int? ServicoId { get; set; }
        public string Descricao { get; set; } = "";
        public string Unidade { get; set; } = "un";
        public decimal Quantidade { get; set; } = 1;
        public decimal PrecoUnitario { get; set; }
        public decimal Desconto { get; set; } = 0;
        public int Ordem { get; set; } = 0;
        public decimal SubTotal => Quantidade * PrecoUnitario;
        public decimal ValorDesconto => SubTotal * (Desconto / 100);
        public decimal Total => SubTotal - ValorDesconto;
        public Servico? Servico { get; set; }
    }
}