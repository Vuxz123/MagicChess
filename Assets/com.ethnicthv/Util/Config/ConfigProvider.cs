using System.Collections.Generic;
using com.ethnicthv.Util.Config.Exception;
using UnityEngine;
using YamlDotNet.Serialization;

namespace com.ethnicthv.Util.Config
{
    public static class ConfigProvider
    {
        private static Config _config;

        public static Config GetConfig()
        {
            if (_config != null) return _config;
            var json = Resources.Load<TextAsset>("config/config").text;
            var data = JsonUtility.FromJson<Dictionary<string, object>>(json);
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

        // public List<string> GetEventPaths()
        // {
        //     if (_config.TryGetValue("eventPaths", out var list))
        //     {
        //         switch (list)
        //         {
        //             case List<Dictionary<string, string>> listDict:
        //             {
        //                 var paths = new List<string>();
        //             
        //                 var error = 0;
        //             
        //                 foreach (var cell in listDict)
        //                 {
        //                     if (cell.TryGetValue("path", out var path))
        //                     {
        //                         paths.Add(path);
        //                     }
        //                     else
        //                     {
        //                         error++;
        //                     }
        //                 }
        //                 if (error != 0)
        //                 {
        //                     Debug.LogError("Error in config.yml: " + error + " eventPaths");
        //                 }
        //
        //                 return paths;
        //             }
        //             case List<string> paths:
        //                 return paths;
        //         }
        //     }
        //     else
        //     {
        //         Debug.LogError("Error in config.yml: eventPaths not found");
        //     }
        //     return null;
        // }

        public SafeMechanismConfig GetSafeMechanismConfig()
        {
            if (SafeMechanismConfig.GetConfig(out var safeMechanismConfig)) return safeMechanismConfig;
            if(_config.TryGetValue("safeMechanism", out var safeMechanism))
            {
                return safeMechanism switch
                {
                    Dictionary<string, object> dict => new SafeMechanismConfig(dict),
                    _ => throw new ConfigLoadFailException("safeMechanism in config.yml is not a dictionary")
                };
            }
            throw new ConfigNotFoundException("safeMechanism not found in config.yml, please check the config.yml");
        }
    }
}