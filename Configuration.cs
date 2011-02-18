
namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// SCORM Engine Service configuration settings
    /// </summary>
    public class Configuration
    {
        private string appId = null;
        private string securityKey = null;
        private string scormEngineServiceUrl = null;

        /// <summary>
        /// Single constuctor that contains the required properties
        /// </summary>
        /// <param name="scormEngineServiceUrl">URL to the service, ex: http://services.scorm.com/EngineWebServices</param>
        /// <param name="appId">The Application ID obtained by registering with the SCORM Engine Service</param>
        /// <param name="securityKey">The security key (password) linked to the application ID</param>
        public Configuration(string scormEngineServiceUrl, string appId, string securityKey)
        {
            this.appId = appId;
            this.securityKey = securityKey;
            this.scormEngineServiceUrl = scormEngineServiceUrl;
        }

        /// <summary>
        /// The Application ID obtained by registering with the SCORM Engine Service
        /// </summary>
        public string AppId
        {
            get { return appId; }
            set { appId = value; }
        }

        /// <summary>
        /// The security key (password) linked to the application ID
        /// </summary>
        public string SecurityKey
        {
            get { return securityKey; }
            set { securityKey = value; }
        }

        /// <summary>
        /// URL to the service, ex: http://services.scorm.com/EngineWebServices
        /// </summary>
        public string ScormEngineServiceUrl
        {
            get { return scormEngineServiceUrl; }
            set { scormEngineServiceUrl = value; }
        }
    }
}
