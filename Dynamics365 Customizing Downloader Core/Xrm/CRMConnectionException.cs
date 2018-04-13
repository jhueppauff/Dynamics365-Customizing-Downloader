//-----------------------------------------------------------------------
// <copyright file="CRMConnectionException.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Xrm
{
    using System;
    using System.Collections;
    using System.Runtime.Serialization;

    /// <summary>
    /// CRM Connection Exception
    /// </summary>
    public class CrmConnectionException : Exception, ICrmConnectionException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrmConnectionException"/> class.
        /// </summary>
        public CrmConnectionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmConnectionException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">Exception Message</param>
        public CrmConnectionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmConnectionException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">Exception Message</param>
        /// <param name="innerException">Inner Exception Details</param>
        public CrmConnectionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmConnectionException"/> class. Which contains the help link and Exception source 
        /// </summary>
        /// <param name="helpLink">Help Link</param>
        /// <param name="source">Exception Source</param>
        public CrmConnectionException(string helpLink, string source) : this(helpLink)
        {
            this.Source = source;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmConnectionException"/> class. Which contains <see cref="SerializationInfo"/> and <see cref="StreamingContext"/>
        /// </summary>
        /// <param name="info">Serialization Info</param>
        /// <param name="context">Streaming Context</param>
        protected CrmConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// Gets or sets the Exception Help Link
        /// </summary>
        public override string HelpLink { get => base.HelpLink; set => base.HelpLink = value; }

        /// <summary>
        /// Gets or sets the Exception Source
        /// </summary>
        public override string Source { get => base.Source; set => base.Source = value; }

        /// <summary>
        /// Gets the Object Date
        /// </summary>
        /// <param name="info">Serialization Info</param>
        /// <param name="context">Streaming Context</param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}