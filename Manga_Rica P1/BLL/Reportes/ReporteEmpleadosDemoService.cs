// BLL/Reportes/ReporteEmpleadosDemoService.cs
using System;
using System.Data;

namespace Manga_Rica_P1.BLL.Reportes
{
    public sealed class ReporteEmpleadosDemoService
    {
        /// <summary>
        /// Nueva implementacion:
        /// Genera una tabla de ejemplo con N empleados (por defecto 120) para probar paginado.
        /// </summary>
        public DataTable ConstruirTablaDemo(int cantidad = 120) // Nueva implementacion
        {
            // Nueva implementacion: DataTable "plano" desacoplado del motor de reportes
            var dt = new DataTable("Empleados");
            dt.Columns.Add("Id", typeof(long));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Departamento", typeof(string));
            dt.Columns.Add("Salario", typeof(decimal));

            // Nueva implementacion: catálogos simples para variar datos
            string[] nombres =
            {
                "Ana López","Luis Mora","María Pérez","Carlos Gómez","Elena Ruiz","Jorge Soto","Karla Vargas",
                "Pedro Jiménez","Lucía Rojas","Andrés Castro","Sofía Ramírez","Diego Hernández","Paula Méndez",
                "Gustavo Céspedes","Marta Salas","Ernesto Quesada","Daniela Solano","Kevin Araya","Laura Navarro",
                "Felipe León","Valeria Chaves","Adrián Barrantes","Mónica Pacheco","Javier Pizarro","Inés Muñoz",
                "Bruno Rojas","Nuria Aguilar","Esteban Mora","Camila Umaña","Pablo Cedeño","Silvia Araya",
                "Ricardo Prado","Fabiola Núñez","Héctor Alvarado","Gabriela Piedra","Marco Rojas","Nathalia Leiva",
                "Pamela Chacón","Sergio Vargas","Cindy Reyes","Josué Castillo","Katherine Mora","Oscar Granados",
                "Belén Sánchez","Mauricio Segura","Patricia León","Álvaro Solís","Michelle Vega","Iván Paniagua",
                "Yuliana Barboza","David Quirós","Ashley Pérez","Cristian Blanco","Marina Carrillo","Fidel Madrigal",
                "Tatiana Zúñiga","Nelson Araya","Evelyn Cordero","Harold Coto","Roxana Vargas","Elisa Jiménez",
                "Fernando Sáenz","Rafael Segura","Claudia Mora","Edgar Gamboa","Nicolás Díaz","Camilo Pérez",
                "Rita Solís","Olman Castillo","Bárbara Cerdas","Henry Quesada","Melissa Arce","Joel Brenes",
                "Fanny Durán","Gerson Rojas","Joselin Villalobos","Marcos Mora","Viviana Calderón","Ivette León",
                "Isabella Jiménez","Santiago Vargas","Thiago Rodríguez","Valentina Mora","Emilia Chaves","Tomás Ruiz",
                "Martina Solano","Regina Pacheco","Agustín Castro","Emilio Rojas","Facundo León","Damián Gómez",
                "Antonella Navarro","Renata Jiménez","Matías Ramírez","Lucas Porras","Benjamín Cedeño","Sara Cedeño"
            };

            string[] departamentos = { "Contabilidad", "Producción", "RRHH", "TI", "Logística", "Ventas" };

            // Nueva implementacion: generador determinista para reproducibilidad
            var rnd = new Random(12345);

            for (int i = 0; i < cantidad; i++)
            {
                long id = i + 1;

                // Nueva implementacion: seleccion de nombre/dep “cíclica” con algo de aleatoriedad
                string nombre = nombres[i % nombres.Length];
                string depto = departamentos[(i + rnd.Next(0, departamentos.Length)) % departamentos.Length];

                // Nueva implementacion: salario base variable con ligero ruido
                // base 420,000–720,000 +/− 0–39,999
                decimal salarioBase = 420_000m + (i % 7) * 45_000m + rnd.Next(0, 40_000);
                decimal salario = Math.Round(salarioBase, 0);

                dt.Rows.Add(id, nombre, depto, salario);
            }

            return dt;
        }
    }
}
