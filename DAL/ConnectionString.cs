using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.DAL
{
    public static class ConnectionString
    {
        public static string Get()
        {
            return "Host=localhost;Username=postgres;Password=changeme;Database=simpledatastore";
        }
    }
}
