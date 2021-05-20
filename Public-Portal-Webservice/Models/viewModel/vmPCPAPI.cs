using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Public_Portal_Webservice.Models.viewModel
{
    public class vmPCPAPI
    {
        

        public IEnumerable<Complaint> Complaints { get; set; }

        public IEnumerable<Status> Complaint_Status { get; set; }

        public int ComplaintVotesCount { get; set; }

        public int SupporingComplaintCount { get; set; }
        public Complaint SingleComplaint { get; set; }
        public IEnumerable<SupportingComplaint> SupportingComplaints { get; set; }

    }

}