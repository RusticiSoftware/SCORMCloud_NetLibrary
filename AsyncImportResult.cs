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
