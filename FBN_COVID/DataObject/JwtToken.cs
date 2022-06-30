using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FBN_COVID.DataObject
{
    public class JwtToken
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int TokenExpiry { get; set; }
    }
}
