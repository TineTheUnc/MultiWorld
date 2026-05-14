using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace MultiWorld.Common.Types
{
    public class MetaDataConverter : JsonConverter<MetaData>
    {
        public override MetaData ReadJson(JsonReader reader, Type objectType, MetaData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            var meta = new MetaData();

            foreach (var prop in obj.Properties().ToList())
            {
                if (prop.Name.StartsWith("NPC_") && prop.Value.Type == JTokenType.Boolean)
                {
                    if (prop.Value.Value<bool>())
                    {
                        if (Enum.TryParse<NPCFlags>(prop.Name,true, out var flag))
                        {
                            meta.npcFlags |= flag;
                        }
                    }

                    prop.Remove();
                }
            }

            serializer.Populate(obj.CreateReader(), meta);

            return meta;
        }

        public override void WriteJson(JsonWriter writer, MetaData value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var tempSerializer = new JsonSerializer
            {
                Formatting = serializer.Formatting,
                NullValueHandling = serializer.NullValueHandling,
                DefaultValueHandling = serializer.DefaultValueHandling,
                ObjectCreationHandling = serializer.ObjectCreationHandling,
                ReferenceLoopHandling = serializer.ReferenceLoopHandling,
                PreserveReferencesHandling = serializer.PreserveReferencesHandling,
                TypeNameHandling = serializer.TypeNameHandling,
                ContractResolver = serializer.ContractResolver,
                Culture = serializer.Culture,
                DateFormatHandling = serializer.DateFormatHandling,
                DateTimeZoneHandling = serializer.DateTimeZoneHandling
            };

            foreach (var conv in serializer.Converters)
            {
                if (conv == null) continue;
                if (conv.GetType() == typeof(MetaDataConverter)) continue;
                tempSerializer.Converters.Add(conv);
            }

            tempSerializer.Serialize(writer, value);
        }
    }
}
