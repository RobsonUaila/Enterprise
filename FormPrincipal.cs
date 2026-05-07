using Enterprise.Forms;
using System;
using System.Windows.Forms;

namespace Enterprise
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            var form = new FormCliente();
            form.ShowDialog();
        }

        private void btnServicos_Click(object sender, EventArgs e)
        {
            FormServicos form = new FormServicos();
            form.ShowDialog();
        }

        private void btnCotacoes_Click(object sender, EventArgs e)
        {
            var form = new FormCotacao();
            form.ShowDialog();
        }

        private void btnFacturas_Click(object sender, EventArgs e)
        {
            var form = new FormFactura();
            form.ShowDialog();
        }

        private void btnRecibos_Click(object sender, EventArgs e)
        {
            var form = new FormRecibo();
            form.ShowDialog();
        }

        private void btnOrdens_Click(object sender, EventArgs e)
        {
            var form = new FormOrdemTrabalho();
            form.ShowDialog();
        }

        private void btnEmpresa_Click(object sender, EventArgs e)
        {
           var form = new FormEmpresa();
            form.ShowDialog();
        }
    }
}