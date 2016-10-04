using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Problem1.Models
{
    public class UserData
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public bool ReceiveEmail { get; set; }
        public bool ReceiveSmsNotifications { get; set; }

        public string ApiPassword { get; set; }
        public string ApiPasswordCreateData { get; set; }
        public string ApiPasswordLastUsedData { get; set; }

        public string BraintreeClientToken { get; }
        public decimal CreditBalance { get; }
        public string CreateDate { get; }
        public string FacebookId { get; }
        public bool HasNeverSetPassword { get; }
        public string InviteCode { get; }
        public decimal InviteCreditAmount { get; }
        public bool IsExemptFromTracking { get; set; }
        public int LocationCount { get; }
        public bool MustChangePassword { get; }
        public bool MustSetMoreSecurePassword { get; }
 
        public ICollection<CreditCards> CreditCards { get; set; }
        public ICollection<PromotionOptIns> PromotionOptIns { get; set; }
    }
}