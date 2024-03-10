using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Match3Tiles.Scripts.GameData.LevelData.CustomData;
using Match3Tiles.Scripts.Utils;

namespace Match3Tiles.Scripts.GameData.LevelData.Converters
{
    public class BlockTileDataConverter : JsonConverter<BlockTileData>
    {
        public override BlockTileData ReadJson(JsonReader reader, Type objectType, BlockTileData existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if(reader.ReadIntsAndFloats(out int? id, out int? priority, out float? x, out float? y, out float? z))
            {
                reader.Read();
            }

            if(id.HasValue && priority.HasValue && x.HasValue && y.HasValue && z.HasValue)
            {
                return new BlockTileData
                {
                    OriginID = id.Value,
                    Priority = priority.Value,
                    Position = new Vector3(x.Value, y.Value, z.Value)
                };
            }

            return existingValue;
        }

        public override void WriteJson(JsonWriter writer, BlockTileData value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.OriginID);
            writer.WriteValue(value.Priority);
            writer.WriteValue(value.Position.x);
            writer.WriteValue(value.Position.y);
            writer.WriteValue(value.Position.z);
            writer.WriteEndArray();
        }
    }
}
