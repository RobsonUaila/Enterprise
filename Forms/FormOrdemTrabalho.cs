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
    public partial class FormOrdemTrabalho : Form
    {
        private List<ItemDocumento> _itens = new List<ItemDocumento>();
        private OrdemTrabalho? _ordemActual = null;
        private const decimal IVA_PERCENTAGEM = 17m;

        public FormOrdemTrabalho()
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

                var servicos = AppDataConnection.GetServicos();
                cmbServico.DataSource = servicos;
                cmbServico.DisplayMember = "Nome";
                cmbServico.ValueMember = "Id";

                txtNumero.Text = AppDataConnection.GerarNumero("ordens_trabalho", "OT");
                dtpData.Value = DateTime.Now;
                dtpInicio.Value = DateTime.Now;
                dtpFim.Value = DateTime.Now.AddDays(30);

                cmbEstado.Items.AddRange(new object[]
                    { "Aberta", "Em Curso", "Concluída", "Cancelada" });
                cmbEstado.SelectedIndex = 0;

                CarregarOrdens();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }

        private void CarregarOrdens()
        {
            try
            {
                var lista = AppDataConnection.GetOrdens();
                dgvHistorico.DataSource = lista;

                if (dgvHistorico.Columns.Count > 0)
                {
                    string[] ocultas = { "Id","ClienteId","Itens","Cliente","Descricao",
                                         "Observacoes","DataInicio","DataFim","SubTotal",
                                         "ValorIva","Iva","CriadoEm" };
                    foreach (var col in ocultas)
                        if (dgvHistorico.Columns.Contains(col))
                            dgvHistorico.Columns[col].Visible = false;

                    if (dgvHistorico.Columns.Contains("Numero")) dgvHistorico.Columns["Numero"].HeaderText = "Nº ORDEM";
                    if (dgvHistorico.Columns.Contains("Data")) dgvHistorico.Columns["Data"].HeaderText = "DATA";
                    if (dgvHistorico.Columns.Contains("Estado")) dgvHistorico.Columns["Estado"].HeaderText = "ESTADO";
                    if (dgvHistorico.Columns.Contains("LocalObra")) dgvHistorico.Columns["LocalObra"].HeaderText = "LOCAL";
                    if (dgvHistorico.Columns.Contains("Total"))
                    {
                        dgvHistorico.Columns["Total"].HeaderText = "TOTAL (MT)";
                        dgvHistorico.Columns["Total"].DefaultCellStyle.Format = "N2";
                        dgvHistorico.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void BtnAdicionarItem_Click(object sender, EventArgs e)
        {
            if (cmbServico.SelectedItem is not Servico s) return;
            _itens.Add(new ItemDocumento
            {
                ServicoId = s.Id,
                Descricao = s.Nome,
                Unidade = s.Unidade,
                PrecoUnitario = s.PrecoBase,
                Quantidade = nudQuantidade.Value,
                Desconto = nudDesconto.Value,
                Servico = s
            });
            ActualizarGrelhaItens();
        }

        private void BtnRemoverItem_Click(object sender, EventArgs e)
        {
            if (dgvItens.SelectedRows.Count > 0 &&
                MessageBox.Show("Remover item?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (dgvItens.SelectedRows[0].DataBoundItem is ItemDocumento item)
                {
                    _itens.Remove(item);
                    ActualizarGrelhaItens();
                }
            }
        }

        private void ActualizarGrelhaItens()
        {
            dgvItens.DataSource = null;
            dgvItens.DataSource = _itens;

            if (dgvItens.Columns.Count > 0)
            {
                string[] ocultas = { "Id", "ServicoId", "Servico", "Ordem", "SubTotal", "ValorDesconto" };
                foreach (var col in ocultas)
                    if (dgvItens.Columns.Contains(col))
                        dgvItens.Columns[col].Visible = false;

                if (dgvItens.Columns.Contains("Descricao")) dgvItens.Columns["Descricao"].HeaderText = "DESCRIÇÃO";
                if (dgvItens.Columns.Contains("Unidade")) dgvItens.Columns["Unidade"].HeaderText = "UN.";
                if (dgvItens.Columns.Contains("Quantidade")) dgvItens.Columns["Quantidade"].HeaderText = "QTD";
                if (dgvItens.Columns.Contains("PrecoUnitario"))
                {
                    dgvItens.Columns["PrecoUnitario"].HeaderText = "PREÇO UNIT. (MT)";
                    dgvItens.Columns["PrecoUnitario"].DefaultCellStyle.Format = "N2";
                    dgvItens.Columns["PrecoUnitario"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                if (dgvItens.Columns.Contains("Desconto")) dgvItens.Columns["Desconto"].HeaderText = "DESC. %";
                if (dgvItens.Columns.Contains("Total"))
                {
                    dgvItens.Columns["Total"].HeaderText = "TOTAL (MT)";
                    dgvItens.Columns["Total"].DefaultCellStyle.Format = "N2";
                    dgvItens.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }

            CalcularTotais();
        }

        private void CalcularTotais()
        {
            decimal sub = 0;
            foreach (var i in _itens) sub += i.Total;
            decimal vIva = sub * (IVA_PERCENTAGEM / 100);
            decimal total = sub + vIva;

            lblSubTotal.Text = sub.ToString("N2") + " MT";
            lblIva.Text = $"IVA ({IVA_PERCENTAGEM}%):   " + vIva.ToString("N2") + " MT";
            lblTotal.Text = total.ToString("N2") + " MT";
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (cmbCliente.SelectedItem == null)
            { MessageBox.Show("Seleccione um cliente!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                var ordem = new OrdemTrabalho
                {
                    Id = _ordemActual?.Id ?? 0,
                    Numero = txtNumero.Text,
                    ClienteId = (int)cmbCliente.SelectedValue!,
                    Data = dtpData.Value,
                    DataInicio = dtpInicio.Value,
                    DataFim = dtpFim.Value,
                    LocalObra = txtLocalObra.Text.Trim(),
                    Descricao = txtDescricao.Text.Trim(),
                    Observacoes = txtObservacoes.Text.Trim(),
                    Iva = IVA_PERCENTAGEM,
                    Estado = cmbEstado.Text,
                    Itens = _itens
                };

                AppDataConnection.SalvarOrdem(ordem);
                MessageBox.Show($"Ordem {ordem.Numero} guardada!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                CarregarOrdens();
                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            if (_ordemActual == null)
            { MessageBox.Show("Salve a ordem primeiro!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            try
            {
                var emp = AppDataConnection.GetEmpresa();
                PdfGenerator.GerarOrdemTrabalho(_ordemActual, emp!);
            }
            catch (Exception ex) { MessageBox.Show("Erro ao gerar PDF: " + ex.Message); }
        }

        private void LimparFormulario()
        {
            _itens.Clear();
            _ordemActual = null;
            txtNumero.Text = AppDataConnection.GerarNumero("ordens_trabalho", "OT");
            txtLocalObra.Clear();
            txtDescricao.Clear();
            txtObservacoes.Clear();
            dtpData.Value = DateTime.Now;
            cmbEstado.SelectedIndex = 0;
            ActualizarGrelhaItens();
        }

        private void dgvHistorico_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            _ordemActual = dgvHistorico.Rows[e.RowIndex].DataBoundItem as OrdemTrabalho;
            if (_ordemActual != null)
            {
                txtNumero.Text = _ordemActual.Numero;
                txtLocalObra.Text = _ordemActual.LocalObra ?? "";
                txtDescricao.Text = _ordemActual.Descricao ?? "";
                txtObservacoes.Text = _ordemActual.Observacoes ?? "";
                cmbEstado.Text = _ordemActual.Estado;
                if (_ordemActual.DataInicio.HasValue) dtpInicio.Value = _ordemActual.DataInicio.Value;
                if (_ordemActual.DataFim.HasValue) dtpFim.Value = _ordemActual.DataFim.Value;
                _itens = _ordemActual.Itens;
                ActualizarGrelhaItens();
            }
        }

        private void cmbServico_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbServico.SelectedItem is Servico s)
                txtPrecoItem.Text = s.PrecoBase.ToString("N2");
        }
    }

    // ── DESIGNER ─────────────────────────────────────────────
    partial class FormOrdemTrabalho
    {
        private System.ComponentModel.IContainer? components = null;
        protected override void Dispose(bool d) { if (d) components?.Dispose(); base.Dispose(d); }

        private void InitializeComponent()
        {
            txtNumero = new Guna2TextBox();
            txtLocalObra = new Guna2TextBox();
            txtDescricao = new Guna2TextBox();
            txtObservacoes = new Guna2TextBox();
            txtPrecoItem = new Guna2TextBox();
            cmbCliente = new Guna2ComboBox();
            cmbServico = new Guna2ComboBox();
            cmbEstado = new Guna2ComboBox();
            dtpData = new Guna2DateTimePicker();
            dtpInicio = new Guna2DateTimePicker();
            dtpFim = new Guna2DateTimePicker();
            nudQuantidade = new NumericUpDown();
            nudDesconto = new NumericUpDown();
            dgvItens = new Guna2DataGridView();
            dgvHistorico = new Guna2DataGridView();
            btnSalvar = new Guna2Button();
            btnImprimir = new Guna2Button();
            btnAdicionarItem = new Guna2Button();
            btnRemoverItem = new Guna2Button();
            lblSubTotal = new Label();
            lblIva = new Label();
            lblTotal = new Label();

            SuspendLayout();

            Text = "Ordens de Trabalho";
            Size = new Size(1350, 860);
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1200, 700);

            var panelScroll = new Guna2Panel();
            panelScroll.Dock = DockStyle.Fill;
            panelScroll.AutoScroll = true;
            panelScroll.AutoScrollMinSize = new Size(0, 1050);
            panelScroll.BackColor = Color.White;
            panelScroll.Padding = new Padding(20, 20, 20, 50);

            // ── PAINEL DADOS ──────────────────────────────────
            var panelDados = CriarPainel(new Point(20, 20), new Size(1280, 195));
            panelDados.Controls.Add(Lbl("DADOS DA ORDEM DE TRABALHO", new Point(20, 15), 11, true));
            panelDados.Controls.Add(Sep(new Point(20, 45), 1240));

            int col1 = 20, col2 = 340, col3 = 660, col4 = 980;
            int lRow1 = 58, lRow2 = 118;

            // Linha 1 — Número, Data, Estado
            AdicionarGrupoCampo(panelDados, "Nº ORDEM", txtNumero, col1, lRow1, 200);
            AdicionarGrupoData(panelDados, "DATA", dtpData, col2, lRow1, 190);
            AdicionarGrupoCombo(panelDados, "ESTADO", cmbEstado, col3, lRow1, 190);
            cmbEstado.Items.AddRange(new object[] { "Aberta", "Em Curso", "Concluída", "Cancelada" });
            cmbEstado.SelectedIndex = 0;
            AdicionarGrupoData(panelDados, "DATA FIM", dtpFim, col4, lRow1, 270);

            // Linha 2 — Cliente, Início, Local
            AdicionarGrupoCombo(panelDados, "CLIENTE", cmbCliente, col1, lRow2, 280);
            AdicionarGrupoData(panelDados, "DATA INÍCIO", dtpInicio, col2, lRow2, 190);
            AdicionarGrupoCampo(panelDados, "LOCAL DA OBRA", txtLocalObra, col3, lRow2, 590);

            // ── PAINEL DESCRIÇÃO ──────────────────────────────
            var panelDesc = CriarPainel(new Point(20, 228), new Size(1280, 90));
            panelDesc.Controls.Add(Lbl("DESCRIÇÃO DOS TRABALHOS", new Point(20, 10), 9, true));
            txtDescricao.Location = new Point(20, 30);
            txtDescricao.Size = new Size(1238, 48);
            txtDescricao.Multiline = true;
            txtDescricao.BorderRadius = 6;
            txtDescricao.FillColor = Color.White;
            txtDescricao.BorderColor = Color.FromArgb(200, 200, 200);
            txtDescricao.Font = new Font("Segoe UI", 10);
            panelDesc.Controls.Add(txtDescricao);

            // ── PAINEL ITENS ──────────────────────────────────
            var panelItens = CriarPainel(new Point(20, 330), new Size(1280, 395));
            panelItens.Controls.Add(Lbl("ITENS DA ORDEM DE TRABALHO", new Point(20, 15), 11, true));
            panelItens.Controls.Add(Sep(new Point(20, 45), 1240));

            // Barra adicionar
            var barra = new Guna2Panel();
            barra.Location = new Point(15, 55);
            barra.Size = new Size(1248, 55);
            barra.BorderRadius = 8;
            barra.FillColor = Color.FromArgb(245, 247, 250);
            barra.BorderColor = Color.FromArgb(220, 224, 230);
            barra.BorderThickness = 1;

            AdicionarGrupoCombo(barra, "SERVIÇO", cmbServico, 10, 10, 280);
            AdicionarGrupoCampo(barra, "PREÇO UNIT.", txtPrecoItem, 305, 10, 110);
            AdicionarGrupoNumeric(barra, "QTD", nudQuantidade, 425, 10, 80);
            AdicionarGrupoNumeric(barra, "DESC. %", nudDesconto, 510, 10, 70);

            cmbServico.SelectedIndexChanged += cmbServico_SelectedIndexChanged;
            nudQuantidade.Minimum = 1; nudQuantidade.Value = 1; nudQuantidade.DecimalPlaces = 2;
            nudDesconto.Minimum = 0; nudDesconto.Maximum = 100; nudDesconto.DecimalPlaces = 1;

            btnAdicionarItem = CriarBotao("➕ Adicionar", 595, 18, 130, Color.FromArgb(52, 199, 89));
            btnRemoverItem = CriarBotao("🗑️ Remover", 740, 18, 120, Color.FromArgb(255, 59, 48));
            btnAdicionarItem.Click += BtnAdicionarItem_Click;
            btnRemoverItem.Click += BtnRemoverItem_Click;
            barra.Controls.AddRange(new Control[] { btnAdicionarItem, btnRemoverItem });
            panelItens.Controls.Add(barra);

            dgvItens.Location = new Point(15, 118);
            dgvItens.Size = new Size(1248, 260);
            ConfigurarGrid(dgvItens, false);
            panelItens.Controls.Add(dgvItens);

            // ── PAINEL RODAPÉ ─────────────────────────────────
            var panelRodape = CriarPainel(new Point(20, 738), new Size(1280, 120));

            panelRodape.Controls.Add(Lbl("OBSERVAÇÕES", new Point(20, 15), 8, false));
            txtObservacoes.Location = new Point(20, 33);
            txtObservacoes.Size = new Size(500, 72);
            txtObservacoes.Multiline = true;
            txtObservacoes.BorderRadius = 6;
            txtObservacoes.FillColor = Color.White;
            txtObservacoes.BorderColor = Color.FromArgb(200, 200, 200);
            txtObservacoes.Font = new Font("Segoe UI", 9);
            panelRodape.Controls.Add(txtObservacoes);

            // Totais
            var panelTotais = new Guna2Panel();
            panelTotais.Location = new Point(830, 10);
            panelTotais.Size = new Size(430, 100);
            panelTotais.BorderRadius = 10;
            panelTotais.FillColor = Color.FromArgb(248, 249, 252);
            panelTotais.BorderColor = Color.FromArgb(220, 224, 230);
            panelTotais.BorderThickness = 1;

            var lblSubTxt = Lbl("SUBTOTAL:", new Point(15, 12), 10, true);
            var lblIvaTxt = Lbl($"IVA ({IVA_PERCENTAGEM}%):", new Point(15, 40), 10, false);
            var lblTotTxt = Lbl("TOTAL:", new Point(15, 65), 13, true);
            lblTotTxt.ForeColor = Color.FromArgb(0, 102, 204);

            ConfigurarValorLabel(lblSubTotal, new Point(230, 12), 11, false);
            ConfigurarValorLabel(lblIva, new Point(230, 40), 10, false);
            ConfigurarValorLabel(lblTotal, new Point(220, 63), 13, true);
            lblTotal.ForeColor = Color.FromArgb(0, 102, 204);

            panelTotais.Controls.AddRange(new Control[]
                { lblSubTxt, lblSubTotal, lblIvaTxt, lblIva, lblTotTxt, lblTotal });
            panelRodape.Controls.Add(panelTotais);

            btnSalvar = CriarBotao("💾  Salvar", 540, 40, 130, Color.FromArgb(52, 199, 89));
            btnImprimir = CriarBotao("📄  Gerar PDF", 685, 40, 130, Color.FromArgb(255, 149, 0));
            btnSalvar.Click += BtnSalvar_Click;
            btnImprimir.Click += BtnImprimir_Click;
            panelRodape.Controls.AddRange(new Control[] { btnSalvar, btnImprimir });

            // ── HISTÓRICO ─────────────────────────────────────
            var panelHist = CriarPainel(new Point(20, 872), new Size(1280, 200));
            panelHist.Controls.Add(Lbl("HISTÓRICO DE ORDENS DE TRABALHO", new Point(20, 15), 11, true));
            panelHist.Controls.Add(Sep(new Point(20, 45), 1240));
            dgvHistorico.Location = new Point(15, 55);
            dgvHistorico.Size = new Size(1248, 130);
            ConfigurarGrid(dgvHistorico, false);
            dgvHistorico.CellClick += dgvHistorico_CellClick;
            panelHist.Controls.Add(dgvHistorico);

            panelScroll.Controls.AddRange(new Control[]
                { panelDados, panelDesc, panelItens, panelRodape, panelHist });
            Controls.Add(panelScroll);
            ResumeLayout(false);
        }

        // ── HELPERS ──────────────────────────────────────────

        private Guna2Panel CriarPainel(Point loc, Size size) => new Guna2Panel
        {
            Location = loc,
            Size = size,
            BorderRadius = 10,
            FillColor = Color.FromArgb(250, 250, 252),
            BorderColor = Color.FromArgb(220, 224, 230),
            BorderThickness = 1
        };

        private Label Lbl(string txt, Point loc, int size, bool bold) => new Label
        {
            Text = txt,
            Location = loc,
            AutoSize = true,
            Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = bold ? Color.FromArgb(30, 30, 45) : Color.FromArgb(100, 100, 120)
        };

        private Guna2Separator Sep(Point loc, int width) => new Guna2Separator
        {
            Location = loc,
            Size = new Size(width, 2),
            FillColor = Color.FromArgb(0, 122, 255)
        };

        private void AdicionarGrupoCampo(Control p, string label,
            Guna2TextBox txt, int x, int y, int w)
        {
            p.Controls.Add(new Label
            {
                Text = label,
                Location = new Point(x, y),
                Size = new Size(w, 16),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 120)
            });
            txt.Location = new Point(x, y + 18); txt.Size = new Size(w, 32);
            txt.BorderRadius = 6; txt.FillColor = Color.White;
            txt.BorderColor = Color.FromArgb(200, 200, 200);
            txt.Font = new Font("Segoe UI", 10);
            p.Controls.Add(txt);
        }

        private void AdicionarGrupoCombo(Control p, string label,
            Guna2ComboBox cmb, int x, int y, int w)
        {
            p.Controls.Add(new Label
            {
                Text = label,
                Location = new Point(x, y),
                Size = new Size(w, 16),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 120)
            });
            cmb.Location = new Point(x, y + 18); cmb.Size = new Size(w, 32);
            cmb.BorderRadius = 6; cmb.FillColor = Color.White;
            cmb.BorderColor = Color.FromArgb(200, 200, 200);
            cmb.Font = new Font("Segoe UI", 10);
            p.Controls.Add(cmb);
        }

        private void AdicionarGrupoData(Control p, string label,
            Guna2DateTimePicker dtp, int x, int y, int w)
        {
            p.Controls.Add(new Label
            {
                Text = label,
                Location = new Point(x, y),
                Size = new Size(w, 16),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 120)
            });
            dtp.Location = new Point(x, y + 18); dtp.Size = new Size(w, 32);
            dtp.BorderRadius = 6; dtp.FillColor = Color.White;
            dtp.BorderColor = Color.FromArgb(200, 200, 200);
            dtp.Format = DateTimePickerFormat.Short;
            p.Controls.Add(dtp);
        }

        private void AdicionarGrupoNumeric(Control p, string label,
            NumericUpDown nud, int x, int y, int w)
        {
            p.Controls.Add(new Label
            {
                Text = label,
                Location = new Point(x, y),
                Size = new Size(w, 16),
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 100, 120)
            });
            nud.Location = new Point(x, y + 18); nud.Size = new Size(w, 28);
            nud.Font = new Font("Segoe UI", 9); nud.TextAlign = HorizontalAlignment.Right;
            p.Controls.Add(nud);
        }

        private void ConfigurarValorLabel(Label lbl, Point loc, int size, bool bold)
        {
            lbl.Location = loc; lbl.Size = new Size(185, 28); lbl.Text = "0,00 MT";
            lbl.Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular);
            lbl.ForeColor = Color.FromArgb(30, 30, 45);
            lbl.TextAlign = ContentAlignment.MiddleRight;
        }

        private Guna2Button CriarBotao(string texto, int x, int y,
            int width, Color cor) => new Guna2Button
            {
                Text = texto,
                Location = new Point(x, y),
                Size = new Size(width, 40),
                BorderRadius = 8,
                FillColor = cor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10),
                HoverState = { FillColor = ControlPaint.Dark(cor, 0.1f) }
            };

        private void ConfigurarGrid(Guna2DataGridView dgv, bool editavel)
        {
            dgv.ReadOnly = !editavel; dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false; dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.BackgroundColor = Color.White; dgv.BorderStyle = BorderStyle.None;
            dgv.RowHeadersVisible = false; dgv.AllowUserToAddRows = false;
            dgv.Font = new Font("Segoe UI", 9); dgv.RowTemplate.Height = 38;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = Color.FromArgb(230, 230, 230);
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(25, 25, 40);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 38;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 230, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(20, 20, 40);
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 255);
        }

        // ── CAMPOS ───────────────────────────────────────────
        private Guna2TextBox txtNumero = null!;
        private Guna2TextBox txtLocalObra = null!;
        private Guna2TextBox txtDescricao = null!;
        private Guna2TextBox txtObservacoes = null!;
        private Guna2TextBox txtPrecoItem = null!;
        private Guna2ComboBox cmbCliente = null!;
        private Guna2ComboBox cmbServico = null!;
        private Guna2ComboBox cmbEstado = null!;
        private Guna2DateTimePicker dtpData = null!;
        private Guna2DateTimePicker dtpInicio = null!;
        private Guna2DateTimePicker dtpFim = null!;
        private NumericUpDown nudQuantidade = null!;
        private NumericUpDown nudDesconto = null!;
        private Guna2DataGridView dgvItens = null!;
        private Guna2DataGridView dgvHistorico = null!;
        private Guna2Button btnSalvar = null!;
        private Guna2Button btnImprimir = null!;
        private Guna2Button btnAdicionarItem = null!;
        private Guna2Button btnRemoverItem = null!;
        private Label lblSubTotal = null!;
        private Label lblIva = null!;
        private Label lblTotal = null!;
    }
}