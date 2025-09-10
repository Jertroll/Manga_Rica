// UI/Helpers/NavigationService.cs
using Manga_Rica_P1.UI.Helpers;

public class NavigationService
{
    private readonly Panel _host;
    private readonly Dictionary<Type, IModuleView> _cache = new();

    public IModuleView? Current { get; private set; }

    public NavigationService(Panel host) => _host = host;

    public T Navigate<T>(object? parameter = null)
        where T : UserControl, IModuleView, new()
    {
        // Lifecycle: saliendo de la actual
        Current?.OnNavigatedFrom();

        // Obtener o crear
        if (!_cache.TryGetValue(typeof(T), out var view))
        {
            view = new T(); // ctor sin args
            _cache[typeof(T)] = view;
        }

        var ctrl = (Control)view;

        _host.SuspendLayout();
        try
        {
            _host.Controls.Clear();
            if (ctrl.Parent != null) ctrl.Parent = null;
            _host.Controls.Add(ctrl);
            ctrl.BringToFront();
        }
        finally
        {
            _host.ResumeLayout();
        }

        view.OnNavigatedTo(parameter);
        Current = view;
        return (T)view;
    }

    public void RemoveFromCache<T>() where T : IModuleView
    {
        if (_cache.Remove(typeof(T), out var v) && v is IDisposable d) d.Dispose();
    }

    public void ClearCacheExceptCurrent()
    {
        foreach (var kv in _cache.ToList())
        {
            if (!ReferenceEquals(kv.Value, Current) && kv.Value is IDisposable d)
            {
                d.Dispose();
                _cache.Remove(kv.Key);
            }
        }
    }
}
