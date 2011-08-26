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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RusticiSoftware.HostedEngine.Client;
using System.Collections.Specialized;

namespace RusticiSoftware.HostedEngine.Client.WebControls
{
    /// <summary>
    /// A web control that offers an interface for importing courses into the SCORM Cloud.
    /// </summary>
    /// <remarks>
    /// This control is presented as an HTML form with a file input, but has several options to help
    /// configure it's behavior. With default options, it will upload the file specified
    /// to your upload area, then import the uploaded file under the course id that you
    /// have set through the CourseId property.
    /// When a successful import is complete, this control raises a 
    /// "Success" (Success(EventArgs)) event. Typically the user of this control will register
    /// a handler for this event in order to execute some code after the import. The course id
    /// imported is available as the UploadedCourseId property during this time, as well as
    /// the ImportResults property, which is a list of ImportResult objects.If the
    /// import fails for any reason, a "Fail" (Fail(EventArgs)) event will be raised. If this
    /// event is raised, the message describing the error is available through the ErrorMessage
    /// property.
    /// If the IsUpdate property is set, the import control offers an interface to update existing
    /// courses by overwriting their files. (Technical Note: A course's SCORM Manifest (imsmanifest.xml)
    /// will not be overwritten.) In this case, the course id set through the CourseId property
    /// must refer to an existing course. The display of the control can be manipulated with the properties
    /// RenderImportResults, RenderErrorMessages, and InlineStylesEnabled. The last option is particularly
    /// useful if you choose to render the display differently with CSS.
    /// 
    /// A note about usage: It's recommended to embed this control on a very "lightweight" page 
    /// (and then, if needed, embed that lightweight page onto a larger interface
    /// using an iframe). Also, this control *requires* that javascript is enabled on the
    /// browser. Follow those two rules and you don't have to worry about the details. If
    /// you are curious about the details, read below.
    /// 
    /// This control obtains the target URL for it's html form dynamically when
    /// submitted. It does so using an asynchronous javascript call to the URL of the page to which
    /// the control has been added. The control, at page initialization time, checks to see if it should
    /// respond to such a request, and possibly will respond by writing to the Response object and ending
    /// the response. Because this happens so early in the page life cycle, you probably won't have to
    /// worry about any unexpected performance hit associated with loading the whole page (since the response
    /// will end before any OnLoad events are even raised). But to ensure against unexpected behavior,
    /// it's recommended to embed the control on a page that doesn't do much of any other processing itself.
    /// 
    /// FinPa Update: 
    /// 1. Remove writer.Indent
    /// 2. Remove leading spaces/tabs for clientScript
    /// 3. Line 410: add class "actionText" for the action text, instead of hard coded style
    /// 4. Line 421: replace "Loading course" with "Loading"
    /// 5. Line 159-165, 386-389: Add UploadFilename property support through query string
    /// 6. Line 438: add span tag to show alert better
    /// </remarks>
    public class ImportControl : Control
    {
        private String _courseId;
        private String _uploadedCourseId;
        private String _permissionDomain;
        private List<ImportResult> _importResults;
        private String _errorMessage;
        private bool _isUpdate = false;
        private bool _useSmartUpdate = false;
        private bool _inlineStylesEnabled = true;
        private bool _renderImportResults = true;
        private bool _renderErrorMessages = true;

        /// <summary>
        /// The course id under which to import (or update) files.
        /// </summary>
        public String CourseId
        {
            get { return _courseId; }
            set { _courseId = value; }
        }

        /// <summary>
        /// The course id that was just used in the operation that
        /// raised the Success or Fail event.
        /// </summary>
        public String UploadedCourseId
        {
            get { return _uploadedCourseId; }
        }

        /// <summary>
        /// The FTP service related "Permission Domain" with which to associate the course.
        /// </summary>
        public String PermissionDomain
        {
            get { return _permissionDomain; }
            set { _permissionDomain = value; }
        }

        /// <summary>
        /// Results about the import that raised a Success or Fail event.
        /// </summary>
        public List<ImportResult> ImportResults
        {
            get { return _importResults; }
        }

        /// <summary>
        /// The error message, when the Fail event is raised.
        /// </summary>
        public String ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

