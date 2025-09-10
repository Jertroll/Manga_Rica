using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Rica_P1.ENTITY
{
    public sealed class AuthUser
    {
        public int Id { get; init; }
        public string Username { get; init; } = "";
        public string? Rol { get; init; }
    }
}
