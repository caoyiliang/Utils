﻿using System.Reflection;

namespace Utils;

public static class PluginHelper
{
    public static List<Type> GetChildren<T>(string? path = null)
    {
        var result = new List<Type>();
        var pluginName = typeof(T).Name;
        var pluginPath = FindPlugin(path ?? pluginName);
        foreach (string fileName in pluginPath)
        {
            Assembly asm = Assembly.LoadFrom(fileName);
            Type[] t = asm.GetExportedTypes();
            foreach (Type type in t)
            {
                if (type.GetInterface(pluginName) != null)
                {
                    result.Add(type);
                }
            }
        }
        return result;
    }

    public static List<Type> GetChildren(string interfaceName, string? path = null)
    {
        var result = new List<Type>();
        var pluginPath = FindPlugin(path ?? interfaceName);
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

    //查找所有插件的路径
    private static List<string> FindPlugin(string pluginName)
    {
        var pluginPath = new List<string>();
        //获取程序的基目录
        string path = AppDomain.CurrentDomain.BaseDirectory;
        //合并路径，指向插件所在目录。
        path = Path.Combine(path, "Plugins", pluginName);
        foreach (string filename in Directory.GetFiles(path, "*.dll"))
        {
            pluginPath.Add(filename);
        }
        return pluginPath;
    }
}
