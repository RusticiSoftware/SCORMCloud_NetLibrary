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
        /// Creates a new instance of an existing registration.  This essentially creates a
        /// fresh take of a course by the user. The latest version of the associated course
        /// will be used.
        /// </summary>
        /// <param name="registrationId">Unique Identifier for the registration</param>
        /// <returns>Instance ID of the newly created instance</returns>
        public int CreateNewInstance (string registrationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("regid", registrationId);
            XmlDocument response = request.CallService("rustici.registration.createNewInstance");

            XmlNodeList successNodes = response.GetElementsByTagName("success");
            return Convert.ToInt32(successNodes[0].Attributes["instanceid"].Value);
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
            return GetRegistrationList(regIdFilterRegex, courseIdFilterRegex, false);
        }

        /// <summary>
        /// Returns a list of registration id's along with their associated course
        /// </summary>
        /// <param name="regIdFilterRegex">Optional registration id filter</param>
        /// <param name="courseIdFilterRegex">Option course id filter</param>
        /// <returns></returns>
        public List<RegistrationData> GetRegistrationList(string regIdFilterRegex, string courseIdFilterRegex, bool courseIdIsExact)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            if (!String.IsNullOrEmpty(regIdFilterRegex))
                request.Parameters.Add("filter", regIdFilterRegex);
            if (!String.IsNullOrEmpty(courseIdFilterRegex)){
                request.Parameters.Add((courseIdIsExact ? "courseid" : "coursefilter"), courseIdFilterRegex);
            }
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
    }
}
