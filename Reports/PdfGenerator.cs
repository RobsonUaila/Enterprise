// ============================================================
// PdfGenerator.cs — Geração de PDFs com iTextSharp
// ============================================================

using Enterprise.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

// Alias
using PdfDocument = iTextSharp.text.Document;
using PdfFont = iTextSharp.text.Font;
using PdfImage = iTextSharp.text.Image;

namespace Enterprise.Reports
{
    public static class PdfGenerator
    {
        // ── Cores ────────────────────────────────────────────
        private static readonly BaseColor CorPrimaria = new BaseColor(30, 30, 45);
        private static readonly BaseColor CorSecundaria = new BaseColor(52, 152, 219);
        private static readonly BaseColor CorCinza = new BaseColor(245, 245, 245);

        // ── FONTES (CORRIGIDO: agora tudo usa PdfFont) ───────
        private static readonly PdfFont FonteTitulo =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.WHITE);

        private static readonly PdfFont FonteSubTitulo =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, BaseColor.WHITE);

        private static readonly PdfFont FonteNormal =
            FontFactory.GetFont(FontFactory.HELVETICA, 9, new BaseColor(50, 50, 50));

        private static readonly PdfFont FonteNegrito =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, new BaseColor(50, 50, 50));

        private static readonly PdfFont FontePequena =
            FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.GRAY);

        private static readonly PdfFont FonteCabecalho =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.WHITE);

        private static readonly PdfFont FonteTotal =
            FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, BaseColor.WHITE);

        // ── Pasta destino ────────────────────────────────────
        private static string Pasta =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "Enterprise", "PDFs");

        // ════════════════════════════════════════════════════
        // FACTURA (exemplo principal — restante mantém igual)
        // ════════════════════════════════════════════════════
        public static string GerarFactura(Factura factura, Empresa empresa)
        {
            string caminho = PrepCaminho($"Factura_{factura.Numero}.pdf");

            using (var doc = new PdfDocument(PageSize.A4, 40, 40, 40, 40))
            {
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                Cabecalho(doc, empresa, "FACTURA", factura.Numero,
                    factura.Data, factura.DataVencimento, factura.Estado);

                DadosCliente(doc, factura.Cliente!);
                TabelaItens(doc, factura.Itens);

                Totais(doc, factura.SubTotal, factura.Iva,
                    factura.ValorIva, factura.Total, empresa.MoedaSimbolo);

                Rodape(doc, empresa);
            }

            Abrir(caminho);
            return caminho;
        }

        // ════════════════════════════════════════════════════
        // CABEÇALHO
        // ════════════════════════════════════════════════════
        private static void Cabecalho(PdfDocument doc, Empresa empresa,
            string tipo, string numero, DateTime data,
            DateTime? dataExtra, string estado)
        {
            var tbl = new PdfPTable(2) { WidthPercentage = 100 };
            tbl.SetWidths(new float[] { 1.5f, 1f });

            var celEmp = new PdfPCell
            { Border = 0, BackgroundColor = CorPrimaria, Padding = 15 };

            if (!string.IsNullOrEmpty(empresa.LogoPath)
                && File.Exists(empresa.LogoPath))
            {
                var img = PdfImage.GetInstance(empresa.LogoPath);
                img.ScaleToFit(120, 60);
                celEmp.AddElement(img);
            }

            celEmp.AddElement(new Phrase("\n" + empresa.Nome, FonteTitulo));
            tbl.AddCell(celEmp);

            var celDoc = new PdfPCell
            {
                Border = 0,
                BackgroundColor = CorSecundaria,
                Padding = 15
            };

            celDoc.AddElement(new Phrase(tipo, FonteTitulo));
            celDoc.AddElement(new Phrase($"\nNº: {numero}", FonteSubTitulo));
            celDoc.AddElement(new Phrase($"\nData: {data:dd/MM/yyyy}", FonteSubTitulo));

            tbl.AddCell(celDoc);

            doc.Add(tbl);
        }

        // ════════════════════════════════════════════════════
        // CLIENTE
        // ════════════════════════════════════════════════════
        private static void DadosCliente(PdfDocument doc, Cliente cliente)
        {
            var tbl = new PdfPTable(1) { WidthPercentage = 100 };

            var cel = new PdfPCell
            { BackgroundColor = CorCinza, Padding = 10, Border = 0 };

            cel.AddElement(new Phrase(cliente.Nome, FonteNegrito));
            cel.AddElement(new Phrase(cliente.Email ?? "", FonteNormal));

            tbl.AddCell(cel);
            doc.Add(tbl);
        }

        // ════════════════════════════════════════════════════
        // TABELA ITENS
        // ════════════════════════════════════════════════════
        private static void TabelaItens(PdfDocument doc, List<ItemDocumento> itens)
        {
            var tbl = new PdfPTable(2) { WidthPercentage = 100 };

            foreach (var item in itens)
            {
                Cel(tbl, item.Descricao, FonteNormal, BaseColor.WHITE);
                Cel(tbl, item.Total.ToString("N2"), FonteNegrito, BaseColor.WHITE);
            }

            doc.Add(tbl);
        }

        // ════════════════════════════════════════════════════
        // TOTAIS
        // ════════════════════════════════════════════════════
        private static void Totais(PdfDocument doc, decimal subTotal,
            decimal iva, decimal valorIva, decimal total, string moeda)
        {
            var tbl = new PdfPTable(2) { WidthPercentage = 50 };

            Cel(tbl, "TOTAL:", FonteTotal, CorPrimaria);
            Cel(tbl, $"{total:N2} {moeda}", FonteTotal, CorPrimaria);

            doc.Add(tbl);
        }

        // ════════════════════════════════════════════════════
        // RODAPÉ
        // ════════════════════════════════════════════════════
        private static void Rodape(PdfDocument doc, Empresa empresa)
        {
            doc.Add(new Paragraph("\n" + empresa.Nome, FontePequena));
        }

        // ════════════════════════════════════════════════════
        // HELPER CÉLULA (AGORA COMPATÍVEL)
        // ════════════════════════════════════════════════════
        private static void Cel(PdfPTable tbl, string texto,
            PdfFont fonte, BaseColor cor,
            int alinhamento = Element.ALIGN_LEFT)
        {
            tbl.AddCell(new PdfPCell(new Phrase(texto, fonte))
            {
                BackgroundColor = cor,
                HorizontalAlignment = alinhamento,
                Padding = 5,
                Border = 0
            });
        }

        // ── Utils ───────────────────────────────────────────
        private static string PrepCaminho(string nome)
        {
            Directory.CreateDirectory(Pasta);
            return Path.Combine(Pasta, nome);
        }

        private static void Abrir(string caminho)
        {
            Process.Start(new ProcessStartInfo(caminho)
            {
                UseShellExecute = true
            });
        }

        public static string GerarRecibo(Recibo recibo, Empresa empresa)
        {
            string caminho = PrepCaminho($"Recibo_{recibo.Numero}.pdf");

            using (var doc = new PdfDocument(PageSize.A4, 40, 40, 40, 40))
            {
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                Cabecalho(doc, empresa, "RECIBO", recibo.Numero,
                    recibo.Data, null, "Emitido");

                DadosCliente(doc, recibo.Cliente!);

                var tbl = new PdfPTable(2) { WidthPercentage = 100, SpacingBefore = 20 };

                Cel(tbl, "Forma de Pagamento:", FonteNegrito, CorCinza);
                Cel(tbl, recibo.FormaPagamento, FonteNormal, CorCinza);

                Cel(tbl, "Valor Pago:", FonteNegrito, CorCinza);
                Cel(tbl, $"{recibo.ValorPago:N2} {empresa.MoedaSimbolo}", FonteNormal, CorCinza);

                doc.Add(tbl);

                Rodape(doc, empresa);
            }

            Abrir(caminho);
            return caminho;
        }

        //OT

        public static string GerarOrdemTrabalho(OrdemTrabalho ordem, Empresa empresa)
        {
            string caminho = PrepCaminho($"OT_{ordem.Numero}.pdf");

            using (var doc = new PdfDocument(PageSize.A4, 40, 40, 40, 40))
            {
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                Cabecalho(doc, empresa, "ORDEM DE TRABALHO",
                    ordem.Numero, ordem.Data, ordem.DataFim, ordem.Estado);

                DadosCliente(doc, ordem.Cliente!);

                if (!string.IsNullOrEmpty(ordem.Descricao))
                    Observacoes(doc, ordem.Descricao, "Descrição");

                TabelaItens(doc, ordem.Itens);

                Totais(doc, ordem.SubTotal, ordem.Iva,
                    ordem.ValorIva, ordem.Total, empresa.MoedaSimbolo);

                Rodape(doc, empresa);
            }

            Abrir(caminho);
            return caminho;
        }
        private static void Observacoes(PdfDocument doc, string texto, string titulo = "Observações")
        {
            doc.Add(new Paragraph(" "));

            var tbl = new PdfPTable(1) { WidthPercentage = 100 };

            var cel = new PdfPCell
            {
                BackgroundColor = CorCinza,
                Padding = 10,
                Border = 0
            };

            cel.AddElement(new Phrase(titulo, FonteNegrito));
            cel.AddElement(new Phrase("\n" + texto, FonteNormal));

            tbl.AddCell(cel);
            doc.Add(tbl);
        }

        public static string GerarCotacao(Cotacao cotacao, Empresa empresa)
        {
            string caminho = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Cotacao_{cotacao.Numero}.pdf");

            var doc = new iTextSharp.text.Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));

            doc.Open();

            var fonteNormal = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            var fonteTitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);

            // EMPRESA
            doc.Add(new Paragraph(empresa.Nome, fonteTitulo));
            doc.Add(new Paragraph(empresa.Endereco, fonteNormal));
            doc.Add(new Paragraph($"Tel: {empresa.Telefone}", fonteNormal));
            doc.Add(new Paragraph($"Email: {empresa.Email}", fonteNormal));

            doc.Add(new Paragraph("\nCOTAÇÃO", fonteTitulo));
            doc.Add(new Paragraph($"Número: {cotacao.Numero}", fonteNormal));
            doc.Add(new Paragraph($"Data: {cotacao.Data:dd/MM/yyyy}", fonteNormal));

            doc.Add(new Paragraph($"Para: {cotacao.Cliente?.Nome}", fonteNormal));

            doc.Add(new Paragraph("\n"));

            // TABELA
            var tabela = new PdfPTable(4);
            tabela.WidthPercentage = 100;

            tabela.AddCell("Descrição");
            tabela.AddCell("Qtd");
            tabela.AddCell("Preço");
            tabela.AddCell("Total");

            foreach (var item in cotacao.Itens)
            {
                tabela.AddCell(item.Descricao);
                tabela.AddCell(item.Quantidade.ToString());
                tabela.AddCell(item.PrecoUnitario.ToString("N2"));
                tabela.AddCell(item.Total.ToString("N2"));
            }

            doc.Add(tabela);

            doc.Add(new Paragraph("\n"));

            // TOTAIS
            doc.Add(new Paragraph($"Subtotal: {cotacao.SubTotal:N2}", fonteNormal));
            doc.Add(new Paragraph($"IVA ({cotacao.Iva}%): {cotacao.ValorIva:N2}", fonteNormal));
            doc.Add(new Paragraph($"TOTAL: {cotacao.Total:N2}", fonteTitulo));

            doc.Add(new Paragraph("\nValidade: 15 dias", fonteNormal));

            doc.Close();

            System.Diagnostics.Process.Start(new ProcessStartInfo(caminho)
            {
                UseShellExecute = true
            });

            return caminho;
        }
    }
}