using System;
using System.Collections.Generic;
using System.Linq;

namespace Enterprise.Models
{
    public class Factura
    {
        public int Id { get; set; }
        public string Numero { get; set; } = "";
        public int ClienteId { get; set; }
        public int? CotacaoId { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public DateTime? DataVencimento { get; set; }
        public string? LocalObra { get; set; }
        public string? Observacoes { get; set; }
        public decimal Iva { get; set; } = 16;
        
        public DateTime CriadoEm { get; set; }

        public List<ItemDocumento> Itens { get; set; } = new List<ItemDocumento>();

        public decimal SubTotal => Itens.Sum(i => i.Total);
        public decimal ValorIva => SubTotal * (Iva / 100);
        public decimal Total => SubTotal + ValorIva;
        public decimal TotalPago { get; set; }
        public decimal TotalEmDivida => Total - TotalPago;

       

        public Cliente? Cliente { get; set; }
        public Cotacao? Cotacao { get; set; }
    }
}