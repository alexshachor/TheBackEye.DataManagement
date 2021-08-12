using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataManagement.Api
{
    public interface IJwtAuth
    {
        public string Authentication(string username, string password);
    }
}
