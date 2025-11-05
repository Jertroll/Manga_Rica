// Nueva implementacion
using FastReport;
using FastReport.Export.Html;
using FastReport.Export.PdfSimple;
using FastReport.Utils;
using System.Data;
using System.Drawing;

public static class ReporteEmpleadosBuilder
{
    public static Report Crear(DataTable dt, Image? logo = null)
    {
        var report = new Report();
        report.RegisterData(dt, "Empleados");
        report.GetDataSource("Empleados").Enabled = true;

        // Estilo para filas pares (zebra)
        report.Styles.Add(new Style { Name = "EvenRows", Fill = new SolidFill(Color.Honeydew) });

        var page = new ReportPage();
        report.Pages.Add(page);
        page.LeftMargin = page.RightMargin = page.TopMargin = page.BottomMargin = Units.Millimeters * 6;

        // ===== Título =====
        var title = new ReportTitleBand { Height = Units.Centimeters * 1.8f };
        page.Bands.Add(title);

        if (logo != null)
        {
            var pic = new PictureObject
            {
                Bounds = new RectangleF(0, 0, Units.Centimeters * 2.6f, Units.Centimeters * 1.6f),
                Image = logo
            };
            title.Objects.Add(pic);
        }

        var txtTitle = new TextObject
        {
            Bounds = new RectangleF(0, 0, Units.Centimeters * 19f, Units.Centimeters * 1.8f),
            Text = "Empleados",
            HorzAlign = HorzAlign.Center,
            VertAlign = VertAlign.Center,
            Font = new Font("Arial", 12, FontStyle.Bold)
        };
        title.Objects.Add(txtTitle);

        // ===== Encabezado de columnas (se repite en cada página) =====
        var header = new PageHeaderBand { Height = Units.Centimeters * 0.7f };
        page.Bands.Add(header);

        float x = 0;
        TextObject H(string t, float w)
        {
            var o = new TextObject
            {
                Bounds = new RectangleF(Units.Centimeters * x, 0, Units.Centimeters * w, Units.Centimeters * 0.7f),
                Text = t,
                FillColor = Color.PaleGreen,
                Font = new Font("Arial", 10, FontStyle.Bold),
                Border = new Border { Lines = BorderLines.All },
                HorzAlign = HorzAlign.Center,
                VertAlign = VertAlign.Center
            };
            header.Objects.Add(o); x += w; return o;
        }
        H("Carne", 2.6f); H("Nombre", 3.0f); H("Apellido_1", 3.0f); H("Apellido_2", 3.0f);
        H("Salario", 2.5f); H("Puesto", 2.8f); H("Fecha_Ingre", 3.0f); H("Activo", 1.5f);

        // ===== Grupo por Departamento =====
        var gh = new GroupHeaderBand { Height = Units.Centimeters * 0.65f, Condition = "[Empleados.Departamento]" };
        page.Bands.Add(gh);
        gh.Objects.Add(new TextObject
        {
            Bounds = new RectangleF(0, 0, Units.Centimeters * 19f, Units.Centimeters * 0.65f),
            Text = "Departamento: [Empleados.Departamento]",
            Font = new Font("Arial", 10, FontStyle.Bold),
            FillColor = Color.LightGray
        });

        // ===== Detalle =====
        var data = new DataBand
        {
            DataSource = report.GetDataSource("Empleados"),
            Height = Units.Centimeters * 0.55f,
            EvenStyle = "EvenRows"
        };
        page.Bands.Add(data);
        gh.Data = data;  // Enlazar grupo → detalle

        x = 0;
        void C(string expr, float w, HorzAlign a = HorzAlign.Left, string? formato = null)
        {
            var o = new TextObject
            {
                Bounds = new RectangleF(Units.Centimeters * x, 0, Units.Centimeters * w, Units.Centimeters * 0.55f),
                Text = expr,
                HorzAlign = a,
                VertAlign = VertAlign.Center,
                Font = new Font("Arial", 10),
                Border = new Border { Lines = BorderLines.Left | BorderLines.Right | BorderLines.Bottom }
            };
            if (formato == "money")
            {
                var nf = new FastReport.Format.NumberFormat();
                nf.DecimalDigits = 2;
                o.Format = nf;
            }
            if (formato == "date")
            {
                var df = new FastReport.Format.DateFormat();
                df.Format = "dd/MM/yyyy";
                o.Format = df;
            }
            data.Objects.Add(o); x += w;
        }
        C("[Empleados.Carne]", 2.6f);
        C("[Empleados.Nombre]", 3.0f);
        C("[Empleados.Primer_Apellido]", 3.0f);
        C("[Empleados.Segundo_Apellido]", 3.0f);
        C("[Empleados.Salario]", 2.5f, HorzAlign.Right, "money");
        C("[Empleados.Puesto]", 2.8f);
        C("[Empleados.Fecha_Ingreso]", 3.0f, HorzAlign.Center, "date");
        C("[Empleados.Activo]", 1.5f, HorzAlign.Center);

        // ===== Pie de grupo (línea separadora) - CORREGIDO =====
        var gf = new GroupFooterBand { Height = Units.Centimeters * 0.15f };
        gh.GroupFooter = gf;  // ✅ SOLO enlazar al grupo, NO agregar a page.Bands
        gf.Objects.Add(new LineObject { Left = 0, Top = 0, Width = Units.Centimeters * 19f });

        // ===== Pie de página con número =====
        var pf = new PageFooterBand { Height = Units.Centimeters * 0.6f };
        page.Bands.Add(pf);
        pf.Objects.Add(new TextObject
        {
            Bounds = new RectangleF(0, 0, Units.Centimeters * 19f, Units.Centimeters * 0.6f),
            Text = "[PageN]",
            HorzAlign = HorzAlign.Right,
            VertAlign = VertAlign.Center,
            FillColor = Color.PaleGreen,
            Font = new Font("Arial", 10)
        });

        return report;
    }

    // Helpers de exportación
    public static void ExportarHtml(Report rpt, string path)
    {
        rpt.Prepare();
        using var html = new HTMLExport { SinglePage = false, Navigator = true, EmbedPictures = true };
        rpt.Export(html, path);
    }

    public static void ExportarPdf(Report rpt, string path)
    {
        rpt.Prepare();
        using var pdf = new PDFSimpleExport();
        rpt.Export(pdf, path);
    }
}
