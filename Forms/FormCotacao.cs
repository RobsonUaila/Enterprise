using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using Enterprise.Data;
using Enterprise.Models;
using Enterprise.Reports;
using Enterprise.Helpers;

namespace Enterprise.Forms
{
    public partial class FormCotacao : Form
    {
        private System.ComponentModel.IContainer components = null;
        private const decimal IVA_PERCENTAGEM = 16m;

        // CONTROLES DO FORMULÁRIO
        private Guna2Panel panelScroll;
        private Guna2Panel panelDados;
        private Guna2Panel panelTotais;
        private Guna2Panel panelItens;

        private Label lblNumero;
        private Guna2TextBox txtNumero;
        private Label lblData;
        private Guna2DateTimePicker dtpData;
        private Label lblCliente;
        private Guna2ComboBox cmbCliente;
        private Label lblServico;
        private Guna2ComboBox cmbServico;
        private Label lblDesconto;
        private NumericUpDown nudDesconto;
        private Label lblItens;
        private Guna2DataGridView dgvItens;
        private DataGridViewTextBoxColumn colDescricao;
        private DataGridViewTextBoxColumn colQuantidade;
        private DataGridViewTextBoxColumn colValorUnitario;
        private DataGridViewTextBoxColumn colDesconto;
        private DataGridViewTextBoxColumn colTotal;
        private Guna2Button btnAdicionarItem;
        private Guna2Button btnRemoverItem;
        private Guna2Button btnSalvar;
        private Guna2Button btnImprimir;
        private Label lblSubTotal;
        private Label lblIva;
        private Label lblTotal;
        private Label lblHistorico;
        private Guna2DataGridView dgvCotacoes;

        // SEPARADORES
        private Guna2Separator separatorTop;
        private Guna2Separator separatorItens;
        private DataGridViewTextBoxColumn Descricao;
        private DataGridViewTextBoxColumn Quantidade;
        private DataGridViewTextBoxColumn ValorUnitario;
        private DataGridViewTextBoxColumn Desconto;
        private DataGridViewTextBoxColumn Total;
        private Guna2Button btnApagarCotacao;
        private Guna2Separator separatorHistorico;

        public FormCotacao()
        {
            Logger.Info("=== INICIANDO FORMULÁRIO DE COTAÇÃO ===");
            InitializeComponent();
            ConfigurarEventos();
            CarregarDadosIniciais();
            GerarNovaCotacao();
            Logger.Info("Formulário de Cotação inicializado com sucesso");
        }

