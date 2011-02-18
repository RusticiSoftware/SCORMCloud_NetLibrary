using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Client-side proxy for the "rustici.debug.*" Hosted SCORM Engine web
    /// service methods.  
    /// </summary>
    public class DebugService
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public DebugService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        /// <summary>
        /// Check for a basic ping to the cloud server to ensure a successful connection
        /// </summary>
        /// <returns></returns>
        public bool CloudPing()
        {
            ServiceRequest request = new ServiceRequest(configuration);
            XmlDocument response = request.CallService("rustici.debug.ping");
            return response.DocumentElement.Attributes["stat"].Value.Equals("ok");
        }

        /// <summary>
        /// Check for an Authenticated ping to the cloud server to ensure valid credentials
        /// </summary>
        /// <returns></returns>
        public bool CloudAuthPing()
        {
            ServiceRequest request = new ServiceRequest(configuration);
            XmlDocument response = request.CallService("rustici.debug.authPing");
            return response.DocumentElement.Attributes["stat"].Value.Equals("ok");
        }


    }
}
