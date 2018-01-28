//-----------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Windows;

    /// <summary>
    /// Interaction logic for App
    /// </summary>
    public partial class App : Application
    {
        private ApplicationInsightHelper applicationInsightHelper;

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                applicationInsightHelper = new ApplicationInsightHelper();

                //We use this to get access to unhandled exceptions so we can 
                //report app crashes to the Telemetry client
                var currentDomain = AppDomain.CurrentDomain;

                currentDomain.UnhandledException += CurrentDomain_UnhandledException;
                currentDomain.ProcessExit += CurrentDomain_ProcessExit;

                // Open Application
                var mainWindow = new MainWindow(applicationInsightHelper);
                mainWindow.Show();   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            applicationInsightHelper.FlushData();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            applicationInsightHelper.TrackFatalException(e.ExceptionObject as Exception);

            applicationInsightHelper.FlushData();
        }
    }
}
