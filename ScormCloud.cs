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

using System;
using System.Collections.Generic;
using System.Text;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// This class exists for "dead simple" access to the scorm cloud, by
    /// providing a static namespace to a static instance of ScormEngineService.
    /// </summary>
    public class ScormCloud
    {
        protected static ScormEngineService _singleton;
        protected static ScormEngineService Service
        {
            get
            {
                if (_singleton == null) {
                    if (_configuration == null) {
                        throw new ApplicationException("Attempted to use ScormCloud object before setting the configuration!");
                    }
                    _singleton = new ScormEngineService(_configuration);
                }
                return _singleton;
            }
        }
        
        protected static Configuration _configuration;
        public static Configuration Configuration
        {
            get
            {
                return _configuration;
            }
            set
            {
                _configuration = value;
            }
        }

        /// <summary>
        /// Contains all SCORM Engine Package-level (i.e., course) functionality.
        /// </summary>
        public static CourseService CourseService
        {
            get { return Service.CourseService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Dispatch functionality.
        /// </summary>
        public static DispatchService DispatchService
        {
            get { return Service.DispatchService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Package-level (i.e., course) functionality.
        /// </summary>
        public static RegistrationService RegistrationService
        {
            get { return Service.RegistrationService; }
        }

        /// <summary>
        /// Contains all SCORM Cloud invitation functionality.
        /// </summary>
        public static InvitationService InvitationService
        {
            get { return Service.InvitationService; }
        }

        public static LrsAccountService LrsAccountService
        {
            get { return Service.LrsAccountService; }
        }


        /// <summary>
        /// Contains all SCORM Engine Upload/File Management functionality.
        /// </summary>
        public static UploadService UploadService
        {
            get { return Service.UploadService; }
        }

        /// <summary>
        /// Contains all SCORM Engine FTP Management functionality.
        /// </summary>
        public static FtpService FtpService
        {
            get { return Service.FtpService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Data Export functionality
        /// </summary>
        public static ExportService ExportService
        {
            get { return Service.ExportService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Reporting functionality
        /// </summary>
        public static ReportingService ReportingService
        {
            get { return Service.ReportingService; }
        }

        /// <summary>
        /// Contains all SCORM Engine Debug functionality
        /// </summary>
        public static DebugService DebugService
        {
            get { return Service.DebugService; }
        }

        /// <summary>
        /// The Application ID obtained by registering with the SCORM Engine Service
        /// </summary>
        public static string AppId
        {
            get
            {
                return Configuration.AppId;
            }
        }

        /// <summary>
        /// The security key (password) linked to the Application ID
        /// </summary>
        public static string SecurityKey
        {
            get
            {
                return Configuration.SecurityKey;
            }
        }

        /// <summary>
        /// URL to the service, ex: http://cloud.scorm.com/EngineWebServices
        /// </summary>
        public static string ScormEngineServiceUrl
        {
            get
            {
                return Configuration.ScormEngineServiceUrl;
            }
        }

        /// <summary>
        /// Create a new ScormCloud service request
        /// </summary>
        /// <returns></returns>
        public static ServiceRequest CreateNewRequest()
        {
            return new ServiceRequest(Configuration);
        }
    }
}
