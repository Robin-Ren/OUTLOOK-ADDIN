using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutlookAddin.Domain
{
    public class AuthenticationResult
    {
        public List<Tenant> tenants { get; set; }
        public List<string> roles { get; set; }
        public string usertype { get; set; }
        public List<Condo> condos { get; set; }
        public Device device { get; set; }
        public Account account { get; set; }
        public Authentication authentication { get; set; }
    }
}
