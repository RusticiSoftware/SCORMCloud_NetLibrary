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
        private string registrationId;
        private string courseId;
       // private int numberOfInstances;

        /// <summary>
        /// Constructor which takes an XML node as returned by the web service.
        /// </summary>
        /// <param name="regDataEl"></param>
        public RegistrationData(XmlElement regDataEl)
        {
            this.registrationId = regDataEl.Attributes["id"].Value;
            this.courseId = regDataEl.Attributes["courseid"].Value;
            //this.numberOfInstances = Convert.ToInt32(regDataEl.Attributes["instances"].Value);
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
            get { return registrationId; }
        }

        /// <summary>
        /// Unique Identifier for this course
        /// </summary>
        public string CourseId
        {
            get { return courseId; }
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
