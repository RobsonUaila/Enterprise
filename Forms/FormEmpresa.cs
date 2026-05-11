using Guna.UI2.WinForms;
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

        // Componentes do formulário
        private Guna2TextBox txtNome;
        private Guna2TextBox txtNuit;
        private Guna2TextBox txtEndereco;
        private Guna2TextBox txtTelefone;
        private Guna2TextBox txtEmail;
        private Guna2TextBox txtWebsite;
        private Guna2TextBox txtBanco;
        private Guna2TextBox txtContaBancaria;
        private Guna2TextBox txtMoeda;
        private PictureBox picLogo;
        private Guna2Button btnSelecionarLogo;
        private Guna2Button btnSalvar;
        private Panel panelForm;
        private Label lblTitulo;

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
                    txtNuit.Text = empresa.Nuit ?? "";
                    txtEndereco.Text = empresa.Endereco ?? "";
                    txtTelefone.Text = empresa.Telefone ?? "";
                    txtEmail.Text = empresa.Email ?? "";
                    txtWebsite.Text = empresa.Website ?? "";
                    txtBanco.Text = empresa.Banco ?? "";
                    txtContaBancaria.Text = empresa.ContaBancaria ?? "";
                    txtMoeda.Text = empresa.MoedaSimbolo;
                    _logoPath = empresa.LogoPath;

                    if (!string.IsNullOrEmpty(_logoPath) && File.Exists(_logoPath))
                        picLogo.Image = Image.FromFile(_logoPath);
                }
                else
                {
                    txtMoeda.Text = "MT";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar empresa: " + ex.Message);
            }
        }

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

        private bool Validar()
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome da empresa é obrigatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMoeda.Text))
            {
                MessageBox.Show("O símbolo da moeda é obrigatório!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMoeda.Focus();
                return false;
            }

            return true;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (!Validar()) return;

            try
            {
                var empresa = new Empresa
                {
                    Nome = txtNome.Text.Trim(),
                    Nuit = txtNuit.Text.Trim(),
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

                MessageBox.Show("Dados da empresa guardados com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // INITIALIZE COMPONENT - VERSÃO EXTREMAMENTE SIMPLIFICADA
        // ═══════════════════════════════════════════════════════════
        private void InitializeComponent()
        {
            this.txtNome = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtNuit = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtEndereco = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTelefone = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtWebsite = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtBanco = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtContaBancaria = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtMoeda = new Guna.UI2.WinForms.Guna2TextBox();
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.btnSelecionarLogo = new Guna.UI2.WinForms.Guna2Button();
            this.btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            this.panelForm = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.panelForm.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtNome
            // 
            this.txtNome.BorderRadius = 8;
            this.txtNome.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNome.DefaultText = "";
            this.txtNome.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNome.Location = new System.Drawing.Point(23, 107);
            this.txtNome.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNome.Name = "txtNome";
            this.txtNome.PlaceholderText = "Nome da empresa *";
            this.txtNome.SelectedText = "";
            this.txtNome.Size = new System.Drawing.Size(398, 48);
            this.txtNome.TabIndex = 1;
            // 
            // txtNuit
            // 
            this.txtNuit.BorderRadius = 8;
            this.txtNuit.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNuit.DefaultText = "";
            this.txtNuit.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtNuit.Location = new System.Drawing.Point(25, 179);
            this.txtNuit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNuit.Name = "txtNuit";
            this.txtNuit.PlaceholderText = "NUIT da empresa";
            this.txtNuit.SelectedText = "";
            this.txtNuit.Size = new System.Drawing.Size(398, 48);
            this.txtNuit.TabIndex = 2;
            // 
            // txtEndereco
            // 
            this.txtEndereco.BorderRadius = 8;
            this.txtEndereco.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEndereco.DefaultText = "";
            this.txtEndereco.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtEndereco.Location = new System.Drawing.Point(25, 252);
            this.txtEndereco.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEndereco.Name = "txtEndereco";
            this.txtEndereco.PlaceholderText = "Endereço da empresa";
            this.txtEndereco.SelectedText = "";
            this.txtEndereco.Size = new System.Drawing.Size(398, 48);
            this.txtEndereco.TabIndex = 3;
            // 
            // txtTelefone
            // 
            this.txtTelefone.BorderRadius = 8;
            this.txtTelefone.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTelefone.DefaultText = "";
            this.txtTelefone.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtTelefone.Location = new System.Drawing.Point(25, 326);
            this.txtTelefone.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTelefone.Name = "txtTelefone";
            this.txtTelefone.PlaceholderText = "Telefone de contacto";
            this.txtTelefone.SelectedText = "";
            this.txtTelefone.Size = new System.Drawing.Size(398, 48);
            this.txtTelefone.TabIndex = 4;
            // 
            // txtEmail
            // 
            this.txtEmail.BorderRadius = 8;
            this.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtEmail.DefaultText = "";
            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtEmail.Location = new System.Drawing.Point(25, 400);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.PlaceholderText = "Email da empresa";
            this.txtEmail.SelectedText = "";
            this.txtEmail.Size = new System.Drawing.Size(398, 48);
            this.txtEmail.TabIndex = 5;
            // 
            // txtWebsite
            // 
            this.txtWebsite.BorderRadius = 8;
            this.txtWebsite.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtWebsite.DefaultText = "";
            this.txtWebsite.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtWebsite.Location = new System.Drawing.Point(23, 471);
            this.txtWebsite.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtWebsite.Name = "txtWebsite";
            this.txtWebsite.PlaceholderText = "Website da empresa";
            this.txtWebsite.SelectedText = "";
            this.txtWebsite.Size = new System.Drawing.Size(398, 48);
            this.txtWebsite.TabIndex = 6;
            // 
            // txtBanco
            // 
            this.txtBanco.BorderRadius = 8;
            this.txtBanco.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtBanco.DefaultText = "";
            this.txtBanco.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtBanco.Location = new System.Drawing.Point(25, 541);
            this.txtBanco.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBanco.Name = "txtBanco";
            this.txtBanco.PlaceholderText = "Banco";
            this.txtBanco.SelectedText = "";
            this.txtBanco.Size = new System.Drawing.Size(398, 48);
            this.txtBanco.TabIndex = 7;
            // 
            // txtContaBancaria
            // 
            this.txtContaBancaria.BorderRadius = 8;
            this.txtContaBancaria.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtContaBancaria.DefaultText = "";
            this.txtContaBancaria.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtContaBancaria.Location = new System.Drawing.Point(25, 610);
            this.txtContaBancaria.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtContaBancaria.Name = "txtContaBancaria";
            this.txtContaBancaria.PlaceholderText = "Nº da conta bancária";
            this.txtContaBancaria.SelectedText = "";
            this.txtContaBancaria.Size = new System.Drawing.Size(398, 48);
            this.txtContaBancaria.TabIndex = 8;
            // 
            // txtMoeda
            // 
            this.txtMoeda.BorderRadius = 8;
            this.txtMoeda.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMoeda.DefaultText = "";
            this.txtMoeda.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMoeda.Location = new System.Drawing.Point(25, 672);
            this.txtMoeda.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMoeda.Name = "txtMoeda";
            this.txtMoeda.PlaceholderText = "Símbolo da moeda *";
            this.txtMoeda.SelectedText = "";
            this.txtMoeda.Size = new System.Drawing.Size(398, 35);
            this.txtMoeda.TabIndex = 9;
            // 
            // picLogo
            // 
            this.picLogo.BackColor = System.Drawing.Color.White;
            this.picLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogo.Location = new System.Drawing.Point(450, 107);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(200, 120);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabIndex = 10;
            this.picLogo.TabStop = false;
            // 
            // btnSelecionarLogo
            // 
            this.btnSelecionarLogo.BorderRadius = 8;
            this.btnSelecionarLogo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(255)))));
            this.btnSelecionarLogo.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnSelecionarLogo.ForeColor = System.Drawing.Color.White;
            this.btnSelecionarLogo.Location = new System.Drawing.Point(450, 290);
            this.btnSelecionarLogo.Name = "btnSelecionarLogo";
            this.btnSelecionarLogo.Size = new System.Drawing.Size(200, 40);
            this.btnSelecionarLogo.TabIndex = 11;
            this.btnSelecionarLogo.Text = "🖼️ Seleccionar Logo";
            this.btnSelecionarLogo.Click += new System.EventHandler(this.btnSelecionarLogo_Click);
            // 
            // btnSalvar
            // 
            this.btnSalvar.BorderRadius = 10;
            this.btnSalvar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(199)))), ((int)(((byte)(89)))));
            this.btnSalvar.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this.btnSalvar.ForeColor = System.Drawing.Color.White;
            this.btnSalvar.Location = new System.Drawing.Point(450, 350);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(200, 45);
            this.btnSalvar.TabIndex = 12;
            this.btnSalvar.Text = "💾 Guardar Dados";
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // panelForm
            // 
            this.panelForm.AutoScroll = true;
            this.panelForm.BackColor = System.Drawing.Color.White;
            this.panelForm.Controls.Add(this.lblTitulo);
            this.panelForm.Controls.Add(this.txtNome);
            this.panelForm.Controls.Add(this.txtNuit);
            this.panelForm.Controls.Add(this.txtEndereco);
            this.panelForm.Controls.Add(this.txtTelefone);
            this.panelForm.Controls.Add(this.txtEmail);
            this.panelForm.Controls.Add(this.txtWebsite);
            this.panelForm.Controls.Add(this.txtBanco);
            this.panelForm.Controls.Add(this.txtContaBancaria);
            this.panelForm.Controls.Add(this.txtMoeda);
            this.panelForm.Controls.Add(this.picLogo);
            this.panelForm.Controls.Add(this.btnSelecionarLogo);
            this.panelForm.Controls.Add(this.btnSalvar);
            this.panelForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelForm.Location = new System.Drawing.Point(0, 0);
            this.panelForm.Name = "panelForm";
            this.panelForm.Padding = new System.Windows.Forms.Padding(20);
            this.panelForm.Size = new System.Drawing.Size(684, 661);
            this.panelForm.TabIndex = 0;
            // 
            // lblTitulo
            // 
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(300, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "🏢 DADOS DA EMPRESA";
            // 
            // FormEmpresa
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(684, 661);
            this.Controls.Add(this.panelForm);
            this.MinimumSize = new System.Drawing.Size(650, 600);
            this.Name = "FormEmpresa";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Dados da Empresa";
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.panelForm.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}