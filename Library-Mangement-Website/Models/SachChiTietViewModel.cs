using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Mangement_Website.Models
{
    public class SachChiTietViewModel
    {
        public Sach_copy Sach_Copy { get; set; }
        public Sach Sach => Sach_Copy?.Sach; // Truy cập nhanh sách gốc
        public List<Sach_copy> DeXuatSachs { get; set; }

        public List<Sach_copy> SachCopies { get; set; }
        public string image { get; set; }
    }
}