using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string DayBorrowed { get; set; }

        public int DayOvertime { get; set; }
        public string DateReturned { get; set; }

        public string TenDocGia { get; set; }
        public string EmailDocGia { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public Decimal Debt { get; set; }
    }
}