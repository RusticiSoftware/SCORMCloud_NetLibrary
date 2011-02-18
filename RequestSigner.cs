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
            paramNames.Sort();
    		
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
