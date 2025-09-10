using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Rica_P1.UI.Helpers
{
    public interface IModuleView
    {
        string Title { get; }
        void OnNavigatedTo(object? parameter = null);
        void OnNavigatedFrom();
    }
}
