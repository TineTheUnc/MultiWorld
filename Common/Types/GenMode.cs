using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiWorld.Common.Types
{
    [JsonConverter(typeof(GenModeConverter))]
    public enum GenMode
    {
        Off = 0,
        Normal = 1,
        Sepecial = 2,
        RandomMod = 3,
        Unknown = 99
    }

    public class GenModeConverter : JsonConverter<GenMode>
    {
        public override GenMode ReadJson(
            JsonReader reader,
            Type objectType,
            GenMode existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return GenMode.Unknown;

            if (reader.TokenType == JsonToken.Integer)
            {
                var i = Convert.ToInt32(reader.Value);
                return Enum.IsDefined(typeof(GenMode), i)
                    ? (GenMode)i
                    : GenMode.Unknown;
            }

            var raw = reader.Value?.ToString();
            if (string.IsNullOrWhiteSpace(raw))
                return GenMode.Unknown;

            var v = raw.Trim()
                       .ToLower()
                       .Replace(" ", "")
                       .Replace("-", "")
                       .Replace("_", "");

            return v switch
            {
                "off" => GenMode.Off,
                "normal" => GenMode.Normal,
                "sepecial" => GenMode.Sepecial, // typo legacy
                "special" => GenMode.Sepecial,
                "randommod" => GenMode.RandomMod,
                "randommode" => GenMode.RandomMod,
                _ => GenMode.Unknown
            };
        }

        public override void WriteJson(
            JsonWriter writer,
            GenMode value,
            JsonSerializer serializer)
        {
            // serialize ออกเป็นแบบใหม่เสมอ
            writer.WriteValue(value.ToString());
        }
    }
}
