using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcomTest.Common.Helpers
{
    public static class ObjectComparer
    {
        /// <summary>
        /// Pass it two objects and it will tell you if they are equal to one another. Pass in an optional IEnumerable<string> to represent any properties that you might want to ignore</string>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1">E.g. entity</param>
        /// <param name="obj2">E.g. entity</param>
        /// <param name="ignoreProperties"></param>
        /// <returns>true if serializing each yields the same json string</returns>
        /// <remarks>Useful for two separate objects that are not pointers pointing to the same one.</remarks>
        public static bool AreObjectsEqual<T>(T obj1, T obj2, IEnumerable<string> ignoreProperties = null)
        {
            // If both objects are null, they are equal
            if (ReferenceEquals(obj1, null) && ReferenceEquals(obj2, null))
                return true;

            // If one of the objects is null, they are not considered equal
            if (ReferenceEquals(obj1, null) || ReferenceEquals(obj2, null))
                return false;

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new IgnorePropertiesJsonResolver(ignoreProperties),
                Formatting = Formatting.None
            };

            // Serialize objects to JSON for deep copy and compare the strings
            var json1 = JsonConvert.SerializeObject(obj1, jsonSerializerSettings);
            var json2 = JsonConvert.SerializeObject(obj2, jsonSerializerSettings);

            return string.Equals(json1, json2, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        ///  Pass it two objects and it will tell you if they are equal to one another. Pass in an optional IEnumerable<string> to represent any properties that you might want to ignore</string>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1">E.g. IEnumerable of type entity</param>
        /// <param name="obj2">E.g. IEnumerable of type entity</param>
        /// <param name="ignoreProperties"></param>
        /// <returns>true if serializing each yields the same json string</returns>
        /// <reamrks>Useful for two separate collections that are not pointers pointing to the same one.</reamrks>
        public static bool AreCollectionsOfObjectsEqual<T>(IEnumerable<T> obj1, IEnumerable<T> obj2, IEnumerable<string> ignoreProperties = null)
        {
            // If both collections are null, they are equal
            if (obj1 == null && obj2 == null)
                return true;

            // If one of the collections is null, they are not considered equal
            if (obj1 == null || obj2 == null)
                return false;

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new IgnorePropertiesJsonResolver(ignoreProperties),
                Formatting = Formatting.None
            };

            // Serialize objects to JSON for deep copy and compare the strings
            var json1 = JsonConvert.SerializeObject(obj1, jsonSerializerSettings);
            var json2 = JsonConvert.SerializeObject(obj2, jsonSerializerSettings);

            return string.Equals(json1, json2, StringComparison.OrdinalIgnoreCase);
        }


       
    }
}
