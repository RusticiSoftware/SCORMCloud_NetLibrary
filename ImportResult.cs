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
