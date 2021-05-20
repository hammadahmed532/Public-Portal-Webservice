using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Public_Portal_Webservice.Models
{
    public class LoginAcc
    {


        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "username required")]
        public String username { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "passworddd required")]
        [DataType(DataType.Password)]
        public String password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}