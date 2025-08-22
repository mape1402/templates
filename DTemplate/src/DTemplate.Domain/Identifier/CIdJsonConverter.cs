namespace DTemplate.Domain.Identifier
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public sealed class CIdJsonConverter : JsonConverter<CId>
    {
        public override CId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return CIdMetadata.JsonConverter(value);
        }
        
        public override void Write(Utf8JsonWriter writer, CId value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
