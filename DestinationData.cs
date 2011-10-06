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
    /// Data Transfer object that contains information from the dispatch destination listing
    /// service.
    /// </summary>
    public class DestinationData
    {
        private string destinationId;
        private string name;
        private string tags;
        private string createdBy;
        private DateTime createDate;
        private DateTime updateDate;

        /// <summary>
        /// Purpose of this class is to map the return xml from the dispatch destination listing
        /// web service into an object.  This is the main constructor.
        /// </summary>
        /// <param name="destinationDataElement"></param>
        public DestinationData(XmlElement destinationDataElement)
        {
            this.destinationId = destinationDataElement["id"].InnerText;
            this.name = destinationDataElement["name"].InnerText;
            this.createdBy = destinationDataElement["createdBy"].InnerText;
            this.createDate = DateTime.Parse(destinationDataElement["createDate"].InnerText);
            this.updateDate = DateTime.Parse(destinationDataElement["updateDate"].InnerText);

            XmlNodeList tagList = destinationDataElement.GetElementsByTagName("tag");
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
        public static List<DestinationData> ConvertToDestinationDataList(XmlDocument xmlDoc)
        {
            List<DestinationData> allResults = new List<DestinationData>();

            XmlNodeList destinationDataList = xmlDoc.GetElementsByTagName("dispatchDestination");
            foreach (XmlElement destinationData in destinationDataList)
            {
                allResults.Add(new DestinationData(destinationData));
            }

            return allResults;
        }

        /// <summary>
        /// Destination Identifier.
        /// </summary>
        public string DestinationId
        {
            get { return destinationId; }
        }

        /// <summary>
        /// The name of this destination.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The tags associated with the destination.
        /// </summary>
        public string Tags
        {
            get { return tags; }
        }

        /// <summary>
        /// Who created this destination.
        /// </summary>
        public string CreatedBy
        {
            get { return createdBy; }
        }

        /// <summary>
        /// When this destination was created.
        /// </summary>
        public DateTime CreateDate
        {
            get { return createDate; }
        }

        /// <summary>
        /// When this destination was last updated.
        /// </summary>
        public DateTime UpdateDate
        {
            get { return updateDate; }
        }
    }
}