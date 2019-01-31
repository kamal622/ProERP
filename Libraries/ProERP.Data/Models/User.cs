using System;
using System.Collections.Generic;
using ProERP.Core;

namespace ProERP.Data.Models
{
    public partial class User : BaseEntity
    {
        public User()
        {
            this.BreakDowns = new List<BreakDown>();
            this.BreakDowns1 = new List<BreakDown>();
            this.DocumentHistories = new List<DocumentHistory>();
            this.Documents = new List<Document>();
            this.IndentDetails = new List<IndentDetail>();
            this.IndentDetails1 = new List<IndentDetail>();
            this.IndentDetails2 = new List<IndentDetail>();
            this.Indents = new List<Indent>();
            this.LineMachineActiveHistories = new List<LineMachineActiveHistory>();
            this.MaintenanceUserAssignments = new List<MaintenanceUserAssignment>();
            this.PreventiveHoldHistories = new List<PreventiveHoldHistory>();
            this.PreventiveReviewHistories = new List<PreventiveReviewHistory>();
            this.UserAssignments = new List<UserAssignment>();
            this.UserClaims = new List<UserClaim>();
            this.UserLogins = new List<UserLogin>();
            this.UserRoles = new List<UserRole>();
        }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public Nullable<int> UserProfile_Id { get; set; }
        public virtual ICollection<BreakDown> BreakDowns { get; set; }
        public virtual ICollection<BreakDown> BreakDowns1 { get; set; }
        public virtual ICollection<DocumentHistory> DocumentHistories { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails1 { get; set; }
        public virtual ICollection<IndentDetail> IndentDetails2 { get; set; }
        public virtual ICollection<Indent> Indents { get; set; }
        public virtual ICollection<LineMachineActiveHistory> LineMachineActiveHistories { get; set; }
        public virtual ICollection<MaintenanceUserAssignment> MaintenanceUserAssignments { get; set; }
        public virtual ICollection<PreventiveHoldHistory> PreventiveHoldHistories { get; set; }
        public virtual ICollection<PreventiveReviewHistory> PreventiveReviewHistories { get; set; }
        public virtual ICollection<UserAssignment> UserAssignments { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
