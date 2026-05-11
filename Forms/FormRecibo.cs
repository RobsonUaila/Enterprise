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
    public partial class FormRecibo : Form
    {
        private Recibo? _reciboActual = null;
        private const decimal IVA_PERCENTAGEM = 16m;

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
        private Guna2TextBox txtReferencia;
        private Guna2TextBox txtObservacoes;
        private Guna2ComboBox cmbCliente;
        private Guna2ComboBox cmbFactura;
        private Guna2ComboBox cmbFormaPagamento;
        private Guna2DateTimePicker dtpData;
        private NumericUpDown nudValor;
        private Guna2DataGridView dgvRecibos;
        private Guna2Button btnNovo;
        private Guna2Button btnSalvar;
        private Guna2Button btnImprimir;
        private Label lblDivida;
        private Guna2Panel panelScroll;
        private Guna2Panel panelForm;
        private Label lblTituloForm;
        private Guna2Separator linhaForm;
        private Label lblNumero;
        private Label lblData;
        private Label lblCliente;
        private Label lblFactura;
        private Label lblValor;
        private Label lblFormaPagamento;
        private Label lblReferencia;
        private Label lblObservacoes;
        private Guna2Panel panelHistorico;
        private Label lblTituloHist;
        private Guna2Separator linhaHist;
        private Guna2Panel panelTotais;
        private Guna2Button btnApagarRecibo;
        private Label lblDividaTitulo;

        public FormRecibo()
        {
            InitializeComponent();
            AplicarEstilosCustomizados();
            CarregarDados();
        }

        private void CarregarDados()
        {
            try
            {
                var clientes = AppDataConnection.GetClientes();
                cmbCliente.DataSource = clientes;
                cmbCliente.DisplayMember = "Nome";
                cmbCliente.ValueMember = "Id";
                cmbCliente.SelectedIndexChanged += cmbCliente_SelectedIndexChanged;

                cmbFormaPagamento.Items.AddRange(new object[]
                    { "Dinheiro", "Transferência", "Cheque", "M-Pesa", "e-Mola" });

                if (cmbFormaPagamento.Items.Count > 1)
                    cmbFormaPagamento.SelectedIndex = 1;

                txtNumero.Text = AppDataConnection.GerarNumero("recibos", "REC");
                dtpData.Value = DateTime.Now;

                CarregarRecibos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }

        private void cmbCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedValue is int clienteId && clienteId > 0)
            {
                var facturas = AppDataConnection.GetFacturasPendentes(clienteId);

                if (facturas.Count == 0)
                {
                    cmbFactura.DataSource = null;
                    lblDivida.Text = "Em Dívida: 0,00 MT";
                    nudValor.Value = 0;
                    MessageBox.Show("Este cliente não tem facturas pendentes!", "Informação",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                cmbFactura.DataSource = null;
                cmbFactura.DataSource = facturas;
                cmbFactura.DisplayMember = "Numero";
                cmbFactura.ValueMember = "Id";

                cmbFactura.SelectedIndexChanged -= cmbFactura_SelectedIndexChanged;
                cmbFactura.SelectedIndexChanged += cmbFactura_SelectedIndexChanged;
            }
        }

        private void cmbFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFactura.SelectedItem is Factura f)
            {
                lblDivida.Text = "Em Dívida: " + f.TotalEmDivida.ToString("N2") + " MT";
                decimal valorMaximo = Math.Min(f.TotalEmDivida, nudValor.Maximum);
                nudValor.Value = valorMaximo > 0 ? valorMaximo : 0;
            }
        }

        private void CarregarRecibos()
        {
            try
            {
                var lista = AppDataConnection.GetRecibos();
                dgvRecibos.DataSource = lista;

                if (dgvRecibos.Columns.Count > 0)
                {
                    string[] ocultas = { "Id", "FacturaId", "ClienteId", "Factura",
                                         "Cliente", "Referencia", "Observacoes", "CriadoEm" };
                    foreach (var col in ocultas)
                        if (dgvRecibos.Columns.Contains(col))
                            dgvRecibos.Columns[col].Visible = false;

                    if (dgvRecibos.Columns.Contains("Numero"))
                        dgvRecibos.Columns["Numero"].HeaderText = "NÚMERO";
                    if (dgvRecibos.Columns.Contains("Data"))
                        dgvRecibos.Columns["Data"].HeaderText = "DATA";
                    if (dgvRecibos.Columns.Contains("ValorPago"))
                    {
                        dgvRecibos.Columns["ValorPago"].HeaderText = "VALOR PAGO (MT)";
                        dgvRecibos.Columns["ValorPago"].DefaultCellStyle.Format = "N2";
                        dgvRecibos.Columns["ValorPago"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    if (dgvRecibos.Columns.Contains("FormaPagamento"))
                        dgvRecibos.Columns["FormaPagamento"].HeaderText = "FORMA PAG.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private bool Validar()
        {
            if (cmbCliente.SelectedItem == null)
            {
                MessageBox.Show("Selecione um cliente!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbFactura.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma factura!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (nudValor.Value <= 0)
            {
                MessageBox.Show("Informe um valor válido para o pagamento!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                var recibo = new Recibo
                {
                    Id = _reciboActual?.Id ?? 0,
                    Numero = txtNumero.Text,
                    ClienteId = (int)cmbCliente.SelectedValue,
                    FacturaId = (int)cmbFactura.SelectedValue,
                    Data = dtpData.Value,
                    ValorPago = nudValor.Value,
                    FormaPagamento = cmbFormaPagamento.Text,
                    Referencia = txtReferencia.Text.Trim(),
                    Observacoes = txtObservacoes.Text.Trim()
                };

                AppDataConnection.SalvarRecibo(recibo);

                MessageBox.Show($"Recibo {recibo.Numero} guardado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CarregarRecibos();
                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (_reciboActual == null)
            {
                MessageBox.Show("Salve o recibo primeiro!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var empresa = AppDataConnection.GetEmpresa();
                PdfGenerator.GerarRecibo(_reciboActual, empresa);
                MessageBox.Show("PDF gerado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar PDF: " + ex.Message);
            }
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            LimparFormulario();
        }

        private void LimparFormulario()
        {
            _reciboActual = null;
            txtNumero.Text = AppDataConnection.GerarNumero("recibos", "REC");
            txtReferencia.Text = "";
            txtObservacoes.Text = "";
            nudValor.Value = 0;
            dtpData.Value = DateTime.Now;

            if (cmbFormaPagamento.Items.Count > 1)
                cmbFormaPagamento.SelectedIndex = 1;

            lblDivida.Text = "Em Dívida: 0,00 MT";
        }

        private void dgvRecibos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            _reciboActual = dgvRecibos.Rows[e.RowIndex].DataBoundItem as Recibo;

            if (_reciboActual != null)
            {
                txtNumero.Text = _reciboActual.Numero;
                dtpData.Value = _reciboActual.Data;
                nudValor.Value = _reciboActual.ValorPago;
                txtReferencia.Text = _reciboActual.Referencia ?? "";
                txtObservacoes.Text = _reciboActual.Observacoes ?? "";
                cmbFormaPagamento.Text = _reciboActual.FormaPagamento;
            }
        }

        private void AplicarEstilosCustomizados()
        {
            this.BackColor = COR_FUNDO_FORM;

            btnNovo.FillColor = COR_PRIMARIA;
            btnNovo.ForeColor = Color.White;
            btnNovo.Font = new Font("Segoe UI Semibold", 10);

            btnSalvar.FillColor = COR_SUCESSO;
            btnSalvar.ForeColor = Color.White;
            btnSalvar.Font = new Font("Segoe UI Semibold", 10);

            btnImprimir.FillColor = COR_ALERTA;
            btnImprimir.ForeColor = Color.White;
            btnImprimir.Font = new Font("Segoe UI Semibold", 10);

            if (btnApagarRecibo != null)
            {
                btnApagarRecibo.FillColor = COR_PERIGO;
                btnApagarRecibo.ForeColor = Color.White;
                btnApagarRecibo.Font = new Font("Segoe UI Semibold", 10);
            }

            ConfigurarGridEstilo(dgvRecibos);
        }

        private void ConfigurarGridEstilo(Guna2DataGridView dgv)
        {
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

        private void btnApagarRecibo_Click(object sender, EventArgs e)
        {
            if (dgvRecibos.CurrentRow == null)
            {
                MessageBox.Show("Selecione um recibo para apagar!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var recibo = dgvRecibos.CurrentRow.DataBoundItem as Recibo;
            if (recibo == null) return;

            if (MessageBox.Show($"Tem certeza que deseja apagar o recibo {recibo.Numero}?\nEsta ação não pode ser desfeita.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    AppDataConnection.ApagarRecibo(recibo.Id);
                    MessageBox.Show("Recibo apagado com sucesso!", "Sucesso",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarRecibos();
                    LimparFormulario();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao apagar: " + ex.Message, "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panelForm_Paint(object sender, PaintEventArgs e) { }

        // ═══════════════════════════════════════════════════════════
        // INITIALIZE COMPONENT
        // ═══════════════════════════════════════════════════════════
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtNumero = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtReferencia = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtObservacoes = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbCliente = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbFactura = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbFormaPagamento = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dtpData = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.nudValor = new System.Windows.Forms.NumericUpDown();
            this.dgvRecibos = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnNovo = new Guna.UI2.WinForms.Guna2Button();
            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.btnImprimir = new Guna.UI2.WinForms.Guna2Button();
            this.lblDivida = new System.Windows.Forms.Label();
            this.panelScroll = new Guna.UI2.WinForms.Guna2Panel();
            this.panelForm = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTituloForm = new System.Windows.Forms.Label();
            this.linhaForm = new Guna.UI2.WinForms.Guna2Separator();
            this.lblNumero = new System.Windows.Forms.Label();
            this.lblData = new System.Windows.Forms.Label();
            this.lblCliente = new System.Windows.Forms.Label();
            this.lblFactura = new System.Windows.Forms.Label();
            this.lblFormaPagamento = new System.Windows.Forms.Label();
            this.lblValor = new System.Windows.Forms.Label();
            this.lblReferencia = new System.Windows.Forms.Label();
            this.lblObservacoes = new System.Windows.Forms.Label();
            this.panelTotais = new Guna.UI2.WinForms.Guna2Panel();
            this.lblDividaTitulo = new System.Windows.Forms.Label();
            this.panelHistorico = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTituloHist = new System.Windows.Forms.Label();
            this.linhaHist = new Guna.UI2.WinForms.Guna2Separator();
            this.btnApagarRecibo = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudValor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecibos)).BeginInit();
            this.panelScroll.SuspendLayout();
            this.panelForm.SuspendLayout();
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
            this.txtNumero.Size = new System.Drawing.Size(360, 36);
            this.txtNumero.TabIndex = 3;
            // 
            // txtReferencia
            // 
            this.txtReferencia.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.txtReferencia.BorderRadius = 8;
            this.txtReferencia.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtReferencia.DefaultText = "";
            this.txtReferencia.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtReferencia.Location = new System.Drawing.Point(25, 346);
            this.txtReferencia.Name = "txtReferencia";
            this.txtReferencia.PlaceholderText = "Nº de transferência, cheque...";
            this.txtReferencia.SelectedText = "";
            this.txtReferencia.Size = new System.Drawing.Size(360, 36);
            this.txtReferencia.TabIndex = 11;
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.txtObservacoes.BorderRadius = 8;
            this.txtObservacoes.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtObservacoes.DefaultText = "";
            this.txtObservacoes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtObservacoes.Location = new System.Drawing.Point(893, 263);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.PlaceholderText = "Observações do recibo...";
            this.txtObservacoes.SelectedText = "";
            this.txtObservacoes.Size = new System.Drawing.Size(360, 80);
            this.txtObservacoes.TabIndex = 12;
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
            this.cmbCliente.Location = new System.Drawing.Point(25, 170);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(360, 36);
            this.cmbCliente.TabIndex = 5;
            // 
            // cmbFactura
            // 
            this.cmbFactura.BackColor = System.Drawing.Color.Transparent;
            this.cmbFactura.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.cmbFactura.BorderRadius = 8;
            this.cmbFactura.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFactura.FocusedColor = System.Drawing.Color.Empty;
            this.cmbFactura.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbFactura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbFactura.ItemHeight = 30;
            this.cmbFactura.Location = new System.Drawing.Point(893, 170);
            this.cmbFactura.Name = "cmbFactura";
            this.cmbFactura.Size = new System.Drawing.Size(360, 36);
            this.cmbFactura.TabIndex = 7;
            // 
            // cmbFormaPagamento
            // 
            this.cmbFormaPagamento.BackColor = System.Drawing.Color.Transparent;
            this.cmbFormaPagamento.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.cmbFormaPagamento.BorderRadius = 8;
            this.cmbFormaPagamento.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbFormaPagamento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFormaPagamento.FocusedColor = System.Drawing.Color.Empty;
            this.cmbFormaPagamento.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbFormaPagamento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbFormaPagamento.ItemHeight = 30;
            this.cmbFormaPagamento.Location = new System.Drawing.Point(25, 263);
            this.cmbFormaPagamento.Name = "cmbFormaPagamento";
            this.cmbFormaPagamento.Size = new System.Drawing.Size(360, 36);
            this.cmbFormaPagamento.TabIndex = 9;
            // 
            // dtpData
            // 
            this.dtpData.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(210)))));
            this.dtpData.BorderRadius = 8;
            this.dtpData.Checked = true;
            this.dtpData.FillColor = System.Drawing.Color.White;
            this.dtpData.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpData.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpData.Location = new System.Drawing.Point(893, 95);
            this.dtpData.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpData.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpData.Name = "dtpData";
            this.dtpData.Size = new System.Drawing.Size(360, 36);
            this.dtpData.TabIndex = 13;
            this.dtpData.Value = new System.DateTime(2026, 5, 8, 0, 0, 0, 0);
            // 
            // nudValor
            // 
            this.nudValor.DecimalPlaces = 2;
            this.nudValor.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.nudValor.Location = new System.Drawing.Point(25, 441);
            this.nudValor.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudValor.Name = "nudValor";
            this.nudValor.Size = new System.Drawing.Size(360, 27);
            this.nudValor.TabIndex = 14;
            this.nudValor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudValor.ThousandsSeparator = true;
            // 
            // dgvRecibos
            // 
            this.dgvRecibos.AllowUserToAddRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(255)))));
            this.dgvRecibos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecibos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecibos.ColumnHeadersHeight = 38;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecibos.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRecibos.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvRecibos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvRecibos.Location = new System.Drawing.Point(25, 60);
            this.dgvRecibos.MultiSelect = false;
            this.dgvRecibos.Name = "dgvRecibos";
            this.dgvRecibos.ReadOnly = true;
            this.dgvRecibos.RowHeadersVisible = false;
            this.dgvRecibos.RowTemplate.Height = 38;
            this.dgvRecibos.Size = new System.Drawing.Size(1284, 500);
            this.dgvRecibos.TabIndex = 2;
            this.dgvRecibos.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRecibos.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvRecibos.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvRecibos.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvRecibos.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvRecibos.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvRecibos.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvRecibos.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvRecibos.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvRecibos.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRecibos.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvRecibos.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRecibos.ThemeStyle.HeaderStyle.Height = 38;
            this.dgvRecibos.ThemeStyle.ReadOnly = true;
            this.dgvRecibos.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRecibos.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRecibos.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRecibos.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvRecibos.ThemeStyle.RowsStyle.Height = 38;
            this.dgvRecibos.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRecibos.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btnNovo
            // 
            this.btnNovo.BorderRadius = 10;
            this.btnNovo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.btnNovo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnNovo.ForeColor = System.Drawing.Color.White;
            this.btnNovo.Location = new System.Drawing.Point(25, 610);
            this.btnNovo.Name = "btnNovo";
            this.btnNovo.Size = new System.Drawing.Size(110, 45);
            this.btnNovo.TabIndex = 15;
            this.btnNovo.Text = "➕ Novo";
            this.btnNovo.Click += new System.EventHandler(this.btnNovo_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.BorderRadius = 10;
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(199)))), ((int)(((byte)(89)))));
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(150, 610);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(110, 45);
            this.btnSalvar.TabIndex = 16;
            this.btnSalvar.Text = "💾 Salvar";
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnImprimir
            // 
            this.btnImprimir.BorderRadius = 10;
            this.btnImprimir.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(149)))), ((int)(((byte)(0)))));
            this.btnImprimir.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnImprimir.ForeColor = System.Drawing.Color.White;
            this.btnImprimir.Location = new System.Drawing.Point(275, 610);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(110, 45);
            this.btnImprimir.TabIndex = 17;
            this.btnImprimir.Text = "📄 PDF";
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // lblDivida
            // 
            this.lblDivida.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDivida.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(59)))), ((int)(((byte)(48)))));
            this.lblDivida.Location = new System.Drawing.Point(20, 30);
            this.lblDivida.Name = "lblDivida";
            this.lblDivida.Size = new System.Drawing.Size(430, 25);
            this.lblDivida.TabIndex = 1;
            this.lblDivida.Text = "Em Dívida: 0,00 MT";
            this.lblDivida.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.AutoScrollMinSize = new System.Drawing.Size(0, 1100);
            this.panelScroll.BackColor = System.Drawing.Color.White;
            this.panelScroll.Controls.Add(this.panelForm);
            this.panelScroll.Controls.Add(this.panelHistorico);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(25);
            this.panelScroll.Size = new System.Drawing.Size(1313, 749);
            this.panelScroll.TabIndex = 0;
            // 
            // panelForm
            // 
            this.panelForm.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelForm.BorderRadius = 12;
            this.panelForm.BorderThickness = 1;
            this.panelForm.Controls.Add(this.btnApagarRecibo);
            this.panelForm.Controls.Add(this.lblTituloForm);
            this.panelForm.Controls.Add(this.linhaForm);
            this.panelForm.Controls.Add(this.lblNumero);
            this.panelForm.Controls.Add(this.txtNumero);
            this.panelForm.Controls.Add(this.lblData);
            this.panelForm.Controls.Add(this.dtpData);
            this.panelForm.Controls.Add(this.lblCliente);
            this.panelForm.Controls.Add(this.cmbCliente);
            this.panelForm.Controls.Add(this.lblFactura);
            this.panelForm.Controls.Add(this.cmbFactura);
            this.panelForm.Controls.Add(this.lblFormaPagamento);
            this.panelForm.Controls.Add(this.cmbFormaPagamento);
            this.panelForm.Controls.Add(this.lblValor);
            this.panelForm.Controls.Add(this.nudValor);
            this.panelForm.Controls.Add(this.lblReferencia);
            this.panelForm.Controls.Add(this.txtReferencia);
            this.panelForm.Controls.Add(this.lblObservacoes);
            this.panelForm.Controls.Add(this.txtObservacoes);
            this.panelForm.Controls.Add(this.btnNovo);
            this.panelForm.Controls.Add(this.btnSalvar);
            this.panelForm.Controls.Add(this.btnImprimir);
            this.panelForm.Controls.Add(this.panelTotais);
            this.panelForm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelForm.Location = new System.Drawing.Point(0, 0);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(1312, 700);
            this.panelForm.TabIndex = 0;
            this.panelForm.Paint += new System.Windows.Forms.PaintEventHandler(this.panelForm_Paint);
            // 
            // lblTituloForm
            // 
            this.lblTituloForm.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloForm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblTituloForm.Location = new System.Drawing.Point(25, 15);
            this.lblTituloForm.Name = "lblTituloForm";
            this.lblTituloForm.Size = new System.Drawing.Size(300, 28);
            this.lblTituloForm.TabIndex = 0;
            this.lblTituloForm.Text = "📝 NOVO RECIBO";
            // 
            // linhaForm
            // 
            this.linhaForm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.linhaForm.Location = new System.Drawing.Point(25, 48);
            this.linhaForm.Name = "linhaForm";
            this.linhaForm.Size = new System.Drawing.Size(1260, 2);
            this.linhaForm.TabIndex = 1;
            // 
            // lblNumero
            // 
            this.lblNumero.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNumero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblNumero.Location = new System.Drawing.Point(22, 70);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(363, 18);
            this.lblNumero.TabIndex = 2;
            this.lblNumero.Text = "NÚMERO DO RECIBO";
            // 
            // lblData
            // 
            this.lblData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblData.Location = new System.Drawing.Point(890, 70);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(363, 18);
            this.lblData.TabIndex = 12;
            this.lblData.Text = "DATA";
            // 
            // lblCliente
            // 
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblCliente.Location = new System.Drawing.Point(22, 145);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(363, 18);
            this.lblCliente.TabIndex = 4;
            this.lblCliente.Text = "CLIENTE";
            // 
            // lblFactura
            // 
            this.lblFactura.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFactura.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblFactura.Location = new System.Drawing.Point(890, 145);
            this.lblFactura.Name = "lblFactura";
            this.lblFactura.Size = new System.Drawing.Size(363, 18);
            this.lblFactura.TabIndex = 6;
            this.lblFactura.Text = "FACTURA";
            // 
            // lblFormaPagamento
            // 
            this.lblFormaPagamento.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFormaPagamento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblFormaPagamento.Location = new System.Drawing.Point(22, 224);
            this.lblFormaPagamento.Name = "lblFormaPagamento";
            this.lblFormaPagamento.Size = new System.Drawing.Size(363, 18);
            this.lblFormaPagamento.TabIndex = 8;
            this.lblFormaPagamento.Text = "FORMA DE PAGAMENTO";
            // 
            // lblValor
            // 
            this.lblValor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblValor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblValor.Location = new System.Drawing.Point(22, 405);
            this.lblValor.Name = "lblValor";
            this.lblValor.Size = new System.Drawing.Size(363, 18);
            this.lblValor.TabIndex = 13;
            this.lblValor.Text = "VALOR PAGO (MT)";
            // 
            // lblReferencia
            // 
            this.lblReferencia.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblReferencia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblReferencia.Location = new System.Drawing.Point(22, 316);
            this.lblReferencia.Name = "lblReferencia";
            this.lblReferencia.Size = new System.Drawing.Size(363, 18);
            this.lblReferencia.TabIndex = 10;
            this.lblReferencia.Text = "REFERÊNCIA";
            // 
            // lblObservacoes
            // 
            this.lblObservacoes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblObservacoes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblObservacoes.Location = new System.Drawing.Point(890, 224);
            this.lblObservacoes.Name = "lblObservacoes";
            this.lblObservacoes.Size = new System.Drawing.Size(363, 18);
            this.lblObservacoes.TabIndex = 11;
            this.lblObservacoes.Text = "OBSERVAÇÕES";
            // 
            // panelTotais
            // 
            this.panelTotais.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelTotais.BorderRadius = 10;
            this.panelTotais.BorderThickness = 1;
            this.panelTotais.Controls.Add(this.lblDividaTitulo);
            this.panelTotais.Controls.Add(this.lblDivida);
            this.panelTotais.FillColor = System.Drawing.Color.White;
            this.panelTotais.Location = new System.Drawing.Point(803, 565);
            this.panelTotais.Name = "panelTotais";
            this.panelTotais.Size = new System.Drawing.Size(470, 90);
            this.panelTotais.TabIndex = 18;
            // 
            // lblDividaTitulo
            // 
            this.lblDividaTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDividaTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblDividaTitulo.Location = new System.Drawing.Point(20, 30);
            this.lblDividaTitulo.Name = "lblDividaTitulo";
            this.lblDividaTitulo.Size = new System.Drawing.Size(150, 25);
            this.lblDividaTitulo.TabIndex = 0;
            this.lblDividaTitulo.Text = "SALDO EM DÍVIDA:";
            // 
            // panelHistorico
            // 
            this.panelHistorico.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelHistorico.BorderRadius = 12;
            this.panelHistorico.BorderThickness = 1;
            this.panelHistorico.Controls.Add(this.lblTituloHist);
            this.panelHistorico.Controls.Add(this.linhaHist);
            this.panelHistorico.Controls.Add(this.dgvRecibos);
            this.panelHistorico.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelHistorico.Location = new System.Drawing.Point(0, 720);
            this.panelHistorico.Name = "panelHistorico";
            this.panelHistorico.Size = new System.Drawing.Size(1312, 600);
            this.panelHistorico.TabIndex = 1;
            // 
            // lblTituloHist
            // 
            this.lblTituloHist.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloHist.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblTituloHist.Location = new System.Drawing.Point(25, 15);
            this.lblTituloHist.Name = "lblTituloHist";
            this.lblTituloHist.Size = new System.Drawing.Size(300, 28);
            this.lblTituloHist.TabIndex = 0;
            this.lblTituloHist.Text = "📜 HISTÓRICO DE RECIBOS";
            // 
            // linhaHist
            // 
            this.linhaHist.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.linhaHist.Location = new System.Drawing.Point(25, 48);
            this.linhaHist.Name = "linhaHist";
            this.linhaHist.Size = new System.Drawing.Size(1260, 2);
            this.linhaHist.TabIndex = 1;
            // 
            // btnApagarRecibo
            // 
            this.btnApagarRecibo.BorderRadius = 8;
            this.btnApagarRecibo.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnApagarRecibo.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnApagarRecibo.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnApagarRecibo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnApagarRecibo.FillColor = System.Drawing.Color.Red;
            this.btnApagarRecibo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnApagarRecibo.ForeColor = System.Drawing.Color.White;
            this.btnApagarRecibo.Location = new System.Drawing.Point(404, 610);
            this.btnApagarRecibo.Name = "btnApagarRecibo";
            this.btnApagarRecibo.Size = new System.Drawing.Size(129, 45);
            this.btnApagarRecibo.TabIndex = 19;
            this.btnApagarRecibo.Text = "🗑️ Apagar Recibo";
            this.btnApagarRecibo.Click += new System.EventHandler(this.btnApagarRecibo_Click);
            // 
            // FormRecibo
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1313, 749);
            this.Controls.Add(this.panelScroll);
            this.MinimumSize = new System.Drawing.Size(1200, 750);
            this.Name = "FormRecibo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestão de Recibos";
            ((System.ComponentModel.ISupportInitialize)(this.nudValor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecibos)).EndInit();
            this.panelScroll.ResumeLayout(false);
            this.panelForm.ResumeLayout(false);
            this.panelTotais.ResumeLayout(false);
            this.panelHistorico.ResumeLayout(false);
            this.ResumeLayout(false);

        }

      
    }
}