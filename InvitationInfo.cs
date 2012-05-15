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
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    public class InvitationInfo
    {
        private String _id;
        private String[] _errors = new String[0];
        private bool _public;
        private bool _allowNewRegistrations;
        private bool _allowLaunch;
        private UserInvitationStatus[] _userInvitations = new UserInvitationStatus[0];
        private String _url;
        private bool _created;
        private String _message;
        private String _subject;
        private String _courseId;
        private DateTime _createDate;

        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public String[] Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        public bool Public
        {
            get { return _public; }
            set { _public = value; }
        }

        public bool AllowNewRegistrations
        {
            get { return _allowNewRegistrations; }
            set { _allowNewRegistrations = value; }
        }
        
        public bool AllowLaunch
        {
            get { return _allowLaunch; }
            set { _allowLaunch = value; }
        }

        public UserInvitationStatus[] UserInvitations
        {
            get { return _userInvitations; }
            set { _userInvitations = value; }
        }

        public String Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public bool Created
        {
            get { return _created; }
            set { _created = value; }
        }

        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public String Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public String CourseId
        {
            get { return _courseId; }
            set { _courseId = value; }
        }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }
        
        public static List<InvitationInfo> parseListFromXml (XmlDocument invListXml) {
		    XmlElement invListElem = (XmlElement)invListXml.GetElementsByTagName("invitationlist").Item(0);
		    List<InvitationInfo> invList = new List<InvitationInfo>();
		    XmlNodeList invElems = invListElem.GetElementsByTagName("invitationInfo");
            foreach (XmlElement invData in invElems)
            {
                invList.Add(new InvitationInfo(invData));
		    }
		    return invList;
	    }
    	
	    public InvitationInfo(XmlElement invitationInfo){
		    
		    this.Id = XmlUtils.GetChildElemText(invitationInfo, "id");
    		this.AllowLaunch = bool.Parse(XmlUtils.GetChildElemText(invitationInfo, "allowLaunch"));
		    this.AllowNewRegistrations = bool.Parse(XmlUtils.GetChildElemText(invitationInfo, "allowNewRegistrations"));
            this.Created = bool.Parse(XmlUtils.GetChildElemText(invitationInfo, "created"));
    		this.Public = bool.Parse(XmlUtils.GetChildElemText(invitationInfo, "public"));
		    this.Message = XmlUtils.GetChildElemText(invitationInfo, "body");
		    this.Subject = XmlUtils.GetChildElemText(invitationInfo, "subject");
		    this.Url = XmlUtils.GetChildElemText(invitationInfo, "url");
    		this.CourseId = XmlUtils.GetChildElemText(invitationInfo,"courseId");
            this.CreateDate = DateTime.Parse(XmlUtils.GetChildElemText(invitationInfo,"createdDate"));
    		
		    XmlNodeList errElems = invitationInfo.GetElementsByTagName("errors").Item(0).ChildNodes;
    	    String[] errors = new String[errElems.Count];
            int idx = 0;
		    foreach (XmlElement errs in errElems)
            {
			    errors[idx] = errs.InnerText;
                idx++;
		    }
            this.Errors = errors;
		    this.UserInvitations = parseUserInvitations((XmlElement)invitationInfo.GetElementsByTagName("userInvitations").Item(0));
    		
	    }


        private static UserInvitationStatus[] parseUserInvitations(XmlElement userInvitations)
        {
		    if (userInvitations == null) {
			    return new UserInvitationStatus[0];
		    }
            XmlNodeList nl = userInvitations.GetElementsByTagName("userInvitation");
		    UserInvitationStatus[] userInvitationStatuses = new UserInvitationStatus[nl.Count];
            int idx = 0;
		    foreach (XmlElement ui in nl)
            {
                XmlElement userInvitation = ui;
                userInvitationStatuses[idx] = new UserInvitationStatus();
                userInvitationStatuses[idx].Email = XmlUtils.GetChildElemText(userInvitation, "email");
                userInvitationStatuses[idx].IsStarted = bool.Parse(XmlUtils.GetChildElemText(userInvitation, "isStarted"));
                userInvitationStatuses[idx].RegistrationId = XmlUtils.GetChildElemText(userInvitation, "registrationId");
                userInvitationStatuses[idx].Url = XmlUtils.GetChildElemText(userInvitation, "url");
                XmlElement regReport = (XmlElement)userInvitation.GetElementsByTagName("registrationreport").Item(0);
			    if (regReport != null) {
                    userInvitationStatuses[idx].RegSummary = new RegistrationSummary(regReport);
			    }
                idx++;
		    }
		    return userInvitationStatuses; 
	    }

        
    }
}
