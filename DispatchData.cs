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
    /// Data Transfer object that contains information from the dispatch listing
    /// service.
    /// </summary>
    public class DispatchData
    {
        private string dispatchId;
        private string destinationId;
        private string appId;
        private string courseAppId;
        private string courseId;
        private bool enabled;
        private string tags;
        private string notes;
        private string createdBy;
        private DateTime createDate;
        private DateTime updateDate;

        /// <summary>
        /// Purpose of this class is to map the return xml from the dispatch listing
        /// web service into an object.  This is the main constructor.
        /// </summary>
        /// <param name="destinationDataElement"></param>
        public DispatchData(XmlElement dispatchDataElement)
        {
            this.dispatchId = dispatchDataElement["id"].InnerText;
            this.destinationId = dispatchDataElement["destinationId"].InnerText;
            this.appId = dispatchDataElement["appId"].InnerText;
            this.courseAppId = dispatchDataElement["courseAppId"].InnerText;
            this.courseId = dispatchDataElement["courseId"].InnerText;
            this.enabled = bool.Parse(dispatchDataElement["enabled"].InnerText);
            if (dispatchDataElement["notes"] != null)
                this.notes = dispatchDataElement["notes"].InnerText;
            this.createdBy = dispatchDataElement["createdBy"].InnerText;
            this.createDate = DateTime.Parse(dispatchDataElement["createDate"].InnerText);
            this.updateDate = DateTime.Parse(dispatchDataElement["updateDate"].InnerText);

            XmlNodeList tagList = dispatchDataElement.GetElementsByTagName("tag");
            string[] tags = new string[tagList.Count];
            for (int i = 0; i < tagList.Count; ++i)
            {
                tags[i] = tagList[i].InnerText;
            }
            this.tags = string.Join(",", tags);
        }

        /// <summary>
        /// Helper method which takes the full XmlDocument as returned from the dispatch destination listing
        /// web service and returns a List of DestinationData objects.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static List<DispatchData> ConvertToDispatchDataList(XmlDocument xmlDoc)
        {
            List<DispatchData> allResults = new List<DispatchData>();

            XmlNodeList dispatchDataList = xmlDoc.GetElementsByTagName("dispatch");
            foreach (XmlElement dispatchData in dispatchDataList)
            {
                allResults.Add(new DispatchData(dispatchData));
            }

            return allResults;
        }

        /// <summary>
        /// Dispatch Identifier.
        /// </summary>
        public string DispatchId
        {
            get { return dispatchId; }
        }

        /// <summary>
        /// The id of the destination this dispatch is associated with.
        /// </summary>
        public string DestinationId
        {
            get { return destinationId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AppId
        {
            get { return appId; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CourseAppId
        {
            get { return courseAppId; }
        }

        /// <summary>
        /// The id of the course this dispatch is associated with.
        /// </summary>
        public string CourseId
        {
            get { return courseId; }
        }

        /// <summary>
        /// true if this dispatch is enabled; otherwise, false.
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
        }

        /// <summary>
        /// The tags associated with the dispatch.
        /// </summary>
        public string Tags
        {
            get { return tags; }
        }

        /// <summary>
        /// The tags associated with the dispatch.
        /// </summary>
        public string Notes
        {
            get { return notes; }
        }

        /// <summary>
        /// Who created this dispatch.
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
        }

        /// <summary>
        /// When this dispatch was created.
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
        }

        /// <summary>
        /// When this dispatch was last updated.
        /// </summary>
        public DateTime UpdateDate
        {
            get { return updateDate; }
        }
    }
}