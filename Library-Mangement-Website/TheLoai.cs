//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Library_Mangement_Website
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class TheLoai
    {
        public TheLoai()
        {
            this.Saches = new HashSet<Sach>();
        }
    
        public int theloai_id { get; set; }

        [Display(Name = "Genre")]
        public string Ten { get; set; }
    
        public virtual ICollection<Sach> Saches { get; set; }
    }
}
