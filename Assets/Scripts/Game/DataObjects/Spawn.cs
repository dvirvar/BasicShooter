using UnityEngine;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[JsonConverter(typeof(SpawnConverter))]
public class Spawn
{
    public string id;
    public Vector3 position;
    public Vector3 rotation;
    public Spawn(string id,Vector3 position, Vector3 rotation)
    {
        this.id = id;
        this.position = position;
        this.rotation = rotation;
    }
}

class SpawnConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Spawn);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        var json = JObject.Load(reader);
        var id = (string)json["id"];
        var positionJson = (JObject)json["position"];
        var position = new Vector3((float)positionJson["x"], (float)positionJson["y"], (float)positionJson["z"]);
        var rotationJson = (JObject)json["rotation"];
        var rotation = new Vector3((float)rotationJson["x"], (float)rotationJson["y"], (float)rotationJson["z"]);
        return new Spawn(id,position,rotation);
    }
    
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var spawn = (Spawn)value;
        writer.WriteStartObject();
        writer.WritePropertyName("id");
        writer.WriteValue(spawn.id);

        writer.WritePropertyName("position");
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(spawn.position.x);
        writer.WritePropertyName("y");
        writer.WriteValue(spawn.position.y);
        writer.WritePropertyName("z");
        writer.WriteValue(spawn.position.z);
        writer.WriteEndObject();

        writer.WritePropertyName("rotation");
        writer.WriteStartObject();
        writer.WritePropertyName("x");
        writer.WriteValue(spawn.rotation.x);
        writer.WritePropertyName("y");
        writer.WriteValue(spawn.rotation.y);
        writer.WritePropertyName("z");
        writer.WriteValue(spawn.rotation.z);
        writer.WriteEndObject();

        writer.WriteEndObject();
        writer.Flush();
    }
}
