using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    public class AsyncImportResult
    {
        public enum ImportStatus { CREATED, RUNNING, FINISHED, ERROR };

        private ImportStatus status = ImportStatus.CREATED;
        private List<ImportResult> importResults;
        private String errorMessage;

        public ImportStatus Status
        {
            get { return status; }
        }
        public List<ImportResult> ImportResults
        {
            get { return importResults; }
        }
        public String ErrorMessage
        {
            get { return errorMessage; }
        }

        public AsyncImportResult(XmlDocument asyncImportResultXml)
        {
            String statusText = ((XmlElement)asyncImportResultXml
                                        .GetElementsByTagName("status")[0])
                                        .InnerText;
            
            if (statusText.Equals("created")) {
                this.status = ImportStatus.CREATED;
            } else if (statusText.Equals("running")) {
                this.status = ImportStatus.RUNNING;
            } else if (statusText.Equals("finished")) {
                this.status = ImportStatus.FINISHED;
            } else if (statusText.Equals("error")) {
                this.status = ImportStatus.ERROR;
            }

            if (this.status == ImportStatus.FINISHED) {
                this.importResults = ImportResult.ConvertToImportResults(asyncImportResultXml);
            }

            if (this.status == ImportStatus.ERROR) {
                this.errorMessage = ((XmlElement)asyncImportResultXml
                                        .GetElementsByTagName("error")[0])
                                        .InnerText;
            }
        }

        public Boolean IsComplete()
        {
            return ((this.Status == ImportStatus.FINISHED) || (this.Status == ImportStatus.ERROR));
        }

        public Boolean HasError()
        {
            return (this.Status == ImportStatus.ERROR);
        }
    }
}
