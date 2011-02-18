using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    public class ExportService
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public ExportService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        /// <summary>
        /// Calling this method will start a new data export, and return an id 
        /// that can be used to check on the status of the export using a call to Status
        /// </summary>
        /// <returns>The unique id for the started data export</returns>
        public string Start()
        {
            ServiceRequest sr = new ServiceRequest(this.configuration);
            XmlDocument response = sr.CallService("rustici.export.start");
            XmlElement elem = (XmlElement)response.GetElementsByTagName("export_id")[0];
            return elem.InnerText;
        }

        /// <summary>
        /// Calling this method will cancel the export with the passed in export id.
        /// </summary>
        /// <param name="exportId"></param>
        /// <returns>True if successfully canceled</returns>
        public bool Cancel(String exportId)
        {
            ServiceRequest sr = new ServiceRequest(this.configuration);
            sr.Parameters.Add("exportid", exportId);
            sr.CallService("rustici.export.cancel");
            return true;
        }
        
        /// <summary>
        /// A call to this method will return some detailed information about an export, 
        /// including the export status (started, canceled, complete, error), the start date, 
        /// the end date (if complete), the percent complete, and the server from which the 
        /// export should be downloaded.
        /// </summary>
        /// <param name="exportId"></param>
        /// <returns>An Export object containing information about the export</returns>
        public Export Status(String exportId)
        {
            ServiceRequest sr = new ServiceRequest(this.configuration);
            sr.Parameters.Add("exportid", exportId);
            XmlDocument response = sr.CallService("rustici.export.status");
            XmlElement exportElem = (XmlElement)response.GetElementsByTagName("export")[0];
            return Export.ParseFromXmlElement(exportElem);
        }
        
        /// <summary>
        /// Calling this method returns the actual export data for a given export. Note that the export must 
        /// be complete in order to download it's associated data.
        /// </summary>
        /// <param name="toFileName">The file path to write the downloaded export to</param>
        /// <param name="export">An Export object containing information about the export to download</param>
        /// <returns>The filepath where the data was written</returns>
        public String Download(String toFileName, Export export)
        {
            ServiceRequest sr = new ServiceRequest(this.configuration);
            sr.Parameters.Add("exportid", export.Id);
            sr.Server = export.ServerLocation;
            return sr.GetFileFromService(toFileName, "rustici.export.download");
        }

        /// <summary>
        /// Calling this method returns a URL to the actual export data for a given export. Note that the export must 
        /// be complete in order to download it's associated data.
        /// </summary>
        /// <param name="export">An Export object containing information about the export to download</param>
        /// <returns>A URL from which the export data can be downloaded</returns>
        public String GetDownloadUrl(Export export)
        {
            ServiceRequest sr = new ServiceRequest(this.configuration);
            sr.Parameters.Add("exportid", export.Id);
            sr.Server = export.ServerLocation;
            return sr.ConstructUrl("rustici.export.download");
        }
        
        /// <summary>
        /// This method returns a list of export data for all exports current and historical. 
        /// </summary>
        /// <returns></returns>
        public List<Export> List()
        {
            ServiceRequest sr = new ServiceRequest(this.configuration);
            XmlDocument doc = sr.CallService("rustici.export.list");
            XmlElement elem = (XmlElement)doc.GetElementsByTagName("exports")[0];
            return Export.ParseExportListFromXml(elem);
        }
    }
}
