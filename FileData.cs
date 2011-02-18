using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Data Transfer object that contains high-level information from the course listing
    /// service.
    /// </summary>
    public class FileData
    {
        private string name;
        private long size;
        private DateTime lastModified;

        private const string format = "yyyyMMddHHmmss";

        /// <summary>
        /// Purpose of this class is to map the return xml from the course listing
        /// web service into an object.  This is the main constructor.
        /// </summary>
        /// <param name="fileDataElement"></param>
        public FileData(XmlElement fileDataElement)
        {
            this.name = fileDataElement.Attributes["name"].Value;
            this.size = Convert.ToInt64(fileDataElement.Attributes["size"].Value);
            string modifiedStr = fileDataElement.Attributes["modified"].Value;
            this.lastModified = DateTime.ParseExact(modifiedStr, format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Helper method which takes the full XmlDocument as returned from the course listing
        /// web service and returns a List of CourseData objects.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static List<FileData> ConvertToFileDataList(XmlDocument xmlDoc)
        {
            List<FileData> allResults = new List<FileData>();

            XmlNodeList fileDataList = xmlDoc.GetElementsByTagName("file");
            foreach (XmlElement fileData in fileDataList)
            {
                allResults.Add(new FileData(fileData));
            }

            return allResults;
        }

        /// <summary>
        /// File Name
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// File Size
        /// </summary>
        public long Size
        {
            get { return size;  }
        }

        /// <summary>
        /// File Last Modified Date
        /// </summary>
        public DateTime LastModified
        {
            get { return lastModified; }
        }
    }
}