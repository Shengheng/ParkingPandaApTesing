using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Problem1.Models
{
    public class UpdateUserData
    {
        public string Password { get; set; }
        public string CurrentPassword { get; set; }
        public bool ReceiveSmsNotifications { get; set; }
        public bool ReceiveEmail { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

    }
}