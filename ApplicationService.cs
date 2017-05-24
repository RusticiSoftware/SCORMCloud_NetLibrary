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
using System.Collections.ObjectModel;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Client-side proxy for the "rustici.application.*" Hosted SCORM Engine web service methods.  
    /// </summary>
    public class ApplicationService
    {
        /// <summary>
        /// This is the ApplicationManagement AppId, not a normal API AppID
        /// </summary>
        private Configuration configuration = null;

        /// <summary>
        /// Main constructor that provides necessary Management AppID/Secret
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public ApplicationService(Configuration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Creates a new SCORM Cloud Application
        /// </summary>
        /// <param name="name"> A name or description for the new application</param>
        /// <returns>application info object</returns>
        public ApplicationInfo CreateApplication(string name)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("name", name);

            XmlDocument response = request.CallService("rustici.application.createApplication");
            return new ApplicationInfo((XmlElement)response.GetElementsByTagName("application").Item(0));
        }

        /// <summary>
        /// Get details of an Application
        /// </summary>
        /// <param name="appId"> Application appID to get info for</param>
        /// <returns>application info object</returns>
        public ApplicationInfo GetAppInfo(string appId)
        {
            if (string.IsNullOrEmpty(appId))
                throw new ApplicationException("UpdateApplication Invalid Parameters");

            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("childappid", appId);

            XmlDocument response = request.CallService("rustici.application.getAppInfo");
            return new ApplicationInfo((XmlElement)response.GetElementsByTagName("application").Item(0));
        }

        /// <summary>
        /// Update a SCORM Cloud Application
        /// </summary>
        /// <param name="appId"> Application appID to updte</param>
        /// <param name="name">New Name of app</param>
        /// <param name="allowDelete">Whether to allow delete operations on app</param>
        /// <returns>application info object</returns>
        public ApplicationInfo UpdateApplication(string appId, string name = null, bool? allowDelete = null)
        {
            if (string.IsNullOrEmpty(name) && !allowDelete.HasValue)
                throw new ApplicationException("UpdateApplication Invalid Parameters");

            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("childappid", appId);

            if (!string.IsNullOrEmpty(name))
                request.Parameters.Add("name", name);
            if (allowDelete.HasValue)
                request.Parameters.Add("allowdelete", allowDelete.Value ? "true" : "false");

            XmlDocument response = request.CallService("rustici.application.updateApplication");
            return new ApplicationInfo((XmlElement)response.GetElementsByTagName("application").Item(0));
        }

        /// <summary>
        /// Get a List of SCORM Cloud Applications
        /// </summary>
        /// <returns>List of application info objects</returns>
        public List<ApplicationInfo> GetAppList()
        {
            ServiceRequest request = new ServiceRequest(configuration);
            XmlDocument response = request.CallService("rustici.application.getAppList");
            return ApplicationInfo.parseListFromXml(response);
        }


    }
}
