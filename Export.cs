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
