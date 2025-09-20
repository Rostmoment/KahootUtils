using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KahootUtils
{
    public static class HelpfulMethods
    {
        public static DateTime FromUnixTime(long time)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(time).ToLocalTime();
        }
    }
}
