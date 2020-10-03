using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplianceTool.Model
{
    public class Account
    {
        public string userDetailId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string instituteName { get; set; }
        public string contactNumber { get; set; }
        public string businessEmail { get; set; }
    }
}
