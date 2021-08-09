using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Library.Data.Repositories.Impl
{
    public class WordRepository:IWordRepository
    {
        private readonly string _baseUrl = AppDomain.CurrentDomain.BaseDirectory;

        private XDocument _document;
        public WordRepository(XDocument document = null)
        {
            _document = document ?? XDocument.Load(string.Format("{0}Data\\{1}", _baseUrl, "LibraryData.xml"));
        }
        public IEnumerable<Book> GetBooks()
        {
            //XDocument doc = XDocument.Load(string.Format("{0}Data\\{1}", _baseUrl, "LibraryData.xml"));

            var query = from book in _document.Root.Elements("Book")
                        select new Book
                        {
                            Id = (long)book.Attribute("Id"),
                            Title = (string)book.Attribute("Title"),
                        };

            return query.ToArray();
        }

        public IEnumerable<Word> GetMostCommonWords(long BookId)
        {
            var query = (from book in _document.Root.Elements("Book")
                         where (long)book.Attribute("Id") == BookId
                         from word in book.Elements("Word")
                         where ((string)word.Attribute("Name")).Length >= 5
                         select new Word
                         {
                             Name = (string)word.Attribute("Name"),
                             Count = (long)word.Attribute("Count"),
                         }).Take(10);

            return query.ToArray();            
        }

        public IEnumerable<Word> Search(long BookId, string SearchText)
        {
            SearchText = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(SearchText.ToLower());

            var query = (from book in _document.Root.Elements("Book")
                         where (long)book.Attribute("Id") == BookId
                         from word in book.Elements("Word")
                         where ((string)word.Attribute("Name")).StartsWith(SearchText)
                         select new Word
                         {
                             Name = (string)word.Attribute("Name"),
                             Count = (long)word.Attribute("Count"),
                         });

            return query.ToArray();
        }
    }
}