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
using System.Collections.ObjectModel;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Client-side proxy for the "rustici.registration.*" Hosted SCORM Engine web
    /// service methods.  
    /// </summary>
    public class InvitationService
    {
        private Configuration configuration = null;
        private ScormEngineService manager = null;

        /// <summary>
        /// Main constructor that provides necessary configuration information
        /// </summary>
        /// <param name="configuration">Application Configuration Data</param>
        public InvitationService(Configuration configuration, ScormEngineService manager)
        {
            this.configuration = configuration;
            this.manager = manager;
        }

        /// <summary>
        /// Creates a new invitation
        /// </summary>
        /// <param name="courseId">courseId of the course invited to</param>
        /// <param name="publicInvitation">whether the invite is public</param>
        /// <param name="send">whether to send the nvite</param>
        /// <param name="addresses">comma-separated list of email addresses to sent he invite to</param>
        /// <param name="emailSubject">subject of the email</param>
        /// <param name="emailBody">message body of the email</param>
        /// <param name="creatingUserEmail">email of the SCORM Cloud user creating the invite</param>
        /// <param name="async">whether to send the (private) invites asynchronously</param>
        /// <returns>new invitation id</returns>
        public string CreateInvitation(string courseId, bool publicInvitation, bool send,
            string addresses, string emailSubject, string emailBody, string creatingUserEmail, bool async)
        {
            return CreateInvitation(courseId, publicInvitation, send, addresses, emailSubject, emailBody, creatingUserEmail, 0, null,
                null, null, null, null, async);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseId">courseId of the course invited to</param>
        /// <param name="publicInvitation">whether the invite is public</param>
        /// <param name="send">whether to send the nvite</param>
        /// <param name="addresses">comma-separated list of email addresses to sent he invite to</param>
        /// <param name="emailSubject">subject of the email</param>
        /// <param name="emailBody">message body of the email</param>
        /// <param name="creatingUserEmail">email of the SCORM Cloud user creating the invite</param>
        /// <param name="registrationCap">the max number of public invitations to make available</param>
        /// <param name="postbackUrl">url to post back invite regs to</param>
        /// <param name="authType">auth type for postbacks</param>
        /// <param name="urlName">username for postbacks</param>
        /// <param name="urlPass">password for postbacks</param>
        /// <param name="resultsFormat">results format for postbacks</param>
        /// <param name="async">whether to send the (private) invites asynchronously</param>
        /// <returns>new invitation id</returns>
        public string CreateInvitation(string courseId, bool publicInvitation, bool send,
            string addresses, string emailSubject, string emailBody, string creatingUserEmail, int registrationCap,
            string postbackUrl, string authType, string urlName, string urlPass, string resultsFormat,
            bool async)
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("courseid", courseId);
            request.Parameters.Add("public", publicInvitation.ToString().ToLower());
            request.Parameters.Add("send", send.ToString().ToLower());
        
            request.Parameters.Add("registrationCap", registrationCap);
            
            if (addresses != null && addresses.Length > 0)
                request.Parameters.Add("addresses", addresses);
            if (emailSubject != null && emailSubject.Length > 0)
                request.Parameters.Add("emailSubject", emailSubject);
            if (emailBody != null && emailBody.Length > 0)
                request.Parameters.Add("emailBody", emailBody);
            if (creatingUserEmail != null && creatingUserEmail.Length > 0)
                request.Parameters.Add("creatingUserEmail", creatingUserEmail);
            if (postbackUrl != null && postbackUrl.Length > 0)
                request.Parameters.Add("postbackUrl", postbackUrl);
            if (authType != null && authType.Length > 0)
                request.Parameters.Add("authType", authType);
            if (urlName != null && urlName.Length > 0)
                request.Parameters.Add("urlName", urlName);
            if (urlPass != null && urlPass.Length > 0)
                request.Parameters.Add("urlPass", urlPass);
            if (resultsFormat != null && resultsFormat.Length > 0)
                request.Parameters.Add("resultsFormat", resultsFormat);
        
            if (async){
        	    return request.CallService("rustici.invitation.createInvitationAsync").DocumentElement.InnerText;
            } else {
                return request.CallService("rustici.invitation.createInvitation").DocumentElement.InnerText;
            }

        }

        /// <summary>
        /// Get the status of any asynchronous jobs for sending private emails for an invite
        /// </summary>
        /// <param name="invitationId">invitation id of invite in question</param>
        /// <returns>job status value</returns>
        public String GetInvitationStatus(string invitationId) 
        {
		    ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("invitationId", invitationId);
            
            XmlDocument response = request.CallService("rustici.invitation.getInvitationStatus");
            return response.DocumentElement.InnerText;
		
        }

        /// <summary>
        /// Get the invitation info for the invite
        /// </summary>
        /// <param name="invitationId">invitationId of invite inquiring about</param>
        /// <param name="includeRegistrationSummary">whether to include reg summaries for any user invitations returned</param>
        /// <returns>invitation info object</returns>
        public InvitationInfo GetInvitationInfo(string invitationId, bool includeRegistrationSummary) 
        {
            ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("invitationId", invitationId);
            request.Parameters.Add("detail", includeRegistrationSummary.ToString().ToLower());

            XmlDocument response = request.CallService("rustici.invitation.getInvitationInfo");
            return new InvitationInfo((XmlElement)response.GetElementsByTagName("invitationInfo").Item(0));
	    }
    	
    	/// <summary>
    	/// Get a list of invitations that meet the filtering criteria.  no filtering will return all
    	/// </summary>
    	/// <param name="invFilter">filter for the invitation id</param>
    	/// <param name="coursefilter">filter for the courseId</param>
    	/// <returns>a list of InvitationInfo objects</returns>
	    public List<InvitationInfo> GetInvitationList(String invFilter, String coursefilter)
        {
		    ServiceRequest request = new ServiceRequest(configuration);
		    if (invFilter != null) {
                request.Parameters.Add("filter", invFilter);
            }
            if (coursefilter != null) {
                request.Parameters.Add("coursefilter", coursefilter);
            }

            XmlDocument response = request.CallService("rustici.invitation.getInvitationList");
            return InvitationInfo.parseListFromXml(response);
        }
      	
        /// <summary>
        /// Change the accessibility status of the invitation
        /// </summary>
        /// <param name="invitationId">invitationId of the invite in question</param>
        /// <param name="enable">whether to set the invite as launchable</param>
        /// <param name="open">whether new regs can be created against the invite (for public invites only)</param>
	    public void ChangeStatus(String invitationId, bool enable, bool open)
        {
		    ServiceRequest request = new ServiceRequest(configuration);
            request.Parameters.Add("invitationId", invitationId);
            request.Parameters.Add("enable", enable.ToString().ToLower());
            request.Parameters.Add("open", open.ToString().ToLower());
            
		    request.CallService("rustici.invitation.changeStatus");
    		
        }

    }
}
