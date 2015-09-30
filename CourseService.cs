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
using System.Web;
using System.Xml;
using System.Threading;
using System.IO;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Client-side proxy for the "rustici.course.*" Hosted SCORM Engine web
    /// service methods.  
    /// </summary>
    public class CourseService 
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public CourseService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        /// <summary>
        /// Import a SCORM .pif (zip file) from the local filesystem.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="absoluteFilePathToZip">Full path to the .zip file</param>
        /// <returns>List of Import Results</returns>
        public List<ImportResult> ImportCourse(string courseId, string absoluteFilePathToZip)
        {
            return ImportCourse(courseId, absoluteFilePathToZip, null);
        }

        /// <summary>
        /// Import a SCORM .pif (zip file) from the local filesystem.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="absoluteFilePathToZip">Full path to the .zip file</param>
        /// <param name="itemIdToImport">ID of manifest item to import</param>
        /// <returns>List of Import Results</returns>
        public List<ImportResult> ImportCourse(string courseId, string absoluteFilePathToZip, string itemIdToImport)
        {
            return ImportCourse(courseId, absoluteFilePathToZip, itemIdToImport, null);
        }

        /// <summary>
        /// Import a SCORM .pif (zip file) from the local filesystem.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="absoluteFilePathToZip">Full path to the .zip file</param>
        /// <param name="itemIdToImport">ID of manifest item to import. If null, root organization is imported</param>
        /// <param name="permissionDomain">An permission domain to associate this course with, 
        /// for ftp access service (see ftp service below). 
        /// If the domain specified does not exist, the course will be placed in the default permission domain</param>
        /// <returns>List of Import Results</returns>
        public List<ImportResult> ImportCourse(string courseId, string absoluteFilePathToZip,
            string itemIdToImport, string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (!String.IsNullOrEmpty(itemIdToImport))
                request.Parameters.Add("itemid", itemIdToImport);
            if (!String.IsNullOrEmpty(itemIdToImport))
                request.Parameters.Add("pd", permissionDomain);
            request.FileToPost = absoluteFilePathToZip;
            XmlDocument response = request.CallService("rustici.course.importCourse");
            return ImportResult.ConvertToImportResults(response);

        }

        /// <summary>
        /// Import a SCORM .pif (zip file) asynchronously from the local filesystem.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="absoluteFilePathToZip">Full path to the .zip file</param>
        /// <param name="itemIdToImport">ID of manifest item to import. If null, root organization is imported</param>
        /// <param name="permissionDomain">A permission domain to associate this course with, 
        /// for ftp access service (see ftp service below). 
        /// If the domain specified does not exist, the course will be placed in the default permission domain</param>
        /// <returns>Token ID of Asynchronous Import</returns>
        public String ImportCourseAsync(string courseId, string absoluteFilePathToZip,
            string itemIdToImport, string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (!String.IsNullOrEmpty(itemIdToImport))
                request.Parameters.Add("itemid", itemIdToImport);
            if (!String.IsNullOrEmpty(itemIdToImport))
                request.Parameters.Add("pd", permissionDomain);
            request.FileToPost = absoluteFilePathToZip;
            XmlDocument response = request.CallService("rustici.course.importCourseAsync");
            String tokenId = ((XmlElement)response
                        .GetElementsByTagName("token")[0])
                        .FirstChild.InnerText;
            return tokenId;
        }

        /// <summary>
        /// Get a URL to target a file import form for importing a course to the SCORM Cloud.
        /// </summary>
        /// <param name="courseId">the desired id for the course</param>
        /// <param name="redirectUrl">the url for the browser to be redirected to after import</param>
        /// <returns></returns>
        public String GetImportCourseUrl(string courseId, string redirectUrl)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (!String.IsNullOrEmpty(redirectUrl))
                request.Parameters.Add("redirecturl", redirectUrl);
            return request.ConstructUrl("rustici.course.importCourse");
        }
        

        /// <summary>
        /// Import a SCORM .pif (zip file) from an existing .zip file on the
        /// Hosted SCORM Engine server.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="location">The relative path (rooted at your specific appid's upload area) 
        /// where the zip file for importing can be found</param>
        /// <returns>List of Import Results</returns>
        public List<ImportResult> ImportUploadedCourse(string courseId, string location)
	    {
            return ImportUploadedCourse(courseId, location, null, null);
	    }

        /// <summary>
        /// Import a SCORM .pif (zip file) from an existing .zip file on the
        /// Hosted SCORM Engine server.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="path">The relative path (rooted at your specific appid's upload area)
        /// where the zip file for importing can be found</param>
        /// <param name="itemIdToImport">ID of manifest item to import</param>
        /// <returns>List of Import Results</returns>
        public List<ImportResult> ImportUploadedCourse(string courseId, string path, string itemIdToImport)
        {
            return ImportUploadedCourse(courseId, path, itemIdToImport, null);
        }

        /// <summary>
        /// Import a SCORM .pif (zip file) from an existing .zip file on the
        /// Hosted SCORM Engine server.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="path">The relative path (rooted at your specific appid's upload area)
        /// where the zip file for importing can be found</param>
        /// <param name="itemIdToImport">ID of manifest item to import</param>
        /// <param name="permissionDomain">An permission domain to associate this course with, 
        /// for ftp access service (see ftp service below). 
        /// If the domain specified does not exist, the course will be placed in the default permission domain</param>
        /// <param name="server">A specific server to perform the import on, typically this is only specified
        /// by the server in an UploadResult, returned by UploadFile</param>
        /// <returns>List of Import Results</returns>
        public List<ImportResult> ImportUploadedCourse(string courseId, string path, string itemIdToImport, string permissionDomain)
        {
            return ImportUploadedCourse(courseId, path, itemIdToImport, permissionDomain, null, true);
        }

        /// <summary>
        /// Import a SCORM .pif (zip file) from an existing .zip file on the
        /// Hosted SCORM Engine server.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="path">The relative path (rooted at your specific appid's upload area)
        /// where the zip file for importing can be found</param>
        /// <param name="itemIdToImport">ID of manifest item to import</param>
        /// <param name="permissionDomain">An permission domain to associate this course with, 
        /// for ftp access service (see ftp service below). 
        /// If the domain specified does not exist, the course will be placed in the default permission domain</param>
        /// <param name="useAsync">Use async import</param>
        /// <returns>List of Import Results</returns>
        public List<ImportResult> ImportUploadedCourse(string courseId, string path, string itemIdToImport, string permissionDomain, string server, bool useAsync)
        {
            if (useAsync) {
                String importToken = this.ImportUploadedCourseAsync(courseId, path, itemIdToImport, permissionDomain);
                AsyncImportResult result;
                while (true) {
                    Thread.Sleep(2000);
                    result = this.GetAsyncImportResult(importToken);
                    if (result.IsComplete()) 
                        break;
                }
                if (result.HasError()) {
                    throw new ServiceException(result.ErrorMessage);
                }
                else {
                    return result.ImportResults;
                }
            }
            else {
                ServiceRequest request = new ServiceRequest(configuration);
                if (server != null) {
                    request.Server = server;
                }
                request.Parameters.Add("courseid", courseId);
                request.Parameters.Add("path", path);
                if (!String.IsNullOrEmpty(itemIdToImport))
                    request.Parameters.Add("itemid", itemIdToImport);
                if (!String.IsNullOrEmpty(permissionDomain))
                    request.Parameters.Add("pd", permissionDomain);
                XmlDocument response = request.CallService("rustici.course.importCourse");
                return ImportResult.ConvertToImportResults(response);
            }
        }

        public String ImportUploadedCourseAsync(string courseId, string path, string itemIdToImport, string permissionDomain)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("path", path);
            if (!String.IsNullOrEmpty(itemIdToImport))
                request.Parameters.Add("itemid", itemIdToImport);
            if (!String.IsNullOrEmpty(permissionDomain))
                request.Parameters.Add("pd", permissionDomain);
            XmlDocument response = request.CallService("rustici.course.importCourseAsync");
            String tokenId = ((XmlElement)response
                                    .GetElementsByTagName("token")[0])
                                    .FirstChild.InnerText;
            return tokenId;
        }

        public AsyncImportResult GetAsyncImportResult(String tokenId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("token", tokenId);
            XmlDocument response = request.CallService("rustici.course.getAsyncImportResult");
            return new AsyncImportResult(response);
        }


        /// <summary>
        /// Import new version of an existing course from a SCORM .pif (zip file)
        /// on the local filesystem.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="absoluteFilePathToZip">Full path to the .zip file</param>
        /// <returns>List of Import Results</returns>
        public List<ImportResult> VersionCourse(string courseId, string absoluteFilePathToZip)
        {
            UploadResult uploadResult = manager.UploadService.UploadFile(absoluteFilePathToZip, null);
            String server = uploadResult.server;
            String location = uploadResult.location;
            List<ImportResult> results = null;
            try {
                results = VersionUploadedCourse(courseId, location, server);
            }
            finally {
                manager.UploadService.DeleteFile(location);
            }
            return results;
        }

        /// <summary>
        /// Import new version of an existing course from a SCORM .pif (zip file) from 
        /// an existing .zip file on the Hosted SCORM Engine server.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="path">Path to file, relative to your upload root</param>
        /// <returns>List of Import Results</returns>
        public List<ImportResult> VersionUploadedCourse(string courseId, string path, string server)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            if (server != null) {
                request.Server = server;
            }
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("path", path);
            XmlDocument response = request.CallService("rustici.course.versionCourse");
            return ImportResult.ConvertToImportResults(response);
        }

        /// <summary>
        /// Delete the specified course
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        public void DeleteCourse(string courseId)
        {
            DeleteCourse(courseId, false);
        }

        /// <summary>
        /// Delete the specified course
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="deleteLatestVersionOnly">If false, all versions are deleted</param>
        public void DeleteCourse(string courseId, bool deleteLatestVersionOnly)
	    {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (deleteLatestVersionOnly) 
                request.Parameters.Add("versionid", "latest");
            request.CallService("rustici.course.deleteCourse");
	    }

        /// <summary>
        /// Delete the specified version of a course
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="versionId">Specific version of course to delete</param>
        public void DeleteCourseVersion(string courseId, int versionId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("versionid", versionId);
            request.CallService("rustici.course.deleteCourse");
        }

        /// <summary>
        /// Retrieve a list of high-level data about all courses owned by the 
        /// configured appId that meet the filter's criteria.
        /// </summary>
        /// <param name="courseIdFilterRegex">Regular expresion to filter the courses by ID</param>
        /// <returns>List of Course Data objects</returns>
        public List<CourseData> GetCourseList(string courseIdFilterRegex)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            if (!String.IsNullOrEmpty(courseIdFilterRegex))
                request.Parameters.Add("filter", courseIdFilterRegex);
            XmlDocument response = request.CallService("rustici.course.getCourseList");
            return CourseData.ConvertToCourseDataList(response);
        }

        /// <summary>
        /// Retrieve a list of high-level data about all courses owned by the 
        /// configured appId.
        /// </summary>
        /// <returns>List of Course Data objects</returns>
        public List<CourseData> GetCourseList()
        {
            ServiceRequest request = new ServiceRequest(configuration);
            XmlDocument response = request.CallService("rustici.course.getCourseList");
            return CourseData.ConvertToCourseDataList(response);
        }

        /// <summary>
        /// Retrieve the list of course attributes associated with this course.  If
        /// multiple versions of the course exist, the attributes of the latest version
        /// are returned.
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <returns>Dictionary of all attributes associated with this course</returns>
        public Dictionary<string, string> GetAttributes(string courseId)
        {
            return GetAttributes(courseId, Int32.MinValue);
        }

        /// <summary>
        /// Retrieve the list of course attributes associated with a specific version
        /// of the specified course.
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="versionId">Specific version the specified course</param>
        /// <returns>Dictionary of all attributes associated with this course</returns>
        public Dictionary<string, string> GetAttributes(string courseId, int versionId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (versionId != Int32.MinValue)
                request.Parameters.Add("versionid", versionId);
            XmlDocument response = request.CallService("rustici.course.getAttributes");

            // Map the response to a dictionary of name/value pairs
            Dictionary<string, string> attributeDictionary = new Dictionary<string, string>();
            foreach (XmlElement attrEl in response.GetElementsByTagName("attribute"))
            {
                attributeDictionary.Add(attrEl.Attributes["name"].Value, attrEl.Attributes["value"].Value);
            }
                
            return attributeDictionary;
        }

        /// <summary>
        /// Update the specified attributes (name/value pairs)
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="versionId">Specific version the specified course</param>
        /// <param name="attributePairs">Map of name/value pairs</param>
        /// <returns>Dictionary of changed attributes</returns>
        public Dictionary<string, string> UpdateAttributes(string courseId, int versionId, 
            Dictionary<string,string> attributePairs)
	    {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (versionId != Int32.MinValue)
            {
                request.Parameters.Add("versionid", versionId);
            }
                
            foreach (string key in attributePairs.Keys)
            {
                if (!String.IsNullOrEmpty(attributePairs[key]))
                {
                    request.Parameters.Add(key, attributePairs[key]); 
                }
            }

            XmlDocument response = request.CallService("rustici.course.updateAttributes");

            // Map the response to a dictionary of name/value pairs.  This list
            // should contain only those values that have changed.  If a param was 
            // specified who's value is the same as the current value, it will not
            // be included in this list.
            Dictionary<string, string> attributeDictionary = new Dictionary<string, string>();
            foreach (XmlElement attrEl in response.GetElementsByTagName("attribute"))
            {
                attributeDictionary.Add(attrEl.Attributes["name"].Value, attrEl.Attributes["value"].Value);
            }
                
            return attributeDictionary;
	    }

        /// <summary>
        /// Update the specified attributes (name/value pairs) for the specified
        /// course.  If multiple versions of the course exist, only the latest
        /// version's attributes will be updated.
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="attributePairs">Map of name/value pairs</param>
        /// <returns>Dictionary of changed attributes</returns>
        public Dictionary<string, string> UpdateAttributes(string courseId, Dictionary<string, string> attributePairs)
        {
            return UpdateAttributes(courseId, Int32.MinValue, attributePairs);
        }
    	
        /// <summary>
        /// Get the Course Metadata in XML Format
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="versionId">Version of the specified course</param>
        /// <param name="scope">Defines the scope of the data to return: Course or Activity level</param>
        /// <param name="format">Defines the amount of data to return:  Summary or Detailed</param>
        /// <returns>XML string representing the Metadata</returns>
	    public string GetMetadata(string courseId, int versionId, MetadataScope scope, MetadataFormat format)
	    {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (versionId != Int32.MinValue)
            {
                request.Parameters.Add("versionid", versionId);
            }
            request.Parameters.Add("scope", Enum.GetName(scope.GetType(), scope).ToLower());
            request.Parameters.Add("format", Enum.GetName(format.GetType(), format).ToLower());
            XmlDocument response = request.CallService("rustici.course.getMetadata");
            
            // Return the subset of the xml starting with the top <object>
            return response.ChildNodes[1].InnerXml;
	    }

        /// <summary>
        /// Get the Course Metadata in XML Format.
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="scope">Defines the scope of the data to return: Course or Activity level</param>
        /// <param name="format">Defines the amount of data to return:  Summary or Detailed</param>
        /// <returns>XML string representing the Metadata</returns>

        public string GetMetadata(string courseId, MetadataScope scope, MetadataFormat format)
        {
            return GetMetadata(courseId, Int32.MinValue, scope, format);
        }

        /// <summary>
        /// Update course files only.  One or more course files can be updating them by
        /// including them in a .zip file and sending updates via this method
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="versionId">Specific version of the course</param>
        /// <param name="absoluteFilePathToZip">Full path to the .zip file</param>
        public void UpdateAssets(string courseId, int versionId, string absoluteFilePathToZip)
	    {
            UploadResult uploadResult = manager.UploadService.UploadFile(absoluteFilePathToZip, null);
            String server = uploadResult.server;
            String location = uploadResult.location;
            try {
                UpdateAssetsFromUploadedFile(courseId, versionId, location, server);
            }
            finally {
                manager.UploadService.DeleteFile(location);
            }
	    }

        /// <summary>
        /// Update course files only.  One or more course files can be updating them by
        /// including them in a .zip file and sending updates via this method.  I
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <param name="absoluteFilePathToZip">Full path to the .zip file</param>
        /// <remarks>If multiple versions of a course exist, only the latest version's assets will
        /// be updated.</remarks>
        public void UpdateAssets(string courseId, string absoluteFilePathToZip)
        {
            UpdateAssets(courseId, Int32.MinValue, absoluteFilePathToZip);
        }

        /// <summary>
        /// Update course files only.  One or more course files can be updating them by
        /// including them in a .zip file and sending updates via this method.  The
        /// specified file should already exist in the upload domain space.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="versionId">Specific version of the course</param>
        /// <param name="domain">Optional security domain for the file.</param>
        /// <param name="fileName">Name of the file, including extension.</param>
        public void UpdateAssetsFromUploadedFile(string courseId, int versionId, string path, string server)
	    {
            ServiceRequest request = new ServiceRequest(configuration);
            if (server != null) {
                request.Server = server;
            }
            request.Parameters.Add("courseid", courseId);
            if (versionId != Int32.MinValue)
            {
                request.Parameters.Add("versionid", versionId);
            }
            request.Parameters.Add("path", path);
            request.CallService("rustici.course.updateAssets");
	    }

        /// <summary>
        /// Update course files only.  One or more course files can be updating them by
        /// including them in a .zip file and sending updates via this method.  The
        /// specified file should already exist in the upload domain space.  
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="domain">Optional security domain for the file.</param>
        /// <param name="fileName">Name of the file, including extension.</param>
        /// <remarks>If multiple versions of a course exist, only the latest version's assets will
        /// be updated.</remarks>
        public void UpdateAssetsFromUploadedFile(string courseId, string path, string server)
        {
            UpdateAssetsFromUploadedFile(courseId, Int32.MinValue, path, server);
        }

        /// <summary>
        /// Delete one or more files from the specified course directory
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="versionId">Version ID of the specified course</param>
        /// <param name="relativeFilePaths">Path of each file to delete realtive to the course root</param>
        /// <returns>Map of results as a Dictionary of booleans</returns>
	    public Dictionary<string, bool> DeleteFiles(string courseId, int versionId, Collection<string> relativeFilePaths)
	    {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (versionId != Int32.MinValue)
            {
                request.Parameters.Add("versionid", versionId);
            }

            foreach (string fileName in relativeFilePaths)
            {
                request.Parameters.Add("path", fileName);
            }

            XmlDocument response = request.CallService("rustici.course.deleteFiles");

            Dictionary<string, bool> resultsMap = new Dictionary<string, bool>();
            foreach (XmlElement attrEl in response.GetElementsByTagName("result"))
            {
                resultsMap.Add(attrEl.Attributes["path"].Value, 
                    Convert.ToBoolean(attrEl.Attributes["deleted"].Value));
            }

            return resultsMap;
	    }

        /// <summary>
        /// Delete one or more files from the specified course directory. 
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="relativeFilePaths">Path of each file to delete realtive to the course root</param>
        /// <returns>Map of results as a Dictionary of booleans</returns>
        /// <remarks>If  multiple versions of a course exist, only files from the latest version
        /// will be deleted.</remarks>
        public Dictionary<string, bool> DeleteFiles(string courseId, Collection<string> relativeFilePaths)
        {
            return DeleteFiles(courseId, Int32.MinValue, relativeFilePaths);
        }
    	

        /// <summary>
        /// Get the file structure of the given course.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <param name="versionId">Version ID of the specified course</param>
        /// <returns>XML String of the hierarchical file structure of the course</returns>
        public string GetFileStructure(string courseId, int versionId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (versionId != Int32.MinValue)
            {
                request.Parameters.Add("versionid", versionId);
            }
            XmlDocument response = request.CallService("rustici.course.getFileStructure");
            
            // Return the subset of the xml starting with the top <dir>
            return response.ChildNodes[1].InnerXml;
        }

        /// <summary>
        /// Get the file structure of the given course.
        /// </summary>
        /// <param name="courseId">Unique Identifier for this course.</param>
        /// <returns>XML String of the hierarchical file structure of the course</returns>
        /// <remarks>If multiple versions of the course exist, the latest version's
        /// files structure will be retured.</remarks>
        public string GetFileStructure(string courseId)
        {
            return GetFileStructure(courseId, Int32.MinValue);
        }

        /// <summary>
        /// Gets the url to view/edit the package properties for this course.  Typically
        /// used within an IFRAME
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <returns>Signed URL to package property editor</returns>
        /// <param name="notificationFrameUrl">Tells the property editor to render a sub-iframe
        /// with the provided url as the src.  This can be used to simulate an "onload"
        /// by using a notificationFrameUrl that's the same domain as the host system and
        /// calling parent.parent.method()</param>
        public string GetPropertyEditorUrl(string courseId, string stylesheetUrl, string notificationFrameUrl)
        {
            // The local parameter map just contains method methodParameters.  We'll
            // now create a complete parameter map that contains the web-service
            // params as well the actual method params.
            IDictionary<string, object> parameterMap = new Dictionary<string, object>();
            parameterMap.Add("action", "properties.view");
            parameterMap.Add("package", "AppId|" + configuration.AppId + "!PackageId|" + courseId);
            parameterMap.Add("appid", configuration.AppId);
            parameterMap.Add("ts", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            if (!String.IsNullOrEmpty(notificationFrameUrl))
                parameterMap.Add("notificationframesrc", notificationFrameUrl);
            if (!String.IsNullOrEmpty(stylesheetUrl))
                parameterMap.Add("stylesheet", stylesheetUrl);

            // Construct the url, concatonate all parameters as query string parameters
            string url = configuration.ScormEngineServiceUrl + "/widget";
            int cnt = 0;
            foreach (string key in parameterMap.Keys)
            {
                // Create a query string with URL-encoded values
                url += (cnt++ == 0 ? "?" : "&") + key + "=" + HttpUtility.UrlEncode(parameterMap[key].ToString());
            }
            url += "&sig=" + RequestSigner.GetSignatureForRequest(configuration.SecurityKey, parameterMap);

            if (url.Length > 2000)
                throw new ApplicationException("URL > 2000 bytes");

            return url;
        }

        /// <summary>
        /// Gets the url to view/edit the package properties for this course.  Typically
        /// used within an IFRAME
        /// </summary>
        /// <param name="courseId">Unique Identifier for the course</param>
        /// <returns>Signed URL to package property editor</returns>
        public string GetPropertyEditorUrl(string courseId)
        {
            return GetPropertyEditorUrl(courseId, null, null);
        }

        public string GetAssets(String toFileName, String courseId)
        {
            return GetAssets(toFileName, courseId, Int32.MinValue, null);
        }

        public string GetAssets(String toFileName, String courseId, List<String> paths)
        {
            return GetAssets(toFileName, courseId, Int32.MinValue, paths);
        }

        public string GetAssets(String toFileName, String courseId, int versionId, List<String> paths)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if(paths != null && paths.Count > 0){
                foreach (String path in paths){
                    request.Parameters.Add("path", path);
                }
            }
            if (versionId != Int32.MinValue)
            {
                request.Parameters.Add("versionid", versionId);
            }

            //Return file path to downloaded file
            return request.GetFileFromService(toFileName, "rustici.course.getAssets");
        }

        /// <summary>
        /// Get the url that points directly to a course asset
        /// </summary>
        /// <param name="courseId">Unique Course Identifier</param>
        /// <param name="path">Path to asset from root of course</param>
        /// <param name="versionId">Specific Version</param>
        /// <returns>HTTP Url to Asset</returns>
        public String GetAssetUrl(String courseId, String path, int versionId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("path", path);
            if (versionId != Int32.MinValue)
            {
                request.Parameters.Add("versionid", versionId);
            }

            return request.ConstructUrl("rustici.course.getAssets");
        }

        /// <summary>
        /// Get the tags of a course
        /// </summary>        
        /// <param name="courseId">Unique Course Identifier</param>        
        /// <returns>List of tags?</returns>
        public String GetTags(String courseId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("appid ", configuration.AppId);
                                    
            XmlDocument response = request.CallService("rustici.tagging.getCourseTags");

            XmlDocument tagXML = new XmlDocument();
            string tagXMLString = response.ChildNodes[1].InnerXml; 
            tagXML.LoadXml(tagXMLString);            
            string tagList = "";
            for (var n = 0; n < tagXML.FirstChild.ChildNodes.Count; n++)
            {
                XmlNode nextTagNode = tagXML.FirstChild.ChildNodes[n];
                if (n > 0) tagList += ",";
                tagList += nextTagNode.InnerText;                
            }            

            return tagList;

        }
        

        /// <summary>
        /// Get the url that points directly to a course asset
        /// </summary>
        /// <param name="courseId">Unique Course Identifier</param>
        /// <param name="path">Path to asset from root of course</param>
        /// <returns>HTTP Url to Asset</returns>
        public String GetAssetUrl(String courseId, String path)
        {
            return GetAssetUrl(courseId, path, Int32.MinValue);
        }

        /// <summary>
        /// Get the url that can be opened in a browser and used to preview this course, without
        /// the need for a registration.
        /// </summary>
        /// <param name="courseId">Unique Course Identifier</param>
        public String GetPreviewUrl(String courseId)
        {
            return GetPreviewUrl(courseId, null, null);
        }


        public String GetPreviewUrl(String courseId, String redirectOnExitUrl)
        {
            return GetPreviewUrl(courseId, redirectOnExitUrl, null);
        }

        /// <summary>
        /// Get the url that can be opened in a browser and used to preview this course, without
        /// the need for a registration.
        /// </summary>
        /// <param name="courseId">Unique Course Identifier</param>
        /// <param name="versionId">Version Id</param>
        public String GetPreviewUrl(String courseId, String redirectOnExitUrl, String cssUrl)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            if (!String.IsNullOrEmpty(redirectOnExitUrl))
                request.Parameters.Add("redirecturl", redirectOnExitUrl);
            if (!String.IsNullOrEmpty(cssUrl))
                request.Parameters.Add("cssurl", cssUrl);
            return request.ConstructUrl("rustici.course.preview");
        }

        /// <summary>
        /// Returns a boolean of whether or not the a course with the given courseId exists in the associated
        /// appId in the SCORM Cloud.
        /// </summary>
        /// <param name="courseId">Unique Course Identifier</param>
        /// <returns></returns>
        public bool Exists(String courseId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            XmlDocument response = request.CallService("rustici.course.exists");
            return bool.Parse(response.DocumentElement.GetElementsByTagName("result").Item(0).InnerText);
        }
    }
}
