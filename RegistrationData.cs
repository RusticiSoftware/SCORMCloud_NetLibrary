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
    public class RegistrationData
    {
        private string _registrationId;
        private string _courseId;
        private string _courseTitle;
        private string _lastCourseVersionLaunched;
        private string _learnerId;
        private string _learnerFirstName;
        private string _learnerLastName;
        private string _email;

        private DateTime? _createDate;
        private DateTime? _firstAccessDate;
        private DateTime? _lastAccessDate;
        private DateTime? _completedDate;

        private List<InstanceData> _instances;

       // private int numberOfInstances;

        /// <summary>
        /// Constructor which takes an XML node as returned by the web service.
        /// </summary>
        /// <param name="regDataEl"></param>
        public RegistrationData(XmlElement registrationElem)
        {
            this.RegistrationId = registrationElem.Attributes["id"].Value;

            this.CourseId = registrationElem.Attributes["courseid"].Value;

            this.CourseTitle = ((XmlElement)registrationElem
                            .GetElementsByTagName("courseTitle")[0])
                            .InnerText;

            this.LastCourseVersionLaunched = ((XmlElement)registrationElem
                            .GetElementsByTagName("lastCourseVersionLaunched")[0])
                            .InnerText;

            this.LearnerId = ((XmlElement)registrationElem
                            .GetElementsByTagName("learnerId")[0])
                            .InnerText;

            this.LearnerFirstName = ((XmlElement)registrationElem
                            .GetElementsByTagName("learnerFirstName")[0])
                            .InnerText;

            this.LearnerLastName = ((XmlElement)registrationElem
                            .GetElementsByTagName("learnerLastName")[0])
                            .InnerText;

            this.Email = ((XmlElement)registrationElem
                            .GetElementsByTagName("email")[0])
                            .InnerText;

            this.CreateDate = DateTime.Parse(((XmlElement)registrationElem
                            .GetElementsByTagName("createDate")[0])
                            .InnerText);

            this.FirstAccessDate = DateTime.Parse(((XmlElement)registrationElem
                            .GetElementsByTagName("firstAccessDate")[0])
                            .InnerText);

            this.LastAccessDate = DateTime.Parse(((XmlElement)registrationElem
                            .GetElementsByTagName("lastAccessDate")[0])
                            .InnerText);

            this.CompletedDate = DateTime.Parse(((XmlElement)registrationElem
                            .GetElementsByTagName("completedDate")[0])
                            .InnerText);

            this.Instances = InstanceData.ConvertToInstanceDataList((XmlElement)registrationElem.GetElementsByTagName("instances")[0]);

        }

        /// <summary>
        /// Helper method which takes the full XmlDocument as returned from the registration listing
        /// web service and returns a List of RegistrationData objects.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static List<RegistrationData> ConvertToRegistrationDataList(XmlDocument xmlDoc)
        {
            List<RegistrationData> allResults = new List<RegistrationData>();

            XmlNodeList regDataList = xmlDoc.GetElementsByTagName("registration");
            foreach (XmlElement regData in regDataList)
            {
                allResults.Add(new RegistrationData(regData));
            }

            return allResults;
        }

        /// <summary>
        /// Unique Identifier for this registration
        /// </summary>
        public string RegistrationId
        {
            get { return _registrationId; }
            private set { _registrationId = value; }
        }

        /// <summary>
        /// Unique Identifier for this course
        /// </summary>
        public string CourseId
        {
            get { return _courseId; }
            private set { _courseId = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CourseTitle
        {
            get { return _courseTitle; }
            private set { _courseTitle = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string LastCourseVersionLaunched
        {
            get { return _lastCourseVersionLaunched; }
            private set { _lastCourseVersionLaunched = value; }
        }



        /// <summary>
        /// 
        /// </summary>
        public string LearnerId
        {
            get { return _learnerId; }
            private set { _learnerId = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string LearnerFirstName
        {
            get { return _learnerFirstName; }
            private set { _learnerFirstName = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string LearnerLastName
        {
            get { return _learnerLastName; }
            private set { _learnerLastName = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string Email
        {
            get { return _email; }
            private set { _email = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate
        {
            get { return _createDate; }
            private set { _createDate = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public DateTime? FirstAccessDate
        {
            get { return _firstAccessDate; }
            private set { _firstAccessDate = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public DateTime? LastAccessDate
        {
            get { return _lastAccessDate; }
            private set { _lastAccessDate = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        public DateTime? CompletedDate
        {
            get { return _completedDate; }
            private set { _completedDate = value; }
        }


        public List<InstanceData> Instances
        {
            get { return _instances; }
            private set { _instances = value; }
        }
//        /// <summary>
//        /// Number of instances of this course.  Instances are independent registrations
//        /// of the same registration ID.  It is essentially "retakes" of a course by
//        /// the same user under the same registration ID.
//        /// </summary>
//        public int NumberOfInstances
//        {
//            get { return numberOfInstances; }
//        }
    }
}
