using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manga_Rica_P1.UI.Helpers
{
    public class ModuleViewBase : UserControl, IModuleView
    {
        public virtual string Title => Name;
        public virtual void OnNavigatedTo(object? parameter = null) { }
        public virtual void OnNavigatedFrom() { }
    }
}
