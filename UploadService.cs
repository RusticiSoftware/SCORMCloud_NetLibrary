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
    /// Client-side proxy for the "rustici.upload.*" Hosted SCORM Engine web
    /// service methods.  
    /// </summary>
    public class UploadService 
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public UploadService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        /// <summary>
        /// Retrieve upload token, which must be acquired and pass to a subsequent call to UploadFile
        /// </summary>
        /// <returns>An upload token which can be used in a call to UploadFile</returns>
        public UploadToken GetUploadToken()
        {
            ServiceRequest request = new ServiceRequest(configuration);
            XmlDocument response = request.CallService("rustici.upload.getUploadToken");
            return new UploadToken(response);
        }

        /// <summary>
        /// Upload a file to your Scorm Cloud upload space
        /// </summary>
        /// <param name="absoluteFilePathToZip">Absolute local path to file to be uploaded</param>
        /// <param name="permissionDomain">The upload "permission domain" under which to upload the file</param>
        /// <returns>A relative path to the file uploaded, which should be used in a subsequent call to ImportCourse or VersionCourse</returns>
        public UploadResult UploadFile(string absoluteFilePathToZip, string permissionDomain)
        {
            UploadToken token = GetUploadToken();
            return UploadFile(absoluteFilePathToZip, permissionDomain, token);
        }

        /// <summary>
        /// Upload a file to your Scorm Cloud upload space
        /// </summary>
        /// <param name="absoluteFilePathToZip">Absolute local path to file to be uploaded</param>
        /// <param name="permissionDomain">The upload "permission domain" under which to upload the file</param>
        /// <param name="token">A previously acquired upload token to be used for this upload request</param>
        /// <returns>A relative path to the file uploaded, which should be used in a subsequent call to ImportCourse or VersionCourse</returns>
        public UploadResult UploadFile(string absoluteFilePathToZip, string permissionDomain, UploadToken token)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.FileToPost = absoluteFilePathToZip;

            request.Parameters.Add("token", token.tokenId);

            //Forget about backend server, just upload through main domain,
            //as it should be fine, and required if using SSL
            //request.Server = token.server;

            if (!String.IsNullOrEmpty(permissionDomain)) {
                request.Parameters.Add("pd", permissionDomain);
            }

            XmlDocument response = request.CallService("rustici.upload.uploadFile");
            XmlElement location = (XmlElement)response.GetElementsByTagName("location")[0];
            
            UploadResult result = new UploadResult();
            result.server = token.server;
            result.location = location.InnerText;
            return result;
        }

        /// <summary>
        /// Return a url that can be embedded in the action attribute of a form element
        /// </summary>
        /// <param name="redirectUrl">The url to redirect to after the upload is complete</param>
        public String GetUploadUrl(String redirectUrl)
        {
            return GetUploadUrl(redirectUrl, null);
        }

        /// <summary>
        /// Return a url that can be embedded in the action attribute of a form element
        /// </summary>
        /// <param name="redirectUrl">The url to redirect to after the upload is complete</param>
        /// <param name="permissionDomain">The permission domain in which the upload should be placed</param>
        public String GetUploadUrl(String redirectUrl, String permissionDomain)
        {
            return GetUploadUrl(redirectUrl, permissionDomain, GetUploadToken());
        }

        /// <summary>
        /// Return a url that can be embedded in the action attribute of a form element
        /// </summary>
        /// <param name="redirectUrl">The url to redirect to after the upload is complete</param>
        /// <param name="permissionDomain">The permission domain in which the upload should be placed</param>
        public String GetUploadUrl(String redirectUrl, String permissionDomain, UploadToken token)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("token", token.tokenId);

            //Forget about backend server, just upload through main domain,
            //as it should be fine, and required if using SSL
            //request.Server = token.server;

            if (!String.IsNullOrEmpty(permissionDomain)) {
                request.Parameters.Add("pd", permissionDomain);
            }
            if (!String.IsNullOrEmpty(redirectUrl)) {
                request.Parameters.Add("redirecturl", redirectUrl);
            }
            return request.ConstructUrl("rustici.upload.uploadFile");
        }

        /// <summary>
        /// Retrieve a list of high-level data about all files owned by the 
        /// configured appId.
        /// </summary>
        /// <returns>List of Course Data objects</returns>
        public List<FileData> GetFileList()
        {
            return GetFileList(null);
        }

        /// <summary>
        /// Retrieve a list of high-level data about all files owned by the 
        /// configured appId.
        /// </summary>
        /// <returns>List of Course Data objects</returns>
        public List<FileData> GetFileList(string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            if (!String.IsNullOrEmpty(permissionDomain))
                request.Parameters.Add("pd", permissionDomain);
            XmlDocument response = request.CallService("rustici.upload.listFiles");
            return FileData.ConvertToFileDataList(response);
        }

        /// <summary>
        /// Delete the specified files given filenames only (not full path)
        /// </summary>
        /// <returns>List of Course Data objects</returns>
        public void DeleteFileList(Collection<string> fileNames, string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            if (!String.IsNullOrEmpty(permissionDomain))
                request.Parameters.Add("pd", permissionDomain);

            foreach (string file in fileNames)
            {
                request.Parameters.Add("file", file);
            }

            request.CallService("rustici.upload.deleteFiles");
        }

        /// <summary>
        /// Delete the specified files given filenames only (not full path)
        /// from the default domain
        /// </summary>
        /// <returns>List of Course Data objects</returns>
        public void DeleteFileList(Collection<string> fileNames)
        {
            DeleteFileList(fileNames, null);
        }

        /// <summary>
        /// Delete the specified file from the permission domain
        /// </summary>
        /// <returns>List of Course Data objects</returns>
        public void DeleteFile(string fileName, string permissionDomain)
        {
            Collection<string> fileNames = new Collection<string>();
            fileNames.Add(fileName);
            DeleteFileList(fileNames, permissionDomain);
        }

        /// <summary>
        /// Delete the specified file from the default domain
        /// </summary>
        /// <returns>List of Course Data objects</returns>
        public void DeleteFile(string filePath)
        {
            String domain = null;
            String fileName = filePath;

            String[] parts = filePath.Split('/');
            if (parts.Length > 1) {
                domain = parts[0];
                fileName = parts[1];
            }

            DeleteFile(fileName, domain);
        }
 
    }
}