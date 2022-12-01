using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Services
{
    public static class QueryService
    {

        public static string GetRequestMethod(string rawUrl) => rawUrl.TrimStart('/').Split('?')[0];

    }
}
