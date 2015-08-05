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
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Client-side proxy for the "rustici.registration.*" Hosted SCORM Engine web
    /// service methods.  
    /// </summary>
    public class RegistrationService
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public RegistrationService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        /// <summary>
        /// Create a new Registration (Instance of a user taking a course)
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="versionId">Optional versionID, if Int32.MinValue, latest course version is used.</param>
        /// <param name="learnerId">Unique Identifier for the learner</param>
        /// <param name="learnerFirstName">Learner's first name</param>
        /// <param name="learnerLastName">Learner's last name</param>
        /// <param name="resultsPostbackUrl">URL to which the server will post results back to</param>
        /// <param name="authType">Type of Authentication used at results postback time</param>
        /// <param name="postBackLoginName">If postback authentication is used, the logon name</param>
        /// <param name="postBackLoginPassword">If postback authentication is used, the password</param>
        /// <param name="resultsFormat">The Format of the results XML sent to the postback URL</param>
        public void CreateRegistration(string registrationId, string courseId, int versionId, string learnerId,
            string learnerFirstName, string learnerLastName, string resultsPostbackUrl,
            RegistrationResultsAuthType authType, string postBackLoginName, string postBackLoginPassword,
            RegistrationResultsFormat resultsFormat)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("fname", learnerFirstName);
            request.Parameters.Add("lname", learnerLastName);
            request.Parameters.Add("learnerid", learnerId);

            // Required on this signature but not by the actual service
            request.Parameters.Add("authtype", Enum.GetName(authType.GetType(), authType).ToLower());
            request.Parameters.Add("resultsformat", Enum.GetName(resultsFormat.GetType(), resultsFormat).ToLower());

            // Optional:
            if (!String.IsNullOrEmpty(resultsPostbackUrl))
                request.Parameters.Add("postbackurl", resultsPostbackUrl);
            if (!String.IsNullOrEmpty(postBackLoginName))
                request.Parameters.Add("urlname", postBackLoginName);
            if (!String.IsNullOrEmpty(postBackLoginPassword))
                request.Parameters.Add("urlpass", postBackLoginPassword);
            if (versionId != Int32.MinValue)
                request.Parameters.Add("versionid", versionId);

            request.CallService("rustici.registration.createRegistration");
        }


        /// <summary>
        /// Semantics: This method provides a way to update the postback settings for a registration created with the createRegistration call. If you wish to change an authenticated postback to an unauthenticated postback, please call this method with only a url specified.
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="versionId">Optional versionID, if Int32.MinValue, latest course version is used.</param>
        /// <param name="learnerId">Unique Identifier for the learner</param>
        /// <param name="learnerFirstName">Learner's first name</param>
        /// <param name="learnerLastName">Learner's last name</param>
        /// <param name="resultsPostbackUrl">URL to which the server will post results back to</param>
        /// <param name="authType">Type of Authentication used at results postback time</param>
        /// <param name="postBackLoginName">If postback authentication is used, the logon name</param>
        /// <param name="postBackLoginPassword">If postback authentication is used, the password</param>
        /// <param name="resultsFormat">The Format of the results XML sent to the postback URL</param>
        public void UpdatePostbackInfo(string registrationId, string resultsPostbackUrl,
          string postBackLoginName, string postBackLoginPassword, RegistrationResultsAuthType authType,
          RegistrationResultsFormat resultsFormat)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.Parameters.Add("url", resultsPostbackUrl);

            // Required on this signature but not by the actual service
            request.Parameters.Add("authtype", Enum.GetName(authType.GetType(), authType).ToLower());
            request.Parameters.Add("resultsformat", Enum.GetName(resultsFormat.GetType(), resultsFormat).ToLower());

            // Optional:
            if (!String.IsNullOrEmpty(postBackLoginName))
            {
                request.Parameters.Add("name", postBackLoginName);
            }
            else
            {
                request.Parameters.Add("name", "PlaceholderName");
            }

            if (!String.IsNullOrEmpty(postBackLoginPassword))
            {
                request.Parameters.Add("password", postBackLoginPassword);
            }
            else
            {
                request.Parameters.Add("password", "PlaceholderPassword");
            }
            request.CallService("rustici.registration.updatePostbackInfo");
        }

        /// <summary>
        /// Semantics: This method provides a way to update the postback settings for a registration created with the createRegistration call. If you wish to change an authenticated postback to an unauthenticated postback, please call this method with only a url specified.
        /// </summary>
        /// <param name="registrationId"></param>
        /// <param name="resultsPostbackUrl"></param>
        /// <remarks>Assumes Registration Authorization Result is Http Basic, registration result format is full and that username and password are not requred.</remarks>
        public void UpdatePostbackInfo(string registrationId, string resultsPostbackUrl)
        {
            string postBackLoginName = String.Empty;
            string postBackLoginPassword = String.Empty;
            RegistrationResultsAuthType authType = RegistrationResultsAuthType.HTTPBASIC;
            RegistrationResultsFormat resultsFormat = RegistrationResultsFormat.FULL;

            UpdatePostbackInfo(registrationId, resultsPostbackUrl, postBackLoginName, postBackLoginPassword, authType, resultsFormat);
        }


        /// <summary>
        /// Create a new Registration (Instance of a user taking a course)
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="versionId">Optional versionID, if Int32.MinValue, latest course version is used.</param>
        /// <param name="learnerId">Unique Identifier for the learner</param>
        /// <param name="learnerFirstName">Learner's first name</param>
        /// <param name="learnerLastName">Learner's last name</param>
        /// <param name="resultsPostbackUrl">URL to which the server will post results back to</param>
        /// <param name="authType">Type of Authentication used at results postback time</param>
        /// <param name="postBackLoginName">If postback authentication is used, the logon name</param>
        /// <param name="postBackLoginPassword">If postback authentication is used, the password</param>
        /// <param name="resultsFormat">The Format of the results XML sent to the postback URL</param>
        /// <param name="learnerTags">A comma separated list of learner tags to associate with this registration</param>
        /// <param name="courseTags">A comma separated list of course tags to associate with this registration</param>
        /// <param name="registrationTags">A comma separated list of tags to associate with this registration</param>
        public void CreateRegistration(string registrationId, string courseId, int versionId, string learnerId,
          string learnerFirstName, string learnerLastName, string resultsPostbackUrl,
          RegistrationResultsAuthType authType, string postBackLoginName, string postBackLoginPassword,
          RegistrationResultsFormat resultsFormat,
          string learnerTags, string courseTags, string registrationTags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("fname", learnerFirstName);
            request.Parameters.Add("lname", learnerLastName);
            request.Parameters.Add("learnerid", learnerId);

            // Required on this signature but not by the actual service
            request.Parameters.Add("authtype", Enum.GetName(authType.GetType(), authType).ToLower());
            request.Parameters.Add("resultsformat", Enum.GetName(resultsFormat.GetType(), resultsFormat).ToLower());

            // Optional:
            if (!String.IsNullOrEmpty(resultsPostbackUrl))
                request.Parameters.Add("postbackurl", resultsPostbackUrl);
            if (!String.IsNullOrEmpty(postBackLoginName))
                request.Parameters.Add("urlname", postBackLoginName);
            if (!String.IsNullOrEmpty(postBackLoginPassword))
                request.Parameters.Add("urlpass", postBackLoginPassword);
            if (versionId != Int32.MinValue)
                request.Parameters.Add("versionid", versionId);

            // Optional tags
            if (!String.IsNullOrEmpty(learnerTags))
                request.Parameters.Add("learnerTags", learnerTags);
            if (!String.IsNullOrEmpty(courseTags))
                request.Parameters.Add("courseTags", courseTags);
            if (!String.IsNullOrEmpty(registrationTags))
                request.Parameters.Add("registrationTags", registrationTags);

            request.CallService("rustici.registration.createRegistration");
        }

        //TODO: Other overrides of createRegistration....

        /// <summary>
        /// Create a new Registration (Instance of a user taking a course)
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="learnerId">Unique Identifier for the learner</param>
        /// <param name="learnerFirstName">Learner's first name</param>
        /// <param name="learnerLastName">Learner's last name</param>
        public void CreateRegistration(string registrationId, string courseId, string learnerId,
            string learnerFirstName, string learnerLastName)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("fname", learnerFirstName);
            request.Parameters.Add("lname", learnerLastName);
            request.Parameters.Add("learnerid", learnerId);
            request.CallService("rustici.registration.createRegistration");
        }

        // <summary>
        /// Confirm that a registrationExists
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        public bool RegistrationExists(string registrationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            XmlDocument response = request.CallService("rustici.registration.exists");


            XmlElement attrEl = (XmlElement)response.GetElementsByTagName("result")[0];

            return Convert.ToBoolean(attrEl.InnerXml.ToString());
        }


        /// <summary>
        /// Create a new Registration (Instance of a user taking a course)
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="learnerId">Unique Identifier for the learner</param>
        /// <param name="learnerFirstName">Learner's first name</param>
        /// <param name="learnerLastName">Learner's last name</param>
        /// <param name="email">Learner's email</param>
        public void CreateRegistration(string registrationId, string courseId, string learnerId,
            string learnerFirstName, string learnerLastName, string email)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("fname", learnerFirstName);
            request.Parameters.Add("lname", learnerLastName);
            request.Parameters.Add("learnerid", learnerId);
            request.Parameters.Add("email", email);
            request.CallService("rustici.registration.createRegistration");
        }


        /// <summary>
        /// Create a new Registration (Instance of a user taking a course)
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="learnerId">Unique Identifier for the learner</param>
        /// <param name="learnerFirstName">Learner's first name</param>
        /// <param name="learnerLastName">Learner's last name</param>
        /// /// <param name="learnerTags">Learner Tags</param> "tag1|tag2|tag3"
        public void CreateRegistrationWithTags(string registrationId, string courseId, string learnerId,
            string learnerFirstName, string learnerLastName, string email, string registrationTags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("fname", learnerFirstName);
            request.Parameters.Add("lname", learnerLastName);
            request.Parameters.Add("email", email);
            request.Parameters.Add("learnerid", learnerId);

            // Optional tags
            if (!String.IsNullOrEmpty(registrationTags))
                request.Parameters.Add("registrationTags", registrationTags);

            request.CallService("rustici.registration.createRegistration");
        }

        /// <summary>
        /// Creates a new instance of an existing registration.  This essentially creates a
        /// fresh take of a course by the user. The latest version of the associated course
        /// will be used.
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <returns>Instance ID of the newly created instance</returns>
        public int CreateNewInstance(string registrationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            XmlDocument response = request.CallService("rustici.registration.createNewInstance");

            XmlNodeList successNodes = response.GetElementsByTagName("success");
            return Convert.ToInt32(successNodes[0].Attributes["instanceid"].Value);
        }


        /// <summary>
        /// Return a registration detail object for the given registration
        /// </summary>
        /// <param name="registrationId">The unique identifier of the registration</param>
        /// <returns></returns>
        public RegistrationData GetRegistrationDetail(string registrationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);

            request.Parameters.Add("regid", registrationId);

            XmlDocument response = request.CallService("rustici.registration.getRegistrationDetail");

            XmlElement reportElem = (XmlElement)response.GetElementsByTagName("registration")[0];

            return new RegistrationData(reportElem);

        }


        /// <summary>
        /// Return a registration summary object for the given registration
        /// </summary>
        /// <param name="registrationId">The unique identifier of the registration</param>
        /// <returns></returns>
        public RegistrationSummary GetRegistrationSummary(string registrationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.Parameters.Add("resultsformat", "course");
            request.Parameters.Add("dataformat", "xml");
            XmlDocument response = request.CallService("rustici.registration.getRegistrationResult");
            XmlElement reportElem = (XmlElement)response.GetElementsByTagName("registrationreport")[0];
            return new RegistrationSummary(reportElem);
        }

        /// <summary>
        /// Returns the current state of the registration, including completion
        /// and satisfaction type data.  Amount of detail depends on format parameter.
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="resultsFormat">Degree of detail to return</param>
        /// <returns>Registration data in XML Format</returns>
        public string GetRegistrationResult(string registrationId, RegistrationResultsFormat resultsFormat)
        {
            return GetRegistrationResult(registrationId, resultsFormat, DataFormat.XML);
        }

        /// <summary>
        /// Returns the current state of the registration, including completion
        /// and satisfaction type data.  Amount of detail depends on format parameter.
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="resultsFormat">Degree of detail to return</param>
        /// <returns>Registration data in XML Format</returns>
        public string GetRegistrationResult(string registrationId, RegistrationResultsFormat resultsFormat, DataFormat dataFormat)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.Parameters.Add("resultsformat", Enum.GetName(resultsFormat.GetType(), resultsFormat).ToLower());
            if (dataFormat == DataFormat.JSON)
                request.Parameters.Add("dataformat", "json");
            XmlDocument response = request.CallService("rustici.registration.getRegistrationResult");

            // Return the subset of the xml starting with the top <summary>
            return response.ChildNodes[1].InnerXml;
        }

        /// <summary>
        /// Returns a list of registration id's along with their associated course
        /// </summary>
        /// <param name="regIdFilterRegex">Optional registration id filter</param>
        /// <param name="courseIdFilterRegex">Option course id filter</param>
        /// <returns></returns>
        public List<RegistrationData> GetRegistrationList(string regIdFilterRegex, string courseIdFilterRegex)
        {
            return GetRegistrationList(regIdFilterRegex, courseIdFilterRegex, false, null, null);
        }

        /// <summary>
        /// Returns a list of registration id's along with their associated course
        /// </summary>
        /// <param name="regIdFilterRegex">Optional registration id filter</param>
        /// <param name="courseIdFilterRegex">Option course id filter</param>
        /// <returns></returns>
        public List<RegistrationData> GetRegistrationList(string regIdFilterRegex, string courseIdFilterRegex, bool courseIdIsExact)
        {
            return GetRegistrationList(regIdFilterRegex, courseIdFilterRegex, courseIdIsExact, null, null);
        }

        /// <summary>
        /// Returns a list of registration id's along with their associated course
        /// </summary>
        /// <param name="regIdFilterRegex">Optional registration id filter</param>
        /// <param name="courseIdFilterRegex">Option course id filter</param>
        /// <returns></returns>
        public List<RegistrationData> GetRegistrationList(string regIdFilterRegex, string courseIdFilterRegex, bool courseIdIsExact, DateTime? after, DateTime? until)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            if (!String.IsNullOrEmpty(regIdFilterRegex))
                request.Parameters.Add("filter", regIdFilterRegex);
            if (!String.IsNullOrEmpty(courseIdFilterRegex))
            {
                request.Parameters.Add((courseIdIsExact ? "courseid" : "coursefilter"), courseIdFilterRegex);
            }
            if (after != null)
                request.Parameters.Add("after", after.GetValueOrDefault().ToUniversalTime());
            if (until != null)
                request.Parameters.Add("until", until.GetValueOrDefault().ToUniversalTime());

            XmlDocument response = request.CallService("rustici.registration.getRegistrationList");

            // Return the subset of the xml starting with the top <summary>
            return RegistrationData.ConvertToRegistrationDataList(response);
        }

        /// <summary>
        /// Returns a list of registration id's along with their associated course
        /// </summary>
        /// <param name="regIdFilterRegex">Optional registration id filter</param>
        /// <param name="courseIdFilterRegex">Option course id filter</param>
        /// <returns></returns>
        public List<RegistrationData> GetRegistrationListForCourse(string courseId)
        {
            return GetRegistrationList(".*", courseId, true);
        }

        /// <summary>
        /// Returns a list of all registration id's along with their associated course
        /// </summary>
        public List<RegistrationData> GetRegistrationList()
        {
            return GetRegistrationList(null, null);
        }

        /// <summary>
        /// Delete the specified registration
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="deleteLatestInstanceOnly">If false, all instances are deleted</param>
        public void DeleteRegistration(string registrationId, bool deleteLatestInstanceOnly)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            if (deleteLatestInstanceOnly)
                request.Parameters.Add("instanceid", "latest");
            request.CallService("rustici.registration.deleteRegistration");
        }


        /// <summary>
        /// Resets all status data regarding the specified registration -- essentially restarts the course
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        public void ResetRegistration(string registrationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.CallService("rustici.registration.resetRegistration");
        }


        /// <summary>
        /// Clears global objective data for the given registration
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="deleteLatestInstanceOnly">If false, all instances are deleted</param>
        public void ResetGlobalObjectives(string registrationId, bool deleteLatestInstanceOnly)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            if (deleteLatestInstanceOnly)
                request.Parameters.Add("instanceid", "latest");
            request.CallService("rustici.registration.resetGlobalObjectives");
        }

        /// <summary>
        /// Delete the specified instance of the registration
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="instanceId">Specific instance of the registration to delete</param>
        public void DeleteRegistrationInstance(string registrationId, int instanceId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            request.Parameters.Add("instanceid", "instanceId");
            request.CallService("rustici.registration.deleteRegistration");
        }

        /// <summary>
        /// Gets the url to directly launch/view the course registration in a browser
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="redirectOnExitUrl">Upon exit, the url that the SCORM player will redirect to</param>
        /// <returns>URL to launch</returns>
        public string GetLaunchUrl(string registrationId, string redirectOnExitUrl)
        {
            return GetLaunchUrl(registrationId, redirectOnExitUrl, null, null);
        }

        /// <summary>
        /// Gets the url to directly launch/view the course registration in a browser
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="redirectOnExitUrl">Upon exit, the url that the SCORM player will redirect to</param>
        /// <param name="cssUrl">Absolute url that points to a custom player style sheet</param>
        /// <returns>URL to launch</returns>
        public string GetLaunchUrl(string registrationId, string redirectOnExitUrl, string cssUrl)
        {
            return GetLaunchUrl(registrationId, redirectOnExitUrl, cssUrl, null);
        }

        /// <summary>
        /// Gets the url to directly launch/view the course registration in a browser
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <param name="redirectOnExitUrl">Upon exit, the url that the SCORM player will redirect to</param>
        /// <returns>URL to launch</returns>
        /// <param name="debugLogPointerUrl">Url that the server will postback a "pointer" url regarding
        /// a saved debug log that resides on s3</param>
        public string GetLaunchUrl(string registrationId, string redirectOnExitUrl, string cssUrl, string debugLogPointerUrl)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            if (!String.IsNullOrEmpty(redirectOnExitUrl))
                request.Parameters.Add("redirecturl", redirectOnExitUrl);
            if (!String.IsNullOrEmpty(cssUrl))
                request.Parameters.Add("cssurl", cssUrl);
            if (!String.IsNullOrEmpty(debugLogPointerUrl))
                request.Parameters.Add("saveDebugLogPointerUrl", debugLogPointerUrl);

            return request.ConstructUrl("rustici.registration.launch");
        }

        /// <summary>
        /// Returns list of launch info objects, each of which describe a particular launch,
        /// but note, does not include the actual history log for the launch. To get launch
        /// info including the log, use GetLaunchInfo
        /// </summary>
        /// <param name="registrationId"></param>
        /// <returns></returns>
        public List<LaunchInfo> GetLaunchHistory(string registrationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            XmlDocument response = request.CallService("rustici.registration.getLaunchHistory");
            XmlElement launchHistory = ((XmlElement)response.GetElementsByTagName("launchhistory")[0]);
            return LaunchInfo.ConvertToLaunchInfoList(launchHistory);
        }

        /// <summary>
        /// Get the full launch information for the launch with the given launch id
        /// </summary>
        /// <param name="launchId"></param>
        /// <returns></returns>
        public LaunchInfo GetLaunchInfo(string launchId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("launchid", launchId);
            XmlDocument response = request.CallService("rustici.registration.getLaunchInfo");
            XmlElement launchInfoElem = ((XmlElement)response.GetElementsByTagName("launch")[0]);
            return new LaunchInfo(launchInfoElem);
        }

        /// <summary>
        ///  Get the postback attributes that were set in a createRegistration or updatePostbackInfo call.
        /// </summary>
        /// <param name="registrationId">Specifies the registration id</param>
        /// <returns>PostackInfo</returns>
        public PostbackInfo GetPostbackInfo(string registrationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            XmlDocument response = request.CallService("rustici.registration.getPostbackInfo");
            XmlElement postbackInfoElem = ((XmlElement)response.GetElementsByTagName("postbackinfo")[0]);
            return new PostbackInfo(postbackInfoElem);
        }

        /// <summary>
        /// This method provides a way to test a URL for posting registration results back to, as they would be posted when using the postbackurl in the createRegistration call. When called, an example registration result will be posted to the URL given, or else an error will be reported regarding why the post failed.
        /// </summary>
        /// <param name="postbackUrl">Specifies a URL for which to post activity and status data in real time as the course is completed</param>
        /// <returns>Rsp containing result and status code</returns>
        public Rsp TestRegistrationPostUrl(string postbackUrl)
        {
            return TestRegistrationPostUrl(postbackUrl, RegistrationResultsAuthType.HTTPBASIC, "placeholderUrlName", "placeholderUrlPassword", RegistrationResultsFormat.ACTIVITY);
        }

        /// <summary>
        /// This method provides a way to test a URL for posting registration results back to, as they would be posted when using the postbackurl in the createRegistration call. When called, an example registration result will be posted to the URL given, or else an error will be reported regarding why the post failed.
        /// </summary>
        /// <param name="postbackUrl">Specifies a URL for which to post activity and status data in real time as the course is completed</param>
        /// <param name="authType">Optional parameter to specify how to authorize against the given postbackurl, can be "form" or "httpbasic". If form authentication, the username and password for authentication are submitted as form fields "username" and "password", and the registration data as the form field "data". If httpbasic authentication is used, the username and password are placed in the standard Authorization HTTP header, and the registration data is the body of the message (sent as text/xml content type). This field is set to "form" by default.</param>
        /// <param name="urlname">You can optionally specify a login name to be used for credentials when posting to the URL specified in postbackurl</param>
        /// <param name="urlpass">If credentials for the postbackurl are provided, this must be included, it is the password to be used in authorizing the postback of data to the URL specified by postbackurl</param>
        /// <param name="resultsFormat">This parameter allows you to specify a level of detail in the information that is posted back while the course is being taken. It may be one of three values: "course" (course summary), "activity" (activity summary, or "full" (full detail), and is set to "course" by default. The information will be posted as xml, and the format of that xml is specified below under the method "getRegistrationResult"</param>
        /// <returns>Rsp containing result and status code</returns>
        public Rsp TestRegistrationPostUrl(string postbackUrl, RegistrationResultsAuthType authType, string urlname, string urlpass, RegistrationResultsFormat resultsFormat)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("postbackurl", postbackUrl);

            //Api call will fail without a placeholder username and password at least:
            if (!String.IsNullOrEmpty(urlname))
            {
                request.Parameters.Add("name", urlname);
            }
            else
            {
                request.Parameters.Add("name", "placeholderLoginName");
            }

            if (!String.IsNullOrEmpty(urlpass))
            {
                request.Parameters.Add("password", urlpass);
            }
            else
            {
                request.Parameters.Add("password", "placeholderLoginPassword");
            }

            request.Parameters.Add("authtype", Enum.GetName(authType.GetType(), authType).ToLower());
            request.Parameters.Add("resultsformat", Enum.GetName(resultsFormat.GetType(), resultsFormat).ToLower());

            XmlDocument response = request.CallService("rustici.registration.testRegistrationPostUrl");
            XmlElement rspElement = ((XmlElement)response.GetElementsByTagName("rsp")[0]);
            return new Rsp(rspElement);
        }
    }
}