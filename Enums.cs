
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
