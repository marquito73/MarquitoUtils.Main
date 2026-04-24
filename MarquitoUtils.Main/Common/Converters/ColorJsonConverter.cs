using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;

namespace MarquitoUtils.Main.Common.Converters
{
    /// <summary>
    /// A convertor, for convert string to <see cref="Color"/> and vice versa, for json serialization and deserialization.
    /// </summary>
    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color ReadJson(JsonReader reader, Type objectType, Color existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            string s = token.Type == JTokenType.String ? token.ToString() : "#000000";

            return ColorTranslator.FromHtml(s);
        }

        public override void WriteJson(JsonWriter writer, Color value, JsonSerializer serializer)
        {
            Color color = value;

            writer.WriteValue(ColorTranslator.ToHtml(color));
        }
    }
}
