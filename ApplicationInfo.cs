/* Software License Agreement (BSD License)
 * 
 * Copyright (c) 2010-2012, Rustici Software, LLC
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
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RusticiSoftware.HostedEngine.Client
{
    public class ApplicationInfo
    {
        public string AppId { get; set; }
        public string Name { get; set; }
        public bool AllowDeleteApi { get; set; }
        public bool AllowUpdateApi { get; set; }
        public DateTime CreateDate { get; set; }
        public SecretKey[] SecretKeys { get; set; } = new SecretKey[0];

        public static List<ApplicationInfo> parseListFromXml(XmlDocument appListXml)
        {
            XmlElement appListElem = (XmlElement)appListXml.GetElementsByTagName("applicationlist").Item(0);
            List<ApplicationInfo> appList = new List<ApplicationInfo>();
            if (appListElem != null)
            {
                XmlNodeList invElems = appListElem.GetElementsByTagName("application");
                foreach (XmlElement invData in invElems)
                {
                    appList.Add(new ApplicationInfo(invData));
                }
            }

            return appList;
        }


        public ApplicationInfo(XmlElement applicationInfo)
        {
            this.AppId = XmlUtils.GetChildElemText(applicationInfo, "appId");
            this.Name = XmlUtils.GetChildElemText(applicationInfo, "name");
            this.CreateDate = DateTime.Parse(XmlUtils.GetChildElemText(applicationInfo, "createDate"));

            var allowDeleteApi = XmlUtils.GetChildElemText(applicationInfo, "allowDeleteAPI");
            this.AllowDeleteApi = allowDeleteApi != null && bool.Parse(allowDeleteApi);

            var allowUpdateApi = XmlUtils.GetChildElemText(applicationInfo, "allowUpdateAPI");
            this.AllowUpdateApi = allowUpdateApi != null && bool.Parse(allowUpdateApi);

            this.SecretKeys = ParseSecretKeys((XmlElement)applicationInfo.GetElementsByTagName("secretKeys").Item(0));
        }

        private static SecretKey[] ParseSecretKeys(XmlElement secretKeyElements)
        {
            if (secretKeyElements == null)
                return new SecretKey[0];

            XmlNodeList nl = secretKeyElements.GetElementsByTagName("secretKey");
            SecretKey[] secretKeys = new SecretKey[nl.Count];
            int idx = 0;
            foreach (XmlElement sk in nl)
            {
                secretKeys[idx] = new SecretKey(sk);
                idx++;
            }

            return secretKeys;
        }


    }
}
