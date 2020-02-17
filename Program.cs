using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JapanTimesScraper.Models;
using JapanTimesScraper.Helpers;
using System.Globalization;
using Newtonsoft.Json;

namespace JapanTimesScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please provide article tag");
            string input = Console.ReadLine();
            string startDate = "2010-01-01";
            string endDate = "2017-12-01";

            const string dateFormat = "yyyy-MM-dd";

            var filteredArticles = FilterArticles(ArticleScraper.GetArticlesWithTag(input), DateTime.ParseExact(startDate, dateFormat,
                CultureInfo.InvariantCulture), DateTime.ParseExact(endDate, dateFormat,
                CultureInfo.InvariantCulture));

            SaveAsJson(filteredArticles);

            foreach (var i in filteredArticles)
            {
                Console.WriteLine(i.pubDate);
            }

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }

        static void SaveAsJson(List<Article> source, string saveDir = "")
        {
            Console.WriteLine("Saving list of objects as JSON...");
            string jsonString = JsonConvert.SerializeObject(source);

            System.IO.File.WriteAllText(saveDir + "output.json", jsonString);
        }

        static List<Article> FilterArticles(List<Article> articleSource, DateTime startDate, DateTime endDate)
        {
            var filteredArticles = new List<Article>();

            foreach(var i in articleSource.Where(x => x.pubDate >= startDate && x.pubDate <= endDate))
            {
                filteredArticles.Add(i);
            }

            return filteredArticles;
        }
    }
}
