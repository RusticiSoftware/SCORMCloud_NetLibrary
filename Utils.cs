using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.HostedEngine.Client
{
    public class Utils
    {
        private static string isoDateFormat = "yyyy-MM-dd'T'HH:mm:ssZ";
        public static DateTime ParseIsoDate(String isoDate)
        {
            return DateTime.Parse(isoDate);
        }
        public static string GetIsoDateString(DateTime date)
        {
            return date.ToString(isoDateFormat);
        }
    }
}
