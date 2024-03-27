using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YamlDotNet.Serialization;

namespace com.ethnicthv.Util
{
    public static class ConfigProvider
    {
        private static Config _config;

        public static Config GetConfig()
        {
            if (_config != null) return _config;
            var deserializer = new DeserializerBuilder().Build();
            var yaml = Resources.Load<TextAsset>("config/config").text;
            var data = deserializer.Deserialize<Dictionary<string, object>>(yaml);
            _config = new Config(data);
            return _config;
        }
    }
    
    public class Config
    {
        private Dictionary<string, object> _config;

        public Config(Dictionary<string, object> config)
        {
            _config = config;
        }

        public List<string> GetEventPaths()
        {
            if (_config.TryGetValue("eventPaths", out var list))
            {
                switch (list)
                {
                    case List<Dictionary<string, string>> listDict:
                    {
                        var paths = new List<string>();
                    
                        var error = 0;
                    
                        foreach (var cell in listDict)
                        {
                            if (cell.TryGetValue("path", out var path))
                            {
                                paths.Add(path);
                            }
                            else
                            {
                                error++;
                            }
                        }
                        if (error != 0)
                        {
                            UnityEngine.Debug.LogError("Error in config.yml: " + error + " eventPaths");
                        }

                        return paths;
                    }
                    case List<string> paths:
                        return paths;
                }
            }
            else
            {
                UnityEngine.Debug.LogError("Error in config.yml: eventPaths not found");
            }
            return null;
        }
    }
}