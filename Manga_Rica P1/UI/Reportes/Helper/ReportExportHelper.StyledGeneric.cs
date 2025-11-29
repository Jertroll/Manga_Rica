// ReportExportHelper.StyledGeneric.cs
// Requiere ClosedXML (Install-Package ClosedXML)

using System;
using System.Data;
using System.Drawing;          // Image.FromFile
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;



namespace Manga_Rica_P1.UI.Reportes.Export
{
    // Partial de la clase base ReportExportHelper (donde ya tienes CsvEscape, SanitizeFileName, etc.)
    public static partial class ReportExportHelper
    {
        /// <summary>
        /// Exporta un reporte en XLSX/CSV con plantilla estilo corporativo similar a “Planilla”.
        /// No hay defaults: si falta algún parámetro, se muestra error y NO se exporta.
        /// - table: DataTable con las columnas EXACTAS del reporte.
        /// - suggestedFileName: nombre sugerido (sin extensión).
        /// - company, phones, address, title: textos de encabezado (pueden ser string.Empty si no quieres imprimir).
        /// - logoPath: ruta absoluta al logo, o null para omitir logo.
        /// - prependPhonesLabel: true → antepone "Teléfonos: " a phones.
        /// - groupByColumn: null/"" → sin agrupación; si no es null debe existir en table.
        /// - subtotalColumns, moneyColumns, decimalColumns: arreglos NO nulos (pueden ser vacíos).
        /// - logoHeightPx: alto del logo en píxeles (>= 1).
        /// </summary>
        public static async Task ExportStyledXlsxOrCsvAsync(
            IWin32Window owner,
            DataTable table,
            string suggestedFileName,
            string company,
            string phones,
            string address,
            string title,
            string? logoPath,
            bool prependPhonesLabel,
            string? groupByColumn,
            string[] subtotalColumns,
            string[] moneyColumns,
            string[] decimalColumns,
            int logoHeightPx
        )
        {
            // ===== Validaciones estrictas (sin defaults) =====
            if (owner is null) { ShowErr("Owner nulo."); return; }
            if (table is null) { ShowErr("DataTable es nulo."); return; }
            if (table.Columns.Count == 0) { ShowErr("El DataTable no tiene columnas."); return; }
            if (string.IsNullOrWhiteSpace(suggestedFileName)) { ShowErr("Falta 'suggestedFileName'."); return; }
            if (company is null) { ShowErr("'company' es null."); return; }
            if (phones is null) { ShowErr("'phones' es null."); return; }
            if (address is null) { ShowErr("'address' es null."); return; }
            if (title is null) { ShowErr("'title' es null."); return; }
            if (subtotalColumns is null) { ShowErr("'subtotalColumns' es null."); return; }
            if (moneyColumns is null) { ShowErr("'moneyColumns' es null."); return; }
            if (decimalColumns is null) { ShowErr("'decimalColumns' es null."); return; }
            if (logoHeightPx < 1) { ShowErr("'logoHeightPx' debe ser >= 1."); return; }
            if (!string.IsNullOrWhiteSpace(groupByColumn) && !table.Columns.Contains(groupByColumn))
            {
                ShowErr($"La columna de agrupación '{groupByColumn}' no existe en el DataTable.");
                return;
            }

            using var sfd = new SaveFileDialog
            {
                Title = "Guardar reporte",
                Filter = "Excel Workbook (*.xlsx)|*.xlsx|CSV (*.csv)|*.csv",
                AddExtension = true,
                DefaultExt = "xlsx",
                FileName = SanitizeFileName(suggestedFileName),
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                OverwritePrompt = true,
                RestoreDirectory = true
            };

            if (sfd.ShowDialog(owner) != DialogResult.OK) return;

            var ext = Path.GetExtension(sfd.FileName).ToLowerInvariant();
            if (ext == ".csv")
            {
                await WriteGroupedCsvAsync(
                    filePath: sfd.FileName,
                    table: table,
                    company: company,
                    phones: phones,
                    address: address,
                    title: title,
                    prependPhonesLabel: prependPhonesLabel,
                    groupByColumn: groupByColumn,
                    subtotalColumns: subtotalColumns
                );

                MessageBox.Show($"Archivo guardado en:\n{sfd.FileName}", "Exportar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // ===== XLSX con estilo (ClosedXML) =====
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("REPORTE");

            // Estilos base
            ws.Style.Font.FontName = "Segoe UI";
            ws.Style.Font.FontSize = 10;

            int r = 1;

            // ===== Logo (opcional, escalado por altura) =====
            if (!string.IsNullOrWhiteSpace(logoPath) && File.Exists(logoPath))
            {
                int targetHeightPx = logoHeightPx;
                int targetWidthPx;
                using (var img = Image.FromFile(logoPath))
                {
                    double ratio = (double)img.Width / img.Height;
                    targetWidthPx = (int)Math.Round(targetHeightPx * ratio);
                }

                ws.AddPicture(logoPath)
                  .MoveTo(ws.Cell(r, 1))
                  .WithSize(targetWidthPx, targetHeightPx);

                // Ajusta un alto de fila cómodo para el logo (aprox)
                ws.Row(r).Height = targetHeightPx * 0.75 + 6; // puntos
                if (ws.Column(1).Width < 18) ws.Column(1).Width = 18;
            }

            // ===== Encabezado corporativo centrado =====
            if (!string.IsNullOrWhiteSpace(company))
            {
                ws.Cell(r, 2).SetValue(company);
                ws.Range(r, 2, r, 10).Merge().Style
                  .Font.SetBold()
                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }

            r++;
            if (!string.IsNullOrWhiteSpace(phones))
            {
                var phonesLine = prependPhonesLabel ? $"Teléfonos: {phones}" : phones;
                ws.Cell(r, 2).SetValue(phonesLine);
                ws.Range(r, 2, r, 10).Merge().Style
                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }

            r++;
            if (!string.IsNullOrWhiteSpace(address))
            {
                ws.Cell(r, 2).SetValue($"Dirección: {address}");
                ws.Range(r, 2, r, 10).Merge().Style
                  .Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            }

            // ===== Línea amarilla =====
            r++;
            var line = ws.Range(r, 1, r, 10);
            line.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
            line.Style.Border.BottomBorderColor = XLColor.FromHtml("#eab308");

            // ===== Título =====
            r += 2;
            if (!string.IsNullOrWhiteSpace(title))
            {
                ws.Cell(r, 1).SetValue(title);
                ws.Cell(r, 1).Style.Font.Bold = true;
                ws.Cell(r, 1).Style.Font.FontSize = 14;
            }
            r += 2;

            // ===== Cabecera de columnas (dinámica) =====
            var headers = table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            int colCount = headers.Length;

            for (int c = 0; c < colCount; c++)
                ws.Cell(r, c + 1).SetValue(headers[c]);

            var headerRowIndex = r;
            var headerRange = ws.Range(r, 1, r, colCount);
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#f1f5f9");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            r++; // primera fila de datos

            var moneyFmt = "[$₡-es-CR] #,##0.00";
            var decFmt = "#,##0.00";
            var zebraBg = XLColor.FromHtml("#fafafa");

            // Totales
            decimal[] totals = new decimal[subtotalColumns.Length];

            // ===== Datos (agrupado o plano) =====
            if (!string.IsNullOrWhiteSpace(groupByColumn))
            {
                var grupos = table.Rows.Cast<DataRow>()
                               .GroupBy(row => Convert.ToString(row[groupByColumn]) ?? "");

                foreach (var grp in grupos)
                {
                    string groupName = grp.Key;
                    if (!string.IsNullOrWhiteSpace(groupName))
                    {
                        ws.Cell(r, 1).SetValue($"{groupByColumn}: {groupName}");
                        ws.Range(r, 1, r, colCount).Merge().Style
                          .Font.SetBold()
                          .Fill.SetBackgroundColor(XLColor.FromHtml("#fafafa"));
                        r++;
                    }

                    int startData = r;
                    decimal[] subTotals = new decimal[subtotalColumns.Length];

                    foreach (var row in grp)
                    {
                        for (int c = 0; c < colCount; c++)
                        {
                            var cell = ws.Cell(r, c + 1);
                            SetCellValue(cell, row[c]);
                        }

                        ApplyFormats(ws, r, table, moneyColumns, moneyFmt, decimalColumns, decFmt);

                        for (int i = 0; i < subtotalColumns.Length; i++)
                        {
                            var col = subtotalColumns[i];
                            if (table.Columns.Contains(col))
                                subTotals[i] += ToDecimal(row[col]);
                        }

                        r++;
                    }

                    // Zebra + bordes del bloque
                    var tbl = ws.Range(startData - 1, 1, r - 1, colCount);
                    tbl.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    tbl.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                    for (int zr = startData; zr < r; zr += 2)
                        ws.Range(zr, 1, zr, colCount).Style.Fill.BackgroundColor = zebraBg;

                    // Subtotal del grupo
                    if (!string.IsNullOrWhiteSpace(groupName) && subtotalColumns.Length > 0)
                    {
                        ws.Cell(r, 1).SetValue($"Subtotal {groupName}");
                        int firstSubIdx = FirstExistingColumnIndex(table, subtotalColumns);
                        int mergeEnd = firstSubIdx > 1 ? firstSubIdx - 1 : 1;
                        ws.Range(r, 1, r, mergeEnd).Merge();

                        for (int i = 0; i < subtotalColumns.Length; i++)
                        {
                            var colName = subtotalColumns[i];
                            if (!table.Columns.Contains(colName)) continue;
                            int colIndex = table.Columns[colName].Ordinal + 1;
                            ws.Cell(r, colIndex).SetValue(subTotals[i]);
                            ws.Cell(r, colIndex).Style.NumberFormat.Format = moneyFmt;
                        }

                        ws.Range(r, 1, r, colCount).Style
                          .Font.SetBold()
                          .Fill.SetBackgroundColor(XLColor.FromHtml("#f1f5f9"));
                        r += 2;
                    }

                    for (int i = 0; i < totals.Length; i++)
                        totals[i] += subTotals[i];
                }
            }
            else
            {
                // Sin agrupación
                int startData = r;
                foreach (DataRow row in table.Rows)
                {
                    for (int c = 0; c < colCount; c++)
                    {
                        var cell = ws.Cell(r, c + 1);
                        SetCellValue(cell, row[c]);
                    }

                    ApplyFormats(ws, r, table, moneyColumns, moneyFmt, decimalColumns, decFmt);

                    for (int i = 0; i < subtotalColumns.Length; i++)
                    {
                        var col = subtotalColumns[i];
                        if (table.Columns.Contains(col))
                            totals[i] += ToDecimal(row[col]);
                    }

                    r++;
                }

                var tbl = ws.Range(startData - 1, 1, r - 1, colCount);
                tbl.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                tbl.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                for (int zr = startData; zr < r; zr += 2)
                    ws.Range(zr, 1, zr, colCount).Style.Fill.BackgroundColor = zebraBg;

                r += 1;
            }

            // Borde general (cabecera + datos)
            var lastDataRow = r - 1;
            var fullTableRange = ws.Range(headerRowIndex, 1, lastDataRow, colCount);
            fullTableRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

            // ===== Total general (solo si hay columnas de subtotal) =====
            if (subtotalColumns.Length > 0)
            {
                ws.Cell(r, 1).SetValue("TOTAL GENERAL");
                int firstSubIdx = FirstExistingColumnIndex(table, subtotalColumns);
                int mergeEnd = firstSubIdx > 1 ? firstSubIdx - 1 : colCount;
                ws.Range(r, 1, r, mergeEnd).Merge();

                var moneyFmtTotal = "[$₡-es-CR] #,##0.00";
                for (int i = 0; i < subtotalColumns.Length; i++)
                {
                    var colName = subtotalColumns[i];
                    if (!table.Columns.Contains(colName)) continue;
                    int colIndex = table.Columns[colName].Ordinal + 1;
                    ws.Cell(r, colIndex).SetValue(totals[i]);
                    ws.Cell(r, colIndex).Style.NumberFormat.Format = moneyFmtTotal;
                }

                ws.Range(r, 1, r, colCount).Style
                  .Font.SetBold()
                  .Fill.SetBackgroundColor(XLColor.FromHtml("#e6f3ff"));
            }

            // Ajustes finales
            ws.Columns().AdjustToContents();
            ws.SheetView.FreezeRows(headerRowIndex);
            ws.PageSetup.PageOrientation = XLPageOrientation.Landscape;
            ws.PageSetup.PaperSize = XLPaperSize.LetterPaper;
            ws.PageSetup.FitToPages(1, 0);
            ws.PageSetup.Margins.Top = ws.PageSetup.Margins.Bottom =
            ws.PageSetup.Margins.Left = ws.PageSetup.Margins.Right = 0.25;

            wb.SaveAs(sfd.FileName);
            MessageBox.Show($"Archivo guardado en:\n{sfd.FileName}", "Exportar",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ===== CSV agrupado (sin defaults) =====
        private static async Task WriteGroupedCsvAsync(
            string filePath,
            DataTable table,
            string company,
            string phones,
            string address,
            string title,
            bool prependPhonesLabel,
            string? groupByColumn,
            string[] subtotalColumns
        )
        {
            using var sw = new StreamWriter(filePath);

            if (!string.IsNullOrWhiteSpace(company))
                await sw.WriteLineAsync($"EMPRESA;{CsvEscape(company)}");
            if (!string.IsNullOrWhiteSpace(phones))
            {
                var line = prependPhonesLabel ? $"TELEFONOS;{CsvEscape(phones)}" : $";{CsvEscape(phones)}";
                await sw.WriteLineAsync(line);
            }
            if (!string.IsNullOrWhiteSpace(address))
                await sw.WriteLineAsync($"DIRECCION;{CsvEscape(address)}");
            if (!string.IsNullOrWhiteSpace(title))
                await sw.WriteLineAsync($"TITULO;{CsvEscape(title)}");
            await sw.WriteLineAsync();

            var headers = table.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            await sw.WriteLineAsync(string.Join(";", headers.Select(CsvEscape)));

            decimal[] totals = new decimal[subtotalColumns.Length];

            if (!string.IsNullOrWhiteSpace(groupByColumn))
            {
                var grupos = table.Rows.Cast<DataRow>()
                                .GroupBy(r => Convert.ToString(r[groupByColumn]) ?? "");

                foreach (var grp in grupos)
                {
                    var groupName = grp.Key;
                    if (!string.IsNullOrWhiteSpace(groupName))
                        await sw.WriteLineAsync($"{groupByColumn};{CsvEscape(groupName)}");

                    decimal[] subTotals = new decimal[subtotalColumns.Length];

                    foreach (var row in grp)
                    {
                        var cells = headers.Select(h => Convert.ToString(row[h], CultureInfo.InvariantCulture) ?? "");
                        await sw.WriteLineAsync(string.Join(";", cells.Select(CsvEscape)));

                        for (int i = 0; i < subtotalColumns.Length; i++)
                        {
                            var col = subtotalColumns[i];
                            if (table.Columns.Contains(col))
                                subTotals[i] += ToDecimal(row[col]);
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(groupName) && subtotalColumns.Length > 0)
                    {
                        await sw.WriteLineAsync(BuildSubtotalCsvLine(headers.Length, subtotalColumns, table, subTotals));
                        await sw.WriteLineAsync();
                    }

                    for (int i = 0; i < totals.Length; i++)
                        totals[i] += subTotals[i];
                }
            }
            else
            {
                foreach (DataRow row in table.Rows)
                {
                    var cells = headers.Select(h => Convert.ToString(row[h], CultureInfo.InvariantCulture) ?? "");
                    await sw.WriteLineAsync(string.Join(";", cells.Select(CsvEscape)));

                    for (int i = 0; i < subtotalColumns.Length; i++)
                    {
                        var col = subtotalColumns[i];
                        if (table.Columns.Contains(col))
                            totals[i] += ToDecimal(row[col]);
                    }
                }
            }

            if (subtotalColumns.Length > 0)
                await sw.WriteLineAsync(BuildTotalCsvLine(headers.Length, subtotalColumns, table, totals));
        }

        // ===== Helpers internos =====
        private static void ShowErr(string msg)
            => MessageBox.Show(msg, "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Error);

        private static decimal ToDecimal(object? v)
        {
            if (v == null || v == DBNull.Value) return 0m;
            if (v is decimal d) return d;
            if (v is double dd) return (decimal)dd;
            if (v is float ff) return (decimal)ff;
            decimal.TryParse(Convert.ToString(v, CultureInfo.InvariantCulture), NumberStyles.Any,
                             CultureInfo.InvariantCulture, out var res);
            return res;
        }

        private static int FirstExistingColumnIndex(DataTable table, string[] candidates)
        {
            var idx = candidates
                .Where(c => table.Columns.Contains(c))
                .Select(c => table.Columns[c].Ordinal + 1)
                .DefaultIfEmpty(1)
                .Min();
            return Math.Max(1, idx);
        }

        private static string BuildSubtotalCsvLine(int headerCount, string[] subtotalColumns, DataTable table, decimal[] subTotals)
        {
            var cells = Enumerable.Repeat("", headerCount).ToArray();
            int anchor = Math.Max(3, headerCount - subtotalColumns.Length - 1);
            cells[anchor] = "Subtotal";
            for (int i = 0; i < subtotalColumns.Length; i++)
            {
                var colName = subtotalColumns[i];
                if (!table.Columns.Contains(colName)) continue;
                int idx = table.Columns[colName].Ordinal;
                cells[idx] = subTotals[i].ToString(CultureInfo.InvariantCulture);
            }
            return string.Join(";", cells);
        }

        private static string BuildTotalCsvLine(int headerCount, string[] subtotalColumns, DataTable table, decimal[] totals)
        {
            var cells = Enumerable.Repeat("", headerCount).ToArray();
            int anchor = Math.Max(3, headerCount - subtotalColumns.Length - 1);
            cells[anchor] = "TOTAL GENERAL";
            for (int i = 0; i < subtotalColumns.Length; i++)
            {
                var colName = subtotalColumns[i];
                if (!table.Columns.Contains(colName)) continue;
                int idx = table.Columns[colName].Ordinal;
                cells[idx] = totals[i].ToString(CultureInfo.InvariantCulture);
            }
            return string.Join(";", cells);
        }

        // Aplica formatos por nombre de columna (genérico)
        private static void ApplyFormats(
            IXLWorksheet ws, int rowIndex, DataTable table,
            string[] moneyColumns, string moneyFmt,
            string[] decimalColumns, string decimalFmt)
        {
            foreach (DataColumn col in table.Columns)
            {
                int c = col.Ordinal + 1;
                var cell = ws.Cell(rowIndex, c);

                if (moneyColumns.Contains(col.ColumnName))
                {
                    cell.Style.NumberFormat.Format = moneyFmt;
                }
                else if (decimalColumns.Contains(col.ColumnName))
                {
                    cell.Style.NumberFormat.Format = decimalFmt;
                }
            }
        }

        // Asigna valor tipado a la celda para evitar "object -> XLCellValue".
        private static void SetCellValue(IXLCell cell, object? value)
        {
            if (value == null || value == DBNull.Value)
            {
                cell.SetValue(string.Empty);
                return;
            }

            switch (value)
            {
                case string s: cell.SetValue(s); break;
                case DateTime dt: cell.SetValue(dt); break;
                case bool b: cell.SetValue(b); break;
                case byte by: cell.SetValue((int)by); break;
                case short sh: cell.SetValue((int)sh); break;
                case int i: cell.SetValue(i); break;
                case long l: cell.SetValue(l); break;
                case float f: cell.SetValue((double)f); break;
                case double d: cell.SetValue(d); break;
                case decimal m: cell.SetValue(m); break;
                case TimeSpan ts: cell.SetValue(ts); break;
                default:
                    // Si el DataColumn está tipado, intenta convertir por DataType
                    try
                    {
                        cell.SetValue(Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty);
                    }
                    catch
                    {
                        cell.SetValue(value.ToString() ?? string.Empty);
                    }
                    break;
            }
        }
    }
}
