using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Library_Mangement_Website.Models
{
    public class ReaderViewModel
    {
        public int ReaderId { get; set; }

        public string ReaderName { get; set; }
        public string ReaderEmail { get; set; }
        public string DateCreated { get; set; }
        public string DateOfBirth { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
        public string ReaderType { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string ConfirmPassword { get; set; }
    }
}