using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Public_Portal_Webservice.Models
{
    public class complaint_Det_Status_Change
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "please give remarks")]
        public string Remarks_Field_Off { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "upload atleast first image")]
        public HttpPostedFileBase DetailsimageFile1 { get; set; }

        public HttpPostedFileBase DetailsimageFile2 { get; set; }
        public HttpPostedFileBase DetailsimageFile3 { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "enter expected amount")]
        [DataType(DataType.Currency)]
        public int Expected_amount { get; set; }

    }
}