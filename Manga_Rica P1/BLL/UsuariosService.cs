using System;
using System.Data;
using System.Collections.Generic;
using Manga_Rica_P1.DAL;
using Manga_Rica_P1.Entity;

namespace Manga_Rica_P1.BLL
{
    /// <summary>
    /// Servicio de negocio para Usuarios. 
    /// Depende directamente de la clase DAL UsuarioRepository (sin interfaz).
    /// </summary>
    public sealed class UsuariosService
    {
        private readonly UsuarioRepository _repo;
        private static readonly string[] PerfilesPermitidos = { "Admin", "Empleado", "Supervisor" };
// ...


        public UsuariosService(UsuarioRepository repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        // =========================================================
        //  Listado paginado (modo servidor) para la UI
        //  Retorna: (DataTable page, int total)
        // =========================================================
        public (DataTable page, int total) GetPageAsDataTable(int pageIndex, int pageSize, string? filtro)
        {
            var result = _repo.GetPage(pageIndex, pageSize, filtro);
            IEnumerable<Usuario> items = result.items;
            int total = result.total;

            return (ToDataTable(items), total);
        }

        // =========================================================
        //  Consultas de apoyo / compatibilidad
        // =========================================================
        public Usuario? Get(int id) => _repo.GetById(id);

        public Usuario? GetByUsername(string username) => _repo.GetByUsername(username);

        public List<Usuario> GetAll() => _repo.GetAllUsuario();

        // =========================================================
        //  Comandos (CRUD)
        // =========================================================
        public int Create(Usuario u, string? rawPassword = null)
        {
            if (u is null) throw new ArgumentNullException(nameof(u));
            u.username = (u.username ?? "").Trim();
            u.perfil = (u.perfil ?? "").Trim();
            u.password = string.IsNullOrWhiteSpace(rawPassword) ? (u.password ?? "").Trim()
                                                               : rawPassword.Trim();
            if (u.fecha == default) u.fecha = DateTime.Now;

            Validate(u, isCreate: true);
            return _repo.Insert(u);
        }

        public void Update(Usuario u, string? newRawPassword = null)
        {
            if (u is null) throw new ArgumentNullException(nameof(u));
            u.username = (u.username ?? "").Trim();
            u.perfil = (u.perfil ?? "").Trim();
            if (!string.IsNullOrWhiteSpace(newRawPassword)) u.password = newRawPassword.Trim();

            Validate(u, isCreate: false);
            _repo.Update(u);
        }

        public void Delete(int id)
        {
            // Si tu modelo necesita SoftDelete por FKs, cámbialo aquí.
            _repo.Delete(id);
        }

        // =========================================================
        //  Reglas de negocio / Validaciones
        // =========================================================
        private void Validate(Usuario u, bool isCreate)
        {
            // Requeridos
            if (string.IsNullOrWhiteSpace(u.username))
                throw new ArgumentException("El nombre de usuario es obligatorio.");

            if (string.IsNullOrWhiteSpace(u.password))
                throw new ArgumentException("La contraseña es obligatoria.");

            if (string.IsNullOrWhiteSpace(u.perfil))
                throw new ArgumentException("El perfil es obligatorio.");

            if (!PerfilesPermitidos.Contains(u.perfil!, StringComparer.OrdinalIgnoreCase))
                throw new ArgumentException($"Perfil no válido. Permitidos: {string.Join(", ", PerfilesPermitidos)}");

            // Unicidad por Nombre (ignora el propio Id cuando edita)
            if (_repo.ExistsByNombre(u.username, isCreate ? null : u.Id))
                throw new InvalidOperationException("Ya existe un usuario con ese nombre.");

            // Importante: en tu BD actual 'Clave' es VARCHAR(15).
            if (u.password.Length > 15)
                throw new ArgumentException("La contraseña excede 15 caracteres (límite actual de la base de datos).");
        }

        // =========================================================
        //  Utilidades de mapeo para la UI
        // =========================================================
        private static DataTable ToDataTable(IEnumerable<Usuario> items)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Nombre", typeof(string));   // DB: Nombre
            dt.Columns.Add("Perfil", typeof(string));   // DB: Perfil
            dt.Columns.Add("Fecha", typeof(DateTime));  // DB: Fecha

            foreach (var u in items)
            {
                dt.Rows.Add(u.Id, u.username, u.perfil ?? "", u.fecha);
            }
            return dt;
        }


    }
}
