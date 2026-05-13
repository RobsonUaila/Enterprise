using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.Windows.Forms;
using Enterprise.Forms;

namespace Enterprise
{
    public partial class FormPrincipal : Form
    {
        // Componentes do formulário
        private Guna2Panel panelMenu;
        private Guna2Panel panelConteudo;
        private Guna2Button btnDashboard;
        private Guna2Button btnClientes;
        private Guna2Button btnServicos;
        private Guna2Button btnCotacoes;
        private Guna2Button btnFacturas;
        private Guna2Button btnRecibos;
        private Guna2Button btnOrdensTrabalho;
        private Guna2Button btnEmpresa;
        private Guna2Button btnSair;
        private Label lblTitulo;
        private Label lblVersao;

        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.panelMenu = new Guna.UI2.WinForms.Guna2Panel();
            this.panelConteudo = new Guna.UI2.WinForms.Guna2Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblVersao = new System.Windows.Forms.Label();
            this.btnDashboard = new Guna.UI2.WinForms.Guna2Button();
            this.btnClientes = new Guna.UI2.WinForms.Guna2Button();
            this.btnServicos = new Guna.UI2.WinForms.Guna2Button();
            this.btnCotacoes = new Guna.UI2.WinForms.Guna2Button();
            this.btnFacturas = new Guna.UI2.WinForms.Guna2Button();
            this.btnRecibos = new Guna.UI2.WinForms.Guna2Button();
            this.btnOrdensTrabalho = new Guna.UI2.WinForms.Guna2Button();
            this.btnEmpresa = new Guna.UI2.WinForms.Guna2Button();
            this.btnSair = new Guna.UI2.WinForms.Guna2Button();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();

            // ========== FORMULÁRIO PRINCIPAL ==========
            this.Text = "Sistema de Gestão Empresarial";
            this.Size = new System.Drawing.Size(1200, 700);
            this.BackColor = System.Drawing.Color.FromArgb(240, 242, 245);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(1000, 600);

            // ========== PAINEL DE MENU (ESQUERDA) ==========
            this.panelMenu.BackColor = System.Drawing.Color.FromArgb(30, 30, 45);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.Size = new System.Drawing.Size(250, 700);
            this.panelMenu.Padding = new System.Windows.Forms.Padding(10);

            // Título do Sistema
            this.lblTitulo.Text = "🏢 Enterprise";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 18, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Size = new System.Drawing.Size(210, 40);
            this.lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            this.panelMenu.Controls.Add(this.lblTitulo);

            // Linha separadora
            Guna2Separator separator = new Guna2Separator();
            separator.Location = new System.Drawing.Point(15, 70);
            separator.Size = new System.Drawing.Size(220, 2);
            separator.FillColor = System.Drawing.Color.FromArgb(0, 122, 255);
            this.panelMenu.Controls.Add(separator);

            int yPos = 90;

            // ========== BOTÃO DASHBOARD (NOVO) ==========
            this.btnDashboard = CriarBotaoMenu("📊 Dashboard", yPos, Color.FromArgb(0, 122, 255)); // Destacado em azul
            this.btnDashboard.Click += BtnDashboard_Click;
            this.panelMenu.Controls.Add(this.btnDashboard);
            yPos += 55;

            // Linha separadora após o Dashboard
            Guna2Separator separator2 = new Guna2Separator();
            separator2.Location = new System.Drawing.Point(15, yPos);
            separator2.Size = new System.Drawing.Size(220, 1);
            separator2.FillColor = System.Drawing.Color.FromArgb(60, 60, 75);
            this.panelMenu.Controls.Add(separator2);
            yPos += 15;

            // ========== BOTÃO CLIENTES ==========
            this.btnClientes = CriarBotaoMenu("👥 Clientes", yPos);
            this.btnClientes.Click += BtnClientes_Click;
            this.panelMenu.Controls.Add(this.btnClientes);
            yPos += 55;

            // ========== BOTÃO SERVIÇOS ==========
            this.btnServicos = CriarBotaoMenu("🔧 Serviços", yPos);
            this.btnServicos.Click += BtnServicos_Click;
            this.panelMenu.Controls.Add(this.btnServicos);
            yPos += 55;

            // ========== BOTÃO COTAÇÕES ==========
            this.btnCotacoes = CriarBotaoMenu("📄 Cotações", yPos);
            this.btnCotacoes.Click += BtnCotacoes_Click;
            this.panelMenu.Controls.Add(this.btnCotacoes);
            yPos += 55;

            // ========== BOTÃO FACTURAS ==========
            this.btnFacturas = CriarBotaoMenu("📊 Facturas", yPos);
            this.btnFacturas.Click += BtnFacturas_Click;
            this.panelMenu.Controls.Add(this.btnFacturas);
            yPos += 55;

