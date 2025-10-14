using System;
using System.Collections.Generic;
using System.Data;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Reglas de negocio para Solicitud.
    /// - Requeridos: Cédula, Nombre, Primer_Apellido, Estado_Civil, Celular, Nacionalidad, Dirección.
    /// - Fecha_Nacimiento no puede ser igual a hoy.
    /// - Telefono es opcional.
    /// </summary>
    public sealed class SolicitudesService
    {
        private readonly SolicitudRepository _repo;
        public SolicitudesService(SolicitudRepository repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        // =========================================================
        //  Paginado para la UI (modo servidor)
        // =========================================================
        public (DataTable page, int total) GetPageAsDataTable(int pageIndex, int pageSize, string? filtro)
        {
            var result = _repo.GetPage(pageIndex, pageSize, filtro);
            return (ToDataTable(result.items), result.total);
        }

        // =========================================================
        //  Consultas auxiliares
        // =========================================================
        public Solicitudes? Get(int id) => _repo.GetById(id);

        public List<Solicitudes> GetAll(string? filtro = null)
        {
            var (items, _) = _repo.GetPage(1, int.MaxValue / 4, filtro);
            return new List<Solicitudes>(items);
        }

        // =========================================================
        //  Comandos (CRUD)
        // =========================================================
        public int Create(Solicitudes s)
        {
            if (s is null) throw new ArgumentNullException(nameof(s));
            Normalize(s);
            Validate(s, isCreate: true);
            return _repo.Insert(s);
        }

        public void Update(Solicitudes s)
        {
            if (s is null) throw new ArgumentNullException(nameof(s));
            Normalize(s);
            Validate(s, isCreate: false);
            _repo.Update(s);
        }

        public void Delete(int id) => _repo.Delete(id);

        // =========================================================
        //  Reglas de negocio / Validaciones
        // =========================================================
        private void Validate(Solicitudes s, bool isCreate)
        {
            if (string.IsNullOrWhiteSpace(s.Cedula))
                throw new ArgumentException("La cédula es obligatoria.");
            if (string.IsNullOrWhiteSpace(s.Primer_Apellido))
                throw new ArgumentException("El primer apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(s.Nombre))
                throw new ArgumentException("El nombre es obligatorio.");
            if (s.Fecha_Nacimiento == default)
                throw new ArgumentException("La fecha de nacimiento es obligatoria.");
            if (string.IsNullOrWhiteSpace(s.Estado_Civil))
                throw new ArgumentException("El estado civil es obligatorio.");
            if (string.IsNullOrWhiteSpace(s.Celular))
                throw new ArgumentException("El celular es obligatorio.");
            if (string.IsNullOrWhiteSpace(s.Nacionalidad))
                throw new ArgumentException("La nacionalidad es obligatoria.");
            if (string.IsNullOrWhiteSpace(s.Direccion))
                throw new ArgumentException("La dirección es obligatoria.");

            if (s.Fecha_Nacimiento.Date == DateTime.Today)
                throw new ArgumentException("Seleccione una fecha de nacimiento válida (distinta a hoy).");

            if (_repo.ExistsByCedula(s.Cedula, isCreate ? null : s.Id))
                throw new InvalidOperationException("Ya existe una solicitud con esa cédula.");
        }

        private static void Normalize(Solicitudes s)
        {
            s.Cedula = (s.Cedula ?? "").Trim();
            s.Primer_Apellido = (s.Primer_Apellido ?? "").Trim();
            s.Segundo_Apellido = string.IsNullOrWhiteSpace(s.Segundo_Apellido) ? null : s.Segundo_Apellido.Trim();
            s.Nombre = (s.Nombre ?? "").Trim();
            s.Estado_Civil = (s.Estado_Civil ?? "").Trim();
            // Telefono opcional → null si viene vacío
            s.Telefono = string.IsNullOrWhiteSpace(s.Telefono) ? null : s.Telefono.Trim();
            s.Celular = (s.Celular ?? "").Trim();
            s.Nacionalidad = (s.Nacionalidad ?? "").Trim();
            s.Direccion = (s.Direccion ?? "").Trim();

            s.Laboro = s.Laboro != 0 ? 1 : 0;
            s.Fecha_Nacimiento = s.Fecha_Nacimiento.Date;
        }

        // =========================================================
        //  Mapeo a DataTable para la grilla
        // =========================================================
        private static DataTable ToDataTable(IEnumerable<Solicitudes> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Cedula", typeof(string));
            dt.Columns.Add("Apellido 1", typeof(string));
            dt.Columns.Add("Apellido 2", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Fecha Nacimiento", typeof(DateTime));
            dt.Columns.Add("Estado Civil", typeof(string));
            dt.Columns.Add("Telefono", typeof(string));     // opcional
            dt.Columns.Add("Celular", typeof(string));
            dt.Columns.Add("Nacionalidad", typeof(string));
            dt.Columns.Add("Laboro", typeof(int));
            dt.Columns.Add("Direccion", typeof(string));

            foreach (var s in items)
            {
                dt.Rows.Add(
                    s.Id,
                    s.Cedula,
                    s.Primer_Apellido,
                    s.Segundo_Apellido,
                    s.Nombre,
                    s.Fecha_Nacimiento,
                    s.Estado_Civil,
                    s.Telefono ?? string.Empty,  // ← evita null en la grilla
                    s.Celular,
                    s.Nacionalidad,
                    s.Laboro,
                    s.Direccion
                );
            }
            return dt;
        }
    }
}
