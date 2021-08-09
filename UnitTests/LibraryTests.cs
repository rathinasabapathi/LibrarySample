using Library.Data.Repositories;
using Library.Data.Repositories.Impl;
using NUnit.Framework;
using System.Linq;
using System.Xml.Linq;

namespace Library.Tests
{
    [TestFixture]
    public class LibraryTests
    {
        //private const string SAMPLE_TEXT = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 
        //        Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate 
        //        velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        private const string SAMPLE_TEXT = @"<Books><Book Id=""1"" Title=""Test Book""><Word Name=""Theme"" Count=""588"" /><Word Name=""Project"" Count=""79"" /><Word Name=""Gutenberg"" Count=""22"" /><Word Name=""EBook"" Count=""2"" /></Book></Books>";

        private XDocument _document = XDocument.Parse(SAMPLE_TEXT);
        private IWordRepository repository;

        public LibraryTests()
        {
            repository = new WordRepository(_document);
        }

        [Test]
        public void GetBooksTest()
        {
            var books = repository.GetBooks();
            Assert.True(!books.Any());
            Assert.AreEqual("Test Book", books.Select(b => b.Title).FirstOrDefault());
        }

        [Test]
        public void GetMostCommonWordsTest()
        {
            var commonWords = repository.GetMostCommonWords(1);
            Assert.IsNotNull(commonWords);
            Assert.AreEqual("Theme", commonWords.Select(c => c.Name).FirstOrDefault());
            Assert.AreEqual("588", commonWords.Select(c => c.Count).FirstOrDefault());
        }

        [Test]
        public void SearchTest()
        {
            var notFound = repository.Search(1, "wordnotthere");
            Assert.True(!notFound.Any());

            var found = repository.Search(1, "pro");
            Assert.AreEqual(1, found.Count());
            Assert.AreEqual("Project", found.Select(c => c.Name).FirstOrDefault());



        }
    }
}