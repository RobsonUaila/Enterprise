using System;

namespace Enterprise.Models
{
    public class Recibo
    {
        public int Id { get; set; }
        public string Numero { get; set; } = "";
        public int FacturaId { get; set; }
        public int ClienteId { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public decimal ValorPago { get; set; }
        public string FormaPagamento { get; set; } = "Transferência";
        public string? Referencia { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CriadoEm { get; set; }
        public Cliente? Cliente { get; set; }
        public Factura? Factura { get; set; }
    }
}