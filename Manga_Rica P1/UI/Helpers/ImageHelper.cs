using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Manga_Rica_P1.UI.Shared
{
    public static class ImageHelper
    {
        // === API principal: procesa y guarda ===
        // - Convierte SIEMPRE a JPG.
        // - Redimensiona con Fill (recorta al centro) o Fit (encaja sin recortar).
        // - Devuelve la ruta relativa guardada (para la BD más adelante).
        public static string ProcessAndSave(
            string sourcePath,
            string destRoot,
            string subfolder,
            string preferredNameWithoutExt,
            int targetW,
            int targetH,
            bool fill = true,
            long jpegQuality = 85L)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
                throw new FileNotFoundException("No se encontró la imagen origen.", sourcePath);

            // Asegura carpeta destino
            var folder = EnsureFolder(destRoot, subfolder);

            // Nombre final (si viene vacío, generamos uno)
            var baseName = string.IsNullOrWhiteSpace(preferredNameWithoutExt)
                ? Path.GetFileNameWithoutExtension(sourcePath)
                : preferredNameWithoutExt;

            // Normalizamos nombre a algo seguro
            baseName = SanitizeFileName(baseName);

            // Usaremos JPG siempre
            var fileName = $"{baseName}_{DateTime.Now:yyyyMMddHHmmss}.jpg";
            var destFullPath = Path.Combine(folder, fileName);

            using (var img = Image.FromFile(sourcePath))
            using (var bmp = fill ? ResizeFill(img, targetW, targetH)
                                  : ResizeFit(img, targetW, targetH))
            {
                SaveJpeg(bmp, destFullPath, jpegQuality);
            }

            // Ruta relativa (para guardar luego en BD)
            var relative = Path.Combine(subfolder, fileName).Replace('\\', '/');
            return relative;
        }

        // ========== Helpers internos ==========

        public static string EnsureFolder(string root, string subfolder)
        {
            var folder = Path.Combine(root, subfolder);
            Directory.CreateDirectory(folder);
            return folder;
        }

        public static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Trim();
        }

        public static Bitmap ResizeFit(Image src, int maxW, int maxH)
        {
            var ratio = Math.Min((double)maxW / src.Width, (double)maxH / src.Height);
            var w = Math.Max(1, (int)Math.Round(src.Width * ratio));
            var h = Math.Max(1, (int)Math.Round(src.Height * ratio));

            var bmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            bmp.SetResolution(96, 96);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clear(Color.White);
                g.DrawImage(src, new Rectangle(0, 0, w, h));
            }
            return bmp;
        }

        public static Bitmap ResizeFill(Image src, int targetW, int targetH)
        {
            var ratio = Math.Max((double)targetW / src.Width, (double)targetH / src.Height);
            var w = Math.Max(1, (int)Math.Round(src.Width * ratio));
            var h = Math.Max(1, (int)Math.Round(src.Height * ratio));

            var x = (w - targetW) / 2;
            var y = (h - targetH) / 2;

            using var tmp = new Bitmap(w, h, PixelFormat.Format24bppRgb);
            tmp.SetResolution(96, 96);
            using (var g = Graphics.FromImage(tmp))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.Clear(Color.White);
                g.DrawImage(src, new Rectangle(0, 0, w, h));
            }

            var crop = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb);
            crop.SetResolution(96, 96);
            using (var g = Graphics.FromImage(crop))
            {
                g.DrawImage(tmp, new Rectangle(0, 0, targetW, targetH), new Rectangle(x, y, targetW, targetH), GraphicsUnit.Pixel);
            }
            return crop;
        }

        public static void SaveJpeg(Bitmap bmp, string path, long quality = 85L)
        {
            var codec = ImageCodecInfo.GetImageEncoders().First(c => c.MimeType == "image/jpeg");
            using var ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            bmp.Save(path, codec, ep);
        }
    }
}
