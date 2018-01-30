//-----------------------------------------------------------------------
// <copyright file="ICrmConnectionException.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Core.Xrm
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Interface for the CRMConnectionException
    /// </summary>
    public interface ICrmConnectionException
    {
        /// <summary>
        /// Gets or sets the Exception Help Link
        /// </summary>
        string HelpLink { get; set; }

        /// <summary>
        /// Gets or sets the Source of the Exception
        /// </summary>
        string Source { get; set; }

        bool Equals(object obj);

        int GetHashCode();

        void GetObjectData(SerializationInfo info, StreamingContext context);
    }
}