using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    public class CourseVersion
    {
        private String versionId;
        private String updateDate;

        public CourseVersion(XmlElement versionElement)
        {
            this.versionId = versionElement.GetElementsByTagName("versionId").Item(0).FirstChild.Value;
            this.updateDate = versionElement.GetElementsByTagName("updateDate").Item(0).FirstChild.Value;
        }

        /// <summary>
        /// Version Identifier
        /// </summary>
        public string VersionId
        {
            get { return versionId; }
        }

        /// <summary>
        /// Update Date
        /// </summary>
        public string UpdateDate
        {
            get { return updateDate; }
        }
    }
}
