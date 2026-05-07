using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using Enterprise.Data;
using Enterprise.Models;

namespace Enterprise.Forms
{
    public partial class FormCliente : Form
    {
        private Cliente? _clienteSelecionado;
        private System.ComponentModel.IContainer? components = null;

        // Componentes do formulário
        private Guna2TextBox txtNome;
        private Guna2TextBox txtFiltro;
        private Guna2ComboBox cmbTipo;
        private Guna2DataGridView dgvClientes;
        private Guna2Button btnSalvar;
        private Guna2Button btnNovo;
        private Guna2Button btnApagar;
        private Guna2Panel panelScroll;
        private Guna2Panel panelForm;
        private Label lblTitulo;
        private Guna2Separator linha;
        private Label lblNome;
        private Label lblTipo;
        private Guna2Panel panelGrid;
        private Label lblListaTitulo;
        private Guna2Separator linhaGrid;
        private Label lblTotal;

        public FormCliente()
        {
            InitializeComponent();
            AplicarEstilosCustomizados();
            CarregarClientes();
        }

        private void CarregarClientes()
        {
            try
            {
                string filtro = txtFiltro.Text;
                if (filtro == "🔍 Pesquisar por nome, NUIT ou telefone...")
                    filtro = "";

                var lista = AppDataConnection.GetClientes(filtro);
                dgvClientes.DataSource = lista;

                if (dgvClientes.Columns.Count > 0)
                {
                    EsconderColuna("Id");
                    EsconderColuna("CriadoEm");
                    EsconderColuna("Telefone2");
                    EsconderColuna("Endereco");
                    EsconderColuna("Observacoes");
                    EsconderColuna("Nuit");
                    EsconderColuna("Email");
                    EsconderColuna("Telefone");
                    EsconderColuna("Cidade");

                    RenomearColuna("Nome", "NOME");
                    RenomearColuna("Tipo", "TIPO");
                }

                lblTotal.Text = $"📊 {lista.Count} clientes registados";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar clientes: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EsconderColuna(string nome)
        {
            if (dgvClientes.Columns.Contains(nome))
                dgvClientes.Columns[nome].Visible = false;
        }

        private void RenomearColuna(string nome, string header)
        {
            if (dgvClientes.Columns.Contains(nome))
                dgvClientes.Columns[nome].HeaderText = header;
        }

        private void LimparFormulario()
        {
            _clienteSelecionado = null;
            txtNome.Text = "";
            cmbTipo.SelectedIndex = 0;
            btnSalvar.Text = "💾 Salvar";
            btnSalvar.FillColor = Color.FromArgb(52, 199, 89);
            txtNome.Focus();
        }

        private void PreencherFormulario(Cliente c)
        {
            txtNome.Text = c.Nome;
            cmbTipo.Text = c.Tipo ?? "Particular";
            btnSalvar.Text = "✏️ Actualizar";
            btnSalvar.FillColor = Color.FromArgb(0, 122, 255);
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            LimparFormulario();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome é obrigatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            try
            {
                var cliente = new Cliente
                {
                    Id = _clienteSelecionado?.Id ?? 0,
                    Nome = txtNome.Text.Trim(),
                    Tipo = cmbTipo.Text,
                };

                AppDataConnection.SalvarCliente(cliente);

                MessageBox.Show("Cliente guardado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimparFormulario();
                CarregarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (_clienteSelecionado == null)
            {
                MessageBox.Show("Selecione um cliente!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show($"Apagar '{_clienteSelecionado.Nome}'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    AppDataConnection.ApagarCliente(_clienteSelecionado.Id);
                    LimparFormulario();
                    CarregarClientes();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao apagar: " + ex.Message, "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            _clienteSelecionado = dgvClientes.Rows[e.RowIndex].DataBoundItem as Cliente;
            if (_clienteSelecionado != null)
                PreencherFormulario(_clienteSelecionado);
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (txtFiltro.Text != "🔍 Pesquisar por nome, NUIT ou telefone...")
                CarregarClientes();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        // ═══════════════════════════════════════════════════════════
        // APLICAR ESTILOS CUSTOMIZADOS (chamado após InitializeComponent)
        // ═══════════════════════════════════════════════════════════
        private void AplicarEstilosCustomizados()
        {
            this.BackColor = Color.White;

            btnNovo.FillColor = Color.FromArgb(100, 100, 120);
            btnNovo.ForeColor = Color.White;
            btnNovo.Font = new Font("Segoe UI Semibold", 10);

            btnSalvar.FillColor = Color.FromArgb(52, 199, 89);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.Font = new Font("Segoe UI Semibold", 10);

            btnApagar.FillColor = Color.FromArgb(255, 59, 48);
            btnApagar.ForeColor = Color.White;
            btnApagar.Font = new Font("Segoe UI Semibold", 10);
        }

        // ═══════════════════════════════════════════════════════════
        // INITIALIZE COMPONENT - FORMATO DESIGNER SAFE
        // ═══════════════════════════════════════════════════════════
        // NOTA: Mantenha este método SIMPLES. Não use métodos helper,
        // expressões complexas, ou operadores ?? aqui.
        // O Designer do VS analisa este método e falha com código avançado.
        // ═══════════════════════════════════════════════════════════

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtNome = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtFiltro = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbTipo = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dgvClientes = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.btnNovo = new Guna.UI2.WinForms.Guna2Button();
            this.btnApagar = new Guna.UI2.WinForms.Guna2Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panelScroll = new Guna.UI2.WinForms.Guna2Panel();
            this.panelForm = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.linha = new Guna.UI2.WinForms.Guna2Separator();
            this.lblNome = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.panelGrid = new Guna.UI2.WinForms.Guna2Panel();
            this.lblListaTitulo = new System.Windows.Forms.Label();
            this.linhaGrid = new Guna.UI2.WinForms.Guna2Separator();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).BeginInit();
            this.panelScroll.SuspendLayout();
            this.panelForm.SuspendLayout();
            this.panelGrid.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNome
            // 
            this.txtNome.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtNome.BorderRadius = 6;
            this.txtNome.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNome.DefaultText = "";
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtNome.Location = new System.Drawing.Point(20, 92);
            this.txtNome.Name = "txtNome";
            this.txtNome.PlaceholderText = "";
            this.txtNome.SelectedText = "";
            this.txtNome.Size = new System.Drawing.Size(360, 45);
            this.txtNome.TabIndex = 3;
            // 
            // txtFiltro
            // 
            this.txtFiltro.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtFiltro.BorderRadius = 8;
            this.txtFiltro.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFiltro.DefaultText = "";
            this.txtFiltro.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFiltro.Location = new System.Drawing.Point(20, 66);
            this.txtFiltro.Name = "txtFiltro";
            this.txtFiltro.PlaceholderText = "🔍 Pesquisar por nome, NUIT ou telefone...";
            this.txtFiltro.SelectedText = "";
            this.txtFiltro.Size = new System.Drawing.Size(830, 41);
            this.txtFiltro.TabIndex = 2;
            // 
            // cmbTipo
            // 
            this.cmbTipo.BackColor = System.Drawing.Color.Transparent;
            this.cmbTipo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbTipo.BorderRadius = 6;
            this.cmbTipo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTipo.FocusedColor = System.Drawing.Color.Empty;
            this.cmbTipo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbTipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbTipo.ItemHeight = 30;
            this.cmbTipo.Items.AddRange(new object[] {
            "Particular",
            "Empresa"});
            this.cmbTipo.Location = new System.Drawing.Point(20, 185);
            this.cmbTipo.Name = "cmbTipo";
            this.cmbTipo.Size = new System.Drawing.Size(360, 36);
            this.cmbTipo.TabIndex = 5;
            // 
            // dgvClientes
            // 
            this.dgvClientes.AllowUserToAddRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvClientes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(40)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvClientes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvClientes.ColumnHeadersHeight = 38;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvClientes.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvClientes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvClientes.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvClientes.Location = new System.Drawing.Point(20, 123);
            this.dgvClientes.MultiSelect = false;
            this.dgvClientes.Name = "dgvClientes";
            this.dgvClientes.ReadOnly = true;
            this.dgvClientes.RowHeadersVisible = false;
            this.dgvClientes.RowTemplate.Height = 38;
            this.dgvClientes.Size = new System.Drawing.Size(830, 555);
            this.dgvClientes.TabIndex = 3;
            this.dgvClientes.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvClientes.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvClientes.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvClientes.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvClientes.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvClientes.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvClientes.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvClientes.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(40)))));
            this.dgvClientes.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvClientes.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.dgvClientes.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvClientes.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvClientes.ThemeStyle.HeaderStyle.Height = 38;
            this.dgvClientes.ThemeStyle.ReadOnly = true;
            this.dgvClientes.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvClientes.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvClientes.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dgvClientes.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvClientes.ThemeStyle.RowsStyle.Height = 38;
            this.dgvClientes.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvClientes.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btnSalvar
            // 
            this.btnSalvar.BorderRadius = 8;
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(199)))), ((int)(((byte)(89)))));
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(139, 249);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(120, 40);
            this.btnSalvar.TabIndex = 7;
            this.btnSalvar.Text = "💾 Salvar";
            // 
            // btnNovo
            // 
            this.btnNovo.BorderRadius = 8;
            this.btnNovo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.btnNovo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnNovo.ForeColor = System.Drawing.Color.White;
            this.btnNovo.Location = new System.Drawing.Point(20, 249);
            this.btnNovo.Name = "btnNovo";
            this.btnNovo.Size = new System.Drawing.Size(105, 40);
            this.btnNovo.TabIndex = 6;
            this.btnNovo.Text = "➕ Novo";
            // 
            // btnApagar
            // 
            this.btnApagar.BorderRadius = 8;
            this.btnApagar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(59)))), ((int)(((byte)(48)))));
            this.btnApagar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnApagar.ForeColor = System.Drawing.Color.White;
            this.btnApagar.Location = new System.Drawing.Point(270, 249);
            this.btnApagar.Name = "btnApagar";
            this.btnApagar.Size = new System.Drawing.Size(110, 40);
            this.btnApagar.TabIndex = 8;
            this.btnApagar.Text = "🗑️ Apagar";
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTotal.ForeColor = System.Drawing.Color.Gray;
            this.lblTotal.Location = new System.Drawing.Point(20, 665);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(400, 22);
            this.lblTotal.TabIndex = 4;
            // 
            // panelScroll
            // 
            this.panelScroll.AutoScroll = true;
            this.panelScroll.BackColor = System.Drawing.Color.White;
            this.panelScroll.Controls.Add(this.panelForm);
            this.panelScroll.Controls.Add(this.panelGrid);
            this.panelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScroll.Location = new System.Drawing.Point(0, 0);
            this.panelScroll.Name = "panelScroll";
            this.panelScroll.Padding = new System.Windows.Forms.Padding(20);
            this.panelScroll.Size = new System.Drawing.Size(1334, 749);
            this.panelScroll.TabIndex = 0;
            // 
            // panelForm
            // 
            this.panelForm.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelForm.BorderRadius = 10;
            this.panelForm.BorderThickness = 1;
            this.panelForm.Controls.Add(this.lblTitulo);
            this.panelForm.Controls.Add(this.linha);
            this.panelForm.Controls.Add(this.lblNome);
            this.panelForm.Controls.Add(this.txtNome);
            this.panelForm.Controls.Add(this.lblTipo);
            this.panelForm.Controls.Add(this.cmbTipo);
            this.panelForm.Controls.Add(this.btnNovo);
            this.panelForm.Controls.Add(this.btnSalvar);
            this.panelForm.Controls.Add(this.btnApagar);
            this.panelForm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelForm.Location = new System.Drawing.Point(23, 20);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(400, 720);
            this.panelForm.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(360, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "DADOS DO CLIENTE";
            // 
            // linha
            // 
            this.linha.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.linha.Location = new System.Drawing.Point(20, 45);
            this.linha.Name = "linha";
            this.linha.Size = new System.Drawing.Size(360, 2);
            this.linha.TabIndex = 1;
            // 
            // lblNome
            // 
            this.lblNome.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblNome.Location = new System.Drawing.Point(20, 60);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(360, 18);
            this.lblNome.TabIndex = 2;
            this.lblNome.Text = "NOME *";
            // 
            // lblTipo
            // 
            this.lblTipo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblTipo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblTipo.Location = new System.Drawing.Point(20, 154);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(360, 18);
            this.lblTipo.TabIndex = 4;
            this.lblTipo.Text = "TIPO";
            // 
            // panelGrid
            // 
            this.panelGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelGrid.BorderRadius = 10;
            this.panelGrid.BorderThickness = 1;
            this.panelGrid.Controls.Add(this.lblListaTitulo);
            this.panelGrid.Controls.Add(this.linhaGrid);
            this.panelGrid.Controls.Add(this.txtFiltro);
            this.panelGrid.Controls.Add(this.dgvClientes);
            this.panelGrid.Controls.Add(this.lblTotal);
            this.panelGrid.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.panelGrid.Location = new System.Drawing.Point(440, 20);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(870, 720);
            this.panelGrid.TabIndex = 1;
            // 
            // lblListaTitulo
            // 
            this.lblListaTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblListaTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblListaTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblListaTitulo.Name = "lblListaTitulo";
            this.lblListaTitulo.Size = new System.Drawing.Size(300, 25);
            this.lblListaTitulo.TabIndex = 0;
            this.lblListaTitulo.Text = "LISTA DE CLIENTES";
            // 
            // linhaGrid
            // 
            this.linhaGrid.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.linhaGrid.Location = new System.Drawing.Point(20, 45);
            this.linhaGrid.Name = "linhaGrid";
            this.linhaGrid.Size = new System.Drawing.Size(830, 2);
            this.linhaGrid.TabIndex = 1;
            // 
            // FormCliente
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1334, 749);
            this.Controls.Add(this.panelScroll);
            this.MinimumSize = new System.Drawing.Size(1100, 650);
            this.Name = "FormCliente";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestão de Clientes";
            ((System.ComponentModel.ISupportInitialize)(this.dgvClientes)).EndInit();
            this.panelScroll.ResumeLayout(false);
            this.panelForm.ResumeLayout(false);
            this.panelGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}