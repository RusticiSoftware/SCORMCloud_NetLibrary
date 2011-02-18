
namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// This class serves as the main Service Facade for all SCORM Engine functionality
    /// </summary>
    public class ScormEngineService
    {
        private Configuration configuration = null;
        private CourseService courseService = null;
        private RegistrationService registrationService = null;
        private UploadService uploadService = null;
        private FtpService ftpService = null;
        private ExportService exportService = null;
        private ReportingService reportingService = null;
        private DebugService debugService = null;

        /// <summary>
        /// SCORM Engine Service constructor that that takes the three required properties.
        /// </summary>
        /// <param name="scormEngineServiceUrl">URL to the service, ex: http://services.scorm.com/EngineWebServices</param>
        /// <param name="appId">The Application ID obtained by registering with the SCORM Engine Service</param>
        /// <param name="securityKey">The security key (password) linked to the application ID</param>
        public ScormEngineService(string scormEngineServiceUrl, string appId, string securityKey) : 
            this(new Configuration(scormEngineServiceUrl, appId, securityKey))
        {
        }

        /// <summary>
        /// SCORM Engine Service constructor that takes a single configuration parameter
        /// </summary>
        /// <param name="config">The Configuration object to be used to configure the Scorm Engine Service client</param>
        public ScormEngineService(Configuration config)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            configuration = config;
            courseService = new CourseService(configuration, this);
            registrationService = new RegistrationService(configuration, this);
            uploadService = new UploadService(configuration, this);
            ftpService = new FtpService(configuration, this);
            exportService = new ExportService(configuration, this);
            reportingService = new ReportingService(configuration, this);
            debugService = new DebugService(configuration, this);
        }

        /// <summary>
        /// Contains all SCORM Engine Package-level (i.e., course) functionality.
        /// </summary>
        public CourseService CourseService
        {
            get { return courseService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Package-level (i.e., course) functionality.
        /// </summary>
        public RegistrationService RegistrationService
        {
            get { return registrationService; }
        }


        /// <summary>
        /// Contains all SCORM Engine Upload/File Management functionality.
        /// </summary>
        public UploadService UploadService
        {
            get { return uploadService; }
        }

        /// <summary>
        /// Contains all SCORM Engine FTP Management functionality.
        /// </summary>
        public FtpService FtpService
        {
            get { return ftpService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Data Export functionality
        /// </summary>
        public ExportService ExportService
        {
            get { return exportService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Reporting functionality
        /// </summary>
        public ReportingService ReportingService
        {
            get { return reportingService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Debug functionality
        /// </summary>
        public DebugService DebugService
        {
            get { return debugService; }
        }

        /// <summary>
        /// The Application ID obtained by registering with the SCORM Engine Service
        /// </summary>
        public string AppId
        {
            get
            {
                return configuration.AppId;
            }
        }

        /// <summary>
        /// The security key (password) linked to the Application ID
        /// </summary>
        public string SecurityKey
        {
            get
            {
                return configuration.SecurityKey;
            }
        }

        /// <summary>
        /// URL to the service, ex: http://cloud.scorm.com/EngineWebServices
        /// </summary>
        public string ScormEngineServiceUrl
        {
            get
            {
                return configuration.ScormEngineServiceUrl;
            }
        }

        public ServiceRequest CreateNewRequest()
        {
            return new ServiceRequest(this.configuration);
        }
    }
}
