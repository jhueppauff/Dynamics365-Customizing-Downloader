//-----------------------------------------------------------------------
// <copyright file="TaskExtensions.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace Dynamics365CustomizingManager.Helpers
{
    public static class TaskExtensions
    {
        public static void FireAndForget(this Task task)
        {
            // This method allows you to call an async method without awaiting it.
            // Use it when you don't want or need to wait for the task to complete.
        }
    }
}
