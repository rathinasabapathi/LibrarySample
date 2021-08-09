using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.Repositories
{
    interface IWordRepository
    {
        IEnumerable<Book> GetBooks();

        IEnumerable<Word> GetMostCommonWords(long bookId);

        IEnumerable<Word> Search(long BookId, string SearchText);
    }
}
