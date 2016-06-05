using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace AspNetUpgrade
{
    public static class NewtonsoftExtensions
    {
        //public static void Rename(this JToken token, string newName)
        //{
        //    var parent = token.Parent;
        //    if (parent == null)
        //        throw new InvalidOperationException("The parent is missing.");
        //    var newToken = new JProperty(newName, token);
        //    parent.Replace(newToken);
        //}


        public static void Rename(this JToken token, string newName)
        {
            var parent = token.Parent;
            if (parent == null)
                throw new InvalidOperationException("The parent is missing.");
            var newToken = new JProperty(newName, token);
            parent.Replace(newToken);
        }

        public static JToken Rename(this JToken json, Func<string, string> map)
        {
            JProperty prop = json as JProperty;
            if (prop != null)
            {
                return new JProperty(map(prop.Name), Rename(prop.Value, map));
            }

            JArray arr = json as JArray;
            if (arr != null)
            {
                var cont = arr.Select(el => Rename(el, map));
                return new JArray(cont);
            }

            JObject o = json as JObject;
            if (o != null)
            {
                var cont = o.Properties().Select(el => Rename(el, map));
                return new JObject(cont);
            }

            return json;
        }

        public static JObject GetOrAddProperty(this JObject root, string propertyName, JProperty insertAfter)
        {
            var newProperty = (JObject)root[propertyName];
            if (newProperty == null)
            {
                newProperty = new JObject();
                var property = new JProperty(propertyName, newProperty);
                if (insertAfter != null)
                {
                    insertAfter.AddAfterSelf(property);
                }
                else
                {
                    root.Add(property);
                }
            }

            return newProperty;
        }
    }
}