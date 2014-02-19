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
    /// Client-side proxy for the "rustici.registration.*" Hosted SCORM Engine web
    /// service methods.
    /// </summary>
    public class TaggingService
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public TaggingService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        /* LEARNER RELATED */
        public string SetLearnerTags(string learnerID, string learnerTags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("learnerid", learnerID);
            request.Parameters.Add("tags", learnerTags);
            XmlDocument response = request.CallService("rustici.tagging.setLearnerTags");
            return response.InnerXml;
        }
        public string GetLearnerTags(string learnerID)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("learnerid", learnerID);
            XmlDocument response = request.CallService("rustici.tagging.getLearnerTags");
            return response.FirstChild.InnerText;
        }
        public string AddOrRemoveLearnerTag(string learnerID, string learnerTag, string ADD_or_REMOVE)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("learnerid", learnerID);
            request.Parameters.Add("tag", learnerTag);

            string serviceToCall = "addLearnerTag";
            if (ADD_or_REMOVE == "REMOVE") serviceToCall = "removeLearnerTag";
            XmlDocument response = request.CallService("rustici.tagging." + serviceToCall);

            return response.InnerXml;
        }



        /* REGISTRATION RELATED */
        public string SetRegistrationTags(string regID, string regTags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", regID);
            request.Parameters.Add("tags", regTags);
            XmlDocument response = request.CallService("rustici.tagging.setRegistrationTags");
            return response.InnerXml;
        }

        public string AddOrRemoveRegistrationTags(string regID, string regTag, string ADD_or_REMOVE)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", regID);
            request.Parameters.Add("tags", regTag);

            string serviceToCall = "addRegistrationTag";
            if (ADD_or_REMOVE == "REMOVE") serviceToCall = "removeRegistrationTag";
            XmlDocument response = request.CallService("rustici.tagging." + serviceToCall);

            return response.InnerXml;
        }
        public string GetRegistrationTags(string regID)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", regID);
            XmlDocument response = request.CallService("rustici.tagging.removeRegistrationTag");
            return response.InnerXml;
        }


        /* COURSE RELATED */
        public string SetCourseTags(string courseID, string regTags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseID", courseID);
            request.Parameters.Add("tags", regTags);
            XmlDocument response = request.CallService("rustici.tagging.setCourseTags");
            return response.InnerXml;
        }

        public string AddOrRemoveCourseTags(string courseID, string regTag, string ADD_or_REMOVE)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseID", courseID);
            request.Parameters.Add("tags", regTag);

            string serviceToCall = "addCourseTag";
            if (ADD_or_REMOVE == "REMOVE") serviceToCall = "removeCourseTag";
            XmlDocument response = request.CallService("rustici.tagging." + serviceToCall);

            return response.InnerXml;
        }
        public string GetCourseTags(string courseID)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseID", courseID);
            XmlDocument response = request.CallService("rustici.tagging.removeRegistrationTag");
            return response.InnerXml;
        }



    }
}
