using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Shared.Models.Auth
{
    public class AuthorizedUser
    {
        public long AccountNo { get; set; } = 0;

        public string AuthToken { get; set; } = string.Empty;
    }

    public class RequestLock
    { }
}
