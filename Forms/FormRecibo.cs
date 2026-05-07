using Guna.UI2.WinForms;
using System;
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

        public FormRecibo()
        {
            InitializeComponent();
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
            if (cmbCliente.SelectedValue is int clienteId)
            {
                var facturas = AppDataConnection.GetFacturasPendentes(clienteId);
                cmbFactura.DataSource = facturas;
                cmbFactura.DisplayMember = "Numero";
                cmbFactura.ValueMember = "Id";
            }
        }

        private void cmbFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFactura.SelectedItem is Factura f)
            {
                lblDivida.Text = "Em Dívida: " + f.TotalEmDivida.ToString("N2") + " MT";
                nudValor.Value = Math.Min(f.TotalEmDivida, nudValor.Maximum);
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
                    string[] ocultas = { "Id","FacturaId","ClienteId","Factura",
                                         "Cliente","Referencia","Observacoes","CriadoEm" };
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

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedItem == null || cmbFactura.SelectedItem == null)
            {
                MessageBox.Show("Seleccione cliente e factura!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

                MessageBox.Show("Recibo guardado com sucesso!", "Sucesso",
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar PDF: " + ex.Message);
            }
        }

        private void LimparFormulario()
        {
            _reciboActual = null;
            txtNumero.Text = AppDataConnection.GerarNumero("recibos", "REC");
            txtReferencia.Text = "";
            txtObservacoes.Text = "";
            nudValor.Value = 0;
            dtpData.Value = DateTime.Now;
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

        // ═══════════════════════════════════════════════════════════
        // INITIALIZE COMPONENT - FORMATO DESIGNER SAFE
        // ═══════════════════════════════════════════════════════════

        private void InitializeComponent()
        {
            txtNumero = new Guna2TextBox();
            txtReferencia = new Guna2TextBox();
            txtObservacoes = new Guna2TextBox();
            cmbCliente = new Guna2ComboBox();
            cmbFactura = new Guna2ComboBox();
            cmbFormaPagamento = new Guna2ComboBox();
            dtpData = new Guna2DateTimePicker();
            nudValor = new NumericUpDown();
            btnNovo = new Guna2Button();
            btnSalvar = new Guna2Button();
            btnImprimir = new Guna2Button();
            lblDivida = new Label();
            dgvRecibos = new Guna2DataGridView();

            SuspendLayout();

            // ── FORM ─────────────────────────────────────────
            Text = "Gestão de Recibos";
            Size = new Size(1000, 660);
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 600);

            // ════════════════════════════════════════════════
            // PAINEL ESQUERDO — Histórico de recibos
            // ════════════════════════════════════════════════
            Guna2Panel panelGrid = new Guna2Panel();
            panelGrid.Location = new Point(15, 15);
            panelGrid.Size = new Size(530, 620);
            panelGrid.BorderRadius = 10;
            panelGrid.FillColor = Color.FromArgb(250, 250, 252);
            panelGrid.BorderColor = Color.FromArgb(220, 224, 230);
            panelGrid.BorderThickness = 1;

            Label lblHistTitulo = new Label();
            lblHistTitulo.Text = "HISTÓRICO DE RECIBOS";
            lblHistTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblHistTitulo.ForeColor = Color.FromArgb(30, 30, 45);
            lblHistTitulo.Location = new Point(20, 15);
            lblHistTitulo.Size = new Size(300, 25);
            panelGrid.Controls.Add(lblHistTitulo);

            Guna2Separator sep1 = new Guna2Separator();
            sep1.Location = new Point(20, 45);
            sep1.Size = new Size(490, 2);
            sep1.FillColor = Color.FromArgb(0, 122, 255);
            panelGrid.Controls.Add(sep1);

            dgvRecibos.Location = new Point(15, 58);
            dgvRecibos.Size = new Size(500, 550);
            dgvRecibos.ReadOnly = true;
            dgvRecibos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecibos.MultiSelect = false;
            dgvRecibos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecibos.BackgroundColor = Color.White;
            dgvRecibos.BorderStyle = BorderStyle.None;
            dgvRecibos.RowHeadersVisible = false;
            dgvRecibos.AllowUserToAddRows = false;
            dgvRecibos.Font = new Font("Segoe UI", 9);
            dgvRecibos.RowTemplate.Height = 38;
            dgvRecibos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRecibos.GridColor = Color.FromArgb(230, 230, 230);
            dgvRecibos.EnableHeadersVisualStyles = false;
            dgvRecibos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(25, 25, 40);
            dgvRecibos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRecibos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvRecibos.ColumnHeadersHeight = 38;
            dgvRecibos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRecibos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 230, 255);
            dgvRecibos.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 20, 40);
            dgvRecibos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 255);
            dgvRecibos.CellClick += dgvRecibos_CellClick;
            panelGrid.Controls.Add(dgvRecibos);

            // ════════════════════════════════════════════════
            // PAINEL DIREITO — Formulário
            // ════════════════════════════════════════════════
            Guna2Panel panelForm = new Guna2Panel();
            panelForm.Location = new Point(560, 15);
            panelForm.Size = new Size(415, 620);
            panelForm.BorderRadius = 10;
            panelForm.FillColor = Color.FromArgb(250, 250, 252);
            panelForm.BorderColor = Color.FromArgb(220, 224, 230);
            panelForm.BorderThickness = 1;

            Label lblFormTitulo = new Label();
            lblFormTitulo.Text = "NOVO RECIBO";
            lblFormTitulo.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblFormTitulo.ForeColor = Color.FromArgb(30, 30, 45);
            lblFormTitulo.Location = new Point(20, 15);
            lblFormTitulo.Size = new Size(300, 25);
            panelForm.Controls.Add(lblFormTitulo);

            Guna2Separator sep2 = new Guna2Separator();
            sep2.Location = new Point(20, 45);
            sep2.Size = new Size(375, 2);
            sep2.FillColor = Color.FromArgb(0, 122, 255);
            panelForm.Controls.Add(sep2);

            // Campos
            int y = 60;
            int larg = 375;

            // Número
            Label lblNumero = new Label();
            lblNumero.Text = "Número";
            lblNumero.Location = new Point(20, y);
            lblNumero.AutoSize = true;
            lblNumero.Font = new Font("Segoe UI", 9);
            lblNumero.ForeColor = Color.FromArgb(80, 80, 100);
            panelForm.Controls.Add(lblNumero);

            txtNumero.Location = new Point(20, y + 20);
            txtNumero.Size = new Size(larg, 34);
            txtNumero.BorderRadius = 6;
            txtNumero.FillColor = Color.White;
            txtNumero.BorderColor = Color.FromArgb(200, 200, 210);
            txtNumero.Font = new Font("Segoe UI", 10);
            panelForm.Controls.Add(txtNumero);
            y += 62;

            // Data
            Label lblData = new Label();
            lblData.Text = "Data";
            lblData.Location = new Point(20, y);
            lblData.AutoSize = true;
            lblData.Font = new Font("Segoe UI", 9);
            lblData.ForeColor = Color.FromArgb(80, 80, 100);
            panelForm.Controls.Add(lblData);

            dtpData.Location = new Point(20, y + 20);
            dtpData.Size = new Size(larg, 34);
            dtpData.BorderRadius = 6;
            dtpData.FillColor = Color.White;
            dtpData.BorderColor = Color.FromArgb(200, 200, 200);
            dtpData.Format = DateTimePickerFormat.Long;
            dtpData.Value = DateTime.Now;
            panelForm.Controls.Add(dtpData);
            y += 62;

            // Cliente
            Label lblCliente = new Label();
            lblCliente.Text = "Cliente";
            lblCliente.Location = new Point(20, y);
            lblCliente.AutoSize = true;
            lblCliente.Font = new Font("Segoe UI", 9);
            lblCliente.ForeColor = Color.FromArgb(80, 80, 100);
            panelForm.Controls.Add(lblCliente);

            cmbCliente.Location = new Point(20, y + 20);
            cmbCliente.Size = new Size(larg, 34);
            cmbCliente.BorderRadius = 6;
            cmbCliente.FillColor = Color.White;
            cmbCliente.BorderColor = Color.FromArgb(200, 200, 210);
            cmbCliente.Font = new Font("Segoe UI", 10);
            panelForm.Controls.Add(cmbCliente);
            y += 62;

            // Factura
            Label lblFactura = new Label();
            lblFactura.Text = "Factura";
            lblFactura.Location = new Point(20, y);
            lblFactura.AutoSize = true;
            lblFactura.Font = new Font("Segoe UI", 9);
            lblFactura.ForeColor = Color.FromArgb(80, 80, 100);
            panelForm.Controls.Add(lblFactura);

            cmbFactura.Location = new Point(20, y + 20);
            cmbFactura.Size = new Size(larg, 34);
            cmbFactura.BorderRadius = 6;
            cmbFactura.FillColor = Color.White;
            cmbFactura.BorderColor = Color.FromArgb(200, 200, 210);
            cmbFactura.Font = new Font("Segoe UI", 10);
            cmbFactura.SelectedIndexChanged += cmbFactura_SelectedIndexChanged;
            panelForm.Controls.Add(cmbFactura);
            y += 62;

            // Label Em Dívida
            lblDivida.Text = "Em Dívida: 0,00 MT";
            lblDivida.Location = new Point(20, y);
            lblDivida.Size = new Size(larg, 22);
            lblDivida.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblDivida.ForeColor = Color.FromArgb(255, 59, 48);
            panelForm.Controls.Add(lblDivida);
            y += 32;

            // Valor a Pagar
            Label lblValor = new Label();
            lblValor.Text = "Valor a Pagar (MT)";
            lblValor.Location = new Point(20, y);
            lblValor.AutoSize = true;
            lblValor.Font = new Font("Segoe UI", 9);
            lblValor.ForeColor = Color.FromArgb(80, 80, 100);
            panelForm.Controls.Add(lblValor);

            nudValor.Location = new Point(20, y + 20);
            nudValor.Size = new Size(larg, 34);
            nudValor.Maximum = 9999999;
            nudValor.DecimalPlaces = 2;
            nudValor.Font = new Font("Segoe UI", 10);
            nudValor.TextAlign = HorizontalAlignment.Right;
            panelForm.Controls.Add(nudValor);
            y += 62;

            // Forma de Pagamento
            Label lblFormaPagamento = new Label();
            lblFormaPagamento.Text = "Forma de Pagamento";
            lblFormaPagamento.Location = new Point(20, y);
            lblFormaPagamento.AutoSize = true;
            lblFormaPagamento.Font = new Font("Segoe UI", 9);
            lblFormaPagamento.ForeColor = Color.FromArgb(80, 80, 100);
            panelForm.Controls.Add(lblFormaPagamento);

            cmbFormaPagamento.Location = new Point(20, y + 20);
            cmbFormaPagamento.Size = new Size(larg, 34);
            cmbFormaPagamento.BorderRadius = 6;
            cmbFormaPagamento.FillColor = Color.White;
            cmbFormaPagamento.BorderColor = Color.FromArgb(200, 200, 210);
            cmbFormaPagamento.Font = new Font("Segoe UI", 10);
            panelForm.Controls.Add(cmbFormaPagamento);
            y += 62;

            // Referência
            Label lblReferencia = new Label();
            lblReferencia.Text = "Referência (nº transf., cheque...)";
            lblReferencia.Location = new Point(20, y);
            lblReferencia.AutoSize = true;
            lblReferencia.Font = new Font("Segoe UI", 9);
            lblReferencia.ForeColor = Color.FromArgb(80, 80, 100);
            panelForm.Controls.Add(lblReferencia);

            txtReferencia.Location = new Point(20, y + 20);
            txtReferencia.Size = new Size(larg, 34);
            txtReferencia.BorderRadius = 6;
            txtReferencia.FillColor = Color.White;
            txtReferencia.BorderColor = Color.FromArgb(200, 200, 210);
            txtReferencia.Font = new Font("Segoe UI", 10);
            panelForm.Controls.Add(txtReferencia);
            y += 62;

            // Observações
            Label lblObservacoes = new Label();
            lblObservacoes.Text = "Observações";
            lblObservacoes.Location = new Point(20, y);
            lblObservacoes.AutoSize = true;
            lblObservacoes.Font = new Font("Segoe UI", 9);
            lblObservacoes.ForeColor = Color.FromArgb(80, 80, 100);
            panelForm.Controls.Add(lblObservacoes);

            txtObservacoes.Location = new Point(20, y + 20);
            txtObservacoes.Size = new Size(larg, 65);
            txtObservacoes.Multiline = true;
            txtObservacoes.BorderRadius = 6;
            txtObservacoes.FillColor = Color.White;
            txtObservacoes.BorderColor = Color.FromArgb(200, 200, 210);
            txtObservacoes.Font = new Font("Segoe UI", 10);
            panelForm.Controls.Add(txtObservacoes);
            y += 93;

            // ── BOTÕES ─────────────────────────────────────
            btnNovo.Text = "➕  Novo";
            btnNovo.Location = new Point(20, y);
            btnNovo.Size = new Size(110, 42);
            btnNovo.BorderRadius = 8;
            btnNovo.FillColor = Color.FromArgb(0, 122, 255);
            btnNovo.ForeColor = Color.White;
            btnNovo.Font = new Font("Segoe UI Semibold", 10);
            btnNovo.Click += btnNovo_Click;
            panelForm.Controls.Add(btnNovo);

            btnSalvar.Text = "💾  Salvar";
            btnSalvar.Location = new Point(145, y);
            btnSalvar.Size = new Size(110, 42);
            btnSalvar.BorderRadius = 8;
            btnSalvar.FillColor = Color.FromArgb(52, 199, 89);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.Font = new Font("Segoe UI Semibold", 10);
            btnSalvar.Click += btnSalvar_Click;
            panelForm.Controls.Add(btnSalvar);

            btnImprimir.Text = "📄  PDF";
            btnImprimir.Location = new Point(270, y);
            btnImprimir.Size = new Size(125, 42);
            btnImprimir.BorderRadius = 8;
            btnImprimir.FillColor = Color.FromArgb(155, 89, 182);
            btnImprimir.ForeColor = Color.White;
            btnImprimir.Font = new Font("Segoe UI Semibold", 10);
            btnImprimir.Click += btnImprimir_Click;
            panelForm.Controls.Add(btnImprimir);

            // Adiciona tudo ao Form
            Controls.Add(panelGrid);
            Controls.Add(panelForm);

            ResumeLayout(false);
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            LimparFormulario();
        }

        // ── CAMPOS ───────────────────────────────────────────
        private Guna2TextBox txtNumero;
        private Guna2TextBox txtReferencia;
        private Guna2TextBox txtObservacoes;
        private Guna2ComboBox cmbCliente;
        private Guna2ComboBox cmbFactura;
        private Guna2ComboBox cmbFormaPagamento;
        private Guna2DateTimePicker dtpData;
        private NumericUpDown nudValor;
        private Guna2Button btnNovo;
        private Guna2Button btnSalvar;
        private Guna2Button btnImprimir;
        private Label lblDivida;
        private Guna2DataGridView dgvRecibos;
    }
}