        // =====================================================
        // INITIALIZE COMPONENT - CÓDIGO MÍNIMO PARA FUNCIONAR
        // =====================================================
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelScroll = new Guna.UI2.WinForms.Guna2Panel();
            this.panelDados = new Guna.UI2.WinForms.Guna2Panel();
            this.lblNumero = new System.Windows.Forms.Label();
            this.txtNumero = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblData = new System.Windows.Forms.Label();
            this.dtpData = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.separatorTop = new Guna.UI2.WinForms.Guna2Separator();
            this.lblCliente = new System.Windows.Forms.Label();
            this.cmbCliente = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblServico = new System.Windows.Forms.Label();
            this.cmbServico = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblDesconto = new System.Windows.Forms.Label();
            this.nudDesconto = new System.Windows.Forms.NumericUpDown();
            this.panelItens = new Guna.UI2.WinForms.Guna2Panel();
            this.lblItens = new System.Windows.Forms.Label();
            this.separatorItens = new Guna.UI2.WinForms.Guna2Separator();
            this.dgvItens = new Guna.UI2.WinForms.Guna2DataGridView();
            this.Descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantidade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValorUnitario = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Desconto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdicionarItem = new Guna.UI2.WinForms.Guna2Button();
            this.btnRemoverItem = new Guna.UI2.WinForms.Guna2Button();
            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.btnImprimir = new Guna.UI2.WinForms.Guna2Button();
            this.panelTotais = new Guna.UI2.WinForms.Guna2Panel();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.lblIva = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.lblHistorico = new System.Windows.Forms.Label();
            this.separatorHistorico = new Guna.UI2.WinForms.Guna2Separator();
            this.dgvCotacoes = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnApagarCotacao = new Guna.UI2.WinForms.Guna2Button();
            this.panelScroll.SuspendLayout();
            this.panelDados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesconto)).BeginInit();
            this.panelItens.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.panelTotais.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCotacoes)).BeginInit();
            this.SuspendLayout();
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.BackColor = System.Drawing.Color.White;
            this.panelScroll.Controls.Add(this.panelDados);
            this.panelScroll.Controls.Add(this.panelItens);
            this.panelScroll.Controls.Add(this.panelTotais);
            this.panelScroll.Controls.Add(this.lblHistorico);
            this.panelScroll.Controls.Add(this.separatorHistorico);
            this.panelScroll.Controls.Add(this.dgvCotacoes);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(20, 20, 20, 40);
            this.panelScroll.Size = new System.Drawing.Size(1318, 710);
            this.panelScroll.TabIndex = 0;
            // 
            // panelDados
            // 
            this.panelDados.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelDados.BorderRadius = 12;
            this.panelDados.BorderThickness = 1;
            this.panelDados.Controls.Add(this.lblNumero);
            this.panelDados.Controls.Add(this.txtNumero);
            this.panelDados.Controls.Add(this.lblData);
            this.panelDados.Controls.Add(this.dtpData);
            this.panelDados.Controls.Add(this.separatorTop);
            this.panelDados.Controls.Add(this.lblCliente);
            this.panelDados.Controls.Add(this.cmbCliente);
            this.panelDados.Controls.Add(this.lblServico);
            this.panelDados.Controls.Add(this.cmbServico);
            this.panelDados.Controls.Add(this.lblDesconto);
            this.panelDados.Controls.Add(this.nudDesconto);
            this.panelDados.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelDados.Location = new System.Drawing.Point(20, 20);
            this.panelDados.Name = "panelDados";
            this.panelDados.Padding = new System.Windows.Forms.Padding(25, 20, 25, 20);
            this.panelDados.Size = new System.Drawing.Size(1290, 200);
            this.panelDados.TabIndex = 1;
            // 
            // lblNumero
            // 
            this.lblNumero.AutoSize = true;
            this.lblNumero.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblNumero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblNumero.Location = new System.Drawing.Point(25, 28);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(100, 19);
            this.lblNumero.TabIndex = 0;
            this.lblNumero.Text = "COTAÇÃO Nº:";
            // 
            // txtNumero
            // 
            this.txtNumero.BorderRadius = 8;
            this.txtNumero.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNumero.DefaultText = "";
            this.txtNumero.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNumero.Location = new System.Drawing.Point(131, 20);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.PlaceholderText = "";
            this.txtNumero.ReadOnly = true;
            this.txtNumero.SelectedText = "";
            this.txtNumero.Size = new System.Drawing.Size(180, 36);
            this.txtNumero.TabIndex = 1;
            // 
            // lblData
            // 
            this.lblData.AutoSize = true;
            this.lblData.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblData.Location = new System.Drawing.Point(345, 28);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(49, 19);
            this.lblData.TabIndex = 2;
            this.lblData.Text = "DATA:";
            // 
            // dtpData
            // 
            this.dtpData.BorderRadius = 8;
            this.dtpData.Checked = true;
            this.dtpData.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dtpData.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpData.Location = new System.Drawing.Point(400, 20);
            this.dtpData.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpData.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpData.Name = "dtpData";
            this.dtpData.Size = new System.Drawing.Size(160, 36);
            this.dtpData.TabIndex = 3;
            this.dtpData.Value = new System.DateTime(2026, 5, 8, 18, 57, 23, 891);
            // 
            // separatorTop
            // 
            this.separatorTop.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.separatorTop.Location = new System.Drawing.Point(25, 62);
            this.separatorTop.Name = "separatorTop";
            this.separatorTop.Size = new System.Drawing.Size(1240, 2);
            this.separatorTop.TabIndex = 4;
            // 
            // lblCliente
            // 
            this.lblCliente.AutoSize = true;
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblCliente.Location = new System.Drawing.Point(25, 85);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(66, 19);
            this.lblCliente.TabIndex = 5;
            this.lblCliente.Text = "CLIENTE:";
            // 
            // cmbCliente
            // 
            this.cmbCliente.BackColor = System.Drawing.Color.Transparent;
            this.cmbCliente.BorderRadius = 8;
            this.cmbCliente.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCliente.FocusedColor = System.Drawing.Color.Empty;
            this.cmbCliente.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCliente.ItemHeight = 30;
            this.cmbCliente.Location = new System.Drawing.Point(131, 70);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(180, 36);
            this.cmbCliente.TabIndex = 6;
            // 
            // lblServico
            // 
            this.lblServico.AutoSize = true;
            this.lblServico.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblServico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblServico.Location = new System.Drawing.Point(345, 85);
            this.lblServico.Name = "lblServico";
            this.lblServico.Size = new System.Drawing.Size(70, 19);
            this.lblServico.TabIndex = 7;
            this.lblServico.Text = "SERVIÇO:";
            // 
            // cmbServico
            // 
            this.cmbServico.BackColor = System.Drawing.Color.Transparent;
            this.cmbServico.BorderRadius = 8;
            this.cmbServico.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbServico.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServico.FocusedColor = System.Drawing.Color.Empty;
            this.cmbServico.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbServico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbServico.ItemHeight = 30;
            this.cmbServico.Location = new System.Drawing.Point(349, 117);
            this.cmbServico.Name = "cmbServico";
            this.cmbServico.Size = new System.Drawing.Size(200, 36);
            this.cmbServico.TabIndex = 8;
            // 
            // lblDesconto
            // 
            this.lblDesconto.AutoSize = true;
            this.lblDesconto.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDesconto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblDesconto.Location = new System.Drawing.Point(25, 149);
            this.lblDesconto.Name = "lblDesconto";
            this.lblDesconto.Size = new System.Drawing.Size(113, 19);
            this.lblDesconto.TabIndex = 11;
            this.lblDesconto.Text = "DESCONTO (%):";
            // 
            // nudDesconto
            // 
            this.nudDesconto.DecimalPlaces = 2;
            this.nudDesconto.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.nudDesconto.Location = new System.Drawing.Point(153, 143);
            this.nudDesconto.Name = "nudDesconto";
            this.nudDesconto.Size = new System.Drawing.Size(130, 25);
            this.nudDesconto.TabIndex = 12;
            // 
            // panelItens
            // 
            this.panelItens.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelItens.BorderRadius = 12;
            this.panelItens.BorderThickness = 1;
            this.panelItens.Controls.Add(this.btnApagarCotacao);
            this.panelItens.Controls.Add(this.lblItens);
            this.panelItens.Controls.Add(this.separatorItens);
            this.panelItens.Controls.Add(this.dgvItens);
            this.panelItens.Controls.Add(this.btnAdicionarItem);
            this.panelItens.Controls.Add(this.btnRemoverItem);
            this.panelItens.Controls.Add(this.btnSalvar);
            this.panelItens.Controls.Add(this.btnImprimir);
            this.panelItens.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelItens.Location = new System.Drawing.Point(20, 240);
            this.panelItens.Name = "panelItens";
            this.panelItens.Padding = new System.Windows.Forms.Padding(25, 20, 25, 20);
            this.panelItens.Size = new System.Drawing.Size(1290, 380);
            this.panelItens.TabIndex = 8;
            // 
            // lblItens
            // 
            this.lblItens.AutoSize = true;
            this.lblItens.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblItens.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblItens.Location = new System.Drawing.Point(25, 8);
            this.lblItens.Name = "lblItens";
            this.lblItens.Size = new System.Drawing.Size(159, 21);
            this.lblItens.TabIndex = 0;
            this.lblItens.Text = "ITENS DA COTAÇÃO";
            // 
            // separatorItens
            // 
            this.separatorItens.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.separatorItens.Location = new System.Drawing.Point(25, 48);
            this.separatorItens.Name = "separatorItens";
            this.separatorItens.Size = new System.Drawing.Size(1240, 2);
            this.separatorItens.TabIndex = 1;
            // 
            // dgvItens
            // 
            dataGridViewCellStyle13.BackColor = System.Drawing.Color.White;
            this.dgvItens.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvItens.ColumnHeadersHeight = 40;
            this.dgvItens.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Descricao,
            this.Quantidade,
            this.ValorUnitario,
            this.Desconto,
            this.Total});
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItens.DefaultCellStyle = dataGridViewCellStyle15;
            this.dgvItens.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItens.Location = new System.Drawing.Point(25, 73);
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.RowHeadersVisible = false;
            this.dgvItens.RowTemplate.Height = 35;
            this.dgvItens.Size = new System.Drawing.Size(1240, 242);
            this.dgvItens.TabIndex = 10;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvItens.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItens.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvItens.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvItens.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItens.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvItens.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvItens.ThemeStyle.ReadOnly = false;
            this.dgvItens.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvItens.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItens.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItens.ThemeStyle.RowsStyle.Height = 35;
            this.dgvItens.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItens.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItens.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvItens_CellContentClick_1);
            // 
            // Descricao
            // 
            this.Descricao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Descricao.HeaderText = "DESCRIÇÃO";
            this.Descricao.Name = "Descricao";
            // 
            // Quantidade
            // 
            this.Quantidade.HeaderText = "QTD";
            this.Quantidade.Name = "Quantidade";
            // 
            // ValorUnitario
            // 
            this.ValorUnitario.HeaderText = "VALOR UNIT. (MT)";
            this.ValorUnitario.Name = "ValorUnitario";
            // 
            // Desconto
            // 
            this.Desconto.HeaderText = "DESC. (%)";
            this.Desconto.Name = "Desconto";
            // 
            // Total
            // 
            this.Total.HeaderText = "TOTAL (MT)";
            this.Total.Name = "Total";
            // 
            // btnAdicionarItem
            // 
            this.btnAdicionarItem.BorderRadius = 8;
            this.btnAdicionarItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(199)))), ((int)(((byte)(89)))));
            this.btnAdicionarItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnAdicionarItem.ForeColor = System.Drawing.Color.White;
            this.btnAdicionarItem.Location = new System.Drawing.Point(25, 325);
            this.btnAdicionarItem.Name = "btnAdicionarItem";
            this.btnAdicionarItem.Size = new System.Drawing.Size(140, 40);
            this.btnAdicionarItem.TabIndex = 11;
            this.btnAdicionarItem.Text = "➕ Adicionar";
            // 
            // btnRemoverItem
            // 
            this.btnRemoverItem.BorderRadius = 8;
            this.btnRemoverItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(59)))), ((int)(((byte)(48)))));
            this.btnRemoverItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnRemoverItem.ForeColor = System.Drawing.Color.White;
            this.btnRemoverItem.Location = new System.Drawing.Point(180, 325);
            this.btnRemoverItem.Name = "btnRemoverItem";
            this.btnRemoverItem.Size = new System.Drawing.Size(140, 40);
            this.btnRemoverItem.TabIndex = 12;
            this.btnRemoverItem.Text = "🗑️ Remover";
            // 
            // btnSalvar
            // 
            this.btnSalvar.BorderRadius = 8;
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(335, 325);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(140, 40);
            this.btnSalvar.TabIndex = 13;
            this.btnSalvar.Text = "💾 Salvar";
            // 
            // btnImprimir
            // 
            this.btnImprimir.BorderRadius = 8;
            this.btnImprimir.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(149)))), ((int)(((byte)(0)))));
            this.btnImprimir.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnImprimir.ForeColor = System.Drawing.Color.White;
            this.btnImprimir.Location = new System.Drawing.Point(490, 325);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(140, 40);
            this.btnImprimir.TabIndex = 14;
            this.btnImprimir.Text = "📄 Gerar PDF";
            // 
            // panelTotais
            // 
            this.panelTotais.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelTotais.BorderRadius = 12;
            this.panelTotais.BorderThickness = 1;
            this.panelTotais.Controls.Add(this.lblSubTotal);
            this.panelTotais.Controls.Add(this.lblIva);
            this.panelTotais.Controls.Add(this.lblTotal);
            this.panelTotais.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelTotais.Location = new System.Drawing.Point(900, 640);
            this.panelTotais.Name = "panelTotais";
            this.panelTotais.Size = new System.Drawing.Size(410, 160);
            this.panelTotais.TabIndex = 11;
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSubTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblSubTotal.Location = new System.Drawing.Point(20, 20);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(370, 35);
            this.lblSubTotal.TabIndex = 0;
            this.lblSubTotal.Text = "SUBTOTAL: 0,00 MT";
            this.lblSubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblIva
            // 
            this.lblIva.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblIva.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblIva.Location = new System.Drawing.Point(20, 55);
            this.lblIva.Name = "lblIva";
            this.lblIva.Size = new System.Drawing.Size(370, 35);
            this.lblIva.TabIndex = 1;
            this.lblIva.Text = "IVA: 0,00 MT";
            this.lblIva.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblTotal.Location = new System.Drawing.Point(20, 105);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(370, 45);
            this.lblTotal.TabIndex = 2;
            this.lblTotal.Text = "TOTAL: 0,00 MT";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHistorico
            // 
            this.lblHistorico.AutoSize = true;
            this.lblHistorico.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblHistorico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblHistorico.Location = new System.Drawing.Point(40, 820);
            this.lblHistorico.Name = "lblHistorico";
            this.lblHistorico.Size = new System.Drawing.Size(202, 21);
            this.lblHistorico.TabIndex = 12;
            this.lblHistorico.Text = "HISTÓRICO DE COTAÇÕES";
            // 
            // separatorHistorico
            // 
            this.separatorHistorico.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.separatorHistorico.Location = new System.Drawing.Point(40, 858);
            this.separatorHistorico.Name = "separatorHistorico";
            this.separatorHistorico.Size = new System.Drawing.Size(1270, 2);
            this.separatorHistorico.TabIndex = 13;
            // 
            // dgvCotacoes
            // 
            dataGridViewCellStyle16.BackColor = System.Drawing.Color.White;
            this.dgvCotacoes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle16;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCotacoes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle17;
            this.dgvCotacoes.ColumnHeadersHeight = 40;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCotacoes.DefaultCellStyle = dataGridViewCellStyle18;
            this.dgvCotacoes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCotacoes.Location = new System.Drawing.Point(40, 875);
            this.dgvCotacoes.Name = "dgvCotacoes";
            this.dgvCotacoes.ReadOnly = true;
            this.dgvCotacoes.RowHeadersVisible = false;
            this.dgvCotacoes.RowTemplate.Height = 35;
            this.dgvCotacoes.Size = new System.Drawing.Size(1270, 150);
            this.dgvCotacoes.TabIndex = 13;
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvCotacoes.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvCotacoes.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCotacoes.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvCotacoes.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvCotacoes.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCotacoes.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvCotacoes.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCotacoes.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvCotacoes.ThemeStyle.ReadOnly = true;
            this.dgvCotacoes.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvCotacoes.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvCotacoes.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCotacoes.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvCotacoes.ThemeStyle.RowsStyle.Height = 35;
            this.dgvCotacoes.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCotacoes.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // bnt Apagar cotacao
            // 
            this.btnApagarCotacao.BorderRadius = 8;
            this.btnApagarCotacao.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnApagarCotacao.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnApagarCotacao.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnApagarCotacao.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnApagarCotacao.FillColor = System.Drawing.Color.Red;
            this.btnApagarCotacao.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnApagarCotacao.ForeColor = System.Drawing.Color.White;
            this.btnApagarCotacao.Location = new System.Drawing.Point(659, 321);
            this.btnApagarCotacao.Name = "bntApagarCotacao";
            this.btnApagarCotacao.Size = new System.Drawing.Size(146, 45);
            this.btnApagarCotacao.TabIndex = 15;
            this.btnApagarCotacao.Text = "Apagar Cotacao";
            this.btnApagarCotacao.Click += new System.EventHandler(btnApagarCotacao_Click);
            // 
            // FormCotacao
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1318, 710);
            this.Controls.Add(this.panelScroll);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "FormCotacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestão de Cotações - AC Electricidade e Serviços";
            this.panelScroll.ResumeLayout(false);
            this.panelScroll.PerformLayout();
            this.panelDados.ResumeLayout(false);
            this.panelDados.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesconto)).EndInit();
            this.panelItens.ResumeLayout(false);
            this.panelItens.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.panelTotais.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCotacoes)).EndInit();
            this.ResumeLayout(false);

        }

        // =====================================================
        // EVENTOS E LÓGICA
        // =====================================================

        private void ConfigurarEventos()
        {
            Logger.Debug("Configurando eventos do formulário");

            if (btnAdicionarItem != null)
                btnAdicionarItem.Click += BtnAdicionarItem_Click;
            else
                Logger.Error("btnAdicionarItem não foi inicializado!");

            if (btnRemoverItem != null)
                btnRemoverItem.Click += BtnRemoverItem_Click;

            if (btnSalvar != null)
                btnSalvar.Click += BtnSalvar_Click;

            if (btnImprimir != null)
                btnImprimir.Click += BtnImprimir_Click;

            if (nudDesconto != null)
                nudDesconto.ValueChanged += (s, e) => AtualizarTotais();

            if (dgvItens != null)
            {
                dgvItens.CellValueChanged += (s, e) => AtualizarTotais();
                dgvItens.RowsAdded += (s, e) => AtualizarTotais();
                dgvItens.RowsRemoved += (s, e) => AtualizarTotais();
            }

            if (dgvCotacoes != null)
                dgvCotacoes.CellDoubleClick += DgvCotacoes_CellDoubleClick;

            Logger.Debug("Eventos configurados com sucesso");
        }

        private void CarregarDadosIniciais()
        {
            Logger.Info("Carregando dados iniciais...");

            try
            {
                if (cmbCliente == null)
                {
                    Logger.Error("cmbCliente não foi inicializado!");
                    return;
                }

                if (cmbServico == null)
                {
                    Logger.Error("cmbServico não foi inicializado!");
                    return;
                }

                Logger.Debug("Carregando clientes...");
                var clientes = AppDataConnection.GetClientes();
                cmbCliente.DataSource = clientes;
                cmbCliente.DisplayMember = "Nome";
                cmbCliente.ValueMember = "Id";
                Logger.Info($"Clientes carregados: {clientes.Count} registos");

                Logger.Debug("Carregando serviços...");
                var servicos = AppDataConnection.GetServicos();
                cmbServico.DataSource = servicos;
                cmbServico.DisplayMember = "Nome";
                cmbServico.ValueMember = "Id";
                Logger.Info($"Serviços carregados: {servicos.Count} registos");


                Logger.Debug("Carregando histórico...");
                CarregarHistorico();

                Logger.Info("Dados iniciais carregados com sucesso");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar dados iniciais", ex);
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarHistorico()
        {
            Logger.Debug("Carregando histórico de cotações...");

            try
            {
                var lista = AppDataConnection.GetCotacoes();
                if (dgvCotacoes != null)
                {
                    dgvCotacoes.DataSource = lista;
                    Logger.Info($"Histórico carregado: {lista.Count} cotações encontradas");

                    if (dgvCotacoes.Columns.Count > 0)
                    {
                        string[] ocultas = { "Id", "ClienteId", "Itens", "Cliente", "SubTotal", "ValorIva", "Iva", "DataValidade", "CriadoEm", "Observacoes" };
                        foreach (var col in ocultas)
                            if (dgvCotacoes.Columns.Contains(col))
                                dgvCotacoes.Columns[col].Visible = false;

                        if (dgvCotacoes.Columns.Contains("Numero"))
                            dgvCotacoes.Columns["Numero"].HeaderText = "Nº COTAÇÃO";
                        if (dgvCotacoes.Columns.Contains("Data"))
                            dgvCotacoes.Columns["Data"].HeaderText = "DATA";
                        if (dgvCotacoes.Columns.Contains("Estado"))
                            dgvCotacoes.Columns["Estado"].HeaderText = "ESTADO";
                        if (dgvCotacoes.Columns.Contains("Total"))
                        {
                            dgvCotacoes.Columns["Total"].HeaderText = "TOTAL (MT)";
                            dgvCotacoes.Columns["Total"].DefaultCellStyle.Format = "N2";
                            dgvCotacoes.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                }

                Logger.Debug("Histórico configurado com sucesso");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar histórico", ex);
                MessageBox.Show("Erro ao carregar histórico: " + ex.Message);
            }
        }

        private void AtualizarTotais()
        {
            if (dgvItens == null || lblSubTotal == null) return;

            try
            {
                Logger.Debug("Atualizando totais...");

                decimal sub = 0;
                int itemCount = 0;

                foreach (DataGridViewRow row in dgvItens.Rows)
                {
                    if (row.IsNewRow) continue;

                    decimal qtd = 0;
                    decimal preco = 0;

                    if (row.Cells["Quantidade"] != null && row.Cells["Quantidade"].Value != null)
                        decimal.TryParse(row.Cells["Quantidade"].Value.ToString(), out qtd);

                    if (row.Cells["ValorUnitario"] != null && row.Cells["ValorUnitario"].Value != null)
                        decimal.TryParse(row.Cells["ValorUnitario"].Value.ToString(), out preco);

                    decimal totalItem = qtd * preco;

                    if (row.Cells["Total"] != null)
                        row.Cells["Total"].Value = totalItem;

                    sub += totalItem;
                    itemCount++;
                }

                decimal descGeral = nudDesconto.Value;
                decimal subDesconto = sub * (1 - descGeral / 100);
                decimal ivaValor = subDesconto * (IVA_PERCENTAGEM / 100);
                decimal totalFinal = subDesconto + ivaValor;

                lblSubTotal.Text = "SUBTOTAL: " + sub.ToString("N2") + " MT";
                lblIva.Text = "IVA: " + ivaValor.ToString("N2") + " MT";
                lblTotal.Text = "TOTAL: " + totalFinal.ToString("N2") + " MT";

                Logger.Debug($"Totais atualizados - Itens: {itemCount}, Subtotal: {sub:N2}, IVA: {ivaValor:N2}, Total: {totalFinal:N2}");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao atualizar totais", ex);
            }
        }

        private void GerarNovaCotacao()
        {
            Logger.Info("Gerando nova cotação...");

            try
            {
                if (txtNumero != null)
                    txtNumero.Text = AppDataConnection.GerarNumero("cotacoes", "COT");

                if (dtpData != null)
                    dtpData.Value = DateTime.Now;

                if (nudDesconto != null)
                    nudDesconto.Value = 0;

                if (dgvItens != null)
                    dgvItens.Rows.Clear();

                AtualizarTotais();

                Logger.Info($"Nova cotação gerada: {txtNumero?.Text ?? "N/A"}");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao gerar nova cotação", ex);
            }
        }

        private void BtnAdicionarItem_Click(object sender, EventArgs e)
        {
            Logger.Info("Tentando adicionar item...");

            try
            {
                if (cmbServico?.SelectedItem == null)
                {
                    Logger.Warning("Tentativa de adicionar item sem serviço selecionado");
                    MessageBox.Show("Selecione um serviço!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var servico = (Servico)cmbServico.SelectedItem;
                dgvItens.Rows.Add(servico.Nome, 1, servico.PrecoBase, 0, servico.PrecoBase);
                AtualizarTotais();

                Logger.Info($"Item adicionado: {servico.Nome} - Preço: {servico.PrecoBase:N2}");
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao adicionar item", ex);
                MessageBox.Show("Erro ao adicionar item: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRemoverItem_Click(object sender, EventArgs e)
        {
            Logger.Info("Tentando remover item...");

            try
            {
                if (dgvItens?.CurrentRow == null || dgvItens.CurrentRow.IsNewRow)
                {
                    Logger.Warning("Tentativa de remover item sem seleção");
                    MessageBox.Show("Selecione um item para remover.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Remover este item?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string itemDesc = dgvItens.CurrentRow.Cells["Descricao"]?.Value?.ToString() ?? "Unknown";
                    dgvItens.Rows.RemoveAt(dgvItens.CurrentRow.Index);
                    AtualizarTotais();
                    Logger.Info($"Item removido: {itemDesc}");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao remover item", ex);
                MessageBox.Show("Erro ao remover item: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Validar()
        {
            Logger.Debug("Validando formulário...");

            if (string.IsNullOrWhiteSpace(txtNumero?.Text))
            {
                Logger.Warning("Validação falhou: Número da cotação vazio");
                MessageBox.Show("O número da cotação é obrigatório.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNumero?.Focus();
                return false;
            }

            if (cmbCliente?.SelectedIndex < 0)
            {
                Logger.Warning("Validação falhou: Nenhum cliente selecionado");
                MessageBox.Show("Selecione um cliente.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCliente?.Focus();
                return false;
            }

            bool temItens = false;
            if (dgvItens != null)
            {
                foreach (DataGridViewRow row in dgvItens.Rows)
                    if (!row.IsNewRow) { temItens = true; break; }
            }

            if (!temItens)
            {
                Logger.Warning("Validação falhou: Nenhum item adicionado");
                MessageBox.Show("Adicione pelo menos um item.", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            Logger.Info("Validação passou com sucesso");
            return true;
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            Logger.Info($"=== INICIANDO SALVAMENTO DA COTAÇÃO {txtNumero?.Text ?? "N/A"} ===");

            if (!Validar())
            {
                Logger.Warning("Salvamento cancelado - Validação falhou");
                return;
            }

            try
            {
                Logger.Debug("Criando objeto Cotacao...");
                var cotacao = new Cotacao
                {
                    Numero = txtNumero.Text,
                    ClienteId = (int)cmbCliente.SelectedValue,
                    Data = dtpData.Value,
                    DataValidade = dtpData.Value.AddDays(15),
                    Iva = IVA_PERCENTAGEM,

                    Itens = new System.Collections.Generic.List<ItemDocumento>()
                };

                Logger.Debug($"Cotação {cotacao.Numero} - Cliente ID: {cotacao.ClienteId}, Data: {cotacao.Data:dd/MM/yyyy}");

                int itemIndex = 0;
                foreach (DataGridViewRow row in dgvItens.Rows)
                {
                    if (row.IsNewRow) continue;
                    itemIndex++;

                    decimal qtd = 0, preco = 0;
                    decimal.TryParse(row.Cells["Quantidade"]?.Value?.ToString(), out qtd);
                    decimal.TryParse(row.Cells["ValorUnitario"]?.Value?.ToString(), out preco);

                    cotacao.Itens.Add(new ItemDocumento
                    {
                        Descricao = row.Cells["Descricao"]?.Value?.ToString() ?? "",
                        Quantidade = qtd,
                        PrecoUnitario = preco,
                        Unidade = "un"
                    });

                    Logger.Debug($"Item {itemIndex}: {row.Cells["Descricao"]?.Value}, Qtd: {qtd}, Preço: {preco:N2}");
                }

                Logger.Info($"Total de {itemIndex} itens para salvar");
                Logger.Debug("Chamando AppDataConnection.SalvarCotacao...");

                AppDataConnection.SalvarCotacao(cotacao);

                Logger.Info($"✅ COTAÇÃO {cotacao.Numero} SALVA COM SUCESSO!");
                MessageBox.Show("Cotação " + cotacao.Numero + " guardada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CarregarHistorico();
                GerarNovaCotacao();
            }
            catch (Exception ex)
            {
                Logger.Error($"❌ ERRO AO SALVAR COTAÇÃO {txtNumero?.Text ?? "N/A"}", ex);
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            Logger.Info($"=== INICIANDO GERAÇÃO DE PDF DA COTAÇÃO {txtNumero?.Text ?? "N/A"} ===");

            if (!Validar())
            {
                Logger.Warning("Geração de PDF cancelada - Validação falhou");
                return;
            }

            try
            {
                Logger.Debug("Carregando dados da empresa...");
                var emp = AppDataConnection.GetEmpresa();

                if (emp == null)
                {
                    Logger.Error("Empresa não encontrada no banco de dados");
                    MessageBox.Show("Dados da empresa não encontrados!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Logger.Info($"Empresa carregada: {emp.Nome}");

                Logger.Debug("Criando objeto Cotacao para PDF...");
                var cotacao = new Cotacao
                {
                    Numero = txtNumero.Text,
                    Data = dtpData.Value,

                    Iva = IVA_PERCENTAGEM,
                    Cliente = (Cliente)cmbCliente.SelectedItem,
                    Itens = new System.Collections.Generic.List<ItemDocumento>()
                };

                int itemIndex = 0;
                foreach (DataGridViewRow row in dgvItens.Rows)
                {
                    if (row.IsNewRow) continue;
                    itemIndex++;

                    decimal qtd = 0, preco = 0, desc = 0;
                    decimal.TryParse(row.Cells["Quantidade"]?.Value?.ToString(), out qtd);
                    decimal.TryParse(row.Cells["ValorUnitario"]?.Value?.ToString(), out preco);
                    decimal.TryParse(row.Cells["Desconto"]?.Value?.ToString(), out desc);

                    cotacao.Itens.Add(new ItemDocumento
                    {
                        Descricao = row.Cells["Descricao"]?.Value?.ToString() ?? "",
                        Quantidade = qtd,
                        PrecoUnitario = preco,
                        Desconto = desc,
                        Unidade = "un"
                    });
                }

                Logger.Debug($"Gerando PDF com {itemIndex} itens...");
                PdfGenerator.GerarCotacao(cotacao, emp);

                Logger.Info($"✅ PDF DA COTAÇÃO {txtNumero?.Text ?? "N/A"} GERADO COM SUCESSO!");
                MessageBox.Show("PDF gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Error($"❌ ERRO AO GERAR PDF DA COTAÇÃO {txtNumero?.Text ?? "N/A"}", ex);
                MessageBox.Show("Erro ao gerar PDF: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvCotacoes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Logger.Info("Carregando cotação para edição...");

            try
            {
                if (e.RowIndex < 0)
                {
                    Logger.Warning("Double-click em linha inválida");
                    return;
                }

                var cotacao = dgvCotacoes.Rows[e.RowIndex].DataBoundItem as Cotacao;
                if (cotacao == null)
                {
                    Logger.Warning("Cotação não encontrada na linha selecionada");
                    return;
                }

                Logger.Info($"Carregando cotação: {cotacao.Numero} - Cliente ID: {cotacao.ClienteId}");

                txtNumero.Text = cotacao.Numero;
                dtpData.Value = cotacao.Data;


                if (cmbCliente.Items.Count > 0)
                {
                    foreach (object item in cmbCliente.Items)
                    {
                        var cliente = item as Cliente;
                        if (cliente != null && cliente.Id == cotacao.ClienteId)
                        {
                            cmbCliente.SelectedItem = item;
                            Logger.Debug($"Cliente selecionado: {cliente.Nome}");
                            break;
                        }
                    }
                }

                dgvItens.Rows.Clear();
                int itemCount = 0;
                foreach (var item in cotacao.Itens)
                {
                    dgvItens.Rows.Add(item.Descricao, item.Quantidade, item.PrecoUnitario, item.Desconto, item.Total);
                    itemCount++;
                }

                Logger.Info($"Cotação carregada com {itemCount} itens");
                AtualizarTotais();
            }
            catch (Exception ex)
            {
                Logger.Error("Erro ao carregar cotação para edição", ex);
                MessageBox.Show("Erro ao carregar cotação: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
            Logger.Debug("Formulário de Cotação sendo fechado");
        }

        // Eventos vazios (mantidos para compatibilidade)
        private void lblServico_Click(object sender, EventArgs e) { }
        private void dgvItens_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void lblIva_Click(object sender, EventArgs e) { }

        private void dgvItens_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnApagarCotacao_Click(object sender, EventArgs e)
        {
            if (dgvCotacoes.CurrentRow == null) {

                MessageBox.Show("Selecione uma Cotacao para Apagar!", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning );

                    return;
            }

            var cotacao = dgvCotacoes.CurrentRow.DataBoundItem as Cotacao;
            if (cotacao == null)
                return;

            if (MessageBox.Show($"Voce tem Certeza que deseja eliminar a cotação {cotacao.Numero}?\n Esta ação nao pode ser desfeita", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try {

                    AppDataConnection.ApagarCotacao(cotacao.Id);
                    MessageBox.Show("Cotacao apagada com Sucesso!", "sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CarregarHistorico();
                    GerarNovaCotacao();


                } catch (Exception ex) {

                    MessageBox.Show("Erro ao apagar: " + ex.Message, "Erro",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }


        }
    } 
}