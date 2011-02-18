using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    public class Export
    {
        public static String STATUS_UNKNOWN = "unknown";
        public static String STATUS_STARTED = "started";
        public static String STATUS_CANCELED = "canceled";
        public static String STATUS_COMPLETE = "complete";
        public static String STATUS_ERROR = "error";
        public static String STATUS_EXPIRED = "expired";

        private String id;
        private String appId;
        private String status = STATUS_UNKNOWN;
        private DateTime startDate;
        private DateTime endDate;
        private double percentComplete = 0.0;
        private String serverLocation = "unavailable";

        public String Id
        {
          get { return id; }
          set { id = value; }
        }     

        public String AppId
        {
          get { return appId; }
          set { appId = value; }
        }        

        public String Status
        {
          get { return status; }
          set { status = value; }
        }        

        public DateTime StartDate
        {
          get { return startDate; }
          set { startDate = value; }
        }        

        public DateTime EndDate
        {
          get { return endDate; }
          set { endDate = value; }
        }        

        public double PercentComplete
        {
          get { return percentComplete; }
          set { percentComplete = value; }
        }        

        public String ServerLocation
        {
          get { return serverLocation; }
          set { serverLocation = value; }
        }

        public static Export ParseFromXmlElement(XmlElement exportElem)
        {
            Export export = new Export();
            export.Id  = exportElem.GetAttribute("id");
            
            export.AppId = XmlUtils.GetChildElemText(exportElem, "appid");
            export.Status = XmlUtils.GetChildElemText(exportElem, "status");
            String startDate = XmlUtils.GetChildElemText(exportElem, "start_date");
            export.StartDate = Utils.ParseIsoDate(startDate);
            if(export.Status.Equals(Export.STATUS_COMPLETE)){
                String endDate = XmlUtils.GetChildElemText(exportElem, "end_date");
                if(endDate != null){
                    export.EndDate = Utils.ParseIsoDate(endDate);
                }
            }
            
            export.PercentComplete =  Double.Parse(XmlUtils.GetChildElemText(exportElem, "percent_complete"));
            
            export.ServerLocation = XmlUtils.GetChildElemText(exportElem, "server_location");
            
            return export;
        }
        
        public static List<Export> ParseExportListFromXml(XmlElement exportListElem)
        {
            List<Export> exports = new List<Export>();
            if(exportListElem != null){
                List<XmlElement> exportList = XmlUtils.GetChildrenByTagName(exportListElem, "export");
                foreach (XmlElement export in exportList){
                    exports.Add(Export.ParseFromXmlElement(export));
                }
            }
            return exports;
        }
    }
}
