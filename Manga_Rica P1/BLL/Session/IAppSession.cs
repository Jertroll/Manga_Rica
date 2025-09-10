// IAppSession.cs
using Manga_Rica_P1.Entity;
using Manga_Rica_P1.ENTITY;
using System;

namespace Manga_Rica_P1.BLL.Session
{
    public interface IAppSession
    {
        AuthUser? CurrentUser { get; set; }
        event EventHandler? UserChanged;
    }
}
