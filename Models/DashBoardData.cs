using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Models
{
   public class DashBoardData
    {
        // Métricas principais
        public decimal ReceitaTotal { get; set; }
        public decimal DespesaTotal { get; set; }
        public decimal LucroTotal { get; set; }
        public decimal MargemLucro { get; set; }
        public decimal SaldoAtual { get; set; }

        // Contagens
        public int TotalVendas { get; set; }
        public int TotalServicos { get; set; }

        // Dados para gráficos
        public List<DadoGrafico> ReceitasPorMes { get; set; } = new();
        public List<DadoGrafico> DespesasPorCategoria { get; set; } = new();
        public List<DadoAtividade> AtividadesRecentes { get; set; } = new();
    }

    public class DadoGrafico
    {
        public string Label { get; set; } = "";
        public decimal Valor { get; set; }
    }

    public class DadoAtividade
    {
        public string Nome { get; set; } = "";
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; } = "";
    }
}