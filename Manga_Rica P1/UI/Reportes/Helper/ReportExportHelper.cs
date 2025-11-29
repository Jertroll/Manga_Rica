using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace Manga_Rica_P1.UI.Reportes.Export
{
    public static partial class ReportExportHelper
    {
        // ====================== PDF (WebView2 -> PDF con diálogo) ======================
        public static async Task ExportWebView2ToPdfAsync(
            IWin32Window owner,
            WebView2 web,
            string suggestedFileNameWithoutExt = "Reporte",
            Action<CoreWebView2PrintSettings>? customizeSettings = null)
        {
            if (web.CoreWebView2 == null)
                await web.EnsureCoreWebView2Async();

            await WaitForNavigationAsync(web);

            using var sfd = new SaveFileDialog
            {
                Title = "Guardar reporte en PDF",
                Filter = "PDF (*.pdf)|*.pdf",
                AddExtension = true,
                DefaultExt = "pdf",
                OverwritePrompt = true,
                RestoreDirectory = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = SanitizeFileName($"{suggestedFileNameWithoutExt}.pdf")
            };
            if (sfd.ShowDialog(owner) != DialogResult.OK) return;

            var settings = web.CoreWebView2.Environment.CreatePrintSettings();
            settings.ShouldPrintHeaderAndFooter = false;
            settings.ShouldPrintBackgrounds = true;
            settings.Orientation = CoreWebView2PrintOrientation.Portrait;
            settings.MarginTop = settings.MarginBottom = settings.MarginLeft = settings.MarginRight = 0.5;

            customizeSettings?.Invoke(settings);

            bool ok = await web.CoreWebView2.PrintToPdfAsync(sfd.FileName, settings);
            MessageBox.Show(
                ok ? $"PDF guardado en:\n{sfd.FileName}" : "No se pudo generar el PDF.",
                "Exportar a PDF",
                MessageBoxButtons.OK,
                ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning
            );
        }

        private static Task WaitForNavigationAsync(WebView2 web)
        {
            var core = web.CoreWebView2;
            if (core == null) return Task.CompletedTask;

            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            void Handler(object? s, CoreWebView2NavigationCompletedEventArgs e)
            {
                core.NavigationCompleted -= Handler;
                tcs.TrySetResult(true);
            }

            core.NavigationCompleted += Handler;

            // Pequeño colchón por si no hay navegación activa:
            _ = Task.Delay(50).ContinueWith(_ =>
            {
                core.NavigationCompleted -= Handler;
                tcs.TrySetResult(true);
            }, TaskScheduler.Default);

            return tcs.Task;
        }

        // ====================== EXCEL (DataTable -> XLSX/CSV) ======================
        // Si no usas ClosedXML, exporta CSV como fallback.
        public static async Task ExportTableToExcelAsync(
            IWin32Window owner,
            DataTable table,
            string suggestedFileNameWithoutExt = "Reporte",
            bool forceCsvFallback = false)
        {
            using var sfd = new SaveFileDialog
            {
                Title = "Guardar reporte en Excel",
                Filter = forceCsvFallback
                    ? "CSV (*.csv)|*.csv"
                    : "Excel Workbook (*.xlsx)|*.xlsx|CSV (*.csv)|*.csv",
                AddExtension = true,
                OverwritePrompt = true,
                RestoreDirectory = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                FileName = SanitizeFileName($"{suggestedFileNameWithoutExt}")
            };
            if (sfd.ShowDialog(owner) != DialogResult.OK) return;

            var ext = Path.GetExtension(sfd.FileName).ToLowerInvariant();

            try
            {
                string savedPath = sfd.FileName;

                if (ext == ".csv" || forceCsvFallback)
                {
                    await WriteCsvAsync(sfd.FileName, table);
                }
                else
                {
#if USE_CLOSEDXML
                    using var wb = new ClosedXML.Excel.XLWorkbook();
                    var ws = wb.Worksheets.Add(table, "Reporte");
                    ws.Columns().AdjustToContents();
                    wb.SaveAs(sfd.FileName);
#else
                    var alt = Path.ChangeExtension(sfd.FileName, ".csv");
                    await WriteCsvAsync(alt, table);
                    savedPath = alt;
#endif
                }

                MessageBox.Show($"Archivo guardado en:\n{savedPath}", "Exportar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo exportar: {ex.Message}", "Exportar",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static async Task WriteCsvAsync(string filePath, DataTable table)
        {
            using var sw = new StreamWriter(filePath);
            // Encabezados
            await sw.WriteLineAsync(string.Join(",", table.Columns.Cast<DataColumn>()
                .Select(c => CsvEscape(c.ColumnName))));
            // Filas
            foreach (DataRow row in table.Rows)
            {
                var cells = row.ItemArray.Select(v => CsvEscape(v?.ToString() ?? ""));
                await sw.WriteLineAsync(string.Join(",", cells));
            }
        }

        private static string CsvEscape(string s)
        {
            if (s.Contains('"') || s.Contains(',') || s.Contains('\n') || s.Contains('\r'))
            {
                s = s.Replace("\"", "\"\"");
                return $"\"{s}\"";
            }
            return s;
        }

        // ====================== Builder: aplanar VM de empleados ======================
        public static DataTable BuildEmpleadosDataTable(object vm)
        {
            var dt = new DataTable("Empleados");
            dt.Columns.Add("Departamento");
            dt.Columns.Add("Carne");
            dt.Columns.Add("Apellido 1");
            dt.Columns.Add("Apellido 2");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Salario", typeof(decimal));
            dt.Columns.Add("Puesto");
            dt.Columns.Add("Fecha ingreso", typeof(DateTime));

            // 1) VM agrupado (Departamentos|Grupos|PorDepartamento|Secciones)
            var grupos = GetEnumerableProp(vm, "Departamentos", "Grupos", "PorDepartamento", "Secciones");
            if (grupos != null)
            {
                foreach (var g in grupos)
                {
                    var depto = GetStringProp(g, "Nombre", "Departamento", "Title", "Titulo") ?? "";
                    var items = GetEnumerableProp(g, "Empleados", "Items", "Filas", "Lista", "Detalle");
                    if (items == null) continue;

                    foreach (var emp in items)
                        AddEmpleadoRow(dt, depto, emp);
                }
                return dt;
            }

            // 2) Fallback: lista plana en el root (Empleados|Items|Filas|Lista)
            var flat = GetEnumerableProp(vm, "Empleados", "Items", "Filas", "Lista");
            if (flat != null)
            {
                foreach (var emp in flat)
                    AddEmpleadoRow(dt, "", emp);
                return dt;
            }

            return dt;
        }

        // —— helpers de reflexión tolerantes a nombres —— //
        private static IEnumerable? GetEnumerableProp(object obj, params string[] candidates)
        {
            var p = GetProp(obj, candidates);
            var v = p?.GetValue(obj);
            if (v is string || v is null) return null;
            return v as IEnumerable;
        }

        private static string? GetStringProp(object obj, params string[] candidates)
        {
            var p = GetProp(obj, candidates);
            var v = p?.GetValue(obj);
            return v?.ToString();
        }

        private static decimal? GetDecimalProp(object obj, params string[] candidates)
        {
            var p = GetProp(obj, candidates);
            var v = p?.GetValue(obj);
            if (v == null) return null;
            if (v is decimal d) return d;
            if (v is double dd) return (decimal)dd;
            if (v is float ff) return (decimal)ff;
            if (decimal.TryParse(v.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out var res))
                return res;
            return null;
        }

        private static DateTime? GetDateProp(object obj, params string[] candidates)
        {
            var p = GetProp(obj, candidates);
            var v = p?.GetValue(obj);
            if (v == null) return null;
            if (v is DateTime dt) return dt;
            if (DateTime.TryParse(v.ToString(), out var res)) return res;
            return null;
        }

        private static PropertyInfo? GetProp(object obj, params string[] candidates)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return props.FirstOrDefault(pi =>
                candidates.Any(c => string.Equals(pi.Name, c, StringComparison.OrdinalIgnoreCase)));
        }

        private static void AddEmpleadoRow(DataTable dt, string depto, object emp)
        {
            var carne = GetStringProp(emp, "Carne", "Carnet", "Codigo", "IdEmpleado") ?? "";
            var ap1 = GetStringProp(emp, "Apellido1", "ApellidoPaterno", "PrimerApellido") ?? "";
            var ap2 = GetStringProp(emp, "Apellido2", "ApellidoMaterno", "SegundoApellido") ?? "";
            var nombre = GetStringProp(emp, "Nombre", "NombreCompleto", "Nombres") ?? "";
            var puesto = GetStringProp(emp, "Puesto", "Cargo") ?? "";
            var salario = GetDecimalProp(emp, "Salario", "Sueldo") ?? 0m;
            var fIngreso = GetDateProp(emp, "FechaIngreso", "FechaAlta");

            var row = dt.NewRow();
            row["Departamento"] = depto;
            row["Carne"] = carne;
            row["Apellido 1"] = ap1;
            row["Apellido 2"] = ap2;
            row["Nombre"] = nombre;
            row["Salario"] = salario;
            row["Puesto"] = puesto;
            row["Fecha ingreso"] = (object?)fIngreso ?? DBNull.Value;
            dt.Rows.Add(row);
        }

        // ====================== Util ======================
        public static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }
    }
}
