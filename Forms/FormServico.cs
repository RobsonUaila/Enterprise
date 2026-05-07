// ============================================================
// FormServicos.cs — Gestão de Serviços com Guna UI
// ============================================================

using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Enterprise.Data;
using Enterprise.Models;

namespace Enterprise.Forms
{
    public partial class FormServicos : Form
    {
        private Servico? _servicoSelecionado;
        private System.ComponentModel.IContainer? components = null;

        // Componentes do formulário
        private Guna2TextBox txtNome;
        private Guna2TextBox txtCodigo;
        private Guna2TextBox txtDescricao;
        private Guna2TextBox txtFiltro;
        private Guna2ComboBox cmbCategoria;
        private Guna2ComboBox cmbUnidade;
        private Guna2CheckBox chkActivo;
        private Guna2DataGridView dgvServicos;
        private Guna2Button btnSalvar;
        private Guna2Button btnNovo;
        private Guna2Button btnApagar;
        private Guna2Panel panelForm;
        private Label lblTitulo;
        private Guna2Separator linha;
        private Label lblCodigo;
        private Label lblNome;
        private Label lblDescricao;
        private Label lblCategoria;
        private Guna2Panel panelGrid;
        private Label lblListaTitulo;
        private Guna2Separator linhaGrid;
        private Label lblTotal;

        public FormServicos()
        {
            InitializeComponent();
            CarregarCategorias();
            CarregarServicos();
        }

