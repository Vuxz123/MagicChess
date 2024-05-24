using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace com.ethnicthv.Networking
{
    public class NetworkID
    {
        public Dictionary<string, byte> IDMap { get; private set; }

        public NetworkID()
        {
            var json = Resources.Load<TextAsset>("config/network_id").text;
            IDMap = JsonConvert.DeserializeObject<Dictionary<string, byte>>(json);
        }
        
        public byte GetID(string key)
        {
            if (!IDMap.ContainsKey(key)) throw new KeyNotFoundException($"Key {key} not found in IDMap.");
            return IDMap[key];
        }
    }
}