using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Enterprise.Data;
using Enterprise.Models;

namespace Enterprise.Forms
{
    public partial class FormEmpresa : Form
    {
        private string? _logoPath = null;

        public FormEmpresa()
        {
            InitializeComponent();
            CarregarEmpresa();
        }

        private void CarregarEmpresa()
        {
            try
            {
                var empresa = AppDataConnection.GetEmpresa();
                if (empresa != null)
                {
                    txtNome.Text = empresa.Nome;
                    txtNif.Text = empresa.Nuit ?? "";
                    txtEndereco.Text = empresa.Endereco ?? "";
                    txtTelefone.Text = empresa.Telefone ?? "";
                    txtEmail.Text = empresa.Email ?? "";
                    txtWebsite.Text = empresa.Website ?? "";
                    txtBanco.Text = empresa.Banco ?? "";
                    txtContaBancaria.Text = empresa.ContaBancaria ?? "";
                    txtMoeda.Text = empresa.MoedaSimbolo;
                    _logoPath = empresa.LogoPath;

                    // Mostra logo se existir
                    if (!string.IsNullOrEmpty(_logoPath) && File.Exists(_logoPath))
                        picLogo.Image = Image.FromFile(_logoPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar empresa: " + ex.Message);
            }
        }

        // Permite seleccionar o logo da empresa
        private void btnSelecionarLogo_Click(object sender, EventArgs e)
        {
            using var dlg = new OpenFileDialog
            {
                Title = "Seleccionar Logo",
                Filter = "Imagens|*.png;*.jpg;*.jpeg;*.bmp"
            };

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                _logoPath = dlg.FileName;
                picLogo.Image = Image.FromFile(_logoPath);
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome da empresa é obrigatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var empresa = new Empresa
                {
                    Nome = txtNome.Text.Trim(),
                    Nuit = txtNif.Text.Trim(),
                    Endereco = txtEndereco.Text.Trim(),
                    Telefone = txtTelefone.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Website = txtWebsite.Text.Trim(),
                    Banco = txtBanco.Text.Trim(),
                    ContaBancaria = txtContaBancaria.Text.Trim(),
                    MoedaSimbolo = txtMoeda.Text.Trim(),
                    LogoPath = _logoPath
                };

                AppDataConnection.SalvarEmpresa(empresa);

                MessageBox.Show("Dados da empresa guardados!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    partial class FormEmpresa
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtNome = new TextBox(); txtNif = new TextBox();
            txtEndereco = new TextBox(); txtTelefone = new TextBox();
            txtEmail = new TextBox(); txtWebsite = new TextBox();
            txtBanco = new TextBox(); txtContaBancaria = new TextBox();
            txtMoeda = new TextBox();
            picLogo = new PictureBox();
            btnSelecionarLogo = new Button();
            btnSalvar = new Button();
            panelForm = new Panel();

            SuspendLayout();
            Text = "Dados da Empresa";
            Size = new Size(700, 700);
            StartPosition = FormStartPosition.CenterParent;

            panelForm.Dock = DockStyle.Fill;
            panelForm.Padding = new Padding(20);
            panelForm.BackColor = Color.White;

            // Logo
            picLogo.Location = new Point(450, 20);
            picLogo.Size = new Size(200, 120);
            picLogo.BorderStyle = BorderStyle.FixedSingle;
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            panelForm.Controls.Add(picLogo);

            int y = 20;
            Campo(panelForm, "Nome da Empresa *", txtNome, y); y += 58;
            Campo(panelForm, "NIF", txtNif, y); y += 58;
            Campo(panelForm, "Endereço", txtEndereco, y); y += 58;
            Campo(panelForm, "Telefone", txtTelefone, y); y += 58;
            Campo(panelForm, "Email", txtEmail, y); y += 58;
            Campo(panelForm, "Website", txtWebsite, y); y += 58;
            Campo(panelForm, "Banco", txtBanco, y); y += 58;
            Campo(panelForm, "Conta Bancária", txtContaBancaria, y); y += 58;
            Campo(panelForm, "Símbolo Moeda", txtMoeda, y); y += 58;

            txtMoeda.Text = "MT";

            // Botão logo
            btnSelecionarLogo.Text = "🖼️ Seleccionar Logo";
            btnSelecionarLogo.Location = new Point(450, 150);
            btnSelecionarLogo.Size = new Size(200, 35);
            btnSelecionarLogo.BackColor = Color.FromArgb(52, 152, 219);
            btnSelecionarLogo.ForeColor = Color.White;
            btnSelecionarLogo.FlatStyle = FlatStyle.Flat;
            btnSelecionarLogo.FlatAppearance.BorderSize = 0;
            btnSelecionarLogo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnSelecionarLogo.Cursor = Cursors.Hand;
            btnSelecionarLogo.Click += btnSelecionarLogo_Click;
            panelForm.Controls.Add(btnSelecionarLogo);

            // Botão salvar
            btnSalvar.Text = "💾 Guardar Dados";
            btnSalvar.Location = new Point(20, y);
            btnSalvar.Size = new Size(180, 40);
            btnSalvar.BackColor = Color.FromArgb(39, 174, 96);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.FlatStyle = FlatStyle.Flat;
            btnSalvar.FlatAppearance.BorderSize = 0;
            btnSalvar.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnSalvar.Cursor = Cursors.Hand;
            btnSalvar.Click += btnSalvar_Click;
            panelForm.Controls.Add(btnSalvar);

            Controls.Add(panelForm);
            ResumeLayout(false);
        }

        private void Campo(Panel p, string label, Control ctrl, int y)
        {
            p.Controls.Add(new Label
            {
                Text = label,
                Location = new Point(20, y),
                Size = new Size(400, 18),
                Font = new Font("Segoe UI", 9)
            });
            ctrl.Location = new Point(20, y + 20);
            ctrl.Size = new Size(400, 30);
            ctrl.Font = new Font("Segoe UI", 10);
            p.Controls.Add(ctrl);
        }

        private TextBox txtNome, txtNif, txtEndereco, txtTelefone;
        private TextBox txtEmail, txtWebsite, txtBanco, txtContaBancaria, txtMoeda;
        private PictureBox picLogo;
        private Button btnSelecionarLogo, btnSalvar;
        private Panel panelForm;
    }
}