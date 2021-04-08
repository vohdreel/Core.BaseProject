using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BaseProject.Util
{
    public static class JSONExtensions
    {
        public static T Read<T>(string filePath)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        public static void Write<T>(T model, string filePath)
        {
            File.WriteAllText(filePath, JsonConvert.SerializeObject(model));
        }

    }
}
