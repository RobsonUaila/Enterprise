using Enterprise.Data;
using Enterprise.Models;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using Charting = System.Windows.Forms.DataVisualization.Charting;

namespace Enterprise.Forms
{
    public class FormDashboard : Form
    {
        private DashBoardData? _dashboard;
        private DateTime _dataInicio;
        private DateTime _dataFim;

        // Componentes do formulário
        private Guna2Panel panelFiltros;
        private Guna2Panel panelCards;
        private Guna2Panel panelGraficos;
        private Guna2DateTimePicker dtpInicio;
        private Guna2DateTimePicker dtpFim;
        private Guna2Button btnFiltrar;
        private Guna2Button btnAtualizar;

        // Cards
        private Guna2Panel cardReceita;
        private Guna2Panel cardDespesa;
        private Guna2Panel cardLucro;
        private Guna2Panel cardSaldo;
        private Label lblReceita;
        private Label lblDespesa;
        private Label lblLucro;
        private Label lblSaldo;

        // Gráficos
        private Charting.Chart chartTendencia;
        private Charting.Chart chartDespesas;
        private Charting.Chart chartReceitas;

        public FormDashboard()
        {
            InicializarDatas();
            ConfigurarFormulario();
            CriarPainelFiltros();
            CriarPainelCards();
            CriarPainelGraficos();
            AdicionarControlesAoFormulario();

            // Usar Shown em vez de Load para garantir que tudo está renderizado
            this.Shown += (s, e) => CarregarDashboard();
        }

        private void InicializarDatas()
        {
            _dataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _dataFim = DateTime.Now;
        }

        private void ConfigurarFormulario()
        {
            this.Text = "Dashboard Financeiro";
            this.Size = new Size(1350, 800);
            this.BackColor = Color.FromArgb(240, 242, 245);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new Size(1200, 700);
            this.AutoScroll = true;
        }

        private void AdicionarControlesAoFormulario()
        {
            this.Controls.Clear();
            this.Controls.Add(panelFiltros);
            this.Controls.Add(panelCards);
            this.Controls.Add(panelGraficos);
        }

        private void CriarPainelFiltros()
        {
            panelFiltros = new Guna2Panel
            {
                Location = new Point(20, 20),
                Size = new Size(1310, 70),
                FillColor = Color.White,
                BorderRadius = 12,
                BorderColor = Color.FromArgb(220, 224, 230),
                BorderThickness = 1,
                Padding = new Padding(20, 15, 20, 15)
            };

            var lblPeriodo = new Label
            {
                Text = "PERIODO:",
                Location = new Point(10, 18),
                Size = new Size(70, 25),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 120)
            };

            dtpInicio = new Guna2DateTimePicker
            {
                Location = new Point(80, 12),
                Size = new Size(130, 36),
                Value = _dataInicio,
                Format = DateTimePickerFormat.Short,
                BorderRadius = 6
            };

            var lblAte = new Label
            {
                Text = "Até",
                Location = new Point(220, 18),
                Size = new Size(30, 25),
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 100, 120)
            };

            dtpFim = new Guna2DateTimePicker
            {
                Location = new Point(260, 12),
                Size = new Size(130, 36),
                Value = _dataFim,
                Format = DateTimePickerFormat.Short,
                BorderRadius = 6
            };

            btnFiltrar = new Guna2Button
            {
                Text = "Filtrar",
                Location = new Point(410, 12),
                Size = new Size(100, 36),
                BorderRadius = 8,
                FillColor = Color.FromArgb(0, 122, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9)
            };
            btnFiltrar.Click += (s, e) => {
                _dataInicio = dtpInicio.Value;
                _dataFim = dtpFim.Value;
                CarregarDashboard();
            };

