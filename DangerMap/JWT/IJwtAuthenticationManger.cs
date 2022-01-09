using DangerMap.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DangerMap.JWT
{
    public interface IJwtAuthenticationManger
    {
        public AuthenticationResponse Authenticate(AccountLoginDto idPassword);
        public AuthenticationResponse Refresh(AuthenticationResponse refreshCred);
    }
}
