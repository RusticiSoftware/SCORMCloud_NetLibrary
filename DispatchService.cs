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
    /// Client-side proxy for the "rustici.dispatch.*" Hosted SCORM Engine web
    /// service methods.  
    /// </summary>
    public class DispatchService
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public DispatchService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        /// <summary>
        /// List of existing dispatch destinations in your account. Callers should assume
        /// another page of data is available by calling again with the page parameter
        /// incremented, until an empty list is returned.
        /// </summary>
        /// <param name="page">Which page of results to return. Page numbers start at 1.</param>
        /// <returns>List of Destination Data objects.</returns>
        public List<DestinationData> GetDestinationList(int page)
        {
            return GetDestinationList(page, null);
        }

        /// <summary>
        /// List of existing dispatch destinations in your account. Callers should assume
        /// another page of data is available by calling again with the page parameter
        /// incremented, until an empty list is returned.
        /// </summary>
        /// <param name="page">Which page of results to return. Page numbers start at 1.</param>
        /// <param name="tags">A comma separated list of tags to filter results by. Results must be tagged with every tag in the list.</param>
        /// <returns>List of Destination Data objects.</returns>
        public List<DestinationData> GetDestinationList(int page, string tags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("page", page);
            if (!string.IsNullOrEmpty(tags))
                request.Parameters.Add("tags", tags);
            XmlDocument response = request.CallService("rustici.dispatch.getDestinationList");
            return DestinationData.ConvertToDestinationDataList(response);
        }

        /// <summary>
        /// List of all existing dispatch destinations in your account.
        /// </summary>
        /// <returns>List of Destination Data objects.</returns>
        public List<DestinationData> GetDestinationList()
        {
            return GetDestinationList(null);
        }
        /// <summary>
        /// List of all existing dispatch destinations in your account.
        /// </summary>
        /// <param name="tags">A comma separated list of tags to filter results by. Results must be tagged with every tag in the list.</param>
        /// <returns>List of Destination Data objects.</returns>
        public List<DestinationData> GetDestinationList(string tags)
        {
            List<DestinationData> allResults = new List<DestinationData>();
            List<DestinationData> pageResults;
            int page = 0;

            do
            {
                pageResults = GetDestinationList(++page, tags);
                allResults.AddRange(pageResults);
            } while (pageResults.Count > 0);

            return allResults;
        }

        /// <summary>
        /// Get information about the dispatch destination named by the given destinationid parameter.
        /// </summary>
        /// <param name="destinationId">The id of the destination being accessed.</param>
        /// <returns>Destination Data object.</returns>
        public DestinationData GetDestinationInfo(string destinationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("destinationid", destinationId);
            XmlDocument response = request.CallService("rustici.dispatch.getDestinationInfo");

            return new DestinationData((XmlElement)response.GetElementsByTagName("dispatchDestination")[0]);
        }

        /// <summary>
        /// Create a new Dispatch Destination in your SCORM Cloud account.
        /// </summary>
        /// <param name="name">The name of the new destination.</param>
        /// <param name="tags">A comma separated list of tags to add to this destination.</param>
        /// <param name="email">The email address associated with the user creating this destination.</param>
        /// <returns>The id for the newly created destination.</returns>
        public string CreateDestination(string name, string tags, string email)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("name", name);
            if (!string.IsNullOrEmpty(tags))
                request.Parameters.Add("tags", tags);
            if (!string.IsNullOrEmpty(email))
                request.Parameters.Add("email", email);
            XmlDocument response = request.CallService("rustici.dispatch.createDestination");
            String destinationId = ((XmlElement)response
                                    .GetElementsByTagName("destinationId")[0])
                                    .FirstChild.InnerText;
            return destinationId;
        }

        /// <summary>
        /// Update the Dispatch Destination.
        /// </summary>
        /// <param name="destinationId">The id of the destination being updated.</param>
        /// <param name="name">The new name for this destination.</param>
        /// <param name="tags">A comma separated list of tags to set for this destination. An empty string will remove all existing tags, a null value will retain them.</param>
        public void UpdateDestination(string destinationId, string name, string tags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("destinationid", destinationId);
            if (!string.IsNullOrEmpty(name))
                request.Parameters.Add("name", name);
            if (tags != null)
                request.Parameters.Add("tags", tags);
            request.CallService("rustici.dispatch.updateDestination");
        }

        /// <summary>
        /// Delete the Dispatch Destination.
        /// </summary>
        /// <param name="destinationId">The id of the destination being deleted.</param>
        public void DeleteDestination(string destinationId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("destinationid", destinationId);
            request.CallService("rustici.dispatch.deleteDestination");
        }

        /// <summary>
        /// List of existing dispatch in your account. Callers should assume
        /// another page of data is available by calling again with the page parameter
        /// incremented, until an empty list is returned.
        /// </summary>
        /// <param name="page">Which page of results to return. Page numbers start at 1.</param>
        /// <param name="destinationId">Show only dispatches belonging to the destination named by this id.</param>
        /// <param name="courseId">Show only dispatches for the course named by this id.</param>
        /// <param name="tags">A comma separated list of tags to filter results by. Results must be tagged with every tag in the list.</param>
        /// <returns>List of Destination Data objects.</returns>
        public List<DispatchData> GetDispatchList(int page, string destinationId, string courseId, string tags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("page", page);
            if (!string.IsNullOrEmpty(destinationId))
                request.Parameters.Add("destinationid", destinationId);
            if (!string.IsNullOrEmpty(courseId))
                request.Parameters.Add("courseid", courseId);
            if (!string.IsNullOrEmpty(tags))
                request.Parameters.Add("tags", tags);
            XmlDocument response = request.CallService("rustici.dispatch.getDispatchList");
            return DispatchData.ConvertToDispatchDataList(response);
        }

        /// <summary>
        /// List of all existing dispatches in your account.
        /// </summary>
        /// <returns>List of Dispatch Data objects.</returns>
        public List<DispatchData> GetDispatchList()
        {
            return GetDispatchList(null, null, null);
        }
        /// <summary>
        /// List of all existing dispatches in your account.
        /// </summary>
        /// <param name="destinationId">Show only dispatches belonging to the destination named by this id.</param>
        /// <param name="courseId">Show only dispatches for the course named by this id.</param>
        /// <param name="tags">A comma separated list of tags to filter results by. Results must be tagged with every tag in the list.</param>
        /// <returns>List of Destination Data objects.</returns>
        public List<DispatchData> GetDispatchList(string destinationId, string courseId, string tags)
        {
            List<DispatchData> allResults = new List<DispatchData>();
            List<DispatchData> pageResults;
            int page = 0;

            do
            {
                pageResults = GetDispatchList(++page, destinationId, courseId, tags);
                allResults.AddRange(pageResults);
            } while (pageResults.Count > 0);

            return allResults;
        }

        /// <summary>
        /// Get information about the dispatch named by the given dispatchId parameter.
        /// </summary>
        /// <param name="dispatchId">The id of the dispatch being accessed.</param>
        /// <returns>Dispatch Data object.</returns>
        public DispatchData GetDispatchInfo(string dispatchId)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("dispatchid", dispatchId);
            XmlDocument response = request.CallService("rustici.dispatch.getDispatchInfo");

            return new DispatchData((XmlElement)response.GetElementsByTagName("dispatch")[0]);
        }

        /// <summary>
        /// Create a new Dispatch in your SCORM Cloud account.
        /// </summary>
        /// <param name="destinationId">The id of the destination this dispatch will be associated with.</param>
        /// <param name="courseId">The id of the course this dispatch will be associated with.</param>
        /// <param name="tags">A comma separated list of tags to add to this dispatch.</param>
        /// <param name="email">The email address associated with the user creating this dispatch.</param>
        /// <returns>The id for the newly created dispatch.</returns>
        public string CreateDispatch(string destinationId, string courseId, string tags, string email)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("destinationid", destinationId);
            request.Parameters.Add("courseid", courseId);
            if (!string.IsNullOrEmpty(tags))
                request.Parameters.Add("tags", tags);
            if (!string.IsNullOrEmpty(email))
                request.Parameters.Add("email", email);
            XmlDocument response = request.CallService("rustici.dispatch.createDispatch");
            String dispatchId = ((XmlElement)response
                                    .GetElementsByTagName("dispatchId")[0])
                                    .FirstChild.InnerText;
            return dispatchId;
        }

        /// <summary>
        /// Update the selected dispatches in your SCORM Cloud account. Selection of dispatches to update is
        /// based either on a specific dispatch using the dispatchid parameter, or groups of dispatches using
        /// the destinationid, courseid, or tags parameters in any combination.
        /// </summary>
        /// <param name="dispatchId">The id of the dispatch to update.</param>
        /// <param name="destinationId">The id of the destination used to select the dispatch group to update.</param>
        /// <param name="courseId">The id of the course used to select the dispatch group to update.</param>
        /// <param name="tags">A comma separated list of tags used to select the dispatch group to update. Each dispatch selected will have to be tagged with each tag in the list.</param>
        /// <param name="enabled">Setting "true" or "false" will enable or disable the selected group of dispatches.</param>
        /// <param name="addTags">A comma separated list of tags to add to the selected dispatches.</param>
        /// <param name="removeTags">A comma separated list of tags to remove from the selected dispatches.</param>
        /// <returns>The id for the newly created dispatch.</returns>
        public void UpdateDispatches(string dispatchId, string destinationId, string courseId, string tags, bool? enabled, string addTags, string removeTags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            if (!string.IsNullOrEmpty(dispatchId))
                request.Parameters.Add("dispatchid", dispatchId);
            if (!string.IsNullOrEmpty(destinationId))
                request.Parameters.Add("destinationid", destinationId);
            if (!string.IsNullOrEmpty(courseId))
                request.Parameters.Add("courseid", courseId);
            if (!string.IsNullOrEmpty(tags))
                request.Parameters.Add("tags", tags);
            if (enabled.HasValue)
                request.Parameters.Add("enabled", enabled.Value);
            if (!string.IsNullOrEmpty(addTags))
                request.Parameters.Add("addtags", addTags);
            if (!string.IsNullOrEmpty(removeTags))
                request.Parameters.Add("removetags", removeTags);
            request.CallService("rustici.dispatch.updateDispatches");
        }

        /// <summary>
        /// Uses the dispatches selected by the given parameters to create and deliver a package
        /// containing the resources used to import and launch those dispatches in client systems.
        /// This will save a zip file, which in turn contains zip files for each of the selected dispatches.
        /// </summary>
        /// <param name="toFileName">File to save download to.</param>
        /// <param name="dispatchId">The id of the dispatch to download.</param>
        /// <param name="destinationId">The id of the destination used to select the dispatch group to download.</param>
        /// <param name="courseId">The id of the course used to select the dispatch group to download.</param>
        /// <param name="tags">A comma separated list of tags used to select the dispatch group to download. Each dispatch selected will have to be tagged with each tag in the list.</param>
        /// <returns>Dispatch Data object.</returns>
        public string DownloadDispatches(string toFileName, string dispatchId, string destinationId, string courseId, string tags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            if (!string.IsNullOrEmpty(dispatchId))
                request.Parameters.Add("dispatchid", dispatchId);
            if (!string.IsNullOrEmpty(destinationId))
                request.Parameters.Add("destinationid", destinationId);
            if (!string.IsNullOrEmpty(courseId))
                request.Parameters.Add("courseid", courseId);
            if (!string.IsNullOrEmpty(tags))
                request.Parameters.Add("tags", tags);

            //Return file path to downloaded file
            return request.GetFileFromService(toFileName, "rustici.dispatch.downloadDispatches");
        }

        /// <summary>
        /// Delete the selected dispatches from your SCORM Cloud account, using the parameters given.
        /// Selection of dispatches to delete is based either on a specific dispatch using the
        /// dispatchid parameter, or groups of dispatches using the destinationid, courseid, or
        /// tags parameters in any combination.
        /// </summary>
        /// <param name="dispatchId">The id of the dispatch being deleted.</param>
        /// <param name="destinationId">The id of the destination used to select the dispatch group to delete.</param>
        /// <param name="courseId">The id of the course used to select the dispatch group to delete.</param>
        /// <param name="tags">A comma separated list of tags used to select the dispatch group to delete. Each dispatch selected will have to be tagged with each tag in the list.</param>
        public void DeleteDispatches(string dispatchId, string destinationId, string courseId, string tags)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            if (!string.IsNullOrEmpty(dispatchId))
                request.Parameters.Add("dispatchid", dispatchId);
            if (!string.IsNullOrEmpty(destinationId))
                request.Parameters.Add("destinationid", destinationId);
            if (!string.IsNullOrEmpty(courseId))
                request.Parameters.Add("courseid", courseId);
            if (!string.IsNullOrEmpty(tags))
                request.Parameters.Add("tags", tags);

            request.CallService("rustici.dispatch.deleteDispatches");
        }
    }
}