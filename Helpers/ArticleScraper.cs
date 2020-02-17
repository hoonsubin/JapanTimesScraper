using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using JapanTimesScraper.Models;
using System.IO;
using Newtonsoft.Json;

namespace JapanTimesScraper.Helpers
{
    static class ArticleScraper
    {

        static public List<Article> GetArticlesWithTag(string tagKey)
        {
            var articles = new List<Article>();
            try
            {
                Console.WriteLine("Searching articles with tag " + tagKey + "...");
                var urls = GetArticleUrlsWithTag(tagKey);
                
                Console.WriteLine("Parsing article contents from search results...");
                foreach (var link in urls)
                {
                    articles.Add(GetArticleContent(link));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return articles;
        }

        /// <summary>
        /// Parses a given Japan Times article and makes an article object out of it
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Article content</returns>
        static Article GetArticleContent(string uri)
        {
            
            HtmlWeb web = new HtmlWeb();

            // get html document
            var document = web.Load(uri);

            // parse through the html file with the given xpath
            var titleNode = document.DocumentNode.SelectSingleNode("//hgroup/h1");
            var contentNodes = document.DocumentNode.SelectNodes("//div[@id='jtarticle']/p");
            var authorNode = document.DocumentNode.SelectSingleNode("//h5[@role='author']");
            // author is a nullable value
            string author = "none";
            if (authorNode != null)
                author = authorNode.InnerText;
            var pubDateNode = document.DocumentNode.SelectSingleNode("//time/@datetime");

            // convert string to DateTime
            DateTime pubDate = DateTime.Parse(pubDateNode.InnerText);

            string articleContent = "";

            foreach (var p in contentNodes)
            {
                articleContent += p.InnerText;
            }

            // create an article object with the found entries
            var article = new Article(titleNode.InnerText, author, pubDate, articleContent, uri);
            
            return article;
        }

        static uint GetMaxPageNo(HtmlDocument resultPage)
        {
            var pagesNodes = resultPage.DocumentNode.SelectNodes("//span[@class='pages']/a[@class='page-numbers']");

            if (pagesNodes == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToUInt32(pagesNodes.Last().InnerText);
            }
        }

        static List<string> GetArticleUrlsWithTag(string tagKey)
        {
            string tag = tagKey.Replace(' ', '-').ToLower();

            string uri = $"https://www.japantimes.co.jp/tag/{tag}/page/";

            HtmlWeb web = new HtmlWeb();
            // get html document
            var document = web.Load(uri + 2);

            uint maxPageNo = GetMaxPageNo(document);

            var tagResults = new List<string>();

            for(int i = 1; i <= maxPageNo; i++)
            {
                tagResults.Add(uri + i);
            }
            var outputLinks = new List<string>();
            foreach(var result in tagResults)
            {
                var resPage = web.Load(result);
                var articles = resPage.DocumentNode.SelectNodes("//article/div/header/hgroup/p");
                foreach(var articleItem in articles)
                {
                    string articleUrl = articleItem.SelectSingleNode("./a[@href]").GetAttributeValue("href", string.Empty);

                    outputLinks.Add(articleUrl);
                }
            }

            return outputLinks;

        }

    }
}
