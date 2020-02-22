﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CsvHelper;
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
            //string input = Console.ReadLine();
            string input = "carlos-ghosn";
            string startDate = "2018-09-19";
            //string endDate = "2017-12-01";

            const string dateFormat = "yyyy-MM-dd";

            var filteredArticles = FilterArticles(ArticleScraper.GetArticlesWithTag(input), DateTime.ParseExact(startDate, dateFormat,
                CultureInfo.InvariantCulture), DateTime.Now);

            //SaveAsJson(filteredArticles);

            SaveAsCsv(filteredArticles);

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }

        static void SaveAsJson(List<Article> source, string saveDir = "")
        {
            Console.WriteLine("Saving list of objects as JSON...");
            string jsonString = JsonConvert.SerializeObject(source);

            System.IO.File.WriteAllText(saveDir + "output.json", jsonString);
        }

        static void SaveAsCsv(List<Article> source, string saveDir = "")
        {

            Console.WriteLine("Saving list of objects as CSV...");

            using (var writer = new StreamWriter(saveDir + "output.csv"))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csvWriter.Configuration.Delimiter = ",";
                csvWriter.WriteField("Title");
                csvWriter.WriteField("Author");
                csvWriter.WriteField("PubDate");
                csvWriter.WriteField("Content");
                csvWriter.WriteField("Source");
                csvWriter.NextRecord();
                foreach (var i in source)
                {
                    csvWriter.WriteField(i.title);
                    csvWriter.WriteField(i.author);
                    csvWriter.WriteField(i.pubDate.ToString("yyyy-MM-dd"));
                    csvWriter.WriteField(i.content);
                    csvWriter.WriteField(i.source);
                    csvWriter.NextRecord();
                }
                writer.Flush();

            }


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
