using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Models
{
    public class MovimentoFinanceiro
    {
        public int Id { get; set; }
        public String tipo { get; set; } = "";
        public String categoria { get; set; }="";
        public String descricao { get; set; } = "";
        public decimal valor { get; set; }
        public DateTime data { get; set; }
        public int? DocumentoId { get; set; }
        public string? documentoTipo { get; set; }
        public String? FormaPagamento { get; set; }
        public string? Referencia { get; set; }
        public string? Observacoes { get; set; }
        public DateTime CriadoEm { get; set; }

    }
}
