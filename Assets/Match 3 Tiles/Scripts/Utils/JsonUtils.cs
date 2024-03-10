using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace Match3Tiles.Scripts.Utils
{
    public static class JsonUtils
    {
        public static bool ReadInt(this JsonReader reader, out int? value)
        {
            value = 0;

            try
            {
                value = reader.ReadAsInt32();
            }
            catch(Exception e)
            {
                Debug.LogError($"Read int value error!: {e.Message}");
                return false;
            }

            return true;
        }

        public static bool ReadFloat(this JsonReader reader, out float? value)
        {
            value = 0;

            try
            {
                value = (float)reader.ReadAsDouble();
            }
            catch (Exception e)
            {
                Debug.LogError($"Read float value error!: {e.Message}");
                return false;
            }

            return true;
        }

        public static bool ReadInts(this JsonReader reader, out int? value1, out int? value2, out int? value3)
        {
            value1 = value2 = value3 = 0;
            
            try
            {
                reader.ReadInt(out value1);
                reader.ReadInt(out value2);
                reader.ReadInt(out value3);
            }
            catch(Exception exception)
            {
                Debug.LogError(exception.Message);
                return false;
            }

            return true;
        }

        public static bool ReadIntsAndFloats(this JsonReader reader
            , out int? value1, out int? value2
            , out float? x, out float? y, out float? z)
        {
            value1 = value2 = 0;
            x = y = z = 0;
            
            try
            {
                reader.ReadInt(out value1);
                reader.ReadInt(out value2);
                reader.ReadFloat(out x);
                reader.ReadFloat(out y);
                reader.ReadFloat(out z);
            }
            catch(Exception exception)
            {
                Debug.LogError(exception.Message);
                return false;
            }

            return true;
        }
    }
}
