using UnityEngine;
using System;

namespace CrossPromo
{
    public class JsonHelper
    {

        public static T ParseFromJsonString<T>(string json)
        {
            T wrapper = JsonUtility.FromJson<T>(json);
            return wrapper;
        }

        public static string ParseFromJsonStringToArray(string json)
        {
            int index = 0;
            string[] a = new string[] { };

            for (int i = 0; i < json.Length; i++)
            {
                if (json[i].ToString() == "[" || json[i].ToString() == "]" || json[i].ToString() == ",")
                    continue;

                a.SetValue(json[i].ToString(), index);
                index++;
            }

            return a.ToString();
        }

        public static T FromJson<T>(string json)
        {
            T wrapper = JsonUtility.FromJson<T>(json);
            return wrapper;
        }

        public static T[] FromJsonArray<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }

        private string fixJson(string value)
        {
            value = "{\"Items\":" + value + "}";
            return value;
        }
    }
}
