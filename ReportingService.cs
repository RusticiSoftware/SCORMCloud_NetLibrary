using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Client-side proxy for the "rustici.reporting.*" Hosted SCORM Engine web
    /// service methods.  
    /// </summary>
    public class ReportingService
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public ReportingService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        public String GetReportageServiceUrl()
        {
            return this.configuration.ScormEngineServiceUrl.Replace("EngineWebServices", "");
        }

        public String GetReportageAuth(ReportageNavPermission navPermission, bool isAdmin)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("navpermission", navPermission.ToString().ToLower());
            request.Parameters.Add("admin", isAdmin.ToString().ToLower());
            XmlDocument response = request.CallService("rustici.reporting.getReportageAuth");
            XmlNode authNode = response.GetElementsByTagName("auth")[0];
            return authNode.InnerText;
        }
        
        /// <summary>
        /// Calling this method returns a URL which will authenticate and launch a Reportage session, starting
        /// at the specified Reportage URL entry point.
        /// </summary>
        /// <returns>A URL from which the export data can be downloaded</returns>
        public String GetReportUrl(String reportageAuth, String reportUrl)
        {
            ServiceRequest request = new ServiceRequest(configuration);
    	    //Relative path, auto add the right server name
    	    string fullReportUrl = reportUrl;
            if (reportUrl.StartsWith("/Reportage"))
            {
                fullReportUrl = "http://" + request.Server + reportUrl;
            }
            request.Parameters.Add("auth", reportageAuth);
            request.Parameters.Add("reporturl", fullReportUrl);
    	    return request.ConstructUrl("rustici.reporting.launchReport");
        }

        public String LaunchReportageUrl()
        {
            string reportAuth = this.GetReportageAuth(ReportageNavPermission.FREENAV, true);

            string reportUrl = ScormCloud.ReportingService.GetReportageServiceUrl() + "Reportage/reportage.php?appId=" + this.configuration.AppId;
            
            return this.GetReportUrl(reportAuth,reportUrl);
        }


    }
}
