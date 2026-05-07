using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Enterprise.Forms
{
    partial class FormCotacao
    {
        private System.ComponentModel.IContainer components = null;

        private Guna2Panel panelScroll;
        private Guna2Panel panelDados;
        private Guna2Panel panelTotais;
        private Guna2Panel panelItens;
        private Guna2Panel panelBotoes;

        private Guna2TextBox txtNumero;

        private Guna2ComboBox cmbCliente;
        private Guna2ComboBox cmbServico;
        private Guna2ComboBox cmbEstado;
        private NumericUpDown nudDesconto;

        private Guna2DateTimePicker dtpData;

        private Guna2DataGridView dgvItens;
        private Guna2DataGridView dgvCotacoes;

        private Label lblSubTotal;
        private Label lblIva;
        private Label lblTotal;

        private Guna2Button btnAdicionarItem;
        private Guna2Button btnRemoverItem;
        private Guna2Button btnSalvar;
        private Guna2Button btnImprimir;

        private Label lblNumero;
        private Label lblData;
        private Label lblCliente;
        private Label lblServico;
        private Label lblEstado;
        private Label lblDesconto;

        private Guna2Separator separator1;
        private Guna2Separator separator2;
        private Guna2Separator separator3;

        private const decimal IVA_PERCENTAGEM = 16m;

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelScroll = new Guna.UI2.WinForms.Guna2Panel();
            this.panelDados = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTituloDados = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.separator1 = new Guna.UI2.WinForms.Guna2Separator();
            this.lblNumero = new System.Windows.Forms.Label();
            this.txtNumero = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblData = new System.Windows.Forms.Label();
            this.dtpData = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.lblCliente = new System.Windows.Forms.Label();
            this.cmbCliente = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblServico = new System.Windows.Forms.Label();
            this.cmbServico = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cmbEstado = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblDesconto = new System.Windows.Forms.Label();
            this.nudDesconto = new System.Windows.Forms.NumericUpDown();
            this.panelItens = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTituloItens = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.separator2 = new Guna.UI2.WinForms.Guna2Separator();
            this.dgvItens = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelBotoes = new Guna.UI2.WinForms.Guna2Panel();
            this.btnAdicionarItem = new Guna.UI2.WinForms.Guna2Button();
            this.btnRemoverItem = new Guna.UI2.WinForms.Guna2Button();
            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.btnImprimir = new Guna.UI2.WinForms.Guna2Button();
            this.panelTotais = new Guna.UI2.WinForms.Guna2Panel();
            this.lblSubTotal = new System.Windows.Forms.Label();
            this.separator3 = new Guna.UI2.WinForms.Guna2Separator();
            this.lblIva = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.dgvCotacoes = new Guna.UI2.WinForms.Guna2DataGridView();
            this.panelScroll.SuspendLayout();
            this.panelDados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesconto)).BeginInit();
            this.panelItens.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).BeginInit();
            this.panelBotoes.SuspendLayout();
            this.panelTotais.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCotacoes)).BeginInit();
            this.SuspendLayout();
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.AutoScrollMargin = new System.Drawing.Size(0, 10);
            this.panelScroll.BackColor = System.Drawing.Color.White;
            this.panelScroll.Controls.Add(this.panelDados);
            this.panelScroll.Controls.Add(this.panelItens);
            this.panelScroll.Controls.Add(this.panelBotoes);
            this.panelScroll.Controls.Add(this.panelTotais);
            this.panelScroll.Controls.Add(this.dgvCotacoes);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(20, 20, 20, 40);
            this.panelScroll.Size = new System.Drawing.Size(1334, 749);
            this.panelScroll.TabIndex = 0;
            // 
            // panelDados
            // 
            this.panelDados.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelDados.BorderRadius = 12;
            this.panelDados.BorderThickness = 1;
            this.panelDados.Controls.Add(this.lblTituloDados);
            this.panelDados.Controls.Add(this.separator1);
            this.panelDados.Controls.Add(this.lblNumero);
            this.panelDados.Controls.Add(this.txtNumero);
            this.panelDados.Controls.Add(this.lblData);
            this.panelDados.Controls.Add(this.dtpData);
            this.panelDados.Controls.Add(this.lblCliente);
            this.panelDados.Controls.Add(this.cmbCliente);
            this.panelDados.Controls.Add(this.lblServico);
            this.panelDados.Controls.Add(this.cmbServico);
            this.panelDados.Controls.Add(this.lblEstado);
            this.panelDados.Controls.Add(this.cmbEstado);
            this.panelDados.Controls.Add(this.lblDesconto);
            this.panelDados.Controls.Add(this.nudDesconto);
            this.panelDados.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelDados.Location = new System.Drawing.Point(0, 0);
            this.panelDados.Name = "panelDados";
            this.panelDados.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelDados.Size = new System.Drawing.Size(1280, 280);
            this.panelDados.TabIndex = 0;
            // 
            // lblTituloDados
            // 
            this.lblTituloDados.BackColor = System.Drawing.Color.Transparent;
            this.lblTituloDados.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTituloDados.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblTituloDados.Location = new System.Drawing.Point(20, 5);
            this.lblTituloDados.Name = "lblTituloDados";
            this.lblTituloDados.Size = new System.Drawing.Size(224, 27);
            this.lblTituloDados.TabIndex = 0;
            this.lblTituloDados.Text = "📋 DADOS DA COTAÇÃO";
            // 
            // separator1
            // 
            this.separator1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.separator1.Location = new System.Drawing.Point(20, 35);
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(1230, 2);
            this.separator1.TabIndex = 1;
            // 
            // lblNumero
            // 
            this.lblNumero.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblNumero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblNumero.Location = new System.Drawing.Point(20, 50);
            this.lblNumero.Name = "lblNumero";
            this.lblNumero.Size = new System.Drawing.Size(90, 48);
            this.lblNumero.TabIndex = 2;
            this.lblNumero.Text = "COTAÇÃO Nº:";
            this.lblNumero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNumero
            // 
            this.txtNumero.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtNumero.BorderRadius = 8;
            this.txtNumero.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNumero.DefaultText = "";
            this.txtNumero.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNumero.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.txtNumero.Location = new System.Drawing.Point(116, 50);
            this.txtNumero.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNumero.Name = "txtNumero";
            this.txtNumero.PlaceholderText = "";
            this.txtNumero.SelectedText = "";
            this.txtNumero.Size = new System.Drawing.Size(229, 48);
            this.txtNumero.TabIndex = 3;
            // 
            // lblData
            // 
            this.lblData.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblData.Location = new System.Drawing.Point(1004, 50);
            this.lblData.Name = "lblData";
            this.lblData.Size = new System.Drawing.Size(60, 36);
            this.lblData.TabIndex = 4;
            this.lblData.Text = "DATA:";
            this.lblData.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblData.Click += new System.EventHandler(this.lblData_Click);
            // 
            // dtpData
            // 
            this.dtpData.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.dtpData.BorderRadius = 8;
            this.dtpData.Checked = true;
            this.dtpData.FillColor = System.Drawing.Color.White;
            this.dtpData.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.dtpData.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpData.Location = new System.Drawing.Point(1070, 50);
            this.dtpData.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpData.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpData.Name = "dtpData";
            this.dtpData.Size = new System.Drawing.Size(180, 36);
            this.dtpData.TabIndex = 5;
            this.dtpData.Value = new System.DateTime(2026, 5, 7, 1, 5, 17, 822);
            // 
            // lblCliente
            // 
            this.lblCliente.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblCliente.Location = new System.Drawing.Point(20, 136);
            this.lblCliente.Name = "lblCliente";
            this.lblCliente.Size = new System.Drawing.Size(90, 36);
            this.lblCliente.TabIndex = 8;
            this.lblCliente.Text = "CLIENTE:";
            this.lblCliente.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbCliente
            // 
            this.cmbCliente.BackColor = System.Drawing.Color.Transparent;
            this.cmbCliente.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbCliente.BorderRadius = 8;
            this.cmbCliente.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCliente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCliente.FocusedColor = System.Drawing.Color.Empty;
            this.cmbCliente.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbCliente.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCliente.ItemHeight = 30;
            this.cmbCliente.Location = new System.Drawing.Point(116, 136);
            this.cmbCliente.Name = "cmbCliente";
            this.cmbCliente.Size = new System.Drawing.Size(250, 36);
            this.cmbCliente.TabIndex = 9;
            // 
            // lblServico
            // 
            this.lblServico.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblServico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblServico.Location = new System.Drawing.Point(504, 62);
            this.lblServico.Name = "lblServico";
            this.lblServico.Size = new System.Drawing.Size(60, 36);
            this.lblServico.TabIndex = 10;
            this.lblServico.Text = "SERVIÇO:";
            this.lblServico.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbServico
            // 
            this.cmbServico.BackColor = System.Drawing.Color.Transparent;
            this.cmbServico.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbServico.BorderRadius = 8;
            this.cmbServico.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbServico.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbServico.FocusedColor = System.Drawing.Color.Empty;
            this.cmbServico.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbServico.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbServico.ItemHeight = 30;
            this.cmbServico.Location = new System.Drawing.Point(570, 62);
            this.cmbServico.Name = "cmbServico";
            this.cmbServico.Size = new System.Drawing.Size(220, 36);
            this.cmbServico.TabIndex = 11;
            // 
            // lblEstado
            // 
            this.lblEstado.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblEstado.Location = new System.Drawing.Point(20, 226);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(90, 36);
            this.lblEstado.TabIndex = 14;
            this.lblEstado.Text = "ESTADO:";
            this.lblEstado.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblEstado.Click += new System.EventHandler(this.lblEstado_Click);
            // 
            // cmbEstado
            // 
            this.cmbEstado.BackColor = System.Drawing.Color.Transparent;
            this.cmbEstado.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbEstado.BorderRadius = 8;
            this.cmbEstado.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEstado.FocusedColor = System.Drawing.Color.Empty;
            this.cmbEstado.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.cmbEstado.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbEstado.ItemHeight = 30;
            this.cmbEstado.Items.AddRange(new object[] {
            "Pendente",
            "Aprovada",
            "Recusada"});
            this.cmbEstado.Location = new System.Drawing.Point(116, 226);
            this.cmbEstado.Name = "cmbEstado";
            this.cmbEstado.Size = new System.Drawing.Size(180, 36);
            this.cmbEstado.TabIndex = 15;
            // 
            // lblDesconto
            // 
            this.lblDesconto.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDesconto.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblDesconto.Location = new System.Drawing.Point(507, 226);
            this.lblDesconto.Name = "lblDesconto";
            this.lblDesconto.Size = new System.Drawing.Size(75, 25);
            this.lblDesconto.TabIndex = 18;
            this.lblDesconto.Text = "DESCONTO (%):";
            this.lblDesconto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudDesconto
            // 
            this.nudDesconto.DecimalPlaces = 2;
            this.nudDesconto.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.nudDesconto.Location = new System.Drawing.Point(588, 224);
            this.nudDesconto.Name = "nudDesconto";
            this.nudDesconto.Size = new System.Drawing.Size(100, 27);
            this.nudDesconto.TabIndex = 19;
            this.nudDesconto.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // panelItens
            // 
            this.panelItens.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelItens.BorderRadius = 12;
            this.panelItens.BorderThickness = 1;
            this.panelItens.Controls.Add(this.lblTituloItens);
            this.panelItens.Controls.Add(this.separator2);
            this.panelItens.Controls.Add(this.dgvItens);
            this.panelItens.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelItens.Location = new System.Drawing.Point(0, 300);
            this.panelItens.Name = "panelItens";
            this.panelItens.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelItens.Size = new System.Drawing.Size(1280, 320);
            this.panelItens.TabIndex = 1;
            // 
            // lblTituloItens
            // 
            this.lblTituloItens.BackColor = System.Drawing.Color.Transparent;
            this.lblTituloItens.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTituloItens.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblTituloItens.Location = new System.Drawing.Point(20, 5);
            this.lblTituloItens.Name = "lblTituloItens";
            this.lblTituloItens.Size = new System.Drawing.Size(211, 27);
            this.lblTituloItens.TabIndex = 0;
            this.lblTituloItens.Text = "🛒 ITENS DA COTAÇÃO";
            // 
            // separator2
            // 
            this.separator2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.separator2.Location = new System.Drawing.Point(20, 35);
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(1230, 2);
            this.separator2.TabIndex = 1;
            // 
            // dgvItens
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.dgvItens.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvItens.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvItens.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvItens.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvItens.Location = new System.Drawing.Point(20, 50);
            this.dgvItens.Name = "dgvItens";
            this.dgvItens.RowHeadersVisible = false;
            this.dgvItens.RowTemplate.Height = 35;
            this.dgvItens.Size = new System.Drawing.Size(1230, 240);
            this.dgvItens.TabIndex = 2;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvItens.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvItens.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvItens.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.dgvItens.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvItens.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvItens.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvItens.ThemeStyle.HeaderStyle.Height = 23;
            this.dgvItens.ThemeStyle.ReadOnly = false;
            this.dgvItens.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvItens.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvItens.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvItens.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvItens.ThemeStyle.RowsStyle.Height = 35;
            this.dgvItens.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvItens.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // panelBotoes
            // 
            this.panelBotoes.BorderRadius = 12;
            this.panelBotoes.Controls.Add(this.btnAdicionarItem);
            this.panelBotoes.Controls.Add(this.btnRemoverItem);
            this.panelBotoes.Controls.Add(this.btnSalvar);
            this.panelBotoes.Controls.Add(this.btnImprimir);
            this.panelBotoes.FillColor = System.Drawing.Color.White;
            this.panelBotoes.Location = new System.Drawing.Point(0, 640);
            this.panelBotoes.Name = "panelBotoes";
            this.panelBotoes.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelBotoes.Size = new System.Drawing.Size(874, 80);
            this.panelBotoes.TabIndex = 2;
            // 
            // btnAdicionarItem
            // 
            this.btnAdicionarItem.BorderRadius = 10;
            this.btnAdicionarItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(199)))), ((int)(((byte)(89)))));
            this.btnAdicionarItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnAdicionarItem.ForeColor = System.Drawing.Color.White;
            this.btnAdicionarItem.Location = new System.Drawing.Point(20, 15);
            this.btnAdicionarItem.Name = "btnAdicionarItem";
            this.btnAdicionarItem.Size = new System.Drawing.Size(140, 45);
            this.btnAdicionarItem.TabIndex = 0;
            this.btnAdicionarItem.Text = "➕ Adicionar Item";
            // 
            // btnRemoverItem
            // 
            this.btnRemoverItem.BorderRadius = 10;
            this.btnRemoverItem.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(59)))), ((int)(((byte)(48)))));
            this.btnRemoverItem.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnRemoverItem.ForeColor = System.Drawing.Color.White;
            this.btnRemoverItem.Location = new System.Drawing.Point(175, 15);
            this.btnRemoverItem.Name = "btnRemoverItem";
            this.btnRemoverItem.Size = new System.Drawing.Size(140, 45);
            this.btnRemoverItem.TabIndex = 1;
            this.btnRemoverItem.Text = "🗑️ Remover Item";
            // 
            // btnSalvar
            // 
            this.btnSalvar.BorderRadius = 10;
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(330, 15);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(150, 45);
            this.btnSalvar.TabIndex = 2;
            this.btnSalvar.Text = "💾 Salvar Cotação";
            // 
            // btnImprimir
            // 
            this.btnImprimir.BorderRadius = 10;
            this.btnImprimir.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(149)))), ((int)(((byte)(0)))));
            this.btnImprimir.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnImprimir.ForeColor = System.Drawing.Color.White;
            this.btnImprimir.Location = new System.Drawing.Point(495, 15);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(140, 45);
            this.btnImprimir.TabIndex = 3;
            this.btnImprimir.Text = "📄 Gerar PDF";
            // 
            // panelTotais
            // 
            this.panelTotais.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelTotais.BorderRadius = 12;
            this.panelTotais.BorderThickness = 1;
            this.panelTotais.Controls.Add(this.lblSubTotal);
            this.panelTotais.Controls.Add(this.separator3);
            this.panelTotais.Controls.Add(this.lblIva);
            this.panelTotais.Controls.Add(this.lblTotal);
            this.panelTotais.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.panelTotais.Location = new System.Drawing.Point(880, 640);
            this.panelTotais.Name = "panelTotais";
            this.panelTotais.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panelTotais.Size = new System.Drawing.Size(400, 120);
            this.panelTotais.TabIndex = 3;
            // 
            // lblSubTotal
            // 
            this.lblSubTotal.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblSubTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblSubTotal.Location = new System.Drawing.Point(20, 15);
            this.lblSubTotal.Name = "lblSubTotal";
            this.lblSubTotal.Size = new System.Drawing.Size(360, 25);
            this.lblSubTotal.TabIndex = 0;
            this.lblSubTotal.Text = "SUBTOTAL: 0,00 MT";
            this.lblSubTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // separator3
            // 
            this.separator3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.separator3.Location = new System.Drawing.Point(23, 43);
            this.separator3.Name = "separator3";
            this.separator3.Size = new System.Drawing.Size(360, 1);
            this.separator3.TabIndex = 1;
            // 
            // lblIva
            // 
            this.lblIva.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblIva.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(100)))));
            this.lblIva.Location = new System.Drawing.Point(20, 47);
            this.lblIva.Name = "lblIva";
            this.lblIva.Size = new System.Drawing.Size(360, 25);
            this.lblIva.TabIndex = 2;
            this.lblIva.Text = "IVA: 0,00 MT";
            this.lblIva.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.lblTotal.Location = new System.Drawing.Point(23, 75);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(360, 30);
            this.lblTotal.TabIndex = 3;
            this.lblTotal.Text = "TOTAL: 0,00 MT";
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dgvCotacoes
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.dgvCotacoes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCotacoes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCotacoes.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvCotacoes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvCotacoes.Location = new System.Drawing.Point(0, 780);
            this.dgvCotacoes.Name = "dgvCotacoes";
            this.dgvCotacoes.RowHeadersVisible = false;
            this.dgvCotacoes.RowTemplate.Height = 35;
            this.dgvCotacoes.Size = new System.Drawing.Size(1280, 140);
            this.dgvCotacoes.TabIndex = 4;
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvCotacoes.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvCotacoes.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvCotacoes.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvCotacoes.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.dgvCotacoes.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvCotacoes.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvCotacoes.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvCotacoes.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvCotacoes.ThemeStyle.HeaderStyle.Height = 23;
            this.dgvCotacoes.ThemeStyle.ReadOnly = false;
            this.dgvCotacoes.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvCotacoes.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvCotacoes.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvCotacoes.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvCotacoes.ThemeStyle.RowsStyle.Height = 35;
            this.dgvCotacoes.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvCotacoes.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // FormCotacao
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1334, 749);
            this.Controls.Add(this.panelScroll);
            this.MinimumSize = new System.Drawing.Size(1200, 700);
            this.Name = "FormCotacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestão de Cotações - AC Electricidade e Serviços";
            this.panelScroll.ResumeLayout(false);
            this.panelDados.ResumeLayout(false);
            this.panelDados.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDesconto)).EndInit();
            this.panelItens.ResumeLayout(false);
            this.panelItens.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvItens)).EndInit();
            this.panelBotoes.ResumeLayout(false);
            this.panelTotais.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCotacoes)).EndInit();
            this.ResumeLayout(false);

        }

        private Guna2HtmlLabel lblTituloDados;
        private Guna2HtmlLabel lblTituloItens;
    }
}