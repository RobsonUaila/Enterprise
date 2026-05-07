using System;
using System.Collections.Generic;
using System.Linq;

namespace Enterprise.Models
{
    public class OrdemTrabalho
    {
        public int Id { get; set; }
        public string Numero { get; set; } = "";
        public int ClienteId { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string? LocalObra { get; set; }
        public string? Descricao { get; set; }
        public string? Observacoes { get; set; }
        public decimal Iva { get; set; } = 17;
        public string Estado { get; set; } = "Aberta";
        public DateTime CriadoEm { get; set; }

        public List<ItemDocumento> Itens { get; set; } = new List<ItemDocumento>();

        public decimal SubTotal => Itens.Sum(i => i.Total);
        public decimal ValorIva => SubTotal * (Iva / 100);
        public decimal Total => SubTotal + ValorIva;

        public Cliente? Cliente { get; set; }
    }
}