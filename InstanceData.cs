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
    /// Data class to hold high-level Registration Data
    /// </summary>
    public class InstanceData
    {
        private string _instanceId;
        private string _courseVersion;
        private DateTime _updateDate;

        /// <summary>
        /// Constructor which takes an XML node as returned by the web service.
        /// </summary>
        /// <param name="instanceElem"></param>
        public InstanceData(XmlElement instanceElem)
        {
            this.InstanceId =  ((XmlElement)instanceElem
                                        .GetElementsByTagName("instanceId")[0])
                                        .InnerText;
            this.CourseVersion =  ((XmlElement)instanceElem
                                        .GetElementsByTagName("courseVersion")[0])
                                        .InnerText;
            this.UpdateDate =  DateTime.Parse(((XmlElement)instanceElem
                                        .GetElementsByTagName("updateDate")[0])
                                        .InnerText);
        }


        /// <summary>
        /// Helper method which takes the instances element as returned from the registration detail 
        /// web service and returns a List of InstanceData objects.
        /// </summary>
        /// <param name="xmlInstancesElem"></param>
        /// <returns></returns>
        public static List<InstanceData> ConvertToInstanceDataList(XmlElement xmlInstancesElem)
        {
            List<InstanceData> allResults = new List<InstanceData>();

            XmlNodeList instDataList = xmlInstancesElem.GetElementsByTagName("instance");

            foreach (XmlElement instData in instDataList)
            {
                allResults.Add(new InstanceData(instData));
            }

            return allResults;
        }


        /// <summary>
        /// Unique Identifier for this Instance within this Course
        /// </summary>
        public string InstanceId
        {
            get { return _instanceId; }
            private set { _instanceId = value; }

        }


        /// <summary>
        /// Version of the Course this Instance refers to
        /// </summary>
        public string CourseVersion
        {
            get { return _courseVersion; }
            private set { _courseVersion = value; }
        }


        /// <summary>
        /// Date this instance/version of the course was updated
        /// </summary>
        public DateTime UpdateDate
        {
            get { return _updateDate; }
            private set { _updateDate = value; }
        }


    }
}
