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


namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// Determines to which instance/version the actions in the ScormEngineManager class should be applied
    /// </summary>
    public enum Scope
    {
        /// <summary>
        /// Action applies to latest version/instance only
        /// </summary>
        LATEST,
        /// <summary>
        /// Action applies to latest version/instance only
        /// </summary>
        ALL,
        /// <summary>
        /// Action applies to version/instance explicitly set on the ExternalPackageId/ExternalRegistrationid
        /// </summary>
        SPECIFIED_ON_EXTERNAL_ID,
    }

    /// <summary>
    /// Formal parameters for metadata scope
    /// </summary>
    public enum MetadataScope
    {
        /// <summary>
        /// Package/Course metadata
        /// </summary>
        COURSE,
        /// <summary>
        /// A recursive list of all activies
        /// </summary>
        ACTIVITY,
    }

    /// <summary>
    /// Formal parameters for metadata format
    /// </summary>
    public enum MetadataFormat
    {
        /// <summary>
        /// Most common high-level information
        /// </summary>
        SUMMARY,

        /// <summary>
        /// Complete SCORM Metadata for all specified elements
        /// </summary>
        DETAILED,
    }

    /// <summary>
    /// Formal paramters for the registration results format
    /// </summary>
    public enum RegistrationResultsFormat
    {
        /// <summary>
        /// Course Level only - Main high-level information about
        /// the registration status
        /// </summary>
        COURSE, 

        /// <summary>
        /// Summary data about each individual activity in the course.
        /// </summary>
        ACTIVITY, 

        /// <summary>
        /// Complete SCORM Data known about the course.
        /// </summary>
        FULL
    }

    /// <summary>
    /// Formal paramters for the data format
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        /// Return data is formatted as XML
        /// </summary>
        XML,

        /// <summary>
        /// Return data is formatted as JSON
        /// </summary>
        JSON
    }

    public enum RegistrationResultsAuthType
    {
        FORM, 
        HTTPBASIC
    }

    public enum ErrorCode
    {
        INVALID_WEB_SERVICE_RESPONSE = 300
    }

    public enum ReportageNavPermission 
    {
    	/// <summary>
    	/// No navigation allowed
    	/// </summary>
        NONAV,
        
        /// <summary>
        /// Narrowing of report population is allowed, but not widening
        /// </summary>
        DOWNONLY,
        
        /// <summary>
        /// Full navigation rights allowed
        /// </summary>
        FREENAV
    }
    	
}
