using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Problem1.Models
{
    public class ResponseOfUser
    {
        public UserData Data { get; set; }
        public int? Error { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}