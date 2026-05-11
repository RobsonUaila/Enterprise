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
        // FACTURA 
        // ════════════════════════════════════════════════════
        public static string GerarFactura(Factura factura, Empresa empresa)
        {
            string caminho = PrepCaminho($"Factura_{factura.Numero}.pdf");

            using (var doc = new PdfDocument(PageSize.A4, 35, 35, 35, 35))
            {
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                // ================= CORES =================
                BaseColor azul = new BaseColor(52, 152, 219);
                BaseColor preto = new BaseColor(25, 25, 40);
                BaseColor cinza = new BaseColor(240, 240, 240);
                BaseColor borda = new BaseColor(220, 220, 220);
                BaseColor brancocolor = BaseColor.WHITE;

                // ================= FONTES =================
                Font titulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 22, BaseColor.WHITE);
                Font subtitulo = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
                Font normal = FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);
                Font normalBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK);
                Font branco = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
                Font totalFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13, BaseColor.WHITE);

                // =====================================================
                // CABEÇALHO
                // =====================================================
                PdfPTable header = new PdfPTable(2);
                header.WidthPercentage = 100;
                header.SetWidths(new float[] { 65, 35 });

                // ========== ESQUERDA (DADOS DA EMPRESA) ==========
                PdfPCell empresaCell = new PdfPCell();
                empresaCell.BackgroundColor = preto;
                empresaCell.Border = Rectangle.NO_BORDER;
                empresaCell.Padding = 20;
                empresaCell.FixedHeight = 120;

                Paragraph nomeEmpresa = new Paragraph(empresa.Nome, titulo);
                nomeEmpresa.SpacingAfter = 10;
                empresaCell.AddElement(nomeEmpresa);

                empresaCell.AddElement(new Paragraph($"Telefone: {empresa.Telefone ?? "N/A"}", branco));
                empresaCell.AddElement(new Paragraph($"Email: {empresa.Email ?? "N/A"}", branco));

                header.AddCell(empresaCell);

                // ========== DIREITA (DADOS DA FACTURA) ==========
                PdfPCell facturaCell = new PdfPCell();
                facturaCell.BackgroundColor = azul;
                facturaCell.Border = Rectangle.NO_BORDER;
                facturaCell.Padding = 20;
                facturaCell.FixedHeight = 120;

                Paragraph tituloFactura = new Paragraph("FACTURA", titulo);
                tituloFactura.Alignment = Element.ALIGN_RIGHT;
                facturaCell.AddElement(tituloFactura);

                facturaCell.AddElement(new Paragraph($"Nº: {factura.Numero}", subtitulo));
                facturaCell.AddElement(new Paragraph($"Data: {factura.Data:dd/MM/yyyy}", subtitulo));
                facturaCell.AddElement(new Paragraph($"Data Vencimento: {factura.DataVencimento:dd/MM/yyyy}", subtitulo));

                header.AddCell(facturaCell);
                doc.Add(header);

                // Espaço após cabeçalho
                doc.Add(new Paragraph(" "));

                // =====================================================
                // DADOS DO CLIENTE
                // =====================================================
                if (factura.Cliente != null)
                {
                    PdfPTable clienteTable = new PdfPTable(1);
                    clienteTable.WidthPercentage = 100;

                    PdfPCell clienteCell = new PdfPCell();
                    clienteCell.Border = Rectangle.BOX;
                    clienteCell.BorderColor = borda;
                    clienteCell.Padding = 12;
                    clienteCell.BackgroundColor = cinza;

                    clienteCell.AddElement(new Paragraph($"CLIENTE: {factura.Cliente.Nome}", normalBold));

                    if (!string.IsNullOrEmpty(factura.Cliente.Tipo))
                        clienteCell.AddElement(new Paragraph($"Tipo: {factura.Cliente.Tipo}", normal));

                    clienteTable.AddCell(clienteCell);
                    doc.Add(clienteTable);
                    doc.Add(new Paragraph(" "));
                }

                // =====================================================
                // TABELA DE ITENS
                // =====================================================
                PdfPTable itensTable = new PdfPTable(5);
                itensTable.WidthPercentage = 100;
                itensTable.SetWidths(new float[] { 45, 10, 15, 15, 15 });
                itensTable.SpacingBefore = 10;

                // Cabeçalho da tabela
                AddHeaderCell(itensTable, "DESCRIÇÃO", preto, brancocolor);
                AddHeaderCell(itensTable, "QTD", preto, brancocolor);
                AddHeaderCell(itensTable, "PREÇO UNIT.", preto, brancocolor);
                AddHeaderCell(itensTable, "DESC. %", preto, brancocolor);
                AddHeaderCell(itensTable, "TOTAL", preto, brancocolor);

                // Linhas da tabela
                foreach (var item in factura.Itens)
                {
                    AddCell(itensTable, item.Descricao, Element.ALIGN_LEFT);
                    AddCell(itensTable, item.Quantidade.ToString("N2"), Element.ALIGN_RIGHT);
                    AddCell(itensTable, item.PrecoUnitario.ToString("N2"), Element.ALIGN_RIGHT);
                    AddCell(itensTable, item.Desconto.ToString("N1") + "%", Element.ALIGN_RIGHT);
                    AddCell(itensTable, item.Total.ToString("N2"), Element.ALIGN_RIGHT);
                }

                doc.Add(itensTable);

                // =====================================================
                // TOTAIS
                // =====================================================
                decimal subTotal = 0;
                foreach (var item in factura.Itens)
                    subTotal += item.Total;

                decimal valorIva = subTotal * (factura.Iva / 100);
                decimal total = subTotal + valorIva;

                PdfPTable totaisTable = new PdfPTable(2);
                totaisTable.WidthPercentage = 40;
                totaisTable.HorizontalAlignment = Element.ALIGN_RIGHT;
                totaisTable.SetWidths(new float[] { 40, 60 });
                totaisTable.SpacingBefore = 15;

                AddTotalCell(totaisTable, "SUBTOTAL:", $"{subTotal:N2} MT", false);
                AddTotalCell(totaisTable, $"IVA ({factura.Iva}%):", $"{valorIva:N2} MT", false);

                // Linha separadora
                PdfPCell lineCell = new PdfPCell(new Phrase(" "));
                lineCell.Colspan = 2;
                lineCell.Border = Rectangle.TOP_BORDER;
                lineCell.BorderWidthTop = 1f;
                lineCell.Padding = 5;
                totaisTable.AddCell(lineCell);

                AddTotalCell(totaisTable, "TOTAL:", $"{total:N2} MT", true);

                doc.Add(totaisTable);

                // =====================================================
                // OBSERVAÇÕES
                // =====================================================
                if (!string.IsNullOrEmpty(factura.Observacoes))
                {
                    doc.Add(new Paragraph(" "));
                    PdfPTable obsTable = new PdfPTable(1);
                    obsTable.WidthPercentage = 100;

                    PdfPCell obsCell = new PdfPCell();
                    obsCell.Border = Rectangle.BOX;
                    obsCell.BorderColor = borda;
                    obsCell.Padding = 10;
                    obsCell.BackgroundColor = cinza;

                    obsCell.AddElement(new Paragraph("OBSERVAÇÕES:", normalBold));
                    obsCell.AddElement(new Paragraph(factura.Observacoes, normal));

                    obsTable.AddCell(obsCell);
                    doc.Add(obsTable);
                }

                // =====================================================
                // RODAPÉ
                // =====================================================
                doc.Add(new Paragraph(" "));
                PdfPTable footerTable = new PdfPTable(1);
                footerTable.WidthPercentage = 100;

                PdfPCell footerCell = new PdfPCell();
                footerCell.Border = Rectangle.NO_BORDER;
                footerCell.Padding = 10;
                footerCell.HorizontalAlignment = Element.ALIGN_CENTER;

                footerCell.AddElement(new Paragraph(empresa.Nome,
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.GRAY)));
                footerCell.AddElement(new Paragraph("Documento gerado electronicamente",
                    FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 7, BaseColor.GRAY)));

                footerTable.AddCell(footerCell);
                doc.Add(footerTable);
            }

            Abrir(caminho);
            return caminho;
        }

        // ════════════════════════════════════════════════════
        // RECIBO
        // ════════════════════════════════════════════════════
        public static string GerarRecibo(Recibo recibo, Empresa empresa)
        {
            string caminho = PrepCaminho($"Recibo_{recibo.Numero}.pdf");

            using (var doc = new PdfDocument(PageSize.A4, 40, 40, 40, 40))
            {
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                Cabecalho(doc, empresa, "RECIBO", recibo.Numero, recibo.Data, null);
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

        // ════════════════════════════════════════════════════
        // ORDEM DE TRABALHO
        // ════════════════════════════════════════════════════
        public static string GerarOrdemTrabalho(OrdemTrabalho ordem, Empresa empresa)
        {
            string caminho = PrepCaminho($"OT_{ordem.Numero}.pdf");

            using (var doc = new PdfDocument(PageSize.A4, 40, 40, 40, 40))
            {
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                Cabecalho(doc, empresa, "ORDEM DE TRABALHO", ordem.Numero, ordem.Data, ordem.DataFim);
                DadosCliente(doc, ordem.Cliente!);

                if (!string.IsNullOrEmpty(ordem.Descricao))
                    Observacoes(doc, ordem.Descricao, "Descrição");

                TabelaItens(doc, ordem.Itens);

                decimal subTotal = 0;
                foreach (var item in ordem.Itens)
                    subTotal += item.Total;

                decimal valorIva = subTotal * (ordem.Iva / 100);
                decimal total = subTotal + valorIva;

                Totais(doc, subTotal, ordem.Iva, valorIva, total, empresa.MoedaSimbolo);
                Rodape(doc, empresa);
            }

            Abrir(caminho);
            return caminho;
        }

        // ════════════════════════════════════════════════════
        // COTAÇÃO
        // ════════════════════════════════════════════════════
        public static string GerarCotacao(Cotacao cotacao, Empresa empresa)
        {
            string caminho = PrepCaminho($"Cotacao_{cotacao.Numero}.pdf");

            using (var doc = new PdfDocument(PageSize.A4, 40, 40, 40, 40))
            {
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                // CABEÇALHO
                var headerTable = new PdfPTable(2) { WidthPercentage = 100 };
                headerTable.SetWidths(new float[] { 1.5f, 1f });

                var leftCell = new PdfPCell { Border = 0, Padding = 10 };

                if (!string.IsNullOrEmpty(empresa.LogoPath) && File.Exists(empresa.LogoPath))
                {
                    var img = PdfImage.GetInstance(empresa.LogoPath);
                    img.ScaleToFit(80, 50);
                    leftCell.AddElement(img);
                }

                leftCell.AddElement(new Phrase(empresa.Nome, FonteTitulo));
                leftCell.AddElement(new Phrase(empresa.Endereco ?? "", FontePequena));
                leftCell.AddElement(new Phrase($"Email: {empresa.Email ?? ""}", FontePequena));
                leftCell.AddElement(new Phrase($"Tel: {empresa.Telefone ?? ""}", FontePequena));

                headerTable.AddCell(leftCell);

                var rightCell = new PdfPCell
                {
                    Border = 0,
                    Padding = 10,
                    HorizontalAlignment = Element.ALIGN_RIGHT
                };

                rightCell.AddElement(new Paragraph("COTAÇÃO", new Font(FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.WHITE))));

                var infoTable = new PdfPTable(2);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 1f, 1.5f });

                AddInfoCell(infoTable, "DATA:", $"{cotacao.Data:dd-MMMM-yyyy}", true);
                AddInfoCell(infoTable, "COTAÇÃO nº:", cotacao.Numero, true);
                AddInfoCell(infoTable, "PARA:", cotacao.Cliente?.Nome ?? "", true);

                rightCell.AddElement(infoTable);
                headerTable.AddCell(rightCell);
                doc.Add(headerTable);

                // LINHA SEPARADORA
                var lineTable = new PdfPTable(1) { WidthPercentage = 100 };
                var lineCell = new PdfPCell(new Phrase(" "))
                {
                    Border = Rectangle.TOP_BORDER,
                    BorderWidthTop = 1f,
                    BorderColorTop = BaseColor.LIGHT_GRAY,
                    Padding = 5
                };
                lineTable.AddCell(lineCell);
                doc.Add(lineTable);

                // TABELA DE ITENS
                var tabela = new PdfPTable(4);
                tabela.WidthPercentage = 100;
                tabela.SetWidths(new float[] { 3f, 1f, 1.5f, 1.5f });
                tabela.SpacingBefore = 10f;
                tabela.SpacingAfter = 10f;

                AddHeaderCell(tabela, "DESCRIÇÃO");
                AddHeaderCell(tabela, "QTY");
                AddHeaderCell(tabela, "VALOR");
                AddHeaderCell(tabela, "TOTAL");

                foreach (var item in cotacao.Itens)
                {
                    AddBodyCell(tabela, item.Descricao);
                    AddBodyCell(tabela, item.Quantidade.ToString("N0"), Element.ALIGN_CENTER);
                    AddBodyCell(tabela, $"{item.PrecoUnitario:N2}", Element.ALIGN_RIGHT);
                    AddBodyCell(tabela, $"{item.Total:N2}", Element.ALIGN_RIGHT);
                }

                doc.Add(tabela);

                // TOTAIS
                var totaisTable = new PdfPTable(2);
                totaisTable.WidthPercentage = 40;
                totaisTable.HorizontalAlignment = Element.ALIGN_RIGHT;
                totaisTable.SetWidths(new float[] { 1f, 1f });

                decimal subTotal = 0;
                foreach (var item in cotacao.Itens)
                    subTotal += item.Total;

                decimal ivaValor = subTotal * (cotacao.Iva / 100);
                decimal totalFinal = subTotal + ivaValor;

                AddTotalCell(totaisTable, "SUBTOTAL:", $"{subTotal:N2}");
                AddTotalCell(totaisTable, "IVA:", $"{ivaValor:N2}");

                var separatorCell = new PdfPCell(new Phrase(" "))
                {
                    Colspan = 2,
                    Border = Rectangle.TOP_BORDER,
                    BorderWidthTop = 0.5f,
                    Padding = 3
                };
                totaisTable.AddCell(separatorCell);

                AddTotalCell(totaisTable, "TOTAL:", $"{totalFinal:N2}", true);

                doc.Add(totaisTable);

                // RODAPÉ
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph("A cotação tem validade de 15 dias.",
                    FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 9, BaseColor.GRAY)));
                doc.Add(new Paragraph("\n"));
                doc.Add(new Paragraph(empresa.Nome,
                    FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.GRAY)));
            }

            Abrir(caminho);
            return caminho;
        }

        // ════════════════════════════════════════════════════
        // MÉTODOS AUXILIARES
        // ════════════════════════════════════════════════════

        private static void Cabecalho(PdfDocument doc, Empresa empresa, string tipo, string numero, DateTime data, DateTime? dataExtra)
        {
            var tbl = new PdfPTable(2) { WidthPercentage = 100 };
            tbl.SetWidths(new float[] { 1.5f, 1f });

            var celEmp = new PdfPCell { Border = 0, BackgroundColor = CorPrimaria, Padding = 15 };

            if (!string.IsNullOrEmpty(empresa.LogoPath) && File.Exists(empresa.LogoPath))
            {
                var img = PdfImage.GetInstance(empresa.LogoPath);
                img.ScaleToFit(120, 60);
                celEmp.AddElement(img);
            }

            celEmp.AddElement(new Phrase("\n" + empresa.Nome, FonteTitulo));
            tbl.AddCell(celEmp);

            var celDoc = new PdfPCell { Border = 0, BackgroundColor = CorSecundaria, Padding = 15 };

            celDoc.AddElement(new Phrase(tipo, FonteTitulo));
            celDoc.AddElement(new Phrase($"\nNº: {numero}", FonteSubTitulo));
            celDoc.AddElement(new Phrase($"\nData: {data:dd/MM/yyyy}", FonteSubTitulo));

            if (dataExtra.HasValue)
                celDoc.AddElement(new Phrase($"Data Extra: {dataExtra.Value:dd/MM/yyyy}", FonteSubTitulo));

            tbl.AddCell(celDoc);
            doc.Add(tbl);
        }

        private static void DadosCliente(PdfDocument doc, Cliente cliente)
        {
            var tbl = new PdfPTable(1) { WidthPercentage = 100 };

            var cel = new PdfPCell { BackgroundColor = CorCinza, Padding = 10, Border = 0 };
            cel.AddElement(new Phrase(cliente.Nome, FonteNegrito));

            tbl.AddCell(cel);
            doc.Add(tbl);
        }

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

        private static void Totais(PdfDocument doc, decimal subTotal, decimal iva, decimal valorIva, decimal total, string moeda)
        {
            var tbl = new PdfPTable(2) { WidthPercentage = 50 };

            Cel(tbl, "TOTAL:", FonteTotal, CorPrimaria);
            Cel(tbl, $"{total:N2} {moeda}", FonteTotal, CorPrimaria);

            doc.Add(tbl);
        }

        private static void Rodape(PdfDocument doc, Empresa empresa)
        {
            doc.Add(new Paragraph("\n" + empresa.Nome, FontePequena));
        }

        private static void Cel(PdfPTable tbl, string texto, PdfFont fonte, BaseColor cor, int alinhamento = Element.ALIGN_LEFT)
        {
            tbl.AddCell(new PdfPCell(new Phrase(texto, fonte))
            {
                BackgroundColor = cor,
                HorizontalAlignment = alinhamento,
                Padding = 5,
                Border = 0
            });
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

        // ── Utils ───────────────────────────────────────────
        private static string PrepCaminho(string nome)
        {
            Directory.CreateDirectory(Pasta);
            return Path.Combine(Pasta, nome);
        }

        private static void Abrir(string caminho)
        {
            Process.Start(new ProcessStartInfo(caminho) { UseShellExecute = true });
        }

        // ════════════════════════════════════════════════════
        // MÉTODOS AUXILIARES PARA TABELAS (sem duplicação)
        // ════════════════════════════════════════════════════

        private static void AddHeaderCell(PdfPTable table, string texto)
        {
            var font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
            var cell = new PdfPCell(new Phrase(texto, font))
            {
                BackgroundColor = new BaseColor(30, 30, 45),
                Padding = 8,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            table.AddCell(cell);
        }

        private static void AddHeaderCell(PdfPTable table, string texto, BaseColor bgColor, BaseColor textColor)
        {
            Font font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, textColor);
            PdfPCell cell = new PdfPCell(new Phrase(texto, font));
            cell.BackgroundColor = bgColor;
            cell.Padding = 8;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            table.AddCell(cell);
        }

        private static void AddCell(PdfPTable table, string texto, int align)
        {
            Font font = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);
            PdfPCell cell = new PdfPCell(new Phrase(texto, font));
            cell.Padding = 6;
            cell.HorizontalAlignment = align;
            table.AddCell(cell);
        }

        private static void AddBodyCell(PdfPTable table, string texto, int align = Element.ALIGN_LEFT)
        {
            var font = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);
            var cell = new PdfPCell(new Phrase(texto, font))
            {
                Padding = 6,
                HorizontalAlignment = align
            };
            table.AddCell(cell);
        }

        private static void AddTotalCell(PdfPTable table, string label, string value, bool isBold = false)
        {
            Font labelFont = isBold ?
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, new BaseColor(0, 102, 204)) :
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.BLACK);

            Font valueFont = isBold ?
                FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 11, new BaseColor(0, 102, 204)) :
                FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.BLACK);

            var labelCell = new PdfPCell(new Phrase(label, labelFont))
            {
                Border = 0,
                Padding = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };

            var valueCell = new PdfPCell(new Phrase(value, valueFont))
            {
                Border = 0,
                Padding = 5,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }

        private static void AddInfoCell(PdfPTable table, string label, string value, bool isRightAlign = false)
        {
            var fontLabel = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE);
            var fontValue = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.WHITE);

            var labelCell = new PdfPCell(new Phrase(label, fontLabel))
            {
                Border = 0,
                Padding = 3,
                HorizontalAlignment = Element.ALIGN_LEFT
            };

            var valueCell = new PdfPCell(new Phrase(value, fontValue))
            {
                Border = 0,
                Padding = 3,
                HorizontalAlignment = isRightAlign ? Element.ALIGN_RIGHT : Element.ALIGN_LEFT
            };

            table.AddCell(labelCell);
            table.AddCell(valueCell);
        }
    }
}