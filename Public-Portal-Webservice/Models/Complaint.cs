//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Public_Portal_Webservice.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Complaint
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Complaint()
        {
            this.Complaint_Det_Remarks = new HashSet<Complaint_Det_Remarks>();
            this.Feedbacks = new HashSet<Feedback>();
            this.PendingComplaints = new HashSet<PendingComplaint>();
            this.SupportingComplaints = new HashSet<SupportingComplaint>();
            this.Votings = new HashSet<Voting>();
        }
    
        public int complaint_Id { get; set; }
        public string c_title { get; set; }
        public Nullable<int> account_id { get; set; }
        public string description { get; set; }
        public Nullable<int> UC_area_id { get; set; }
        public Nullable<double> map_long { get; set; }
        public Nullable<double> map_lat { get; set; }
        public System.DateTime date_creation { get; set; }
        public Nullable<System.DateTime> date_stage_2 { get; set; }
        public Nullable<System.DateTime> date_stage_3 { get; set; }
        public Nullable<System.DateTime> date_stage_4 { get; set; }
        public Nullable<System.DateTime> date_stage_6 { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }
        public string image3 { get; set; }
        public int stage_id { get; set; }
        public Nullable<System.DateTime> date_stage_5 { get; set; }
        public Nullable<int> category_id { get; set; }
        public string view_public { get; set; }
        public Nullable<bool> admin_confirm { get; set; }
        public System.DateTime date_last_modified { get; set; }
        public Nullable<int> Forwarded_By_Account_id { get; set; }
        public string complaint_Type { get; set; }
        public Nullable<bool> is_Notified { get; set; }
        public Nullable<double> Expected_amount { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Account Account1 { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint_Det_Remarks> Complaint_Det_Remarks { get; set; }
        public virtual Status Status { get; set; }
        public virtual UCArea UCArea { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PendingComplaint> PendingComplaints { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupportingComplaint> SupportingComplaints { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voting> Votings { get; set; }
    }
}
