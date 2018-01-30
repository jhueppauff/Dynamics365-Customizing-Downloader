//-----------------------------------------------------------------------
// <copyright file="CRMConnectionException.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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

        public CrmConnectionException(string helpLink, string source) : this(helpLink)
        {
            this.Source = source;
        }

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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}