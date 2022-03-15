using System.Reflection;

namespace Utils
{
    public static class PluginHelper
    {
        public static List<T> GetChildren<T>()
        {
            var result = new List<T>();
            var pluginName = typeof(T).Name;
            var pluginPath = FindPlugin(pluginName);
            foreach (string fileName in pluginPath)
            {
                Assembly asm = Assembly.LoadFile(fileName);
                Type[] t = asm.GetExportedTypes();
                foreach (Type type in t)
                {
                    if (type.GetInterface(pluginName) != null)
                    {
                        result.Add((T)Activator.CreateInstance(type));
                    }
                }
            }
            return result;
        }

        //查找所有插件的路径
        private static List<string> FindPlugin(string pluginName)
        {
            List<string> pluginPath = new List<string>();
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
}