            btnAtualizar = new Guna2Button
            {
                Text = "Actualizar",
                Location = new Point(520, 12),
                Size = new Size(110, 36),
                BorderRadius = 8,
                FillColor = Color.FromArgb(52, 199, 89),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 9)
            };
            btnAtualizar.Click += (s, e) => CarregarDashboard();

            panelFiltros.Controls.Add(lblPeriodo);
            panelFiltros.Controls.Add(dtpInicio);
            panelFiltros.Controls.Add(lblAte);
            panelFiltros.Controls.Add(dtpFim);
            panelFiltros.Controls.Add(btnFiltrar);
            panelFiltros.Controls.Add(btnAtualizar);
        }

        private void CriarPainelCards()
        {
            panelCards = new Guna2Panel
            {
                Location = new Point(20, 110),
                Size = new Size(1310, 120),
                FillColor = Color.Transparent
            };

            int cardWidth = 312;
            int cardSpacing = 20;

            cardReceita = CriarCard("RECEITA TOTAL", out lblReceita, 0, cardWidth, Color.FromArgb(52, 199, 89));
            cardDespesa = CriarCard("DESPESA TOTAL", out lblDespesa, 1, cardWidth, Color.FromArgb(255, 59, 48));
            cardLucro = CriarCard("LUCRO LIQUIDO", out lblLucro, 2, cardWidth, Color.FromArgb(0, 122, 255));
            cardSaldo = CriarCard("SALDO ATUAL", out lblSaldo, 3, cardWidth, Color.FromArgb(155, 89, 182));

            panelCards.Controls.Add(cardReceita);
            panelCards.Controls.Add(cardDespesa);
            panelCards.Controls.Add(cardLucro);
            panelCards.Controls.Add(cardSaldo);
        }

        private Guna2Panel CriarCard(string titulo, out Label valorLabel, int posicao, int width, Color cor)
        {
            int cardSpacing = 20;
            int x = posicao * (width + cardSpacing);

            var card = new Guna2Panel
            {
                Location = new Point(x, 0),
                Size = new Size(width, 110),
                BorderRadius = 12,
                FillColor = Color.White,
                BorderColor = Color.FromArgb(220, 224, 230),
                BorderThickness = 1
            };

            var linhaCor = new Guna2Panel
            {
                Location = new Point(0, 0),
                Size = new Size(width, 5),
                FillColor = cor,
                BorderRadius = 5
            };

            var lblTitulo = new Label
            {
                Text = titulo,
                Location = new Point(15, 20),
                Size = new Size(width - 30, 25),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 120)
            };

            valorLabel = new Label
            {
                Text = "0,00 MT",
                Location = new Point(15, 50),
                Size = new Size(width - 30, 40),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = cor,
                TextAlign = ContentAlignment.MiddleLeft
            };

            card.Controls.Add(linhaCor);
            card.Controls.Add(lblTitulo);
            card.Controls.Add(valorLabel);

            return card;
        }

        private void CriarPainelGraficos()
        {
            panelGraficos = new Guna2Panel
            {
                Location = new Point(20, 250),
                Size = new Size(1310, 500),
                FillColor = Color.Transparent
            };

            var panelTendencia = CriarPainelGrafico("Tendencia Mensal", 0, 0, 640, 480);
            chartTendencia = CriarGraficoTendencia();
            panelTendencia.Controls.Add(chartTendencia);

            var panelDespesasChart = CriarPainelGrafico("Despesas por Categoria", 670, 0, 640, 230);
            chartDespesas = CriarGraficoPizza();
            panelDespesasChart.Controls.Add(chartDespesas);

            var panelReceitasChart = CriarPainelGrafico("Receitas por Mes", 670, 250, 640, 230);
            chartReceitas = CriarGraficoBarras();
            panelReceitasChart.Controls.Add(chartReceitas);

            panelGraficos.Controls.Add(panelTendencia);
            panelGraficos.Controls.Add(panelDespesasChart);
            panelGraficos.Controls.Add(panelReceitasChart);
        }

        private Guna2Panel CriarPainelGrafico(string titulo, int x, int y, int width, int height)
        {
            var panel = new Guna2Panel
            {
                Location = new Point(x, y),
                Size = new Size(width, height),
                BorderRadius = 12,
                FillColor = Color.White,
                BorderColor = Color.FromArgb(220, 224, 230),
                BorderThickness = 1,
                Padding = new Padding(15, 40, 15, 15)
            };

            var lblTitulo = new Label
            {
                Text = titulo,
                Location = new Point(15, 12),
                Size = new Size(width - 30, 25),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 30, 45)
            };

            panel.Controls.Add(lblTitulo);
            return panel;
        }

        private Charting.Chart CriarGraficoTendencia()
        {
            var chart = CriarChartBase();

            var area = new Charting.ChartArea("Tendencia") { BackColor = Color.White };
            area.AxisX.Title = "Mes";
            area.AxisX.TitleFont = new Font("Segoe UI", 9);
            area.AxisX.Interval = 1;
            area.AxisY.Title = "Valor (MT)";
            area.AxisY.TitleFont = new Font("Segoe UI", 9);
            area.AxisY.LabelStyle.Format = "N0";
            chart.ChartAreas.Add(area);

            chart.Series.Add(CriarSerie("Receita", Charting.SeriesChartType.Column, Color.FromArgb(52, 199, 89)));
            chart.Series.Add(CriarSerie("Despesa", Charting.SeriesChartType.Column, Color.FromArgb(255, 59, 48)));

            var legend = new Charting.Legend("Legenda") { Docking = Charting.Docking.Bottom };
            chart.Legends.Add(legend);

            return chart;
        }

        private Charting.Chart CriarGraficoPizza()
        {
            var chart = CriarChartBase();
            chart.ChartAreas.Add(new Charting.ChartArea("Pizza") { BackColor = Color.White });

            var serie = new Charting.Series("Despesas")
            {
                ChartType = Charting.SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "{0:N0} MT",
                Font = new Font("Segoe UI", 8)
            };
            chart.Series.Add(serie);

            return chart;
        }

        private Charting.Chart CriarGraficoBarras()
        {
            var chart = CriarChartBase();

            var area = new Charting.ChartArea("Barras") { BackColor = Color.White };
            area.AxisX.Title = "Mes";
            area.AxisX.Interval = 1;
            area.AxisY.Title = "Valor (MT)";
            area.AxisY.LabelStyle.Format = "N0";
            chart.ChartAreas.Add(area);

            chart.Series.Add(CriarSerie("Receitas", Charting.SeriesChartType.Column, Color.FromArgb(0, 122, 255)));

            return chart;
        }

        private Charting.Chart CriarChartBase()
        {
            return new Charting.Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
        }

        private Charting.Series CriarSerie(string nome, Charting.SeriesChartType tipo, Color cor)
        {
            return new Charting.Series(nome)
            {
                ChartType = tipo,
                Color = cor
            };
        }

        private void CarregarDashboard()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // Para teste, comente a linha abaixo e descomente os dados de teste
                _dashboard = AppDataConnection.GetDashboardData(_dataInicio, _dataFim);

                // DADOS DE TESTE (descomente se quiser testar sem banco de dados)
                /*
                _dashboard = new DashBoardData
                {
                    ReceitaTotal = 150000,
                    DespesaTotal = 85000,
                    LucroTotal = 65000,
                    SaldoAtual = 250000,
                    ReceitasPorMes = new List<ChartData>
                    {
                        new ChartData { Label = "Jan", Valor = 25000 },
                        new ChartData { Label = "Fev", Valor = 32000 },
                        new ChartData { Label = "Mar", Valor = 28000 }
                    },
                    DespesasPorCategoria = new List<ChartData>
                    {
                        new ChartData { Label = "Material", Valor = 30000 },
                        new ChartData { Label = "Mao Obra", Valor = 35000 },
                        new ChartData { Label = "Transporte", Valor = 20000 }
                    }
                };
                */

                if (_dashboard == null)
                {
                    MessageBox.Show("Nao foi possivel carregar os dados do dashboard.", "Aviso",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                lblReceita.Text = $"{_dashboard.ReceitaTotal:N2} MT";
                lblDespesa.Text = $"{_dashboard.DespesaTotal:N2} MT";
                lblLucro.Text = $"{_dashboard.LucroTotal:N2} MT";
                lblSaldo.Text = $"{_dashboard.SaldoAtual:N2} MT";

                AtualizarGraficoTendencia();
                AtualizarGraficoDespesas();
                AtualizarGraficoReceitas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dashboard: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void AtualizarGraficoTendencia()
        {
            if (_dashboard?.ReceitasPorMes == null || _dashboard?.DespesasPorCategoria == null)
                return;

            chartTendencia.Series["Receita"].Points.Clear();
            chartTendencia.Series["Despesa"].Points.Clear();

            foreach (var item in _dashboard.ReceitasPorMes)
                chartTendencia.Series["Receita"].Points.AddXY(item.Label, item.Valor);

            foreach (var item in _dashboard.DespesasPorCategoria)
                chartTendencia.Series["Despesa"].Points.AddXY(item.Label, item.Valor);
        }

        private void AtualizarGraficoDespesas()
        {
            if (_dashboard?.DespesasPorCategoria == null) return;

            chartDespesas.Series["Despesas"].Points.Clear();
            foreach (var item in _dashboard.DespesasPorCategoria)
                chartDespesas.Series["Despesas"].Points.AddXY(item.Label, item.Valor);
        }

        private void AtualizarGraficoReceitas()
        {
            if (_dashboard?.ReceitasPorMes == null) return;

            chartReceitas.Series["Receitas"].Points.Clear();
            foreach (var item in _dashboard.ReceitasPorMes)
                chartReceitas.Series["Receitas"].Points.AddXY(item.Label, item.Valor);
        }
    }
}