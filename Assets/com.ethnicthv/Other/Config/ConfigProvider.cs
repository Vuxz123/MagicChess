using Newtonsoft.Json;
using UnityEngine;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Other.Config
{
    public static class ConfigProvider
    {
        private static Config _config;

        public static Config GetConfig()
        {
            if (_config != null) return _config;
            var json = Resources.Load<TextAsset>("config/config").text;
            _config = JsonConvert.DeserializeObject<Config>(json);
            return _config;
        }

        public static void Init()
        {
            GetConfig();
        }
    }

    public class Config
    {
        public SafeMechanismConfig SafeMechanismConfig { get; set; }

        public Config()
        {
            Debug.Log("Config loaded");
        }
    }
}