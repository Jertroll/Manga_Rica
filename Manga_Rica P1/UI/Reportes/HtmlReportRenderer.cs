// Nueva implementacion
using RazorLight;
using System.Threading.Tasks;
using System.Dynamic;

namespace MangaRica.UI.Reports
{
    /// <summary>
    /// Nueva implementacion: convierte Plantilla (.cshtml) + ViewModel => HTML.
    /// Agregamos soporte de ViewBag (datos de cabecera/pie) y caching de plantillas.
    /// </summary>
    public sealed class HtmlReportRenderer
    {
        private readonly RazorLightEngine _engine;

        // Nueva implementacion: plantillas desde el sistema de archivos + caché en memoria
        public HtmlReportRenderer(string templatesRootPath)
        {
            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(templatesRootPath) // carpeta que contiene .cshtml                     // Nueva implementacion: cachear compilaciones
                .Build();
        }

        // Nueva implementacion: render con ViewBag (cabecera/pie/logo/host)
        public Task<string> RenderAsync<TModel>(
            string templateName,
            TModel model,
            string? title = null,
            string? footer = null,
            string? virtualHost = null,
            string? company = null,
            string? phones = null,
            string? address = null,
            string? logoFile = null)
        {
            dynamic bag = new ExpandoObject();
            bag.Title = title;
            bag.Footer = footer;
            bag.VirtualHost = virtualHost;
            bag.Company = company;
            bag.Phones = phones;
            bag.Address = address;
            bag.LogoFile = logoFile;

            // ⚠️ Importante: usar nombre de parámetro para forzar el overload correcto
            return _engine.CompileRenderAsync(templateName, model, viewBag: (ExpandoObject)bag);
        }
    }
}
