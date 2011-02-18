using System;

namespace RusticiSoftware.HostedEngine.Client
{
    /// <summary>
    /// General exception type for errors generated in the Hosted Engine
    /// web service client
    /// </summary>
    [Serializable]
    public class ServiceException : ApplicationException
    {
        private int errorCode = 0;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ServiceException()
        {
        }

        /// <summary>
        /// Constructor with exception message
        /// </summary>
        /// <param name="errorCode">Error Code</param>
        /// <param name="message">Message to display</param>
        public ServiceException(int errorCode, string message) : base(message)
        {
            this.errorCode = errorCode;
        }

        /// <summary>
        /// Constructor with exception message
        /// </summary>
        /// <param name="errorCode">Error Code</param>
        /// <param name="message">Message to display</param>
        public ServiceException(ErrorCode errorCode, string message)
            :
            base(message + "errorCode: " + errorCode)
        {
            this.errorCode = (int)errorCode;
        }

        /// <summary>
        /// Constructor with exception message
        /// </summary>
        /// <param name="message">Message to display</param>
        public ServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructor with message and inner exception
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="inner">Inner Exception</param>
        public ServiceException(string message, Exception inner) : base(message, inner)
        {
        }

        public int ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }
    }
}