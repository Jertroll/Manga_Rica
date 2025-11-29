using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace Manga_Rica_P1.UI.Reportes.Export
{
    /// <summary>
    /// Builder genérico para proyectar una secuencia fuertemente tipada (o anónima)
    /// a un DataTable con columnas declaradas explícitamente.
    /// </summary>
    public static class ReportTableBuilder
    {
        public sealed class Column<T>
        {
            public string Header { get; }
            public Func<T, object?> Selector { get; }
            public Type? DataType { get; }

            public Column(string header, Func<T, object?> selector, Type? dataType = null)
            {
                Header = header ?? throw new ArgumentNullException(nameof(header));
                Selector = selector ?? throw new ArgumentNullException(nameof(selector));
                DataType = dataType;
            }
        }

        public static DataTable From<T>(IEnumerable<T> rows, params Column<T>[] columns)
        {
            if (rows == null) throw new ArgumentNullException(nameof(rows));
            if (columns == null || columns.Length == 0)
                throw new ArgumentException("Debes definir al menos una columna.", nameof(columns));

            var dt = new DataTable("Reporte");
            foreach (var c in columns)
                dt.Columns.Add(c.Header, c.DataType ?? typeof(string));

            foreach (var r in rows)
            {
                var dr = dt.NewRow();
                for (int i = 0; i < columns.Length; i++)
                    dr[i] = columns[i].Selector(r) ?? DBNull.Value;
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
