using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

[JsonConverter(typeof(CharacterConverter))]
public class Character 
{
    public Vector3 rotation;
    public PlayerCustomization characterCustomization;

    [JsonConstructor]
    public Character(Vector3 rotation, PlayerCustomization characterCustomization)
    {
        this.rotation = rotation;
        this.characterCustomization = characterCustomization;
    }
}

class CharacterConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Character);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var json = JObject.Load(reader);
        var rotationToken = json["rotation"];
        Vector3 rotation;
        if (rotationToken != null)
        {
            var rotationJson = (JObject)json["rotation"];
            rotation = new Vector3((float)rotationJson["x"], (float)rotationJson["y"], (float)rotationJson["z"]);
        } else
        {
            rotation = Vector3.zero;
        }
        var customizationJson = (JObject)json["characterCustomization"];
        var customization = new PlayerCustomization((string)customizationJson["jellyColor"], (string)customizationJson["donutColor"]);
        return new Character(rotation,customization);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var character = (Character)value;
        writer.WriteStartObject();
      
        writer.WritePropertyName("rotation");
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(character.rotation.x);
        writer.WritePropertyName("y");
        writer.WriteValue(character.rotation.y);
        writer.WritePropertyName("z");
        writer.WriteValue(character.rotation.z);
        writer.WriteEndObject();

        writer.WritePropertyName("characterCustomization");
        writer.WriteStartObject();
        writer.WritePropertyName("jellyColor");
        writer.WriteValue(character.characterCustomization.jellyColor);
        writer.WritePropertyName("donutColor");
        writer.WriteValue(character.characterCustomization.donutColor);
        writer.WriteEndObject();

        writer.WriteEndObject();
        writer.Flush();
    }
}
