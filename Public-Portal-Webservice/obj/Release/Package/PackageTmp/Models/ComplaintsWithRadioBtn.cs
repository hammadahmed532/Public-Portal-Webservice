using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Public_Portal_Webservice.Models
{
    public class ComplaintsWithRadioBtn
    {


        public int complaint_Id { get; set; }
        public string SelectedRadio { get; set; }
        public string Remarks { get; set; }
        public int expectedAmount { get; set; }
        public int budgetYear { get; set; }
    }
}