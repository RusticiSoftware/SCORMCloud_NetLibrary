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
    /// Data Transfer object that contains high-level information from the course listing
    /// service.
    /// </summary>
    public class CourseData
    {
        private String courseId;
        private int numberOfVersions;
        private int numberOfRegistrations;
        private String title;

        /// <summary>
        /// Purpose of this class is to map the return xml from the course listing
        /// web service into an object.  This is the main constructor.
        /// </summary>
        /// <param name="courseDataElement"></param>
        public CourseData(XmlElement courseDataElement)
        {
            this.courseId = courseDataElement.Attributes["id"].Value;
            this.numberOfVersions = Convert.ToInt32(courseDataElement.Attributes["versions"].Value);
            this.numberOfRegistrations = Convert.ToInt32(courseDataElement.Attributes["registrations"].Value);
            this.title = courseDataElement.Attributes["title"].Value;
        }

        /// <summary>
        /// Helper method which takes the full XmlDocument as returned from the course listing
        /// web service and returns a List of CourseData objects.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static List<CourseData> ConvertToCourseDataList(XmlDocument xmlDoc)
        {
            List<CourseData> allResults = new List<CourseData>();

            XmlNodeList courseDataList = xmlDoc.GetElementsByTagName("course");
            foreach (XmlElement courseData in courseDataList)
            {
                allResults.Add(new CourseData(courseData));
            }

            return allResults;
        }

        /// <summary>
        /// Course Identifier as specified at import-time
        /// </summary>
        public string CourseId
        {
            get { return courseId; }
        }

        /// <summary>
        /// Count of the number of versions for this course/package
        /// </summary>
        public int NumberOfVersions
        {
            get { return numberOfVersions; }
        }

        /// <summary>
        /// Count of the number of existing registrations there are for this
        /// course -- the number of instances that a user has taken this course.
        /// </summary>
        public int NumberOfRegistrations
        {
            get { return numberOfRegistrations; }
        }

        /// <summary>
        /// The title of this course
        /// </summary>
        public string Title
        {
            get { return title; }
        }
    }
}
