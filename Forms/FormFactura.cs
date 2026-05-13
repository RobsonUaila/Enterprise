using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Enterprise.Data;
using Enterprise.Models;
using Enterprise.Reports;

namespace Enterprise.Forms
{
    public partial class FormFactura : Form
    {
        private List<ItemDocumento> _itens = new List<ItemDocumento>();
        private Factura? _facturaActual = null;
        private const decimal IVA_PERCENTAGEM = 16m;
        private bool _isLoading = true;

        // ═══════════════════════════════════════════════════════════
        // CORES PADRÃO DO SISTEMA (Design System)
        // ═══════════════════════════════════════════════════════════
        private static readonly Color COR_PRIMARIA = Color.FromArgb(0, 122, 255);
        private static readonly Color COR_SUCESSO = Color.FromArgb(52, 199, 89);
        private static readonly Color COR_PERIGO = Color.FromArgb(255, 59, 48);
        private static readonly Color COR_ALERTA = Color.FromArgb(255, 149, 0);
        private static readonly Color COR_FUNDO_PAINEL = Color.FromArgb(248, 249, 252);
        private static readonly Color COR_FUNDO_FORM = Color.White;
        private static readonly Color COR_BORDA_PAINEL = Color.FromArgb(220, 224, 230);
        private static readonly Color COR_BORDA_INPUT = Color.FromArgb(200, 200, 210);
        private static readonly Color COR_TEXTO_TITULO = Color.FromArgb(30, 30, 45);
        private static readonly Color COR_TEXTO_LABEL = Color.FromArgb(100, 100, 120);
        private static readonly Color COR_TEXTO_NORMAL = Color.FromArgb(80, 80, 100);
        private static readonly Color COR_TEXTO_TOTAL = Color.FromArgb(0, 102, 204);
        private static readonly Color COR_FUNDO_INPUT_READONLY = Color.FromArgb(245, 245, 248);
        private static readonly Color COR_GRID_HEADER = Color.FromArgb(25, 25, 40);
        private static readonly Color COR_GRID_SELECAO = Color.FromArgb(210, 230, 255);
        private static readonly Color COR_GRID_LINHA_ALT = Color.FromArgb(248, 250, 255);
        private static readonly Color COR_GRID_LINHA = Color.White;
        private static readonly Color COR_GRID_GRID = Color.FromArgb(230, 230, 230);
        private static readonly Color COR_LINHA_SEPARADOR = Color.FromArgb(0, 122, 255);
        private static readonly Color COR_LINHA_DIVISORIA = Color.FromArgb(200, 200, 200);

        // Componentes do formulário
        private Guna2TextBox txtNumero;
        private Guna2TextBox txtLocalObra;
        private Guna2TextBox txtObservacoes;
        private Guna2TextBox txtPrecoItem;
        private Guna2ComboBox cmbCliente;
        private Guna2ComboBox cmbServico;
        private Guna2ComboBox cmbCotacao;
        private Guna2DateTimePicker dtpData;
        private NumericUpDown nudQuantidade;
        private NumericUpDown nudDesconto;
        private Guna2DataGridView dgvItens;
        private Guna2DataGridView dgvHistorico;
        private Guna2Button btnSalvar;
        private Guna2Button btnImprimir;
        private Guna2Button btnAdicionarItem;
        private Guna2Button btnRemoverItem;
        private Guna2Button btnConverterCotacao;
        private Label lblSubTotal;
        private Label lblIva;
        private Label lblTotal;
        private Guna2Panel panelScroll;
        private Guna2Panel panelDados;
        private Label lblTituloDados;
        private Guna2Separator linhaDados;
        private Label lblNumero;
        private Label lblData;
        private Label lblCliente;
        private Label lblCotacao;
        private Guna2Panel panelItens;
        private Label lblTituloItens;
        private Guna2Separator linhaItens;
        private Guna2Panel barraItens;
        private Label lblServicoBarra;
        private Label lblPrecoBarra;
        private Label lblQtdBarra;
        private Label lblDescBarra;
        private Guna2Panel panelRodape;
        private Label lblObs;
        private Guna2Panel panelTotais;
        private Label lblSubTxt;
        private Label lblIvaTxt;
        private Guna2Separator lineSep;
        private Label lblTotalTxt;
        private Guna2Panel panelHistorico;
        private Label lblTituloHist;
        private Guna2Button bntApagarFactura;
        private Guna2Separator linhaHist;

        // ═══════════════════════════════════════════════════════════
        // CONSTRUTOR
        // ═══════════════════════════════════════════════════════════
        public FormFactura()
        {
            InitializeComponent();
            this.Load += FormFactura_Load;
        }

