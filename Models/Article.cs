using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapanTimesScraper.Models
{
    class Article
    {
        public Article(string _title, string _author, DateTime _pubDate, string _content, string _source)
        {
            title = _title;
            author = _author;
            pubDate = _pubDate;
            content = _content;
            source = _source;
        }
        public string title { get; set; }

        public string author { get; set; }

        public DateTime pubDate { get; set; }

        public string content { get; set; }

        public string source { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Article objAsArticle = obj as Article;
            if (objAsArticle == null) return false;
            else return Equals(objAsArticle);
        }

        public override int GetHashCode()
        {
            // convert URL string to a number
            return source.GetHashCode();
        }

        public bool Equals(Article other)
        {
            if (other == null) return false;
            return (this.source.Equals(other.source));
        }
    }
}