        /// <summary>
        /// If false, import new course. If true, overwrite files in existing course.
        /// </summary>
        public bool IsUpdate
        {
            get { return _isUpdate; }
            set { _isUpdate = value; }
        }

        /// <summary>
        /// A potential future feature, which should, for now, remain false.
        /// </summary>
        public bool UseSmartUpdate
        {
            get { return _useSmartUpdate; }
            set { _useSmartUpdate = value; }
        }

        /// <summary>
        /// Use html style attributes to provide a default style.
        /// </summary>
        public bool InlineStylesEnabled
        {
            get { return _inlineStylesEnabled; }
            set { _inlineStylesEnabled = value; }
        }

        /// <summary>
        /// Automatically render import results as part of the control's display.
        /// </summary>
        public bool RenderImportResults
        {
            get { return _renderImportResults; }
            set { _renderImportResults = value; }
        }

        /// <summary>
        /// Automatically render error messages as part of the control's display.
        /// </summary>
        public bool RenderErrorMessages
        {
            get { return _renderErrorMessages; }
            set { _renderErrorMessages = value; }
        }

        /// <summary>
        /// FinPa Update: return uploaded filename for information
        /// </summary>
        public string UploadFilename
        {
            get { return this.Page.Request.QueryString["UploadFilename"]; }
        }

        public event EventHandler Success;
        public event EventHandler Fail;

        protected virtual void OnSuccess(EventArgs e)
        {
            if (Success != null) {
                Success(this, e);
            }
        }

