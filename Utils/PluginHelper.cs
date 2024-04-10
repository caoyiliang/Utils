using System.Reflection;

namespace Utils;

public static class PluginHelper
{
    public static List<Type> GetChildren<T>(string? path = null)
    {
        var pluginName = typeof(T).Name;
        var pluginPath = FindPlugin(path ?? pluginName);
        return GetChildren(pluginName, pluginPath);
    }

    public static List<Type> GetChildren(string interfaceName, string? path = null)
    {
        var pluginPath = FindPlugin(path ?? interfaceName);
        return GetChildren(interfaceName, pluginPath);
    }

    private static List<Type> GetChildren(string interfaceName, List<string> pluginPath)
    {
        var result = new List<Type>();
        foreach (string fileName in pluginPath)
        {
            Assembly asm = Assembly.LoadFrom(fileName);
            Type[] t = asm.GetExportedTypes();
            foreach (Type type in t)
            {
                if (type.GetInterface(interfaceName) != null)
                {
                    result.Add(type);
                }
            }
        }
        return result;
    }

    public static object? CreateInstance(Type type, params object?[]? args)
    {
        return Activator.CreateInstance(type, args);
    }

    private static List<string> FindPlugin(string pluginName)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", pluginName);
        return [.. Directory.GetFiles(path, "*.dll")];
    }
}
