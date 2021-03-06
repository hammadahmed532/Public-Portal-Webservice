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
    using System.Runtime.Serialization;

    public partial class Account
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Account()
        {
            this.Complaints = new HashSet<Complaint>();
            this.Complaints1 = new HashSet<Complaint>();
            this.SupportingComplaints = new HashSet<SupportingComplaint>();
            this.Votings = new HashSet<Voting>();
            this.Announcements = new HashSet<Announcement1>();
            this.Announcements1 = new HashSet<Announcement1>();
        }
    
        public int Account_id { get; set; }
        public string Full_name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Nullable<bool> isEmailVerified { get; set; }
        public Nullable<System.Guid> ActivationCode { get; set; }
        public long phone_number { get; set; }
        public string address { get; set; }
        public Nullable<double> map_long { get; set; }
        public Nullable<double> map_lat { get; set; }
        public string email_address { get; set; }
        public Nullable<int> UC_Area_id { get; set; }
        public Nullable<int> Town_id { get; set; }
        public Nullable<System.DateTime> date_created { get; set; }
        public string profession { get; set; }
        public string gender { get; set; }
        public Nullable<int> department_id { get; set; }
        public int Role { get; set; }
    
        [IgnoreDataMember]
        public virtual Category Category { get; set; }
        [IgnoreDataMember]
        public virtual Town Town { get; set; }
        [IgnoreDataMember]
        public virtual UCArea UCArea { get; set; }
        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaints { get; set; }
        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Complaint> Complaints1 { get; set; }
        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupportingComplaint> SupportingComplaints { get; set; }
        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voting> Votings { get; set; }
        [IgnoreDataMember]
        public virtual Role Role1 { get; set; }
        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Announcement1> Announcements { get; set; }
        [IgnoreDataMember]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Announcement1> Announcements1 { get; set; }
    }
}
