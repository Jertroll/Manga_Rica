// AppSession.cs
using Manga_Rica_P1.Entity;
using Manga_Rica_P1.ENTITY;
using System;

namespace Manga_Rica_P1.BLL.Session
{
    public sealed class AppSession : IAppSession
    {
        private AuthUser? _current;
        public AuthUser? CurrentUser
        {
            get => _current;
            set { _current = value; UserChanged?.Invoke(this, EventArgs.Empty); }
        }
        public event EventHandler? UserChanged;
    }
}
