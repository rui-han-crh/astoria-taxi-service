using UnityEngine;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Allows Vector2 to be serialized.
/// </summary>
public class Vector2Converter : JsonConverter<Vector2>
{
    public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Parse the JSON object into a Vector2
        using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
        {
            JsonElement root = doc.RootElement;
            float x = root.GetProperty("x").GetSingle();
            float y = root.GetProperty("y").GetSingle();
            return new Vector2(x, y);
        }
    }

    public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
    {
        // Write the Vector2 as a JSON object
        writer.WriteStartObject();
        writer.WriteNumber("x", value.x);
        writer.WriteNumber("y", value.y);
        writer.WriteEndObject();
    }
}