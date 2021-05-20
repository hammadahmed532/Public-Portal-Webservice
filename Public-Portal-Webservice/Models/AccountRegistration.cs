using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Public_Portal_Webservice.Models
{
    public class AccountRegistration
    {


        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "cannot leave empty")]
        public String Full_name { get; set; }


        [Display(Name = "Phone Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "phone number required")]
        [DataType(DataType.PhoneNumber)]
        public long phone_number { get; set; }

        [Display(Name = "Email")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "email required")]
        [DataType(DataType.EmailAddress)]
        public String email_address { get; set; }


        [Display(Name = "Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "address required")]
        public String address { get; set; }


        [Display(Name = "Username")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "username required")]
        public String username { get; set; }


        [Display(Name = "password")]
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "password required")]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public String password { get; set; }


        [Display(Name = "confirm password")]
        [Compare("password", ErrorMessage = "password does not match with the above given")]
        [DataType(DataType.Password)]
        public String confirmPassword { get; set; }


        [Display(Name = "gender")]
        public String gender { get; set; }


        [Display(Name = "department")]
        public int department_id { get; set; }


    }
}