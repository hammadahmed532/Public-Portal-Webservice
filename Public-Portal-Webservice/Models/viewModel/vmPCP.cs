using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Public_Portal_Webservice.Models.viewModel
{
    public class vmPCP
    {

        public List<ComplaintsWithRadioBtn> compsWithRadioBtn { get; set; }
        public ComplaintsWithRadioBtn comRbtn { get; set; }
        public IEnumerable<Feedback> FeedbacksMultiple { get; set; }
        public IEnumerable<Complaint> comps { get; set; }
        public Complaint com { get; set; }
        public IEnumerable<Complaint> severeComps { get; set; }
        public IEnumerable<Complaint> notificationComplaints { get; set; }

        public IEnumerable<SupportingComplaint> supcomps { get; set; }
        public SupportingComplaint supcomp { get; set; }

        public IEnumerable<Account> accounts { get; set; }
        public Account acc { get; set; }

        public IEnumerable<Category> categories { get; set; }

        public IEnumerable<UCArea> ucs { get; set; }
        public UCArea ucSingle { get; set; }
        

        public IEnumerable<Town> towns { get; set; }
        public Town town { get; set; }

        public IEnumerable<Voting> votes { get; set; }
        public Voting votee { get; set; }

        public IEnumerable<Status> status { get; set; }
        public Complaint_Det_Remarks compDetails { get; set; }
        public IEnumerable<Complaint_Det_Remarks> compDetailsList { get; set; }

        public IEnumerable<Complaint> VotesComplaints { get; set; }
        public IEnumerable<Complaint> MoreComplaints { get; set; }

        public IEnumerable<Announcement1> annoucementsMultiple { get; set; }
        public Announcement1 singleAnnouncement { get; set; }

        public int TotalComplaints { get; set; }
        public int totalResolvedComplaints { get; set; }

        /// <summary>
        /// models with data annotations
        /// </summary>
        public LoginAcc login { get; set; }

        public AccountRegistration accountReg { get; set; }

        public ComplaintRegistration compReg { get; set; }

        public complaint_Det_Status_Change Comp_det_Annotations { get; set; }

    }
}