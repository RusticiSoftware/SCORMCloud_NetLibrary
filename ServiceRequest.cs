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
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Threading;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Main class used to invoke Hosted Engine web service methods.
    /// </summary>
    public class ServiceRequest
    {
        private IDictionary<string, object> methodParameters = new Dictionary<string, object>();
        private string fileToPost = null;
        private Configuration configuration = null;
        private String engineServiceUrl = null;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Application Configuration</param>
        public ServiceRequest(Configuration configuration)
        {
            this.configuration = configuration;
            //Keep local copy of services url in case we need to change
            //it (in just this request), like setting a particular server
            this.engineServiceUrl = configuration.ScormEngineServiceUrl;
        }

        /// <summary>
        /// Method-specific parameters for the web service method
        /// </summary>
        public IDictionary<string, object> Parameters
        {
            get { return methodParameters; }
            set { methodParameters = value; }
        }

        /// <summary>
        /// Optional full file path to the source file if required for the
        /// web method
        /// </summary>
        public string FileToPost
        {
            get
            {
                 return fileToPost;
            }
            set
            {
                if (!File.Exists(value))
                {
                    throw new ArgumentException(
                        String.Format("Path provided for FileToPost does not point to an existing file.  Value: '{0}'",
                                      value));
                }

                fileToPost = value;
            }
        }


        /// <summary>
        /// Server at which to make the request
        /// </summary>
        public string Server
        {
            get
            {
                String serviceUrl = this.configuration.ScormEngineServiceUrl;
                int beginIndex = serviceUrl.IndexOf("://");

                String server = serviceUrl.Substring(beginIndex + 3);
                int endIndex = server.IndexOf("/");
                if (endIndex == -1) {
                    endIndex = server.Length;
                }
                return server.Substring(0, endIndex);
            }
            set
            {
                this.engineServiceUrl = this.engineServiceUrl.Replace(this.Server, value);
            }
        }

        /// <summary>
        /// Main method for invoking HostedEngine web services based on the 
        /// HostedEngine.WebServicesCore engine
        /// </summary>
        /// <param name="methodName">Method name passed on the call to the api</param>
        /// <returns>Response string unless response indicstes an error.  If the web service
        /// response indicates a failure, a ServiceException is thrown</returns>
        public XmlDocument CallService(string methodName)
        {
            return GetXmlResponseFromUrl(ConstructUrl(methodName));
        }

        public string GetFileFromService(string toFileName, string methodName)
        {
            return GetFileResponseFromUrl(toFileName, ConstructUrl(methodName));
        }

        public string GetFileResponseFromUrl(string toFileName, string url)
        {
            byte[] responseBytes = GetResponseFromUrl(url);

            FileStream fs = File.Create(toFileName);
            fs.Write(responseBytes, 0, responseBytes.Length);
            fs.Close();
            return toFileName;
        }

        /// <summary>
        /// Get the xml response from a fully prepared (signed) service call URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public XmlDocument GetXmlResponseFromUrl(string url)
        {
            byte[] responseBytes = GetResponseFromUrl(url);

            string responseText = Encoding.GetEncoding("utf-8").GetString(responseBytes);

            XmlDocument response = AssertNoErrorAndReturnXmlDoc(responseText);

            //            // debug!
            //            string path = @"C:\Documents and Settings\jhayden\Desktop\response.xml";
            //            if (File.Exists(path)) File.Delete(path);
            //            using (TextWriter tw = new StreamWriter(path))
            //                tw.Write(response.OuterXml);

            return response;
        }

        public byte[] GetResponseFromUrl(string url)
        {
            // If we have a file to post, do a POST with the file as the payload, otherwise
            // do a simple GET
            byte[] responseBytes;

            int retries = 6;
            int msWait = 200;

            while (retries > 0) {
                try {
                    //TODO: WebClient nicely wraps this functionaly but if we need more power
                    // or diagnostics we might move to other WebRequest types that are available.
                    using (CustomWebClient wc = new CustomWebClient()) {
                        if (fileToPost != null)
                            responseBytes = wc.UploadFile(url, fileToPost);
                        else
                            responseBytes = wc.DownloadData(url);
                    }
                    return responseBytes;
                }
                catch (WebException) {
                    Thread.Sleep(msWait);
                    retries--;
                    msWait *= 2;
                    if (retries == 0) {
                        throw;
                    }
                }
            }
            throw new Exception("Could not retrieve a response from " + this.Server);
        }

        /// <summary>
        /// This method will evaluate the reponse string and manually validate
        /// the top-level structure.  If an err is present, this will be turned
        /// into a Service Exception.
        /// </summary>
        /// <param name="xmlString">Response from web service as xml</param>
        /// <returns>XML document from the given string, provided no service errors are present</returns>
        private static XmlDocument AssertNoErrorAndReturnXmlDoc(string xmlString)
        {
            if (!xmlString.StartsWith("<?xml"))
            {
                throw new ServiceException(ErrorCode.INVALID_WEB_SERVICE_RESPONSE, "Expecting XML Response from web service call, instead received: " + xmlString);
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlString);
            XmlNodeList rspElements = xmlDoc.GetElementsByTagName("rsp");
            if (rspElements.Count == 0)
            {
                throw new ServiceException(ErrorCode.INVALID_WEB_SERVICE_RESPONSE, "Invalid XML Response from web service call, expected <rsp> tag, instead received: " + xmlString);
            }

            XmlAttribute statusAttr = rspElements[0].Attributes["stat"];
            if (statusAttr == null || statusAttr.Value == null)
            {
                throw new ServiceException(ErrorCode.INVALID_WEB_SERVICE_RESPONSE, "Invalid XML Response from web service call, expected 'stat' attribute on <rsp> tag, instead received: " + xmlString);
            }

            string status = statusAttr.Value.ToLower();
            if (status != "ok")
            {
                if (status!= "fail")
                {
                    throw new ServiceException(ErrorCode.INVALID_WEB_SERVICE_RESPONSE, "Invalid XML Response from web service call, expected 'stat' value of 'ok' or 'fail' attribute on <rsp> tag, instead received stat value: " + status);
                }

                XmlNode errNode = rspElements[0].FirstChild;
                if (errNode.Name != "err")
                {
                    throw new ServiceException(ErrorCode.INVALID_WEB_SERVICE_RESPONSE, "Invalid XML Response from web service call, expected <err> node since stat='fail', instead received : " + xmlString);
                }

                throw new ServiceException(Convert.ToInt32(errNode.Attributes["code"].Value), errNode.Attributes["msg"].Value);
            }

            return xmlDoc;
        }

        /// <summary>
        /// Given the method name and the parameters and configuration associated 
        /// with this object, generate the full URL for the web service invocation.
        /// </summary>
        /// <param name="methodName">Method name for the HOSTED Engine api call</param>
        /// <returns>Fully qualified URL to be used for invocation</returns>
        public string ConstructUrl(string methodName)
        {
            // The local parameter map just contains method methodParameters.  We'll
            // now create a complete parameter map that contains the web-service
            // params as well the actual method params.
            IDictionary<string, object> parameterMap = new Dictionary<string, object>();
            parameterMap.Add("method", methodName);
            parameterMap.Add("appid", configuration.AppId);
            parameterMap.Add("ts", DateTime.UtcNow.ToString("yyyyMMddHHmmss"));
            foreach(string key in methodParameters.Keys)
            {
                parameterMap.Add(key, methodParameters[key]);
            }

            // Construct the url, concatonate all parameters as query string parameters
            string url = this.engineServiceUrl + "/api";
            int cnt = 0;
            foreach(string key in parameterMap.Keys)
            {
                // Create a query string with URL-encoded values
                url += (cnt++ == 0 ? "?" : "&") + key + "=" + HttpUtility.UrlEncode(parameterMap[key].ToString());
            }
            url += "&sig=" + RequestSigner.GetSignatureForRequest(configuration.SecurityKey, parameterMap);

            if (url.Length > 2000)
                throw new ApplicationException("URL > 2000 bytes");

            return url;
        }
    }
}
