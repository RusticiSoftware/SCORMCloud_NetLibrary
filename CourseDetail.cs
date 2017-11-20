using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    public class CourseDetail
    {
        private String courseId;
        private int numberOfVersions;
        private int numberOfRegistrations;
        private String title;
        private List<String> tags;
        private List<CourseVersion> versions;
        private String learningStandard;
        private String tincanActivityId;

        /// <summary>
        /// Purpose of this class is to map the return xml from the course detail
        /// web service into an object.  This is the main constructor.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// 
        /*
         * <course id="course123" title="Golf Explained - Sequencing Versions" versions="3" registrations="16" size="2040257" >
              <versions>
                <version>
                  <versionId><![CDATA[0]]></versionId>
                  <updateDate><![CDATA[2011-07-22T17:39:06.000+0000]]></updateDate>
                </version>
                <version>
                  <versionId><![CDATA[1]]></versionId>
                  <updateDate><![CDATA[2011-07-25T16:19:59.000+0000]]></updateDate>
                </version>
                <version>
                  <versionId><![CDATA[2]]></versionId>
                  <updateDate><![CDATA[2011-07-25T20:15:37.000+0000]]></updateDate>
                </version>
              </versions>
              <tags></tags>
            </course>
         * 
            Tin Can Course Example response:
            <course id="course123" title="Golf Explained - Sequencing Versions" versions="3" registrations="16" size="2040257" >
              <versions>
                <version>
                  <versionId><![CDATA[0]]></versionId>
                  <updateDate><![CDATA[2011-07-22T17:39:06.000+0000]]></updateDate>
                </version>
              </versions>
              <tags></tags>
              <learningStandard><![CDATA[Tin Can]]></learningStandard>
              <tinCanActivityId><![CDATA[scorm.com/GolfExample_TCAPI]]></tinCanActivityId>
            </course>
         */ 
        public CourseDetail(XmlDocument xmlDoc)
        {
            XmlElement courseDetail = (XmlElement) xmlDoc.GetElementsByTagName("course").Item(0);
            this.courseId = courseDetail.Attributes["id"].Value;
            this.numberOfVersions = Convert.ToInt32(courseDetail.Attributes["versions"].Value);
            this.numberOfRegistrations = Convert.ToInt32(courseDetail.Attributes["registrations"].Value);
            this.title = courseDetail.Attributes["title"].Value;

            versions = new List<CourseVersion>();
            XmlNodeList versionListItems = courseDetail.GetElementsByTagName("version");
            foreach (XmlElement version in versionListItems)
                this.versions.Add(new CourseVersion(version));

            tags = new List<string>();
            XmlNodeList tagDataList = courseDetail.GetElementsByTagName("tag");
            foreach (XmlElement tag in tagDataList)
                this.tags.Add(tag.InnerText);

            XmlNodeList learningStandardList = courseDetail.GetElementsByTagName("learningStandard");
            if (learningStandardList.Count > 0)
            {
                this.learningStandard = learningStandardList.Item(0).FirstChild.Value;
            }

            XmlNodeList tincanActivityIdList = courseDetail.GetElementsByTagName("tincanActivivityId");
            if (tincanActivityIdList.Count > 0)
            {
                this.tincanActivityId = tincanActivityIdList.Item(0).FirstChild.Value;
            }
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

        /// <summary>
        /// The list of course versions
        /// </summary>
        public List<CourseVersion> CourseVersions
        {
            get { return versions; }
        }

        /// <summary>
        /// The list of tags
        /// </summary>
        public List<String> Tags
        {
            get { return tags; }
        }

        /// <summary>
        /// Learning Standard (might not exist)
        /// </summary>
        public String LearningStandard
        {
            get { return learningStandard; }
        }

        /// <summary>
        /// tincanActivityId (might not exist)
        /// </summary>
        public String TincanActivityId
        {
            get { return tincanActivityId; }
        }
    }
}
