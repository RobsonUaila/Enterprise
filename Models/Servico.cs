using System;

namespace Enterprise.Models
{
    public class Servico
    {
        public int Id { get; set; }
        public int? CategoriaId { get; set; }
        public string Codigo { get; set; } = "";
        public string Nome { get; set; } = "";
        //public string Descricao { get; set; } = "";
        public string Unidade { get; set; } = "un";
        public decimal PrecoBase { get; set; }
        public bool Activo { get; set; } = true;
        public String? CategoriaNome { get; set; } 
        public DateTime CriadoEm { get; set; } = DateTime.Now;
        public override string ToString() => Nome;
    }

    public class CategoriaServico
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string? Descricao { get; set; }
        public override string ToString() => Nome;
    }
}