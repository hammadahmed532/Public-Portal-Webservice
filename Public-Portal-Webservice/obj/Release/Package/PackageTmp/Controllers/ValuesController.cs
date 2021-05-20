using Public_Portal_Webservice.Models.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Public_Portal_Webservice.Models;

namespace Public_Portal_Webservice.Controllers
{
    public class ValuesController : ApiController
    {
        
        [HttpGet]
        public IHttpActionResult Get()
        {
            //return new string[] { "value1", "value2" };
            PCP5Entities db = new PCP5Entities();
            db.Configuration.ProxyCreationEnabled = false;

            var com = db.Complaints.ToList();
            return Json(com);
            
        }

        [HttpGet]
        public IHttpActionResult TownsList()
        {
            PCP5Entities db = new PCP5Entities();
            db.Configuration.ProxyCreationEnabled = false;

            var com = db.Towns.ToList();
            return Json(com);
        }

        [HttpGet]
        public IHttpActionResult UCList(int town_id)
        {
            PCP5Entities db = new PCP5Entities();
            db.Configuration.ProxyCreationEnabled = false;

            var com = db.UCAreas.Where(a=>a.TownID==town_id).ToList();
            return Json(com);
        }

        [HttpGet]
        public IHttpActionResult TownListComplaints(int town_id)
        {
            vmPCPAPI vmapi = new vmPCPAPI();
            PCP5Entities db = new PCP5Entities();
            db.Configuration.ProxyCreationEnabled = false;

            var coms = db.Complaints.Where(a => a.UCArea.TownID == town_id).ToList();
            // vmapi.Complaint_Status=db.Status.Where(a=>a.Complaints.Where(a=>a.UCArea.TownID==town_id))


            return Json(coms);

        }

        [HttpGet]
        public IHttpActionResult TownListComplaints()
        {
            vmPCPAPI vmapi = new vmPCPAPI();
            PCP5Entities db = new PCP5Entities();
            db.Configuration.ProxyCreationEnabled = false;

            var coms = db.Complaints.Where(a => a.UCArea.TownID == 1).ToList();
            // vmapi.Complaint_Status=db.Status.Where(a=>a.Complaints.Where(a=>a.UCArea.TownID==town_id))


            return Json(coms);

        }

        [HttpGet]
        public IHttpActionResult UCListComplaint(int town_id,int uc_id)
        {
            vmPCPAPI vmapi = new vmPCPAPI();
               PCP5Entities db = new PCP5Entities();
            db.Configuration.ProxyCreationEnabled = false;

            if (uc_id!=0) {
                vmapi.Complaints = db.Complaints.Where(a => a.UC_area_id==uc_id).ToList();
            }
            else
            {
                vmapi.Complaints = db.Complaints.Where(a => a.UCArea.TownID==town_id).ToList();
            }
            return Json(vmapi);
        }

        [HttpGet]
        public IHttpActionResult GetComplaintDetails(int complaintId)
        {
            vmPCPAPI vmapi = new vmPCPAPI();
            PCP5Entities db = new PCP5Entities();
            db.Configuration.ProxyCreationEnabled = false;
            vmapi.SingleComplaint = db.Complaints.Where(a => a.complaint_Id == complaintId).FirstOrDefault();
            vmapi.ComplaintVotesCount= db.Votings.Where(a => a.complaint_id == complaintId).Count();
            vmapi.SupportingComplaints = db.SupportingComplaints.Where(a => a.Complaint_id == complaintId).ToList();
            vmapi.SupporingComplaintCount = vmapi.SupportingComplaints.Count();

            return Json(vmapi);
        } 



        [HttpGet]
        public IHttpActionResult TownWiseComplaint(int som)

        {

            vmPCP vm = new vmPCP();
            PCP5Entities db = new PCP5Entities();

            db.Configuration.ProxyCreationEnabled = false;
            String username = System.Web.HttpContext.Current.User.Identity.Name;
            vm.acc = db.Accounts.Where(a => a.username.Equals(username)).FirstOrDefault();
            //int townId = Int32.Parse(id.ToString());
            vm.comps = db.Complaints.Where(uc => uc.UCArea.TownID == som).ToList();

            vm.severeComps = db.Complaints.Where(sv => sv.UCArea.TownID == som).ToList();
            vm.severeComps = (from q in vm.severeComps
                              orderby q.SupportingComplaints.Count() descending
                              select q).Take(5);
            vm.ucs = db.UCAreas.Where(a => a.TownID == som).ToList();
            vm.categories = db.Categories.ToList();



            var townNameDb = db.Towns.Where(b => b.town_id == som).FirstOrDefault();

            if (User.IsInRole("public"))
            {
                vm.notificationComplaints = db.Complaints.Where(a => a.account_id == vm.acc.Account_id)
                                           .ToList();
            }


            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            return Json(vm);
        }



    }
}
