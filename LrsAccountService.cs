using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.HostedEngine.Client
{
    public class LrsAccountService
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public LrsAccountService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        public void SetAppLrsAuthCallbackUrl(String lrsAuthCallbackUrl)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("lrsAuthCallbackUrl", lrsAuthCallbackUrl);

            request.CallService("rustici.lrsaccount.setAppLrsAuthCallbackUrl");
        }
    }
}
