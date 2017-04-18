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
        public static object ToObject(string jsonStr)
        {
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
            return result;
        }
    }
}