            // ========== BOTÃO RECIBOS ==========
            this.btnRecibos = CriarBotaoMenu("💰 Recibos", yPos);
            this.btnRecibos.Click += BtnRecibos_Click;
            this.panelMenu.Controls.Add(this.btnRecibos);
            yPos += 55;

            // ========== BOTÃO ORDENS DE TRABALHO ==========
            this.btnOrdensTrabalho = CriarBotaoMenu("🔨 Ordens de Trabalho", yPos);
            this.btnOrdensTrabalho.Click += BtnOrdensTrabalho_Click;
            this.panelMenu.Controls.Add(this.btnOrdensTrabalho);
            yPos += 55;

            // ========== BOTÃO EMPRESA ==========
            this.btnEmpresa = CriarBotaoMenu("🏭 Empresa", yPos);
            this.btnEmpresa.Click += BtnEmpresa_Click;
            this.panelMenu.Controls.Add(this.btnEmpresa);
            yPos += 65;

            // ========== BOTÃO SAIR ==========
            this.btnSair = CriarBotaoMenu("🚪 Sair", yPos);
            this.btnSair.FillColor = System.Drawing.Color.FromArgb(220, 53, 69);
            this.btnSair.Click += BtnSair_Click;
            this.panelMenu.Controls.Add(this.btnSair);

            // Versão
            this.lblVersao.Text = "Versão 1.0";
            this.lblVersao.Font = new System.Drawing.Font("Segoe UI", 9, System.Drawing.FontStyle.Italic);
            this.lblVersao.ForeColor = System.Drawing.Color.FromArgb(150, 150, 150);
            this.lblVersao.Location = new System.Drawing.Point(15, 650);
            this.lblVersao.Size = new System.Drawing.Size(220, 20);
            this.lblVersao.TextAlign = ContentAlignment.MiddleCenter;
            this.panelMenu.Controls.Add(this.lblVersao);

            // ========== PAINEL DE CONTEÚDO (vazio, apenas para decoração) ==========
            this.panelConteudo.BackColor = System.Drawing.Color.White;
            this.panelConteudo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConteudo.Padding = new System.Windows.Forms.Padding(20);

            // Label de boas-vindas
            Label lblWelcome = new Label();
            lblWelcome.Text = "Bem-vindo ao Sistema De Gestão\n\nSelecione uma opção no menu lateral para começar.";
            lblWelcome.Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Regular);
            lblWelcome.ForeColor = System.Drawing.Color.FromArgb(80, 80, 100);
            lblWelcome.Location = new System.Drawing.Point(50, 50);
            lblWelcome.Size = new System.Drawing.Size(600, 100);
            lblWelcome.TextAlign = ContentAlignment.MiddleLeft;
            this.panelConteudo.Controls.Add(lblWelcome);

            // Adicionar painéis ao formulário
            this.Controls.Add(this.panelConteudo);
            this.Controls.Add(this.panelMenu);

            this.panelMenu.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private Guna2Button CriarBotaoMenu(string texto, int y, Color? corFundo = null)
        {
            Guna2Button btn = new Guna2Button();
            btn.Text = texto;
            btn.Location = new System.Drawing.Point(15, y);
            btn.Size = new System.Drawing.Size(220, 45);
            btn.BorderRadius = 8;
            btn.FillColor = corFundo ?? System.Drawing.Color.FromArgb(45, 45, 60);
            btn.ForeColor = System.Drawing.Color.White;
            btn.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
            btn.HoverState.FillColor = System.Drawing.Color.FromArgb(0, 122, 255);
            btn.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            btn.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
            btn.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            return btn;
        }

        // ========== EVENTO DO BOTÃO DASHBOARD ==========
        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            var form = new FormDashboard();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog(this);
        }

        // ========== EVENTOS DOS BOTÕES (JANELAS INDEPENDENTES) ==========

        private void BtnClientes_Click(object sender, EventArgs e)
        {
            var form = new FormCliente();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog(this);
        }

        private void BtnServicos_Click(object sender, EventArgs e)
        {
            var form = new FormServicos();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog(this);
        }

        private void BtnCotacoes_Click(object sender, EventArgs e)
        {
            var form = new FormCotacao();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog(this);
        }

        private void BtnFacturas_Click(object sender, EventArgs e)
        {
            var form = new FormFactura();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog(this);
        }

        private void BtnRecibos_Click(object sender, EventArgs e)
        {
            var form = new FormRecibo();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog(this);
        }

        private void BtnOrdensTrabalho_Click(object sender, EventArgs e)
        {
            var form = new FormOrdemTrabalho();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog(this);
        }

        private void BtnEmpresa_Click(object sender, EventArgs e)
        {
            var form = new FormEmpresa();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog(this);
        }

        private void BtnSair_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Tem certeza que deseja sair da aplicação?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private System.ComponentModel.IContainer components = null;
    }
}