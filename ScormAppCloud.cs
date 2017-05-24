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
    /// This is a non static ScormEngineService when multiple apps are required with a single client
    /// </summary>
    public class ScormAppCloud
    {
        public ScormEngineService Service { get; }
        private Configuration _configuration;

        public ScormAppCloud(Configuration configuration)
        {
            _configuration = configuration;
            Service = new ScormEngineService(_configuration);
        }
            
        /// <summary>
        /// The Application ID obtained by registering with the SCORM Engine Service
        /// </summary>
        public string AppId => _configuration.AppId;

        /// <summary>
        /// The security key (password) linked to the Application ID
        /// </summary>
        public string SecurityKey => _configuration.SecurityKey;

        /// <summary>
        /// URL to the service, ex: http://cloud.scorm.com/EngineWebServices
        /// </summary>
        public string ScormEngineServiceUrl => _configuration.ScormEngineServiceUrl;

        /// <summary>
        /// Create a new ScormCloud service request
        /// </summary>
        /// <returns></returns>
        public ServiceRequest CreateNewRequest()
        {
            return new ServiceRequest(_configuration);
        }

    }
}
