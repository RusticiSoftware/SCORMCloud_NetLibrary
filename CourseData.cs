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
