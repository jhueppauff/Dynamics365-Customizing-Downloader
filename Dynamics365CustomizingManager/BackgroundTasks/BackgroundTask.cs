//-----------------------------------------------------------------------
// <copyright file="BackgroundTask.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

using Windows.ApplicationModel.Background;

namespace Dynamics365CustomizingManager.BackgroundTasks
{
    public abstract class BackgroundTask
    {
        public abstract void Register();

        public abstract Task RunAsyncInternal(IBackgroundTaskInstance taskInstance);

        public abstract void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason);

        public bool Match(string name)
        {
            return name == GetType().Name;
        }

        public Task RunAsync(IBackgroundTaskInstance taskInstance)
        {
            SubscribeToEvents(taskInstance);

            return RunAsyncInternal(taskInstance);
        }

        public void SubscribeToEvents(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
        }
    }
}
