using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent", "Nobody");
            var response = client.DownloadString(new Uri("http://www.smallestdotnet.com/smallestdotnet/javascriptdom.ashx"));
            dynamic t = JsonConvert.DeserializeObject<SmallestDotNetThing>(response);

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new IsoDateTimeConverter());
            
        }

        public class SmallestDotNetThing
        {
            public DotNetVersion latestVersion { get; set; }
            public List<DotNetVersion> allVersions { get; set; }
            public List<DotNetVersion> downloadableVersions { get; set; }
        }

        public class DotNetVersion
        {
            public int major { get; set; }
            public int minor { get; set; }
            public string profile { get; set; }
            public int? servicePack { get; set; }
            public string url { get; set; }
        }
    }
}
