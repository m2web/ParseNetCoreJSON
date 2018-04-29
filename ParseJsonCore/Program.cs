using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ParseJsonCore
{
    class Program
    {
        #region Using JSON File

        //static void Main(string[] args)
        //{
        //    var json = "";
        //    var jsonFile = @"C:\Users\m2web\code\ParseJsonCore\ESV_john1_1.json";

        //    if (File.Exists(jsonFile))
        //    {
        //        using (var r = new StreamReader(jsonFile))
        //        {
        //            json = r.ReadToEnd();
        //        }

        //        var verses = JsonToObject(json);

        //        foreach (var verse in verses)
        //        {
        //            Console.WriteLine(verse);
        //        }
        //    }
        //    else
        //    {
        //        Console.WriteLine("No file to load.");
        //    }

        //    Console.WriteLine("Press any key to continue.");
        //    Console.ReadKey();
        //}

        public static List<string> JsonToObject(string json)
        {
            Passage rootObj = JsonConvert.DeserializeObject<Passage>(json);
            return rootObj.passages;
        }

        #endregion

        #region Using HttpClient
        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            var verses = RunAsync().GetAwaiter().GetResult();
            foreach (var verse in verses)
            {
                Console.WriteLine(verse);
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        static async Task<List<string>> RunAsync()
        {
            client.BaseAddress = new Uri("https://api.esv.org");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                HttpResponseMessage response = await client.GetAsync("/v3/passage/html/?q=john%201%3A1");
                response.EnsureSuccessStatusCode();
                var stringResult = await response.Content.ReadAsStringAsync();
                var rootObj = JsonConvert.DeserializeObject<Passage>(stringResult);
                return rootObj.passages;
            }
            catch (Exception e)
            {
                var list = new List<string>();
                list.Add(e.Message);
                return list;
            }
        }

        #endregion

    }
    
//1. To check if the Json structure is OKAY: http://jsonlint.com/
//2. To genarate my Object class from my Json structure: http://json2csharp.com/

    public class PassageMeta
    {
        public string canonical { get; set; }
        public List<int> chapter_start { get; set; }
        public List<int> chapter_end { get; set; }
        public int prev_verse { get; set; }
        public int next_verse { get; set; }
        public List<int> prev_chapter { get; set; }
        public List<int> next_chapter { get; set; }
    }

    public class Passage
    {
        public string query { get; set; }
        public string canonical { get; set; }
        public List<List<int>> parsed { get; set; }
        public List<PassageMeta> passage_meta { get; set; }
        public List<string> passages { get; set; }
    }
}