        private void FormFactura_Load(object sender, EventArgs e)
        {
            _isLoading = true;

            try
            {
                AplicarEstilosCustomizados();
                ConfigurarEventos();
                CarregarDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inicializar formulário: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void ConfigurarEventos()
        {
            if (btnConverterCotacao != null)
                btnConverterCotacao.Click += BtnConverterCotacao_Click;

            if (btnAdicionarItem != null)
                btnAdicionarItem.Click += BtnAdicionarItem_Click;

            if (btnRemoverItem != null)
                btnRemoverItem.Click += BtnRemoverItem_Click;

            if (btnSalvar != null)
                btnSalvar.Click += BtnSalvar_Click;

            if (btnImprimir != null)
                btnImprimir.Click += BtnImprimir_Click;

            if (cmbServico != null)
                cmbServico.SelectedIndexChanged += cmbServico_SelectedIndexChanged;

            if (dgvHistorico != null)
                dgvHistorico.CellClick += dgvHistorico_CellClick;
        }

        private void CarregarDados()
        {
            try
            {
                // Verificar se os controlos existem
                if (cmbCliente == null)
                {
                    MessageBox.Show("Erro: Controlo cmbCliente não inicializado.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cmbServico == null)
                {
                    MessageBox.Show("Erro: Controlo cmbServico não inicializado.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (cmbCotacao == null)
                {
                    MessageBox.Show("Erro: Controlo cmbCotacao não inicializado.", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var clientes = AppDataConnection.GetClientes();
                cmbCliente.DataSource = clientes;
                cmbCliente.DisplayMember = "Nome";
                cmbCliente.ValueMember = "Id";

                var servicos = AppDataConnection.GetServicos();
                cmbServico.DataSource = servicos;
                cmbServico.DisplayMember = "Nome";
                cmbServico.ValueMember = "Id";

                var cotacoes = AppDataConnection.GetCotacoes();
                cmbCotacao.DataSource = cotacoes;
                cmbCotacao.DisplayMember = "Numero";
                cmbCotacao.ValueMember = "Id";

                if (txtNumero != null)
                    txtNumero.Text = AppDataConnection.GerarNumero("facturas", "FAT");

                if (dtpData != null)
                    dtpData.Value = DateTime.Now;

                CarregarFacturas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarFacturas()
        {
            try
            {
                if (dgvHistorico == null) return;

                var lista = AppDataConnection.GetFacturas();
                dgvHistorico.DataSource = lista;

                if (dgvHistorico.Columns.Count > 0)
                {
                    string[] ocultas = { "Id", "ClienteId", "CotacaoId", "Itens", "Cliente", "Cotacao",
                                         "SubTotal", "ValorIva", "LocalObra", "Observacoes",
                                         "DataVencimento", "CriadoEm", "Iva" };
                    foreach (var col in ocultas)
                        if (dgvHistorico.Columns.Contains(col))
                            dgvHistorico.Columns[col].Visible = false;

                    if (dgvHistorico.Columns.Contains("Numero"))
                        dgvHistorico.Columns["Numero"].HeaderText = "Nº FACTURA";
                    if (dgvHistorico.Columns.Contains("Data"))
                        dgvHistorico.Columns["Data"].HeaderText = "DATA";
                    if (dgvHistorico.Columns.Contains("Total"))
                    {
                        dgvHistorico.Columns["Total"].HeaderText = "TOTAL (MT)";
                        dgvHistorico.Columns["Total"].DefaultCellStyle.Format = "N2";
                        dgvHistorico.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dgvHistorico.Columns.Contains("TotalPago"))
                    {
                        dgvHistorico.Columns["TotalPago"].HeaderText = "PAGO (MT)";
                        dgvHistorico.Columns["TotalPago"].DefaultCellStyle.Format = "N2";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar facturas: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnConverterCotacao_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbCotacao?.SelectedItem == null)
                {
                    MessageBox.Show("Selecione uma cotação para converter!", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var cotacao = (Cotacao)cmbCotacao.SelectedItem;

                var cotacaoCompleta = AppDataConnection.GetCotacaoPorId(cotacao.Id);
                if (cotacaoCompleta == null)
                {
                    MessageBox.Show("Cotação não encontrada!", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Carregar o cliente da cotação
                if (cotacaoCompleta.ClienteId > 0 && cmbCliente != null && cmbCliente.Items.Count > 0)
                {
                    for (int i = 0; i < cmbCliente.Items.Count; i++)
                    {
                        var cliente = (Cliente)cmbCliente.Items[i];
                        if (cliente != null && cliente.Id == cotacaoCompleta.ClienteId)
                        {
                            cmbCliente.SelectedIndex = i;
                            break;
                        }
                    }
                }

                _itens = cotacaoCompleta.Itens ?? new List<ItemDocumento>();
                ActualizarGrelhaItens();

                MessageBox.Show($"Itens da cotação {cotacaoCompleta.Numero} carregados com sucesso!",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao converter cotação: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnAdicionarItem_Click(object sender, EventArgs e)
        {
            if (cmbServico?.SelectedItem is not Servico s)
            {
                MessageBox.Show("Selecione um serviço!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _itens.Add(new ItemDocumento
            {
                ServicoId = s.Id,
                Descricao = s.Nome,
                Unidade = s.Unidade,
                PrecoUnitario = s.PrecoBase,
                Quantidade = nudQuantidade?.Value ?? 1,
                Desconto = nudDesconto?.Value ?? 0,
                Servico = s
            });
            ActualizarGrelhaItens();
        }

        private void BtnRemoverItem_Click(object sender, EventArgs e)
        {
            if (dgvItens?.SelectedRows.Count > 0 &&
                MessageBox.Show("Remover item?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dgvItens.SelectedRows[0].DataBoundItem is ItemDocumento item)
                {
                    _itens.Remove(item);
                    ActualizarGrelhaItens();
                }
            }
        }

        private void ActualizarGrelhaItens()
        {
            if (dgvItens == null) return;

            dgvItens.DataSource = null;
            dgvItens.DataSource = _itens;

            if (dgvItens.Columns.Count > 0)
            {
                string[] ocultas = { "Id", "ServicoId", "Servico", "Ordem", "SubTotal", "ValorDesconto" };
                foreach (var col in ocultas)
                    if (dgvItens.Columns.Contains(col))
                        dgvItens.Columns[col].Visible = false;

                if (dgvItens.Columns.Contains("Descricao"))
                    dgvItens.Columns["Descricao"].HeaderText = "DESCRIÇÃO";
                if (dgvItens.Columns.Contains("Unidade"))
                    dgvItens.Columns["Unidade"].HeaderText = "UN.";
                if (dgvItens.Columns.Contains("Quantidade"))
                    dgvItens.Columns["Quantidade"].HeaderText = "QTD";
                if (dgvItens.Columns.Contains("PrecoUnitario"))
                {
                    dgvItens.Columns["PrecoUnitario"].HeaderText = "PREÇO UNIT. (MT)";
                    dgvItens.Columns["PrecoUnitario"].DefaultCellStyle.Format = "N2";
                    dgvItens.Columns["PrecoUnitario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvItens.Columns.Contains("Desconto"))
                    dgvItens.Columns["Desconto"].HeaderText = "DESC. %";
                if (dgvItens.Columns.Contains("Total"))
                {
                    dgvItens.Columns["Total"].HeaderText = "TOTAL (MT)";
                    dgvItens.Columns["Total"].DefaultCellStyle.Format = "N2";
                    dgvItens.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            CalcularTotais();
        }

        private void CalcularTotais()
        {
            if (lblSubTotal == null || lblIva == null || lblTotal == null) return;

            decimal sub = 0;
            foreach (var i in _itens) sub += i.Total;
            decimal vIva = sub * (IVA_PERCENTAGEM / 100);
            decimal total = sub + vIva;

            lblSubTotal.Text = sub.ToString("N2") + " MT";
            lblIva.Text = vIva.ToString("N2") + " MT";
            lblTotal.Text = total.ToString("N2") + " MT";
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (cmbCliente?.SelectedItem == null)
            {
                MessageBox.Show("Selecione um cliente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_itens.Count == 0)
            {
                MessageBox.Show("Adicione pelo menos um serviço!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // PRIMEIRO cria o objeto Factura
                var f = new Factura
                {
                    Id = _facturaActual?.Id ?? 0,
                    Numero = txtNumero?.Text ?? "",
                    ClienteId = (int)cmbCliente.SelectedValue!,
                    Data = dtpData?.Value ?? DateTime.Now,
                    DataVencimento = (dtpData?.Value ?? DateTime.Now).AddDays(30),
                    LocalObra = txtLocalObra?.Text.Trim() ?? "",
                    Observacoes = txtObservacoes?.Text.Trim() ?? "",
                    Iva = IVA_PERCENTAGEM,
                    Itens = _itens
                };

                // DEPOIS de criar 'f', pode usar suas propriedades
                AppDataConnection.SalvarFactura(f);

                AppDataConnection.RegistrarMovimentoFinanceiro(
                    "Receita",
                    "Venda",
                    $"Factura {f.Numero} - {cmbCliente.Text}",
                    f.Total,        // Agora 'f' existe
                    f.Data,         // Agora 'f' existe
                    f.Id,           // Agora 'f' existe
                    "Factura"
                );

                MessageBox.Show($"Factura {f.Numero} guardada com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CarregarFacturas();
                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            if (_facturaActual == null)
            {
                MessageBox.Show("Salve a factura primeiro!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var emp = AppDataConnection.GetEmpresa();
                if (emp != null)
                    PdfGenerator.GerarFactura(_facturaActual, emp);
                MessageBox.Show("PDF gerado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar PDF: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparFormulario()
        {
            _itens.Clear();
            _facturaActual = null;
            if (txtNumero != null)
                txtNumero.Text = AppDataConnection.GerarNumero("facturas", "FAT");
            if (txtLocalObra != null)
                txtLocalObra.Clear();
            if (txtObservacoes != null)
                txtObservacoes.Clear();
            if (dtpData != null)
                dtpData.Value = DateTime.Now;
            ActualizarGrelhaItens();
        }

        private void dgvHistorico_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || dgvHistorico == null) return;

            _facturaActual = dgvHistorico.Rows[e.RowIndex].DataBoundItem as Factura;
            if (_facturaActual != null)
            {
                if (txtNumero != null)
                    txtNumero.Text = _facturaActual.Numero;
                if (txtLocalObra != null)
                    txtLocalObra.Text = _facturaActual.LocalObra ?? "";
                if (txtObservacoes != null)
                    txtObservacoes.Text = _facturaActual.Observacoes ?? "";
                if (dtpData != null)
                    dtpData.Value = _facturaActual.Data;
                _itens = _facturaActual.Itens ?? new List<ItemDocumento>();
                ActualizarGrelhaItens();
            }
        }

        private void cmbServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;

            if (cmbServico?.SelectedItem is Servico s && txtPrecoItem != null)
                txtPrecoItem.Text = s.PrecoBase.ToString("N2");
        }

        // ═══════════════════════════════════════════════════════════
        // APLICAR ESTILOS CUSTOMIZADOS
        // ═══════════════════════════════════════════════════════════
        private void AplicarEstilosCustomizados()
        {
            if (this != null)
                this.BackColor = COR_FUNDO_FORM;

            if (btnSalvar != null)
            {
                btnSalvar.FillColor = COR_SUCESSO;
                btnSalvar.ForeColor = Color.White;
                btnSalvar.Font = new Font("Segoe UI Semibold", 10);
            }

            if (btnImprimir != null)
            {
                btnImprimir.FillColor = COR_ALERTA;
                btnImprimir.ForeColor = Color.White;
                btnImprimir.Font = new Font("Segoe UI Semibold", 10);
            }

            if (btnAdicionarItem != null)
            {
                btnAdicionarItem.FillColor = COR_SUCESSO;
                btnAdicionarItem.ForeColor = Color.White;
                btnAdicionarItem.Font = new Font("Segoe UI Semibold", 10);
            }

            if (btnRemoverItem != null)
            {
                btnRemoverItem.FillColor = COR_PERIGO;
                btnRemoverItem.ForeColor = Color.White;
                btnRemoverItem.Font = new Font("Segoe UI Semibold", 10);
            }

            if (btnConverterCotacao != null)
            {
                btnConverterCotacao.FillColor = COR_PRIMARIA;
                btnConverterCotacao.ForeColor = Color.White;
                btnConverterCotacao.Font = new Font("Segoe UI Semibold", 10);
            }

            if (dgvItens != null) ConfigurarGridEstilo(dgvItens);
            if (dgvHistorico != null) ConfigurarGridEstilo(dgvHistorico);
        }

        private void ConfigurarGridEstilo(Guna2DataGridView dgv)
        {
            if (dgv == null) return;

            dgv.BackgroundColor = COR_FUNDO_FORM;
            dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.Font = new Font("Segoe UI", 9);
            dgv.RowTemplate.Height = 38;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = COR_GRID_GRID;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = COR_GRID_HEADER;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.DefaultCellStyle.SelectionBackColor = COR_GRID_SELECAO;
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 20, 40);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = COR_GRID_LINHA_ALT;
            dgv.DefaultCellStyle.BackColor = COR_GRID_LINHA;
        }

        // ═══════════════════════════════════════════════════════════
        // INITIALIZE COMPONENT - Mantenha o código existente
        // ═══════════════════════════════════════════════════════════
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtNumero = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtLocalObra = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtObservacoes = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtPrecoItem = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbCliente = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbServico = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbCotacao = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtpData = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.nudQuantidade = new System.Windows.Forms.NumericUpDown();
            this.nudDesconto = new System.Windows.Forms.NumericUpDown();
            this.dgvItens = new Guna.UI2.WinForms.Guna2DataGridView();
            this.dgvHistorico = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.btnImprimir = new Guna.UI2.WinForms.Guna2Button();
            this.btnAdicionarItem = new Guna.UI2.WinForms.Guna2Button();
            this.btnRemoverItem = new Guna.UI2.WinForms.Guna2Button();
            this.btnConverterCotacao = new Guna.UI2.WinForms.Guna2Button();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblIva = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panelScroll = new Guna.UI2.WinForms.Guna2Panel();
            this.panelDados = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTituloDados = new System.Windows.Forms.Label();
            this.linhaDados = new Guna.UI2.WinForms.Guna2Separator();
            this.lblNumero = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.lblCotacao = new System.Windows.Forms.Label();
            this.panelItens = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTituloItens = new System.Windows.Forms.Label();
            this.linhaItens = new Guna.UI2.WinForms.Guna2Separator();
            this.barraItens = new Guna.UI2.WinForms.Guna2Panel();
            this.lblServicoBarra = new System.Windows.Forms.Label();
            this.lblPrecoBarra = new System.Windows.Forms.Label();
            this.lblQtdBarra = new System.Windows.Forms.Label();
            this.lblDescBarra = new System.Windows.Forms.Label();
            this.panelRodape = new Guna.UI2.WinForms.Guna2Panel();
            this.lblObs = new System.Windows.Forms.Label();
            this.panelTotais = new Guna.UI2.WinForms.Guna2Panel();
            this.lblSubTxt = new System.Windows.Forms.Label();
            this.lblIvaTxt = new System.Windows.Forms.Label();
            this.lineSep = new Guna.UI2.WinForms.Guna2Separator();
            this.lblTotalTxt = new System.Windows.Forms.Label();
            this.panelHistorico = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTituloHist = new System.Windows.Forms.Label();
            this.linhaHist = new Guna.UI2.WinForms.Guna2Separator();
            this.bntApagarFactura = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantidade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesconto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorico)).BeginInit();
            this.panelScroll.SuspendLayout();
            this.panelDados.SuspendLayout();
            this.panelItens.SuspendLayout();
            this.barraItens.SuspendLayout();
            this.panelRodape.SuspendLayout();
            this.panelTotais.SuspendLayout();
            this.panelHistorico.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNumero
            // 
            this.txtNumero.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.txtNumero.BorderRadius = 8;
            this.txtNumero.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNumero.DefaultText = "";
            this.txtNumero.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNumero.Location = new System.Drawing.Point(25, 95);
            this.txtNumero.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.PlaceholderText = "";
            this.txtNumero.ReadOnly = true;
            this.txtNumero.SelectedText = "";
            this.txtNumero.Size = new System.Drawing.Size(223, 37);
            this.txtNumero.TabIndex = 3;
            // 
            // txtLocalObra
            // 
            this.txtLocalObra.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtLocalObra.DefaultText = "";
            this.txtLocalObra.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtLocalObra.Location = new System.Drawing.Point(0, 0);
            this.txtLocalObra.Name = "txtLocalObra";
            this.txtLocalObra.PlaceholderText = "";
            this.txtLocalObra.SelectedText = "";
            this.txtLocalObra.Size = new System.Drawing.Size(200, 36);
            this.txtLocalObra.TabIndex = 0;
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.txtObservacoes.BorderRadius = 8;
            this.txtObservacoes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtObservacoes.DefaultText = "";
            this.txtObservacoes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtObservacoes.Location = new System.Drawing.Point(25, 42);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.PlaceholderText = "Observações da factura...";
            this.txtObservacoes.SelectedText = "";
            this.txtObservacoes.Size = new System.Drawing.Size(550, 102);
            this.txtObservacoes.TabIndex = 1;
            // 
            // txtPrecoItem
            // 
            this.txtPrecoItem.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.txtPrecoItem.BorderRadius = 6;
            this.txtPrecoItem.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPrecoItem.DefaultText = "";
            this.txtPrecoItem.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPrecoItem.Location = new System.Drawing.Point(310, 29);
            this.txtPrecoItem.Name = "txtPrecoItem";
            this.txtPrecoItem.PlaceholderText = "";
            this.txtPrecoItem.ReadOnly = true;
            this.txtPrecoItem.SelectedText = "";
            this.txtPrecoItem.Size = new System.Drawing.Size(120, 33);
            this.txtPrecoItem.TabIndex = 3;
            // 
            // cmbCliente
            // 
            this.cmbCliente.BackColor = System.Drawing.Color.Transparent;
            this.cmbCliente.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.cmbCliente.BorderRadius = 8;
            this.cmbCliente.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCliente.FocusedColor = System.Drawing.Color.Empty;
            this.cmbCliente.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCliente.ItemHeight = 30;
            this.cmbCliente.Location = new System.Drawing.Point(25, 171);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(223, 36);
            this.cmbCliente.TabIndex = 9;
            // 
            // cmbServico
            // 
            this.cmbServico.BackColor = System.Drawing.Color.Transparent;
            this.cmbServico.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.cmbServico.BorderRadius = 6;
            this.cmbServico.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbServico.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServico.FocusedColor = System.Drawing.Color.Empty;
            this.cmbServico.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbServico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbServico.ItemHeight = 30;
            this.cmbServico.Location = new System.Drawing.Point(15, 29);
            this.cmbServico.Name = "cmbServico";
            this.cmbServico.Size = new System.Drawing.Size(280, 36);
            this.cmbServico.TabIndex = 1;
            // 
            // cmbCotacao
            // 
            this.cmbCotacao.BackColor = System.Drawing.Color.Transparent;
            this.cmbCotacao.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.cmbCotacao.BorderRadius = 8;
            this.cmbCotacao.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCotacao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCotacao.FocusedColor = System.Drawing.Color.Empty;
            this.cmbCotacao.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbCotacao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCotacao.ItemHeight = 30;
            this.cmbCotacao.Location = new System.Drawing.Point(367, 171);
            this.cmbCotacao.Name = "cmbCotacao";
            this.cmbCotacao.Size = new System.Drawing.Size(220, 36);
            this.cmbCotacao.TabIndex = 11;
            // 
            // dtpData
            // 
            this.dtpData.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.dtpData.BorderRadius = 8;
            this.dtpData.Checked = true;
            this.dtpData.FillColor = System.Drawing.Color.White;
            this.dtpData.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpData.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpData.Location = new System.Drawing.Point(370, 95);
            this.dtpData.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpData.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpData.Name = "dtpData";
            this.dtpData.Size = new System.Drawing.Size(220, 36);
            this.dtpData.TabIndex = 5;
            this.dtpData.Value = new System.DateTime(2026, 5, 6, 22, 24, 45, 96);
            // 
            // nudQuantidade
            // 
            this.nudQuantidade.DecimalPlaces = 2;
            this.nudQuantidade.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudQuantidade.Location = new System.Drawing.Point(485, 37);
            this.nudQuantidade.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudQuantidade.Name = "nudQuantidade";
            this.nudQuantidade.Size = new System.Drawing.Size(80, 25);
            this.nudQuantidade.TabIndex = 5;
            this.nudQuantidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudQuantidade.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudDesconto
            // 
            this.nudDesconto.DecimalPlaces = 1;
            this.nudDesconto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudDesconto.Location = new System.Drawing.Point(614, 37);
            this.nudDesconto.Name = "nudDesconto";
            this.nudDesconto.Size = new System.Drawing.Size(80, 25);
            this.nudDesconto.TabIndex = 7;
            this.nudDesconto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dgvItens
            // 
            this.dgvItens.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.dgvItens.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvItens.ColumnHeadersHeight = 38;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItens.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItens.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvItens.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvItens.Location = new System.Drawing.Point(25, 145);
            this.dgvItens.MultiSelect = false;
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.ReadOnly = true;
            this.dgvItens.RowHeadersVisible = false;
            this.dgvItens.RowTemplate.Height = 38;
            this.dgvItens.Size = new System.Drawing.Size(1284, 255);
            this.dgvItens.TabIndex = 3;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvItens.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvItens.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(40)))));
            this.dgvItens.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvItens.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvItens.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvItens.ThemeStyle.HeaderStyle.Height = 38;
            this.dgvItens.ThemeStyle.ReadOnly = true;
            this.dgvItens.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvItens.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvItens.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItens.ThemeStyle.RowsStyle.Height = 38;
            this.dgvItens.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            this.dgvItens.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            // 
            // dgvHistorico
            // 
            this.dgvHistorico.AllowUserToAddRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.dgvHistorico.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvHistorico.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvHistorico.ColumnHeadersHeight = 38;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvHistorico.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvHistorico.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvHistorico.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvHistorico.Location = new System.Drawing.Point(25, 60);
            this.dgvHistorico.MultiSelect = false;
            this.dgvHistorico.Name = "dgvHistorico";
            this.dgvHistorico.ReadOnly = true;
            this.dgvHistorico.RowHeadersVisible = false;
            this.dgvHistorico.RowTemplate.Height = 38;
            this.dgvHistorico.Size = new System.Drawing.Size(1230, 140);
            this.dgvHistorico.TabIndex = 2;
            this.dgvHistorico.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.dgvHistorico.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvHistorico.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvHistorico.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvHistorico.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvHistorico.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvHistorico.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvHistorico.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(40)))));
            this.dgvHistorico.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvHistorico.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvHistorico.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvHistorico.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvHistorico.ThemeStyle.HeaderStyle.Height = 38;
            this.dgvHistorico.ThemeStyle.ReadOnly = true;
            this.dgvHistorico.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvHistorico.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvHistorico.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvHistorico.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvHistorico.ThemeStyle.RowsStyle.Height = 38;
            this.dgvHistorico.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            this.dgvHistorico.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            // 
            // btnSalvar
            // 
            this.btnSalvar.BorderRadius = 10;
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(199)))), ((int)(((byte)(89)))));
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(595, 37);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(160, 45);
            this.btnSalvar.TabIndex = 3;
            this.btnSalvar.Text = "💾 Salvar Factura";
            // 
            // btnImprimir
            // 
            this.btnImprimir.BorderRadius = 10;
            this.btnImprimir.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(149)))), ((int)(((byte)(0)))));
            this.btnImprimir.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnImprimir.ForeColor = System.Drawing.Color.White;
            this.btnImprimir.Location = new System.Drawing.Point(595, 95);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(160, 40);
            this.btnImprimir.TabIndex = 4;
            this.btnImprimir.Text = "📄 Gerar PDF";
            // 
            // btnAdicionarItem
            // 
            this.btnAdicionarItem.BorderRadius = 8;
            this.btnAdicionarItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(199)))), ((int)(((byte)(89)))));
            this.btnAdicionarItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnAdicionarItem.ForeColor = System.Drawing.Color.White;
            this.btnAdicionarItem.Location = new System.Drawing.Point(801, 26);
            this.btnAdicionarItem.Name = "btnAdicionarItem";
            this.btnAdicionarItem.Size = new System.Drawing.Size(115, 34);
            this.btnAdicionarItem.TabIndex = 8;
            this.btnAdicionarItem.Text = "➕ Adicionar";
            this.btnAdicionarItem.Click += new System.EventHandler(this.btnAdicionarItem_Click_1);
            // 
            // btnRemoverItem
            // 
            this.btnRemoverItem.BorderRadius = 8;
            this.btnRemoverItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(59)))), ((int)(((byte)(48)))));
            this.btnRemoverItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnRemoverItem.ForeColor = System.Drawing.Color.White;
            this.btnRemoverItem.Location = new System.Drawing.Point(950, 26);
            this.btnRemoverItem.Name = "btnRemoverItem";
            this.btnRemoverItem.Size = new System.Drawing.Size(115, 34);
            this.btnRemoverItem.TabIndex = 9;
            this.btnRemoverItem.Text = "🗑️ Remover";
            // 
            // btnConverterCotacao
            // 
            this.btnConverterCotacao.BorderRadius = 8;
            this.btnConverterCotacao.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.btnConverterCotacao.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnConverterCotacao.ForeColor = System.Drawing.Color.White;
            this.btnConverterCotacao.Location = new System.Drawing.Point(1120, 171);
            this.btnConverterCotacao.Name = "btnConverterCotacao";
            this.btnConverterCotacao.Size = new System.Drawing.Size(130, 36);
            this.btnConverterCotacao.TabIndex = 12;
            this.btnConverterCotacao.Text = "🔄 Converter";
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblSubTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblSubTotal.Location = new System.Drawing.Point(280, 15);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(170, 25);
            this.lblSubTotal.TabIndex = 1;
            this.lblSubTotal.Text = "0,00 MT";
            this.lblSubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblIva
            // 
            this.lblIva.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblIva.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblIva.Location = new System.Drawing.Point(280, 45);
            this.lblIva.Name = "lblIva";
            this.lblIva.Size = new System.Drawing.Size(170, 25);
            this.lblIva.TabIndex = 3;
            this.lblIva.Text = "0,00 MT";
            this.lblIva.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblTotal.Location = new System.Drawing.Point(260, 88);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(190, 30);
            this.lblTotal.TabIndex = 6;
            this.lblTotal.Text = "0,00 MT";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.AutoScrollMinSize = new System.Drawing.Size(0, 1100);
            this.panelScroll.BackColor = System.Drawing.Color.White;
            this.panelScroll.Controls.Add(this.panelDados);
            this.panelScroll.Controls.Add(this.panelItens);
            this.panelScroll.Controls.Add(this.panelRodape);
            this.panelScroll.Controls.Add(this.panelHistorico);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(25);
            this.panelScroll.Size = new System.Drawing.Size(1334, 749);
            this.panelScroll.TabIndex = 0;
            // 
            // panelDados
            // 
            this.panelDados.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelDados.BorderRadius = 12;
            this.panelDados.BorderThickness = 1;
            this.panelDados.Controls.Add(this.lblTituloDados);
            this.panelDados.Controls.Add(this.linhaDados);
            this.panelDados.Controls.Add(this.lblNumero);
            this.panelDados.Controls.Add(this.txtNumero);
            this.panelDados.Controls.Add(this.lblData);
            this.panelDados.Controls.Add(this.dtpData);
            this.panelDados.Controls.Add(this.lblCliente);
            this.panelDados.Controls.Add(this.cmbCliente);
            this.panelDados.Controls.Add(this.lblCotacao);
            this.panelDados.Controls.Add(this.cmbCotacao);
            this.panelDados.Controls.Add(this.btnConverterCotacao);
            this.panelDados.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelDados.Location = new System.Drawing.Point(0, 0);
            this.panelDados.Name = "panelDados";
            this.panelDados.Size = new System.Drawing.Size(1312, 219);
            this.panelDados.TabIndex = 0;
            this.panelDados.Paint += new System.Windows.Forms.PaintEventHandler(this.panelDados_Paint);
            // 
            // lblTituloDados
            // 
            this.lblTituloDados.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloDados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblTituloDados.Location = new System.Drawing.Point(25, 15);
            this.lblTituloDados.Name = "lblTituloDados";
            this.lblTituloDados.Size = new System.Drawing.Size(250, 28);
            this.lblTituloDados.TabIndex = 0;
            this.lblTituloDados.Text = "📋 DADOS DA FACTURA";
            // 
            // linhaDados
            // 
            this.linhaDados.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.linhaDados.Location = new System.Drawing.Point(25, 48);
            this.linhaDados.Name = "linhaDados";
            this.linhaDados.Size = new System.Drawing.Size(1230, 2);
            this.linhaDados.TabIndex = 1;
            // 
            // lblNumero
            // 
            this.lblNumero.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNumero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblNumero.Location = new System.Drawing.Point(28, 65);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(220, 18);
            this.lblNumero.TabIndex = 2;
            this.lblNumero.Text = "Nº FACTURA";
            // 
            // lblData
            // 
            this.lblData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblData.Location = new System.Drawing.Point(370, 65);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(220, 18);
            this.lblData.TabIndex = 4;
            this.lblData.Text = "DATA";
            // 
            // lblCliente
            // 
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblCliente.Location = new System.Drawing.Point(22, 145);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(226, 18);
            this.lblCliente.TabIndex = 8;
            this.lblCliente.Text = "CLIENTE";
            // 
            // lblCotacao
            // 
            this.lblCotacao.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCotacao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblCotacao.Location = new System.Drawing.Point(367, 145);
            this.lblCotacao.Name = "lblCotacao";
            this.lblCotacao.Size = new System.Drawing.Size(220, 18);
            this.lblCotacao.TabIndex = 10;
            this.lblCotacao.Text = "COTAÇÃO ORIGEM";
            // 
            // panelItens
            // 
            this.panelItens.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelItens.BorderRadius = 12;
            this.panelItens.BorderThickness = 1;
            this.panelItens.Controls.Add(this.lblTituloItens);
            this.panelItens.Controls.Add(this.linhaItens);
            this.panelItens.Controls.Add(this.barraItens);
            this.panelItens.Controls.Add(this.dgvItens);
            this.panelItens.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelItens.Location = new System.Drawing.Point(0, 225);
            this.panelItens.Name = "panelItens";
            this.panelItens.Size = new System.Drawing.Size(1312, 420);
            this.panelItens.TabIndex = 1;
            // 
            // lblTituloItens
            // 
            this.lblTituloItens.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloItens.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblTituloItens.Location = new System.Drawing.Point(25, 15);
            this.lblTituloItens.Name = "lblTituloItens";
            this.lblTituloItens.Size = new System.Drawing.Size(250, 28);
            this.lblTituloItens.TabIndex = 0;
            this.lblTituloItens.Text = "🛒 ITENS DA FACTURA";
            // 
            // linhaItens
            // 
            this.linhaItens.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.linhaItens.Location = new System.Drawing.Point(25, 48);
            this.linhaItens.Name = "linhaItens";
            this.linhaItens.Size = new System.Drawing.Size(1230, 2);
            this.linhaItens.TabIndex = 1;
            // 
            // barraItens
            // 
            this.barraItens.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.barraItens.BorderRadius = 8;
            this.barraItens.BorderThickness = 1;
            this.barraItens.Controls.Add(this.bntApagarFactura);
            this.barraItens.Controls.Add(this.lblServicoBarra);
            this.barraItens.Controls.Add(this.cmbServico);
            this.barraItens.Controls.Add(this.lblPrecoBarra);
            this.barraItens.Controls.Add(this.txtPrecoItem);
            this.barraItens.Controls.Add(this.lblQtdBarra);
            this.barraItens.Controls.Add(this.nudQuantidade);
            this.barraItens.Controls.Add(this.lblDescBarra);
            this.barraItens.Controls.Add(this.nudDesconto);
            this.barraItens.Controls.Add(this.btnAdicionarItem);
            this.barraItens.Controls.Add(this.btnRemoverItem);
            this.barraItens.FillColor = System.Drawing.Color.White;
            this.barraItens.Location = new System.Drawing.Point(25, 60);
            this.barraItens.Name = "barraItens";
            this.barraItens.Size = new System.Drawing.Size(1284, 79);
            this.barraItens.TabIndex = 2;
            // 
            // lblServicoBarra
            // 
            this.lblServicoBarra.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblServicoBarra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblServicoBarra.Location = new System.Drawing.Point(15, 10);
            this.lblServicoBarra.Name = "lblServicoBarra";
            this.lblServicoBarra.Size = new System.Drawing.Size(280, 14);
            this.lblServicoBarra.TabIndex = 0;
            this.lblServicoBarra.Text = "SERVIÇO";
            // 
            // lblPrecoBarra
            // 
            this.lblPrecoBarra.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblPrecoBarra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblPrecoBarra.Location = new System.Drawing.Point(310, 10);
            this.lblPrecoBarra.Name = "lblPrecoBarra";
            this.lblPrecoBarra.Size = new System.Drawing.Size(120, 14);
            this.lblPrecoBarra.TabIndex = 2;
            this.lblPrecoBarra.Text = "PREÇO UNIT.";
            // 
            // lblQtdBarra
            // 
            this.lblQtdBarra.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblQtdBarra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblQtdBarra.Location = new System.Drawing.Point(495, 10);
            this.lblQtdBarra.Name = "lblQtdBarra";
            this.lblQtdBarra.Size = new System.Drawing.Size(80, 14);
            this.lblQtdBarra.TabIndex = 4;
            this.lblQtdBarra.Text = "QTD";
            // 
            // lblDescBarra
            // 
            this.lblDescBarra.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblDescBarra.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblDescBarra.Location = new System.Drawing.Point(611, 10);
            this.lblDescBarra.Name = "lblDescBarra";
            this.lblDescBarra.Size = new System.Drawing.Size(80, 14);
            this.lblDescBarra.TabIndex = 6;
            this.lblDescBarra.Text = "DESC. %";
            // 
            // panelRodape
            // 
            this.panelRodape.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelRodape.BorderRadius = 12;
            this.panelRodape.BorderThickness = 1;
            this.panelRodape.Controls.Add(this.lblObs);
            this.panelRodape.Controls.Add(this.txtObservacoes);
            this.panelRodape.Controls.Add(this.panelTotais);
            this.panelRodape.Controls.Add(this.btnSalvar);
            this.panelRodape.Controls.Add(this.btnImprimir);
            this.panelRodape.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelRodape.Location = new System.Drawing.Point(0, 660);
            this.panelRodape.Name = "panelRodape";
            this.panelRodape.Size = new System.Drawing.Size(1312, 170);
            this.panelRodape.TabIndex = 2;
            // 
            // lblObs
            // 
            this.lblObs.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblObs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblObs.Location = new System.Drawing.Point(25, 15);
            this.lblObs.Name = "lblObs";
            this.lblObs.Size = new System.Drawing.Size(120, 20);
            this.lblObs.TabIndex = 0;
            this.lblObs.Text = "📝 OBSERVAÇÕES";
            // 
            // panelTotais
            // 
            this.panelTotais.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelTotais.BorderRadius = 10;
            this.panelTotais.BorderThickness = 1;
            this.panelTotais.Controls.Add(this.lblSubTxt);
            this.panelTotais.Controls.Add(this.lblSubTotal);
            this.panelTotais.Controls.Add(this.lblIvaTxt);
            this.panelTotais.Controls.Add(this.lblIva);
            this.panelTotais.Controls.Add(this.lineSep);
            this.panelTotais.Controls.Add(this.lblTotalTxt);
            this.panelTotais.Controls.Add(this.lblTotal);
            this.panelTotais.FillColor = System.Drawing.Color.White;
            this.panelTotais.Location = new System.Drawing.Point(780, 15);
            this.panelTotais.Name = "panelTotais";
            this.panelTotais.Size = new System.Drawing.Size(470, 140);
            this.panelTotais.TabIndex = 2;
            // 
            // lblSubTxt
            // 
            this.lblSubTxt.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSubTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblSubTxt.Location = new System.Drawing.Point(20, 15);
            this.lblSubTxt.Name = "lblSubTxt";
            this.lblSubTxt.Size = new System.Drawing.Size(150, 25);
            this.lblSubTxt.TabIndex = 0;
            this.lblSubTxt.Text = "SUBTOTAL:";
            // 
            // lblIvaTxt
            // 
            this.lblIvaTxt.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblIvaTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblIvaTxt.Location = new System.Drawing.Point(20, 45);
            this.lblIvaTxt.Name = "lblIvaTxt";
            this.lblIvaTxt.Size = new System.Drawing.Size(150, 25);
            this.lblIvaTxt.TabIndex = 2;
            this.lblIvaTxt.Text = "IVA:";
            // 
            // lineSep
            // 
            this.lineSep.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.lineSep.Location = new System.Drawing.Point(20, 78);
            this.lineSep.Name = "lineSep";
            this.lineSep.Size = new System.Drawing.Size(430, 1);
            this.lineSep.TabIndex = 4;
            // 
            // lblTotalTxt
            // 
            this.lblTotalTxt.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTotalTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblTotalTxt.Location = new System.Drawing.Point(20, 88);
            this.lblTotalTxt.Name = "lblTotalTxt";
            this.lblTotalTxt.Size = new System.Drawing.Size(150, 30);
            this.lblTotalTxt.TabIndex = 5;
            this.lblTotalTxt.Text = "TOTAL:";
            // 
            // panelHistorico
            // 
            this.panelHistorico.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelHistorico.BorderRadius = 12;
            this.panelHistorico.BorderThickness = 1;
            this.panelHistorico.Controls.Add(this.lblTituloHist);
            this.panelHistorico.Controls.Add(this.linhaHist);
            this.panelHistorico.Controls.Add(this.dgvHistorico);
            this.panelHistorico.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelHistorico.Location = new System.Drawing.Point(0, 845);
            this.panelHistorico.Name = "panelHistorico";
            this.panelHistorico.Size = new System.Drawing.Size(1309, 220);
            this.panelHistorico.TabIndex = 3;
            // 
            // lblTituloHist
            // 
            this.lblTituloHist.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloHist.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblTituloHist.Location = new System.Drawing.Point(25, 15);
            this.lblTituloHist.Name = "lblTituloHist";
            this.lblTituloHist.Size = new System.Drawing.Size(300, 28);
            this.lblTituloHist.TabIndex = 0;
            this.lblTituloHist.Text = "📜 HISTÓRICO DE FACTURAS";
            // 
            // linhaHist
            // 
            this.linhaHist.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.linhaHist.Location = new System.Drawing.Point(25, 48);
            this.linhaHist.Name = "linhaHist";
            this.linhaHist.Size = new System.Drawing.Size(1230, 2);
            this.linhaHist.TabIndex = 1;
            // 
            // btn Apagar Factura
            // 
            this.bntApagarFactura.BorderRadius = 8;
            this.bntApagarFactura.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.bntApagarFactura.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.bntApagarFactura.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.bntApagarFactura.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.bntApagarFactura.FillColor = System.Drawing.Color.Red;
            this.bntApagarFactura.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.bntApagarFactura.ForeColor = System.Drawing.Color.White;
            this.bntApagarFactura.Location = new System.Drawing.Point(1083, 26);
            this.bntApagarFactura.Name = "btnApagarFactura";
            this.bntApagarFactura.Size = new System.Drawing.Size(122, 34);
            this.bntApagarFactura.TabIndex = 10;
            this.bntApagarFactura.Text = "Apagar Factura";
            this.bntApagarFactura.Click += new System.EventHandler(this.btnApagarFactura_Click);
            // 
            // FormFactura
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1334, 749);
            this.Controls.Add(this.panelScroll);
            this.MinimumSize = new System.Drawing.Size(1200, 750);
            this.Name = "FormFactura";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestão de Facturas";
            ((System.ComponentModel.ISupportInitialize)(this.nudQuantidade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesconto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorico)).EndInit();
            this.panelScroll.ResumeLayout(false);
            this.panelDados.ResumeLayout(false);
            this.panelItens.ResumeLayout(false);
            this.barraItens.ResumeLayout(false);
            this.panelRodape.ResumeLayout(false);
            this.panelTotais.ResumeLayout(false);
            this.panelHistorico.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void panelDados_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAdicionarItem_Click_1(object sender, EventArgs e)
        {

        }

        private void btnApagarFactura_Click(object sender, EventArgs e)
        {
            if (dgvHistorico.CurrentRow == null)
            {
                MessageBox.Show("Selecione uma factura para apagar!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var factura = dgvHistorico.CurrentRow.DataBoundItem as Factura;
            if (factura == null) return;

            if (MessageBox.Show($"Tem certeza que deseja apagar a factura {factura.Numero}?\nEsta ação não pode ser desfeita.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    AppDataConnection.ApagarFactura(factura.Id);
                    MessageBox.Show("Factura apagada com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarFacturas();
                    LimparFormulario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao apagar: " + ex.Message, "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}