using System;
using System.Collections.Generic;
using com.ethnicthv.Util.Config.Exception;
using Newtonsoft.Json;
using UnityEngine;

namespace com.ethnicthv.Util.Config
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