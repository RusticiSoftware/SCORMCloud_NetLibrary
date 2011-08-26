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
    /// Holds the results of a course import. Useful for tracking the success of the individual courses during a bulk import.
    /// </summary>
    [Serializable]
    public class ImportResult
    {
        private string title = "";
        private bool wasSuccessful = false;
        private string message = "";
        private List<string> parserWarnings = new List<string>();

        /// <summary>
        /// Xml Constructor that takes the response XML as returned by one of the import-related
        /// web services.
        /// </summary>
        /// <param name="irXml">importresult Element</param>
        public ImportResult(XmlElement irXml)
        {
            this.wasSuccessful = Convert.ToBoolean(irXml.Attributes["successful"].Value);

            foreach(XmlElement child in irXml.ChildNodes)
            {
                switch (child.Name)
                {
                    case "title" :
                        this.title = child.InnerText;
                        break;
                    case "message" :
                        this.message = child.InnerText;
                        break;
                    case "parserwarnings":
                        foreach (XmlElement w in child.ChildNodes)
                        {
                            this.parserWarnings.Add(w.InnerText);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Helper method that takes the entire web service response document and
        /// returns a List of one or more ImportResults.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static List<ImportResult> ConvertToImportResults(XmlDocument xmlDoc)
        {
            List<ImportResult> allResults = new List<ImportResult>();
            
            XmlNodeList importResults = xmlDoc.GetElementsByTagName("importresult");
            foreach (XmlElement ir in importResults)
            {
                allResults.Add(new ImportResult(ir));
            }

            return allResults;
        }

        /// <summary>
        /// The Title of the course that was imported as derived from the manifest
        /// </summary>
        public string Title
        {
            get { return title; }
        }

        /// <summary>
        /// Indicates whether or not the import had any errors
        /// </summary>
        public bool WasSuccessful
        {
            get { return wasSuccessful; }
        }

        /// <summary>
        /// More information regarding the success or failure of the import.
        /// </summary>
        public string Message
        {
            get {return message; }
        }

        /// <summary>
        /// Warnings issued during import process related to the structure of the manifest.
        /// </summary>
        public List<string> ParserWarnings
        {
            get { return parserWarnings; }
        }
    }
}
