using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RusticiSoftware.HostedEngine.Client
{
    class CustomWebClient : WebClient
    {
        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            WebRequest req = base.GetWebRequest(address);
            //Wait 15 minutes for a response
            //(Timeout doesn't apply if uploading a file)
            req.Timeout = (15 * 60 * 1000);
            return req;
        }
    }
}
