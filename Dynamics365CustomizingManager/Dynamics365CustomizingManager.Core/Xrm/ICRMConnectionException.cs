//-----------------------------------------------------------------------
// <copyright file="ICRMConnectionException.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
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

        /// <summary>
        /// Gets the equality
        /// </summary>
        /// <param name="obj">Object data</param>
        /// <returns>Returns <see cref="bool"/></returns>
        bool Equals(object obj);

        /// <summary>
        /// Gets the HashCode
        /// </summary>
        /// <returns>Returns the hash code</returns>
        int GetHashCode();

        /// <summary>
        /// Gets the Object Date
        /// </summary>
        /// <param name="info"><see cref="SerializationInfo"/> Info</param>
        /// <param name="context"><see cref="StreamingContext"/> context</param>
        void GetObjectData(SerializationInfo info, StreamingContext context);
    }
}