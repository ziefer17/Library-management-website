using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Mangement_Website.Models
{
    public class SachViewModel
    {
        public int SachId { get; set; }
        public string TenSach { get; set; }
        public string ThongTin { get; set; }
        public string TheLoaiTen { get; set; }
        public string Authors { get; set; }
        public int TotalCopies { get; set; }
        public string Publisher { get; set; }

        public String YearPublished { get; set; }

    }
}