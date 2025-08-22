namespace DTemplate.Domain.Identifier
{
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public sealed class CIdNulleableJsonConverter : JsonConverter<CId?>
    {
        public override CId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return CIdMetadata.NulleableJsonConverter(value);
        }

        public override void Write(Utf8JsonWriter writer, CId? value, JsonSerializerOptions options)
        {
            var writeValue = value is null ? null : value.ToString();
            writer.WriteStringValue(writeValue);
        }
    }
}
