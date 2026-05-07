using System;
using System.Drawing;
using System.Windows.Forms;

namespace Enterprise
{
    partial class FormPrincipal
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelMenu = new Panel();
            btnClientes = new Button();
            btnServicos = new Button();
            btnCotacoes = new Button();
            btnFacturas = new Button();
            btnRecibos = new Button();
            btnOrdens = new Button();
            btnEmpresa = new Button();
            lblTitulo = new Label();
            panelMenu.SuspendLayout();
            SuspendLayout();

            // Panel Menu
            panelMenu.BackColor = Color.FromArgb(30, 30, 45);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Width = 200;
            panelMenu.Controls.Add(btnClientes);
            panelMenu.Controls.Add(btnServicos);
            panelMenu.Controls.Add(btnCotacoes);
            panelMenu.Controls.Add(btnFacturas);
            panelMenu.Controls.Add(btnRecibos);
            panelMenu.Controls.Add(btnOrdens);
            panelMenu.Controls.Add(btnEmpresa);
            panelMenu.Controls.Add(lblTitulo);

            // Label Título
            lblTitulo.Text = "SISTEMA\nGESTÃO";
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            lblTitulo.Size = new Size(200, 80);
            lblTitulo.Location = new Point(0, 10);

            // Configurar Botões
            ConfigurarBotao(btnClientes, "👥  Clientes", 100);
            ConfigurarBotao(btnServicos, "🔧  Serviços", 150);
            ConfigurarBotao(btnCotacoes, "📋  Cotações", 200);
            ConfigurarBotao(btnFacturas, "🧾  Facturas", 250);
            ConfigurarBotao(btnRecibos, "💰  Recibos", 300);
            ConfigurarBotao(btnOrdens, "🏗️  Ordens Trab.", 350);
            ConfigurarBotao(btnEmpresa, "⚙️  Empresa", 420);

            // Eventos
            btnClientes.Click += btnClientes_Click;
            btnServicos.Click += btnServicos_Click;
            btnCotacoes.Click += btnCotacoes_Click;
            btnFacturas.Click += btnFacturas_Click;
            btnRecibos.Click += btnRecibos_Click;
            btnOrdens.Click += btnOrdens_Click;
            btnEmpresa.Click += btnEmpresa_Click;

            // Form Principal
            ClientSize = new Size(1200, 700);
            Controls.Add(panelMenu);
            IsMdiContainer = true;
            Text = "Sistema de Gestão";
            WindowState = FormWindowState.Maximized;

            panelMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void ConfigurarBotao(Button btn, string texto, int top)
        {
            btn.Text = texto;
            btn.ForeColor = Color.White;
            btn.BackColor = Color.FromArgb(30, 30, 45);
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10);
            btn.Size = new Size(200, 45);
            btn.Location = new Point(0, top);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(15, 0, 0, 0);
            btn.Cursor = Cursors.Hand;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(60, 60, 90);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.FromArgb(30, 30, 45);
        }

        private Panel panelMenu;
        private Button btnClientes;
        private Button btnServicos;
        private Button btnCotacoes;
        private Button btnFacturas;
        private Button btnRecibos;
        private Button btnOrdens;
        private Button btnEmpresa;
        private Label lblTitulo;
    }
}