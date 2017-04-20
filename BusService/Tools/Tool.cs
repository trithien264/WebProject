using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace BusService.Tools
{
    public class Tool
    {
        //public static object ToObject(string jsonStr)
        //{
        //    var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
        //    return result;
        //}

       
        public static T ToObject<T>(Dictionary<string, object> dict)
        {
            return (T)JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(dict));
        }        
        public static object ToObject(string jsonStr)
        {
            if (jsonStr == null || jsonStr.Length == 0)
                return null;
            return JavaScriptObjectDeserializer.DeserializeObject(jsonStr);
        }

        public static Dictionary<string, object> ToDic(string jsonStr)
        {
            if (jsonStr == null || jsonStr.Length == 0)
                return new Dictionary<string, object>();
            return JavaScriptObjectDeserializer.DeserializeDic(jsonStr);
        }

        public static Dictionary<string, object> ToDic(params object[] array)
        {
            if (array != null)
            {
                Dictionary<string, object> ret = new Dictionary<string, object>();
                for (int i = 0; i < array.Length; i++)
                {
                    ret.Add(array[i].ToString(), array[++i]);
                }
                return ret;
            }
            return null;
        }
    }
}
