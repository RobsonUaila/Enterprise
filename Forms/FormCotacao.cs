using System;
using System.Drawing;
using System.Windows.Forms;

namespace Enterprise.Forms
{
    public partial class FormCotacao : Form
    {
        private const decimal IVA_PERCENTAGEM = 16m;

        public FormCotacao()
        {
            InitializeComponent();
            this.Load += FormCotacao_Load;
        }

        private void FormCotacao_Load(object sender, EventArgs e)
        {
            InicializarValores();
            ConfigurarEventos();
            CarregarDadosIniciais();
        }

        private void CarregarDadosIniciais()
        {
            try
            {
                // Carregar dados dos ComboBoxes (se necessário)
                if (cmbEstado != null && cmbEstado.Items.Count == 0)
                {
                    cmbEstado.Items.AddRange(new object[] { "Pendente", "Aprovada", "Recusada" });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }

        private void InicializarValores()
        {
            if (txtNumero != null)
                txtNumero.Text = GerarNumeroCotacao();

            if (dtpData != null)
                dtpData.Value = DateTime.Now;

            if (dtpValidade != null)
                dtpValidade.Value = DateTime.Now.AddDays(15);

            // Definir índices de forma segura
            if (cmbCliente != null && cmbCliente.Items.Count > 0)
                cmbCliente.SelectedIndex = 0;

            if (cmbServico != null && cmbServico.Items.Count > 1)
                cmbServico.SelectedIndex = 1;
            else if (cmbServico != null && cmbServico.Items.Count > 0)
                cmbServico.SelectedIndex = 0;

            if (cmbEstado != null && cmbEstado.Items.Count > 0)
                cmbEstado.SelectedIndex = 0;

            if (txtIva != null)
                txtIva.Text = "16";

            if (nudDesconto != null)
                nudDesconto.Value = 0;

            CalcularTotais();
        }

        private void ConfigurarEventos()
        {
            if (dgvItens != null)
            {
                dgvItens.CellValueChanged += DgvItens_CellValueChanged;
                dgvItens.UserAddedRow += (s, e) => CalcularTotais();
                dgvItens.UserDeletedRow += (s, e) => CalcularTotais();
            }

            if (nudDesconto != null)
                nudDesconto.ValueChanged += (s, e) => CalcularTotais();

            if (btnAdicionarItem != null)
                btnAdicionarItem.Click += BtnAdicionarItem_Click;

            if (btnRemoverItem != null)
                btnRemoverItem.Click += BtnRemoverItem_Click;

            if (btnSalvar != null)
                btnSalvar.Click += BtnSalvar_Click;

            if (btnImprimir != null)
                btnImprimir.Click += BtnImprimir_Click;
        }

        private string GerarNumeroCotacao()
        {
            Random rand = new Random();
            return $"COT-{DateTime.Now.Year}-{rand.Next(1000, 9999)}";
        }

        private void CalcularTotais()
        {
            // Verificação de segurança para evitar NullReferenceException
            if (dgvItens == null || lblSubTotal == null || lblIva == null || lblTotal == null)
                return;

            decimal subtotal = 0;

            try
            {
                foreach (DataGridViewRow row in dgvItens.Rows)
                {
                    if (row == null || row.IsNewRow) continue;

                    decimal qtd = 0, valorUnit = 0;

                    // Verificar se as células existem
                    if (row.Cells.Count > 1 && row.Cells["Quantidade"]?.Value != null)
                        decimal.TryParse(row.Cells["Quantidade"].Value.ToString(), out qtd);

                    if (row.Cells.Count > 2 && row.Cells["ValorUnitario"]?.Value != null)
                        decimal.TryParse(row.Cells["ValorUnitario"].Value.ToString(), out valorUnit);

                    decimal totalItem = qtd * valorUnit;

                    if (row.Cells.Count > 3 && row.Cells["Total"] != null)
                        row.Cells["Total"].Value = totalItem;

                    subtotal += totalItem;
                }
            }
            catch (Exception ex)
            {
                // Log do erro (opcional)
                Console.WriteLine($"Erro ao calcular totais: {ex.Message}");
                return;
            }

            decimal descontoGeral = nudDesconto?.Value ?? 0;
            subtotal = subtotal * (1 - (descontoGeral / 100));

            decimal ivaPercent = 16;
            if (txtIva != null && !string.IsNullOrEmpty(txtIva.Text))
                decimal.TryParse(txtIva.Text, out ivaPercent);

            decimal iva = subtotal * (ivaPercent / 100);
            decimal total = subtotal + iva;

            lblSubTotal.Text = $"SUBTOTAL: {subtotal:N2} MT";
            lblIva.Text = $"IVA ({ivaPercent}%): {iva:N2} MT";
            lblTotal.Text = $"TOTAL: {total:N2} MT";
        }

        private void BtnAdicionarItem_Click(object sender, EventArgs e)
        {
            // Verificação de segurança para todos os controles
            if (cmbServico == null)
            {
                MessageBox.Show("Erro: Controle de serviço não inicializado.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgvItens == null)
            {
                MessageBox.Show("Erro: Grid de itens não inicializado.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (nudQuantidade == null)
            {
                MessageBox.Show("Erro: Controle de quantidade não inicializado.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validar se há item selecionado
            string servico = cmbServico.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(servico) || servico == "Selecione...")
            {
                MessageBox.Show("Selecione um serviço!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbServico.Focus();
                return;
            }

            try
            {
                // Adicionar nova linha ao grid
                int rowIndex = dgvItens.Rows.Add(servico, nudQuantidade.Value, 0, 0);

                // Verificar se há linhas e definir célula atual
                if (dgvItens.Rows.Count > 0 && rowIndex >= 0)
                {
                    int targetRow = dgvItens.Rows.Count - 2;
                    if (targetRow >= 0 && dgvItens.Rows[targetRow].Cells.Count > 2)
                    {
                        dgvItens.CurrentCell = dgvItens.Rows[targetRow].Cells["ValorUnitario"];
                        dgvItens.BeginEdit(true);
                    }
                }

                CalcularTotais();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao adicionar item: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRemoverItem_Click(object sender, EventArgs e)
        {
            if (dgvItens == null)
            {
                MessageBox.Show("Erro: Grid de itens não inicializado.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dgvItens.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecione um item para remover!", "Atenção",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Remover o item selecionado?", "Confirmar",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    dgvItens.Rows.Remove(dgvItens.SelectedRows[0]);
                    CalcularTotais();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao remover item: {ex.Message}", "Erro",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvItens_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Evitar erro quando o índice for inválido
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvItens == null) return;
            if (e.RowIndex >= dgvItens.Rows.Count) return;

            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
                CalcularTotais();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if (dgvItens == null)
            {
                MessageBox.Show("Erro: Grid de itens não inicializado.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verificar se há itens na cotação
            bool temItens = false;
            foreach (DataGridViewRow row in dgvItens.Rows)
            {
                if (!row.IsNewRow)
                {
                    temItens = true;
                    break;
                }
            }

            if (!temItens)
            {
                MessageBox.Show("Adicione pelo menos um item à cotação!", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvCotacoes == null)
            {
                MessageBox.Show("Erro ao salvar: Grid de histórico não inicializado.", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string validade = dtpValidade?.Value.ToString("dd/MM/yyyy") ?? "";
                string cliente = cmbCliente?.SelectedItem?.ToString() ?? "";
                string data = dtpData?.Value.ToString("dd/MM/yyyy") ?? "";
                string total = lblTotal?.Text?.Replace("TOTAL: ", "") ?? "";
                string estado = cmbEstado?.SelectedItem?.ToString() ?? "";

                dgvCotacoes.Rows.Insert(0,
                    txtNumero?.Text ?? "",
                    cliente,
                    data,
                    validade,
                    total,
                    estado
                );

                MessageBox.Show($"Cotação {txtNumero?.Text ?? ""} salva com sucesso!",
                    "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimparFormulario();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimparFormulario()
        {
            if (dgvItens != null)
                dgvItens.Rows.Clear();

            if (txtNumero != null)
                txtNumero.Text = GerarNumeroCotacao();

            if (dtpData != null)
                dtpData.Value = DateTime.Now;

     
            if (cmbCliente != null && cmbCliente.Items.Count > 0)
                cmbCliente.SelectedIndex = 0;

           

            if (cmbServico != null && cmbServico.Items.Count > 1)
                cmbServico.SelectedIndex = 1;
            else if (cmbServico != null && cmbServico.Items.Count > 0)
                cmbServico.SelectedIndex = 0;

            if (cmbEstado != null && cmbEstado.Items.Count > 0)
                cmbEstado.SelectedIndex = 0;

            if (nudDesconto != null)
                nudDesconto.Value = 0;

            if (txtObservacoes != null)
                txtObservacoes.Clear();

            CalcularTotais();
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("📄 Função de gerar PDF será implementada em breve.\n\n" +
                "✓ Cotação pronta para impressão\n" +
                $"Nº: {txtNumero?.Text ?? ""}\n" +
                $"Cliente: {cmbCliente?.Text ?? ""}\n" +
                $"Total: {lblTotal?.Text ?? ""}",
                "Gerar PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }
    }
}