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
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Helper class for generating the required security signature when communicating
    /// with the Hosted SCORM Engine web service.
    /// </summary>
    public class RequestSigner
    {	
	    private static ArrayList excludedParams = new ArrayList(new string[]{ "sig", "filedata" });

        /// <summary>
        /// Return a hex string representing the MD5 hash of the secret key and request params
        /// </summary>
        /// <param name="securityKey">Security Key specific to the active appId</param>
        /// <param name="parameterMap">Map of name/value pairs to be sent via the query string</param>
        /// <returns>MD5 Hash to be sent as "sig" parameter</returns>
	    public static string GetSignatureForRequest(string securityKey, IDictionary<string, object> parameterMap)
	    {	
			    string serializedRequestString = GetSerializedParams(parameterMap);
			    string signatureParts = securityKey + serializedRequestString;

		        return GetMD5Hash(signatureParts);   			
	    }

        // Return the serialized request string. 
        private static string GetSerializedParams(IDictionary<string, object> requestParams)
	    {
		    StringBuilder paramString = new StringBuilder();
		    
            // Put keys into an ArrayList for an easy sort()
            ArrayList paramNames = new ArrayList();
		    foreach(string key in requestParams.Keys)
		    {
		        paramNames.Add(key);
		    }
            paramNames.Sort(CaseInsensitiveComparer.DefaultInvariant);
    		
		    foreach (string paramName in paramNames) {			
			    if (!IsExcludedParam(paramName)){				
				    paramString.Append(paramName);
                    paramString.Append(requestParams[paramName]);
			    }
		    }
		    return paramString.ToString();
	    }

        private static string GetMD5Hash(string str)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(str);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        private static bool IsExcludedParam(String paramName)
        {
            return excludedParams.Contains(paramName);
        }
    }
}
