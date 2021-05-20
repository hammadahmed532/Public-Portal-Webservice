using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Public_Portal_Webservice.Models
{
    public class ComplaintRegistration

    {


        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Title")]
        public string c_title { get; set; }

        [MinLength(60, ErrorMessage = "Must be a minimum of 60 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cannot be left empty")]
        public string description { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "please select a position")]
        public Nullable<double> map_long { get; set; }        
        public Nullable<double> map_lat { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "upload atleast first image")]
        public HttpPostedFileBase imageFile1 { get; set; }

        public HttpPostedFileBase imageFile2 { get; set; }
        public HttpPostedFileBase imageFile3 { get; set; }
        public Nullable<int> category_id { get; set; }
    }
}