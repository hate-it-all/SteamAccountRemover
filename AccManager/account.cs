using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccManager
{
    class account
    {
        public string Name { get; set; }
        public string AccountID { get; set; }
        public string Steam64ID { get; set; }
        public Dictionary<string, bool> Places { get; set; } = new Dictionary<string, bool> { { "Userdata", false }, { "Loginusers", false }, { "Registry", false } };
    }
}
