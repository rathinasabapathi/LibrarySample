using System.Web;
using System.Web.Http;
using System.Web.Routing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//using System.Runtime.Serialization.Json;
//using Newtonsoft.Json;

namespace Library
{
    public class WebApiApplication : HttpApplication
    {
        
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            LibraryDataPreparation();
        }
        private void LibraryDataPreparation()
        {
            string strBase = System.AppDomain.CurrentDomain.BaseDirectory;

            string[] files = Directory.GetFiles(string.Format("{0}Resources\\", strBase));

            XDocument doc = new XDocument(new XElement("Books"));
            int bookId = 1;

            foreach (string file in files)
            {
                var result = File.ReadLines(file)                    
                    .SelectMany(line => (Regex.Replace(line, @"\W|_", " ")).Split(' '))
                    .Where(word => !string.IsNullOrWhiteSpace(word))
                    .GroupBy(word => word)
                    .OrderByDescending(g => g.Count())
                    .ToDictionary(g => g.Key, g => g.Count());

                doc.Element("Books").Add(new XElement("Book",
                    new XAttribute("Id", bookId),
                    new XAttribute("Title", Path.GetFileNameWithoutExtension(file)),
                            //new XElement("Words",
                            from wordcount in result
                            select new XElement("Word", new XAttribute("Name", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
    ToTitleCase(wordcount.Key.ToLower())), new XAttribute("Count", wordcount.Value))

                        ));

                bookId++;
            }

            doc.Save(string.Format("{0}Data\\{1}", strBase, "LibraryData.xml"));
        }
    }
}
