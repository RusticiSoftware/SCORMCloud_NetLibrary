/* Software License Agreement (BSD License)
 * 
 * Copyright (c) 2010-2011, Rustici Software, LLC
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the <organization> nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL Rustici Software, LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */


namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// This class serves as the main Service Facade for all SCORM Engine functionality
    /// </summary>
    public class ScormEngineService
    {
        private Configuration configuration = null;
        private CourseService courseService = null;
        private DispatchService dispatchService = null;
        private RegistrationService registrationService = null;
        private TaggingService taggingService = null;
        private InvitationService invitationService = null;
        private LrsAccountService lrsAccountService = null;
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
	    /// <param name="origin">The origin string that defines the organization, application name and version</param>
        public ScormEngineService(string scormEngineServiceUrl, string appId, string securityKey, string origin) : 
            this(new Configuration(scormEngineServiceUrl, appId, securityKey, origin))
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
            dispatchService = new DispatchService(configuration, this);
            registrationService = new RegistrationService(configuration, this);
            taggingService = new TaggingService(configuration, this);
            invitationService = new InvitationService(configuration, this);
            lrsAccountService = new LrsAccountService(configuration, this);
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
        /// Contains all SCORM Dispatch functionality.
        /// </summary>
        public DispatchService DispatchService
        {
            get { return dispatchService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Package-level (i.e., course) functionality.
        /// </summary>
        public RegistrationService RegistrationService
        {
            get { return registrationService; }
        }

        /// <summary>
        /// Tagging functionality
        /// </summary>
        public TaggingService TaggingService
        {
            get { return taggingService; }
        }

        /// <summary>
        /// Contains all SCORM Cloud invitation managament functionality.
        /// </summary>
        public InvitationService InvitationService
        {
            get { return invitationService; }
        }
        public LrsAccountService LrsAccountService
        {
            get { return lrsAccountService; }
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
