using System;
using System.Collections.Generic;
using System.Data;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Reglas de negocio para Empleados.
    /// </summary>
    public sealed class EmpleadosService
    {
        private readonly EmpleadoRepository _repo;
        public EmpleadosService(EmpleadoRepository repo)
            => _repo = repo ?? throw new ArgumentNullException(nameof(repo));

        // =========================================================
        //  Listado paginado (para PagedSearchGrid)
        // =========================================================
        public (DataTable page, int total) GetPageAsDataTable(int pageIndex, int pageSize, string? filtro)
        {
            var result = _repo.GetPage(pageIndex, pageSize, filtro);
            return (ToDataTable(result.items), result.total);
        }

        // Auxiliares
        public Empleado? GetById(int id) => _repo.GetById(id);
        public Empleado? GetByCedula(string cedula) => _repo.GetByCedula(cedula);
        
        /// <summary>
        /// Verifica si existe un empleado activo (Activo = 1) con la cédula especificada.
        /// </summary>
        public bool ExistsActiveByCedula(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula)) return false;
            
            var empleado = _repo.GetByCedula(cedula.Trim());
            return empleado != null && empleado.Activo == 1;
        }

        // =========================================================
        //  Comandos
        // =========================================================
        public int Create(Empleado e)
        {
            if (e is null) throw new ArgumentNullException(nameof(e));
            Normalize(e);
            Validate(e, isCreate: true);
            return _repo.Insert(e);
        }

        public void Update(Empleado e)
        {
            if (e is null) throw new ArgumentNullException(nameof(e));
            Normalize(e);
            Validate(e, isCreate: false);
            _repo.Update(e);
        }

        public void Delete(int id) => _repo.Delete(id);

        // =========================================================
        //  Validaciones / Normalización
        // =========================================================
        private void Validate(Empleado e, bool isCreate)
        {
            if (e.Carne <= 0) throw new ArgumentException("El carné es obligatorio.");
            if (string.IsNullOrWhiteSpace(e.Cedula)) throw new ArgumentException("La cédula es obligatoria.");
            if (string.IsNullOrWhiteSpace(e.Primer_Apellido)) throw new ArgumentException("El primer apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(e.Nombre)) throw new ArgumentException("El nombre es obligatorio.");
            if (e.Fecha_Nacimiento == default) throw new ArgumentException("La fecha de nacimiento es obligatoria.");
            if (string.IsNullOrWhiteSpace(e.Estado_Civil)) throw new ArgumentException("El estado civil es obligatorio.");
            if (string.IsNullOrWhiteSpace(e.Telefono)) throw new ArgumentException("El teléfono es obligatorio.");
            if (string.IsNullOrWhiteSpace(e.Celular)) throw new ArgumentException("El celular es obligatorio.");
            if (string.IsNullOrWhiteSpace(e.Nacionalidad)) throw new ArgumentException("La nacionalidad es obligatoria.");
            if (string.IsNullOrWhiteSpace(e.Direccion)) throw new ArgumentException("La dirección es obligatoria.");
            if (e.Id_Departamento <= 0) throw new ArgumentException("Seleccione un departamento válido.");
            if (e.Salario < 0) throw new ArgumentException("El salario no puede ser negativo.");
            if (string.IsNullOrWhiteSpace(e.Puesto)) throw new ArgumentException("El puesto es obligatorio.");
            if (e.Fecha_Ingreso == default) throw new ArgumentException("La fecha de ingreso es obligatoria.");

            // En tu BD, Fecha_Salida es NOT NULL -> exigirla
            if (e.Fecha_Salida == default) throw new ArgumentException("La fecha de salida es obligatoria.");

            // Regla heredada del sistema viejo:
            if (e.Fecha_Nacimiento.Date == DateTime.Today)
                throw new ArgumentException("Seleccione una fecha de nacimiento válida (distinta a hoy).");

            // Si creamos, exigir Activo = 1 (como forzaba el módulo viejo)
            if (isCreate && e.Activo == 0)
                throw new ArgumentException("El empleado nuevo debe registrarse como ACTIVO.");

            // Unicidad por cédula
            if (_repo.ExistsByCedula(e.Cedula, isCreate ? null : e.Id))
                throw new InvalidOperationException("Ya existe un empleado con esa cédula.");
        }

        private static void Normalize(Empleado e)
        {
            e.Cedula = (e.Cedula ?? "").Trim();
            e.Primer_Apellido = (e.Primer_Apellido ?? "").Trim();
            e.Segundo_Apellido = (e.Segundo_Apellido ?? "").Trim(); // BD la tiene NOT NULL
            e.Nombre = (e.Nombre ?? "").Trim();
            e.Estado_Civil = (e.Estado_Civil ?? "").Trim();
            e.Telefono = (e.Telefono ?? "").Trim();
            e.Celular = (e.Celular ?? "").Trim();
            e.Nacionalidad = (e.Nacionalidad ?? "").Trim();
            e.Direccion = (e.Direccion ?? "").Trim();
            e.Puesto = (e.Puesto ?? "").Trim();
            e.Foto = (e.Foto ?? "").Trim();

            e.Laboro = e.Laboro != 0 ? 1 : 0;
            e.Activo = e.Activo != 0 ? 1 : 0;
        }

        // =========================================================
        //  Mapeo a DataTable para la grilla
        // =========================================================
        private static DataTable ToDataTable(IEnumerable<Empleado> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Carne", typeof(long));
            dt.Columns.Add("Cedula", typeof(string));
            dt.Columns.Add("Apellido 1", typeof(string));
            dt.Columns.Add("Apellido 2", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Fecha Nacimiento", typeof(DateTime));
            dt.Columns.Add("Estado Civil", typeof(string));
            dt.Columns.Add("Telefono", typeof(string));
            dt.Columns.Add("Celular", typeof(string));
            dt.Columns.Add("Nacionalidad", typeof(string));
            dt.Columns.Add("Laboro", typeof(int));         // 0/1
            dt.Columns.Add("Direccion", typeof(string));
            dt.Columns.Add("Id_Departamento", typeof(int));
            dt.Columns.Add("Departamento", typeof(string)); // NUEVO: nombre del departamento
            dt.Columns.Add("Salario", typeof(decimal));
            dt.Columns.Add("Puesto", typeof(string));
            dt.Columns.Add("Fecha Ingreso", typeof(DateTime));
            dt.Columns.Add("Fecha Salida", typeof(DateTime));
            dt.Columns.Add("Activo", typeof(int));         // 0/1

            foreach (var e in items)
            {
                dt.Rows.Add(
                    e.Id,
                    e.Carne,
                    e.Cedula,
                    e.Primer_Apellido,
                    e.Segundo_Apellido,
                    e.Nombre,
                    e.Fecha_Nacimiento,
                    e.Estado_Civil,
                    e.Telefono,
                    e.Celular,
                    e.Nacionalidad,
                    e.Laboro,
                    e.Direccion,
                    e.Id_Departamento,
                    e.Departamento_Nombre, // NUEVO
                    Convert.ToDecimal(e.Salario),
                    e.Puesto,
                    e.Fecha_Ingreso,
                    e.Fecha_Salida,
                    e.Activo
                );
            }
            return dt;
        }

        //---------------------------------Reportes------------------------------------



    }
}
