// MenuDesplegable.cs
using System;
using System.Linq;
using System.Windows.Forms;

// 👇 agrega este alias:
using WinFormsTimer = System.Windows.Forms.Timer;

namespace Manga_Rica_P1.UI.Ventana_Principal
{
    public sealed class MenuDesplegable : IDisposable
    {
        private readonly Control _contenedor;
        private readonly int _altoMin;
        private int _altoMax;
        private readonly int _paso;

        // 👇 usa el alias aquí
        private readonly WinFormsTimer _timer;
        private bool _expandirObjetivo;

        public bool EstaExpandido => _contenedor.Height > _altoMin;

        public MenuDesplegable(Control contenedor, int altoMin, int altoMax = 0, int paso = 10, int intervaloMs = 15)
        {
            _contenedor = contenedor ?? throw new ArgumentNullException(nameof(contenedor));
            _altoMin = Math.Max(1, altoMin);
            _paso = Math.Max(1, paso);

            _altoMax = altoMax > _altoMin ? altoMax : CalcularAltoContenido(contenedor);
            if (_altoMax <= _altoMin) _altoMax = _altoMin + 1;

            _contenedor.Height = _altoMin;

            // 👇 y aquí
            _timer = new WinFormsTimer { Interval = intervaloMs };
            _timer.Tick += OnTick;
        }

        private static int CalcularAltoContenido(Control cont)
        {
            int maxBottom = cont.Controls.Cast<Control>().Select(c => c.Bottom).DefaultIfEmpty(cont.Height).Max();
            return Math.Max(maxBottom, cont.Height) + 2;
        }

        private void OnTick(object? sender, EventArgs e)
        {
            _contenedor.SuspendLayout();
            if (_expandirObjetivo)
            {
                _contenedor.Height = Math.Min(_contenedor.Height + _paso, _altoMax);
                if (_contenedor.Height >= _altoMax) _timer.Stop();
            }
            else
            {
                _contenedor.Height = Math.Max(_contenedor.Height - _paso, _altoMin);
                if (_contenedor.Height <= _altoMin) _timer.Stop();
            }
            _contenedor.ResumeLayout();
        }

        public void Toggle() { _expandirObjetivo = !_expandirObjetivo; _timer.Start(); }
        public void Expandir() { _expandirObjetivo = true; _timer.Start(); }
        public void Colapsar() { _expandirObjetivo = false; _timer.Start(); }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Tick -= OnTick;
            _timer.Dispose();
        }
    }
}
