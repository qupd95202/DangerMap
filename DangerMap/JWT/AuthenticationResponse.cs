using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.JWT
{   
    /// <summary>
    /// JWT Model
    /// </summary>
    public class AuthenticationResponse
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
