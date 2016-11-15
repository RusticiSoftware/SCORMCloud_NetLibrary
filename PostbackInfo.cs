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
  public class PostbackInfo
  {
    private string _registrationId;
    private string _url;
    private string _registrationResultsAuthType;
    private string _learnerLogin;
    private string _learnerPassword;


    /// <summary>
    /// Inflate launch info object from passed in xml element
    /// </summary>
    /// <param name="launchInfoElem"></param>
    /// <param name="postbackInfoElem"></param>
    public PostbackInfo(XmlElement postbackInfoElem)
    {
      this.RegistrationId = postbackInfoElem.GetAttribute("regid");

      this.Url = ((XmlElement)postbackInfoElem.GetElementsByTagName("url")[0]).InnerText;
      //this.Url = postbackInfoElem.GetAttribute("url");
      this.LearnerLogin = ((XmlElement)postbackInfoElem.GetElementsByTagName("login")[0]).InnerText;
      this.LearnerPassword = ((XmlElement)postbackInfoElem.GetElementsByTagName("password")[0]).InnerText;
      this.RegistrationResultsAuthType = ((XmlElement)postbackInfoElem.GetElementsByTagName("authtype")[0]).InnerText;
    }

    /// <summary>
    /// Unique Identifier for this registration
    /// </summary>
    public string RegistrationId
    {
      get { return _registrationId; }
      private set { _registrationId = value; }
    }

    public string Url
    {
      get { return _url; }
      private set { _url = value; }
    }


    public string RegistrationResultsAuthType
    {
      get { return _registrationResultsAuthType; }
      private set { _registrationResultsAuthType = value; }
    }

    public string LearnerLogin
    {
      get { return _learnerLogin; }
      private set { _learnerLogin = value; }
    }

    public string LearnerPassword
    {
      get { return _learnerPassword; }
      private set { _learnerPassword = value; }
    }



  }
}
