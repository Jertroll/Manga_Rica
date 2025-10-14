// ARCHIVO DESHABILITADO - IMPLEMENTACI�N DE SODA REVERTIDA
// Este archivo formaba parte de la implementaci�n de Soda de la rama de Steven
// Ha sido deshabilitado para revertir los cambios

using System;
using System.Windows.Forms;

namespace Manga_Rica_P1.UI.Soda
{
    /// <summary>
    /// SodaView deshabilitada - implementaci�n de Soda revertida
    /// </summary>
    public class SodaView_DISABLED : UserControl
    {
        public SodaView_DISABLED()
        {
            var label = new Label
            {
                Text = "M�dulo Soda deshabilitado - Implementaci�n revertida",
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            Controls.Add(label);
        }
    }
}