using PruebaNewtech.BOL;
using System.Collections.Generic;

namespace PruebaNewtech.Web.Models
{
    public class BooksAuthorViewModel
    {
        public Books Book { get; set; }
        public IList<Authors> Author { get; set; }
    }
}