        protected virtual void OnFail(EventArgs e)
        {
            if (Fail != null) {
                Fail(this, e);
            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!String.IsNullOrEmpty(this.Page.Request["GetUploadUrl"])) {
                string responseText;
                try {
                    string currentUrl = this.Page.Request.Url.ToString();
                    string baseUrl = currentUrl.Split('?')[0];
                    NameValueCollection qsParams = HttpUtility.ParseQueryString(this.Page.Request.QueryString.ToString());

                    qsParams.Remove("GetUploadUrl");
                    qsParams.Remove("success");
                    qsParams.Remove("location");
                    qsParams.Remove("errcode");
                    qsParams.Remove("msg");
                    qsParams.Remove("server");


                    UploadToken uploadToken = ScormCloud.UploadService.GetUploadToken();
                    qsParams.Set("server", uploadToken.server);

                    string currentUrlPlusCourseId = baseUrl + "?" + qsParams.ToString();
                    string uploadUrl = ScormCloud.UploadService.GetUploadUrl(currentUrlPlusCourseId, this.PermissionDomain, uploadToken);
                    responseText = uploadUrl;
                }
                catch (Exception exc) {
                    responseText = "error:" + exc.Message;
                }

                HttpResponse Response = this.Page.Response;
                Response.Clear();
                Response.Write(responseText);
                HttpContext.Current.ApplicationInstance.CompleteRequest();
                Response.End();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            String uploadControlCourseId = this.Page.Request["UploadControlCourseId"];
            if (!String.IsNullOrEmpty(uploadControlCourseId)) {
                
                try {
                    NameValueCollection qs = this.Page.Request.QueryString;

                    bool isUpdate = this.IsUpdate;
                    bool wasUploadSuccessful = Convert.ToBoolean(qs["success"]);
                    
                    if (wasUploadSuccessful) {
                        this._uploadedCourseId = uploadControlCourseId;

                        string server = qs["server"];
                        string location = qs["location"];

                        if (isUpdate) 
                            UpdateAssetsFromUploadedFile(server, location, this.UploadedCourseId);
                        else
                            ImportCourseFromUploadedFile(server, location, this.UploadedCourseId);
                    }
                    else {
                        String errorCode = qs["errcode"];
                        String message = qs["msg"];

                        SetErrorMessage(message, errorCode, null);
                        OnFail(new EventArgs());
                    }
                }
                catch (Exception ex) {
                    SetErrorMessage(ex.Message, null, ex);
                    OnFail(new EventArgs());
                }
            }
        }

        private void ImportCourseFromUploadedFile(string server, string path, string courseId)
        {
            ImportResult importResult;
            try {
                this._importResults = ScormCloud
                                        .CourseService
                                        .ImportUploadedCourse(
                                            courseId, path, null, 
                                            this.PermissionDomain, 
                                            server, true);

                importResult = this.ImportResults[0];
            }
            finally {
                ScormCloud.UploadService.DeleteFile(path);
            }

            if (!importResult.WasSuccessful) {
                SetErrorMessage(importResult.Message, null, null);
                OnFail(new EventArgs());
            }
            else {
                OnSuccess(new EventArgs());
            }
        }

        private void UpdateAssetsFromUploadedFile(string server, string path, string courseId)
        {
            try {
                if (UseSmartUpdate) {
                    //Right now this is a test track specific feature, a back door
                    //to mimic traditional functionality wherein a course can be reimported
                    //with a new manifest. This may be useful generally in the future,
                    //but for now is not a documented feature.
                    ServiceRequest request = ScormCloud.CreateNewRequest();
                    if (server != null) {
                        request.Server = server;
                    }
                    request.Parameters.Add("courseid", courseId);
                    request.Parameters.Add("path", path);
                    request.Parameters.Add("smartupdate", "true");
                    request.CallService("rustici.course.updateAssets");
                }
                else {
                    ScormCloud.CourseService.UpdateAssetsFromUploadedFile(courseId, path, server);
                }
                OnSuccess(new EventArgs());
            }
            finally {
                ScormCloud.UploadService.DeleteFile(path);
            }
        }

        private void SetErrorMessage(string errMsg, string code, Exception ex)
        {
            StringBuilder msg = new StringBuilder();

            msg.Append(errMsg);
            if (!String.IsNullOrEmpty(code))
                msg.Append("<br><br>Code: " + code);
            if (ex != null)
                msg.Append("<span style=\"visibility:hidden\">StackTrace: " + ex.StackTrace + "</span>");

            ErrorMessage = msg.ToString();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            string currentUrl = this.Page.Request.Url.ToString();
            
            //When using the current url below, don't use the course id on the query string, use the
            //one set for this control. (This situation might come up after a page reload after successful import)
            string baseUrl = currentUrl.Split('?')[0];
            NameValueCollection qsParams = HttpUtility.ParseQueryString(this.Page.Request.QueryString.ToString());
            qsParams.Remove("UploadControlCourseId");
            qsParams.Add("UploadControlCourseId", HttpUtility.UrlEncode(this.CourseId));

            currentUrl = baseUrl + "?" + qsParams.ToString();


            string buttonText = this.IsUpdate ? "Update" : "Import";
            string actionText = this.IsUpdate ? "Upload file(s) for update" : "Import .zip package";

            string clientScript =
@"<script type='text/javascript'>
function " + this.ClientID + @"_Ajax (success, error) {
	this._req = null;  
 	this._sc = function() {
		if (_req.readyState == 4) {
                if(_req.status == 200){
                    success(_req.responseText);
                } else {
                    error(_req.responseText);
                }
		}
	};
	this.ajaxRequest = function(meth, url, as) {
		_req = window.ActiveXObject ? new ActiveXObject('Microsoft.XMLHTTP') : new XMLHttpRequest();
		_req.onreadystatechange = this._sc;
		_req.open(meth, url, as); 
		_req.send('');
	};
}
function " + this.ClientID + @"_Validate() {
    var fileToImportVal = document.getElementById('" + this.ClientID + @"_fileToImport').value;
    var isUpdate = " + this.IsUpdate.ToString().ToLower() + @";

    if (fileToImportVal.length == 0) {
        alert('Please select a file to import');
        return false;
    } 
    else if (!isUpdate && fileToImportVal.toLowerCase().indexOf('.zip') < 0) {
        alert('Please select a .zip file to import.  Selected file does not have an extension of \'.zip\'.');
        return false;
    } 
    else {
        document.getElementById('" + this.ClientID + @"_importFormDiv').style.display = 'none'; 
        document.getElementById('" + this.ClientID + @"_uploadingCourse').style.display = 'block';
        return true;
    }
}
function " + this.ClientID + @"_Submit() {
    if(" + this.ClientID + @"_Validate()) {
        var importFormAjax = new " + this.ClientID + "_Ajax(" + this.ClientID + @"_DataRecieved, " 
                                                                + this.ClientID + @"_Error);
        var fileToImportVal = document.getElementById('" + this.ClientID + @"_fileToImport').value;
        importFormAjax.ajaxRequest('GET', '" +
                currentUrl + (currentUrl.Contains("?") ? "&" : "?") +
                @"GetUploadUrl=true&UploadFilename=' + escape(fileToImportVal), true);
    }
    return false;
}
function " + this.ClientID + @"_DataRecieved(data) {
    if(data.indexOf('error:') >= 0){
        " + this.ClientID + @"_Error(data.substring(6));
        return false;
    }
    var importForm = document.getElementById('" + this.ClientID + @"');
    importForm.action = data;
    importForm.submit();
}
function " + this.ClientID + @"_Error(data) {
    document.getElementById('" + this.ClientID + @"_importFormDiv').style.display = 'block'; 
    document.getElementById('" + this.ClientID + @"_uploadingCourse').style.display = 'none';
    alert('An error occurred while uploading the file: ' + data);
}
</script>";

            writer.Write(clientScript);

            writer.WriteLine("<div id=\"" + this.ClientID + "_importFormDiv\" class=\"importFormDiv\">");

            //writer.Indent++;

            writer.WriteLine("<form id=\"" + this.ClientID + "\" class=\"importForm\" method=\"post\"" +
                             " onsubmit=\"return " + this.ClientID + "_Submit();\" enctype=\"multipart/form-data\">");
            //writer.Indent++;

            writer.WriteLine("<input type=\"file\" width=\"20\" id=\"" + this.ClientID + "_fileToImport\" name=\"fileToImport\" >");
            writer.WriteLine("<input type=\"submit\" id=\"" + this.ClientID + "_importBtn\" name=\"importBtn\" value=\"" + buttonText + "\" >");
            writer.WriteLine("<div class=\"actionText\">" + actionText + "</div>");

            //writer.Indent--;

            writer.WriteLine("</form>");

            //writer.Indent--;

            writer.WriteLine("</div>");

            writer.WriteLine("<div id=\"" + this.ClientID + "_uploadingCourse\" class=\"uploadingCourseDiv\" style=\"font-size: 10pt; font-weight: bold; display: none\">");
            writer.WriteLine("Uploading ......");
            writer.WriteLine("</div>");

            if (this.RenderErrorMessages && !String.IsNullOrEmpty(ErrorMessage)) {
                writer.WriteLine("<div id=\"" + this.ClientID + "_importErrors\" class=\"importErrors\" " +
                                            (InlineStylesEnabled ? "style=\"font-color:#880000\"" : "") + ">");
                writer.WriteLine("<span>" + ErrorMessage + "</span>");    // FLM Update: add span tag
                writer.WriteLine("</div>");
            }

            if (this.RenderImportResults && ImportResults != null && ImportResults.Count > 0 && ImportResults[0].WasSuccessful) {
                ImportResult result = ImportResults[0];
                writer.WriteLine("<div id=\"" + this.ClientID + "_importMessages\" class=\"importMessages\">");
                //writer.Indent++;
                
                writer.WriteLine("<div class=\"importResultTitle\" " +
                                    (InlineStylesEnabled ? "style=\"font-weight:bold\"" : "") + 
                                    ">Title: " + result.Title + "</div>");

                writer.WriteLine("<div class=\"importResultMessage\" " +
                                    (InlineStylesEnabled ? "style=\"font-weight:bold\"" : "") + 
                                    ">Message: " + result.Message + "</div>");
                if(result.ParserWarnings.Count > 0){
                    
                    writer.WriteLine("<div class=\"importParserWarningHeader\" " + 
                                        (InlineStylesEnabled ? "style=\"padding-top:8px; font-weight:bold\"" : "") + 
                                        ">Parser Warnings: </div>");

                    writer.WriteLine("<ul class=\"importParserWarningList\" " +
                                        (InlineStylesEnabled ? "style=\"margin-left:10px\"" : "") + ">");

                    //writer.Indent++;
                    foreach (String warning in result.ParserWarnings) {
                        writer.WriteLine("<li class=\"importParserWarning\">" + warning + "</li>");
                    }
                    //writer.Indent--;
                    writer.WriteLine("</ul>");
                }
                //writer.Indent--;
                writer.WriteLine("</div>");
            }
        }
    }
}

