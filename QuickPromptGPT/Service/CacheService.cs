using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using Newtonsoft.Json;
using QuickPromptGPT.Model;

namespace QuickPromptGPT.Service
{
    public class CacheService
    {
        private readonly CacheData _cacheData;
        private const string _cachePath = "cache.json";

        public CacheService(CacheData cacheData)
        {
            _cacheData = cacheData;


            if (File.Exists(_cachePath))
            {
                string text = File.ReadAllText(_cachePath);
                _cacheData = JsonConvert.DeserializeObject<CacheData>(text);
            }
        }

        public string GetKey()
        {
            return _cacheData.Key;
        }

        public void SetKey(string key)
        {
            _cacheData.Key = key;
        }

        public void Save()
        {
            string jsonString = JsonConvert.SerializeObject(_cacheData);
            System.IO.File.WriteAllText(_cachePath, jsonString);
        }
    }
}