        private void CarregarCategorias()
        {
            try
            {
                var cats = AppDataConnection.GetCategoriasServico();
                cmbCategoria.DataSource = cats;
                cmbCategoria.DisplayMember = "Nome";
                cmbCategoria.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar categorias: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarServicos()
        {
            try
            {
                string filtro = txtFiltro.Text;
                if (filtro == "🔍 Pesquisar por nome, código ou categoria...")
                    filtro = "";

                var lista = AppDataConnection.GetServicos(filtro, false);
                dgvServicos.DataSource = lista;

                if (dgvServicos.Columns.Count > 0)
                {
                    EsconderColuna("Id");
                    EsconderColuna("CriadoEm");
                    EsconderColuna("CategoriaId");
                    EsconderColuna("Descricao");
                    EsconderColuna("Activo");
                    EsconderColuna("PrecoBase");
                    EsconderColuna("Unidade");

                    RenomearColuna("Codigo", "CÓDIGO");
                    RenomearColuna("Nome", "NOME");

                    if (dgvServicos.Columns.Contains("PrecoBase"))
                        dgvServicos.Columns["PrecoBase"].DefaultCellStyle.Format = "N2";
                }

                lblTotal.Text = "📊 " + lista.Count + " serviços registados";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar serviços: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EsconderColuna(string nome)
        {
            if (dgvServicos.Columns.Contains(nome))
                dgvServicos.Columns[nome].Visible = false;
        }

        private void RenomearColuna(string nome, string header)
        {
            if (dgvServicos.Columns.Contains(nome))
                dgvServicos.Columns[nome].HeaderText = header;
        }

        private void LimparFormulario()
        {
            _servicoSelecionado = null;
            txtCodigo.Text = "";
            txtNome.Text = "";
            txtDescricao.Text = "";
            chkActivo.Checked = true;
            if (cmbCategoria.Items.Count > 0) cmbCategoria.SelectedIndex = -1;
            btnSalvar.Text = "💾 Salvar";
            btnSalvar.FillColor = Color.FromArgb(52, 199, 89);
            txtNome.Focus();
        }

        private void PreencherFormulario(Servico s)
        {
            txtCodigo.Text = s.Codigo ?? "";
            txtNome.Text = s.Nome;
            chkActivo.Checked = s.Activo;
            if (s.CategoriaId.HasValue && cmbCategoria.Items.Count > 0)
                cmbCategoria.SelectedValue = s.CategoriaId.Value;
            btnSalvar.Text = "✏️ Actualizar";
            btnSalvar.FillColor = Color.FromArgb(0, 122, 255);
        }

        // ── EVENTOS ──────────────────────────────────────────

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
                int? catId = cmbCategoria.SelectedValue is int selectedId ? selectedId : (int?)null;

                var servico = new Servico
                {
                    Id = _servicoSelecionado?.Id ?? 0,
                    CategoriaId = catId,
                    Codigo = txtCodigo.Text.Trim(),
                    Nome = txtNome.Text.Trim(),
                    Activo = chkActivo.Checked
                };

                AppDataConnection.SalvarServico(servico);

                MessageBox.Show("Serviço guardado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimparFormulario();
                CarregarServicos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            if (_servicoSelecionado == null)
            {
                MessageBox.Show("Selecione um serviço!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Apagar '" + _servicoSelecionado.Nome + "'?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    AppDataConnection.ApagarServico(_servicoSelecionado.Id);
                    LimparFormulario();
                    CarregarServicos();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao apagar: " + ex.Message, "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvServicos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            _servicoSelecionado = dgvServicos.Rows[e.RowIndex].DataBoundItem as Servico;
            if (_servicoSelecionado != null)
                PreencherFormulario(_servicoSelecionado);
        }

        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            if (txtFiltro.Text != "🔍 Pesquisar por nome, código ou categoria...")
                CarregarServicos();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        // ═══════════════════════════════════════════════════════════
        // INITIALIZE COMPONENT - FORMATO DESIGNER SAFE
        // ═══════════════════════════════════════════════════════════

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtNome = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtCodigo = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtDescricao = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtFiltro = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbCategoria = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cmbUnidade = new Guna.UI2.WinForms.Guna2ComboBox();
            this.chkActivo = new Guna.UI2.WinForms.Guna2CheckBox();
            this.dgvServicos = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.btnNovo = new Guna.UI2.WinForms.Guna2Button();
            this.btnApagar = new Guna.UI2.WinForms.Guna2Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.panelForm = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.linha = new Guna.UI2.WinForms.Guna2Separator();
            this.lblCodigo = new System.Windows.Forms.Label();
            this.lblNome = new System.Windows.Forms.Label();
            this.lblDescricao = new System.Windows.Forms.Label();
            this.lblCategoria = new System.Windows.Forms.Label();
            this.panelGrid = new Guna.UI2.WinForms.Guna2Panel();
            this.lblListaTitulo = new System.Windows.Forms.Label();
            this.linhaGrid = new Guna.UI2.WinForms.Guna2Separator();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServicos)).BeginInit();
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
            this.txtNome.Location = new System.Drawing.Point(20, 180);
            this.txtNome.Name = "txtNome";
            this.txtNome.PlaceholderText = "";
            this.txtNome.SelectedText = "";
            this.txtNome.Size = new System.Drawing.Size(360, 39);
            this.txtNome.TabIndex = 5;
            // 
            // txtCodigo
            // 
            this.txtCodigo.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtCodigo.BorderRadius = 6;
            this.txtCodigo.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCodigo.DefaultText = "";
            this.txtCodigo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtCodigo.Location = new System.Drawing.Point(20, 93);
            this.txtCodigo.Name = "txtCodigo";
            this.txtCodigo.PlaceholderText = "";
            this.txtCodigo.SelectedText = "";
            this.txtCodigo.Size = new System.Drawing.Size(360, 39);
            this.txtCodigo.TabIndex = 3;
            // 
            // txtDescricao
            // 
            this.txtDescricao.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txtDescricao.BorderRadius = 6;
            this.txtDescricao.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDescricao.DefaultText = "";
            this.txtDescricao.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDescricao.Location = new System.Drawing.Point(20, 277);
            this.txtDescricao.Multiline = true;
            this.txtDescricao.Name = "txtDescricao";
            this.txtDescricao.PlaceholderText = "";
            this.txtDescricao.SelectedText = "";
            this.txtDescricao.Size = new System.Drawing.Size(360, 54);
            this.txtDescricao.TabIndex = 7;
            this.txtDescricao.TextChanged += new System.EventHandler(this.txtDescricao_TextChanged);
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
            this.txtFiltro.PlaceholderText = "🔍 Pesquisar por nome, código ou categoria...";
            this.txtFiltro.SelectedText = "";
            this.txtFiltro.Size = new System.Drawing.Size(400, 41);
            this.txtFiltro.TabIndex = 2;
            // 
            // cmbCategoria
            // 
            this.cmbCategoria.BackColor = System.Drawing.Color.Transparent;
            this.cmbCategoria.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cmbCategoria.BorderRadius = 6;
            this.cmbCategoria.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbCategoria.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategoria.FocusedColor = System.Drawing.Color.Empty;
            this.cmbCategoria.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbCategoria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbCategoria.ItemHeight = 30;
            this.cmbCategoria.Location = new System.Drawing.Point(20, 385);
            this.cmbCategoria.Name = "cmbCategoria";
            this.cmbCategoria.Size = new System.Drawing.Size(360, 36);
            this.cmbCategoria.TabIndex = 9;
            // 
            // cmbUnidade
            // 
            this.cmbUnidade.BackColor = System.Drawing.Color.Transparent;
            this.cmbUnidade.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbUnidade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnidade.FocusedColor = System.Drawing.Color.Empty;
            this.cmbUnidade.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbUnidade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cmbUnidade.ItemHeight = 30;
            this.cmbUnidade.Location = new System.Drawing.Point(0, 0);
            this.cmbUnidade.Name = "cmbUnidade";
            this.cmbUnidade.Size = new System.Drawing.Size(140, 36);
            this.cmbUnidade.TabIndex = 0;
            // 
            // chkActivo
            // 
            this.chkActivo.Checked = true;
            this.chkActivo.CheckedState.BorderRadius = 0;
            this.chkActivo.CheckedState.BorderThickness = 0;
            this.chkActivo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActivo.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.chkActivo.Location = new System.Drawing.Point(20, 452);
            this.chkActivo.Name = "chkActivo";
            this.chkActivo.Size = new System.Drawing.Size(150, 30);
            this.chkActivo.TabIndex = 10;
            this.chkActivo.Text = "✓ Serviço activo";
            this.chkActivo.UncheckedState.BorderRadius = 0;
            this.chkActivo.UncheckedState.BorderThickness = 0;
            // 
            // dgvServicos
            // 
            this.dgvServicos.AllowUserToAddRows = false;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.dgvServicos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvServicos.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvServicos.ColumnHeadersHeight = 40;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvServicos.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvServicos.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvServicos.Location = new System.Drawing.Point(20, 126);
            this.dgvServicos.MultiSelect = false;
            this.dgvServicos.Name = "dgvServicos";
            this.dgvServicos.ReadOnly = true;
            this.dgvServicos.RowHeadersVisible = false;
            this.dgvServicos.RowTemplate.Height = 35;
            this.dgvServicos.Size = new System.Drawing.Size(660, 490);
            this.dgvServicos.TabIndex = 4;
            this.dgvServicos.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(252)))));
            this.dgvServicos.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvServicos.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvServicos.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvServicos.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvServicos.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvServicos.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvServicos.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.dgvServicos.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvServicos.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.dgvServicos.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvServicos.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvServicos.ThemeStyle.HeaderStyle.Height = 40;
            this.dgvServicos.ThemeStyle.ReadOnly = true;
            this.dgvServicos.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvServicos.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvServicos.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvServicos.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvServicos.ThemeStyle.RowsStyle.Height = 35;
            this.dgvServicos.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvServicos.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btnSalvar
            // 
            this.btnSalvar.BorderRadius = 8;
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(199)))), ((int)(((byte)(89)))));
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(20, 507);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(120, 40);
            this.btnSalvar.TabIndex = 11;
            this.btnSalvar.Text = "💾 Salvar";
            // 
            // btnNovo
            // 
            this.btnNovo.BorderRadius = 8;
            this.btnNovo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.btnNovo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnNovo.ForeColor = System.Drawing.Color.White;
            this.btnNovo.Location = new System.Drawing.Point(146, 507);
            this.btnNovo.Name = "btnNovo";
            this.btnNovo.Size = new System.Drawing.Size(120, 40);
            this.btnNovo.TabIndex = 12;
            this.btnNovo.Text = "➕ Novo";
            // 
            // btnApagar
            // 
            this.btnApagar.BorderRadius = 8;
            this.btnApagar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(59)))), ((int)(((byte)(48)))));
            this.btnApagar.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnApagar.ForeColor = System.Drawing.Color.White;
            this.btnApagar.Location = new System.Drawing.Point(272, 507);
            this.btnApagar.Name = "btnApagar";
            this.btnApagar.Size = new System.Drawing.Size(120, 40);
            this.btnApagar.TabIndex = 13;
            this.btnApagar.Text = "🗑️ Apagar";
            // 
            // lblTotal
            // 
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblTotal.ForeColor = System.Drawing.Color.Gray;
            this.lblTotal.Location = new System.Drawing.Point(460, 66);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(240, 25);
            this.lblTotal.TabIndex = 3;
            this.lblTotal.Text = "📊 0 serviços registados";
            // 
            // panelForm
            // 
            this.panelForm.AutoScroll = true;
            this.panelForm.BorderRadius = 10;
            this.panelForm.Controls.Add(this.lblTitulo);
            this.panelForm.Controls.Add(this.linha);
            this.panelForm.Controls.Add(this.lblCodigo);
            this.panelForm.Controls.Add(this.txtCodigo);
            this.panelForm.Controls.Add(this.lblNome);
            this.panelForm.Controls.Add(this.txtNome);
            this.panelForm.Controls.Add(this.lblDescricao);
            this.panelForm.Controls.Add(this.txtDescricao);
            this.panelForm.Controls.Add(this.lblCategoria);
            this.panelForm.Controls.Add(this.cmbCategoria);
            this.panelForm.Controls.Add(this.chkActivo);
            this.panelForm.Controls.Add(this.btnSalvar);
            this.panelForm.Controls.Add(this.btnNovo);
            this.panelForm.Controls.Add(this.btnApagar);
            this.panelForm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelForm.Location = new System.Drawing.Point(20, 20);
            this.panelForm.Name = "panelForm";
            this.panelForm.Size = new System.Drawing.Size(420, 800);
            this.panelForm.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(300, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "DADOS DO SERVIÇO";
            // 
            // linha
            // 
            this.linha.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.linha.Location = new System.Drawing.Point(20, 45);
            this.linha.Name = "linha";
            this.linha.Size = new System.Drawing.Size(380, 2);
            this.linha.TabIndex = 1;
            // 
            // lblCodigo
            // 
            this.lblCodigo.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCodigo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblCodigo.Location = new System.Drawing.Point(20, 60);
            this.lblCodigo.Name = "lblCodigo";
            this.lblCodigo.Size = new System.Drawing.Size(360, 18);
            this.lblCodigo.TabIndex = 2;
            this.lblCodigo.Text = "CÓDIGO";
            // 
            // lblNome
            // 
            this.lblNome.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblNome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblNome.Location = new System.Drawing.Point(21, 148);
            this.lblNome.Name = "lblNome";
            this.lblNome.Size = new System.Drawing.Size(360, 18);
            this.lblNome.TabIndex = 4;
            this.lblNome.Text = "NOME *";
            // 
            // lblDescricao
            // 
            this.lblDescricao.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblDescricao.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblDescricao.Location = new System.Drawing.Point(23, 239);
            this.lblDescricao.Name = "lblDescricao";
            this.lblDescricao.Size = new System.Drawing.Size(360, 18);
            this.lblDescricao.TabIndex = 6;
            this.lblDescricao.Text = "DESCRIÇÃO";
            // 
            // lblCategoria
            // 
            this.lblCategoria.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblCategoria.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(120)))));
            this.lblCategoria.Location = new System.Drawing.Point(21, 353);
            this.lblCategoria.Name = "lblCategoria";
            this.lblCategoria.Size = new System.Drawing.Size(360, 18);
            this.lblCategoria.TabIndex = 8;
            this.lblCategoria.Text = "CATEGORIA";
            // 
            // panelGrid
            // 
            this.panelGrid.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(224)))), ((int)(((byte)(230)))));
            this.panelGrid.BorderRadius = 10;
            this.panelGrid.BorderThickness = 1;
            this.panelGrid.Controls.Add(this.lblListaTitulo);
            this.panelGrid.Controls.Add(this.linhaGrid);
            this.panelGrid.Controls.Add(this.txtFiltro);
            this.panelGrid.Controls.Add(this.lblTotal);
            this.panelGrid.Controls.Add(this.dgvServicos);
            this.panelGrid.FillColor = System.Drawing.Color.White;
            this.panelGrid.Location = new System.Drawing.Point(460, 20);
            this.panelGrid.Name = "panelGrid";
            this.panelGrid.Size = new System.Drawing.Size(700, 620);
            this.panelGrid.TabIndex = 1;
            // 
            // lblListaTitulo
            // 
            this.lblListaTitulo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblListaTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblListaTitulo.Name = "lblListaTitulo";
            this.lblListaTitulo.Size = new System.Drawing.Size(300, 25);
            this.lblListaTitulo.TabIndex = 0;
            this.lblListaTitulo.Text = "LISTA DE SERVIÇOS";
            // 
            // linhaGrid
            // 
            this.linhaGrid.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.linhaGrid.Location = new System.Drawing.Point(20, 45);
            this.linhaGrid.Name = "linhaGrid";
            this.linhaGrid.Size = new System.Drawing.Size(660, 2);
            this.linhaGrid.TabIndex = 1;
            // 
            // FormServicos
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.panelForm);
            this.Controls.Add(this.panelGrid);
            this.Name = "FormServicos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestão de Serviços";
            ((System.ComponentModel.ISupportInitialize)(this.dgvServicos)).EndInit();
            this.panelForm.ResumeLayout(false);
            this.panelGrid.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void txtDescricao_TextChanged(object sender, EventArgs e)
        {

        }
    }
}