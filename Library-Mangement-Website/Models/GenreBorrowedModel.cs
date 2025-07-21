using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Mangement_Website.Models
{
    public class GenreBorrowedModel
    {
        public string Genre { get; set; }
        public int BorrowedCopies { get; set; }
        public double Percentage { get; set; }
    }
}