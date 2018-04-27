using System;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.Serialization.Json;

namespace ParseJsonCore
{
    class ProgramOLD
    {
        //static void Main(string[] args)
        //{
        //    var json = "";
        //    var jsonFile = @"C:\Users\m2web\code\ParseJsonCore\ESV_john114.json";

        //    using (var r = new StreamReader(jsonFile))
        //    {
        //        json = r.ReadToEnd();
        //    }

        //    Console.WriteLine(ParseJson(json));
        //    Console.WriteLine("**************************************");
        //    Console.WriteLine(ParseToVerse(json));
        //    Console.WriteLine("**************************************");

        //    Console.WriteLine("Press any key to continue.");
        //    Console.ReadKey();
        //}

        static string ParseJson(string inputJson)
        {
            var verses = new Verse();
            try
            {
                verses = JsonConvert.DeserializeObject<Verse>(inputJson);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return verses.Passages;
        }

        public static string ParseToVerse(string json)
        {
            var theVerse = new Verse();
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Verse));
            theVerse = (Verse)ser.ReadObject(ms);
            ms.Close();
            return theVerse.Passages;
        }
    }

    public class Verse
    {
        public string Query;
        public string Passages;
    }
}
