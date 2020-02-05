using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using WebAPI.Models;

namespace WebAPI.Code
{
    public static class Util
    {
        const string _FILENAME = "users.json";
        public static List<User> LoadJson()
        {
            var filePath = string.Format("{0}{1}", ConfigurationManager.AppSettings["jsonPath"], _FILENAME);
            using (StreamReader r = new StreamReader(filePath))
            {
                string json = r.ReadToEnd();
                List<User> items = JsonConvert.DeserializeObject<List<User>>(json);

                return items;
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}