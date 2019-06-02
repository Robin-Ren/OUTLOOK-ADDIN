using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OutlookAddin.Domain
{
    public class LoginEventArgs
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
        [JsonProperty("password")]
        public string Password { get; set; }
        [JsonProperty("devicePlatform")]
        public string DevicePlatform { get; set; } = "ANDROID";
        [JsonProperty("userTypeTag")]
        public string UserTypeTag { get; set; } = "RESIDENT";
        [JsonProperty("condoCode")]
        public string CondoCode { get; set; } = "HAC";
        [JsonProperty("rememberMe")]
        public int? RememberMe { get; set; } = 1;
    }
}
