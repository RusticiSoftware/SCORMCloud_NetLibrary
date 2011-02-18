using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    

    /// <summary>
    /// Data Transfer object that contains information about upload "tokens"
    /// </summary>
    public class UploadToken
    {
        public string server;
        public string tokenId;

        /// <summary>
        /// Helper method which takes the full XmlDocument as returned from the course listing
        /// web service and returns a List of CourseData objects.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public UploadToken(XmlDocument xmlDoc)
        {
            XmlNodeList tokenList = xmlDoc.GetElementsByTagName("token");
            if (tokenList.Count > 0) {
                XmlElement token = (XmlElement)tokenList[0];
                XmlElement server = (XmlElement)token.GetElementsByTagName("server")[0];
                XmlElement id = (XmlElement)token.GetElementsByTagName("id")[0];

                this.server = server.InnerText;
                this.tokenId = id.InnerText;
            }
        }
    }
}
