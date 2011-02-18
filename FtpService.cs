using System;
using System.Collections.Generic;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Client-side proxy for the "rustici.ftp.*" Hosted SCORM Engine web
    /// service methods.  
    /// </summary>
    public class FtpService 
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        public FtpService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        /// <summary>
        /// Creates a new FTP User
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userPass">User's Password</param>
        /// <param name="permissionDomain">permission domain for the new user</param>
        public void CreateUser(string userId, string userPass, string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("userid", userId);
            request.Parameters.Add("userpass", userPass);
            if (!String.IsNullOrEmpty(permissionDomain))
                request.Parameters.Add("pd", permissionDomain);
            request.CallService("rustici.ftp.createUser");
        }

        /// <summary>
        /// Creates a new FTP User with access to the default permission domain
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="userPass">User's Password</param>
        public void CreateUser(string userId, string userPass)
        {
            CreateUser(userId, userPass, null);
        }

        /// <summary>
        /// Delete an FTP User
        /// </summary>
        /// <param name="userId">User ID</param>
        public void DeleteUser(string userId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("userid", userId);
            request.CallService("rustici.ftp.deleteUser");
        }

        /// <summary>
        /// Create a new Permission Domain
        /// </summary>
        /// <param name="permissionDomain">Domain Name</param>
        public void CreatePermissionDomain(string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("pd", permissionDomain);
            request.CallService("rustici.ftp.createPermissionDomain");
        }

        /// <summary>
        /// Delete a Permission Domain
        /// </summary>
        /// <param name="permissionDomain">Domain Name</param>
        public void DeletePermissionDomain(string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("pd", permissionDomain);
            request.CallService("rustici.ftp.deletePermissionDomain");
        }

        /// <summary>
        /// Get a list of all of the configured AppId's permission domains
        /// </summary>
        /// <returns></returns>
        public List<string> GetDomainList()
        {
            ServiceRequest request = new ServiceRequest(configuration);
            XmlDocument response = request.CallService("rustici.ftp.getDomainList");

            // Map the response to a dictionary of name/value pairs
            List<string> result = new List<string>();
            foreach (XmlElement attrEl in response.GetElementsByTagName("domain"))
            {
                result.Add(attrEl.Attributes["id"].Value);
            }

            return result;
        }

        /// <summary>
        /// Associate a specific course with a particular permission domain
        /// </summary>
        /// <param name="courseId">Unique Course Identifier</param>
        /// <param name="permissionDomain">Domain Name</param>
        public void SetCourseDomain(string courseId, string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("pd", permissionDomain);
            request.CallService("rustici.ftp.setCourseDomain");
        }

        /// <summary>
        /// Associate a particular user with a Permission Domain
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="permissionDomain">Permission Domain</param>
        public void SetUserDomain(string userId, string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("userid", userId);
            request.Parameters.Add("pd", permissionDomain);
            request.CallService("rustici.ftp.setUserDomain");
        }


        /// <summary>
        /// Set the ftp password for the user with the given userId
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="oldPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        public void SetUserPassword(string userId, string oldPassword, string newPassword)
        {
            SetUserPassword(userId, oldPassword, newPassword, false);
        }

        /// <summary>
        /// Set the ftp password for the user with the given userId
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="oldPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        /// /// <param name="newPassword">Force password change</param>
        public void SetUserPassword(string userId, string oldPassword, string newPassword, bool force)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("userid", userId);
            request.Parameters.Add("oldpass", oldPassword);
            request.Parameters.Add("newpass", newPassword);
            request.Parameters.Add("force", force.ToString());
            request.CallService("rustici.ftp.setUserPassword");
        }

        // Others....
 
    }
}