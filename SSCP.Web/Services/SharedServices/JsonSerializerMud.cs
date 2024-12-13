namespace SSCP.Web.Services.SharedServices
{
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using MudBlazor;

    // Custom converter for the Severity enum as dictionary keys
    public class SeverityDictionaryConverter : JsonConverter<Dictionary<Severity, int>>
    {
        public override Dictionary<Severity, int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dictionary = new Dictionary<Severity, int>();

            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                    break;

                // Read the key as string and convert to Severity enum
                string key = reader.GetString();
                if (!Enum.TryParse(typeof(Severity), key, out var severity))
                    throw new JsonException($"Invalid key '{key}' for Severity enum.");

                reader.Read();
                int value = reader.GetInt32();

                dictionary.Add((Severity)severity, value);
            }

            return dictionary;
        }

        public override void Write(Utf8JsonWriter writer, Dictionary<Severity, int> value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            foreach (var kvp in value)
            {
                writer.WritePropertyName(kvp.Key.ToString());
                writer.WriteNumberValue(kvp.Value);
            }
            writer.WriteEndObject();
        }
    }

}
