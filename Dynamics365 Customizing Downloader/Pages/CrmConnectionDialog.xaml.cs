//-----------------------------------------------------------------------
// <copyright file="CrmConnectionDialog.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Pages
{
    using System;
    using System.Windows;
    using System.Windows.Threading;
    using Microsoft.Xrm.Tooling.Connector;
    using Microsoft.Xrm.Tooling.CrmConnectControl;

    /// <summary>
    /// Interaction logic for CrmConnectionDialog.xaml
    /// </summary>
    public partial class CrmConnectionDialog : Window
    {
        #region variables

        /// <summary>
        /// Crm Service Client
        /// </summary>
        private CrmServiceClient CrmServiceClient = null;

        /// <summary>
        /// flag to determine if ther is a connection
        /// </summary>
        private bool connectionState = false;

        /// <summary>
        /// Crm Connection Manager
        /// </summary>
        private CrmConnectionManager crmConnectionManager = null;

        /// <summary>
        /// Used to reset the ui without reopening
        /// </summary>
        private bool resetUiFlag = false;
        #endregion

        #region properties

        /// <summary>
        /// Gets the CrmConnectionManager
        /// </summary>
        public CrmConnectionManager CrmConnectionManager { get { return crmConnectionManager; } }
        #endregion

        #region event

        /// <summary>
        /// Raised when the connection was established
        /// </summary>
        public event EventHandler ConnectionToCrmCompleted;
        #endregion

        public CrmConnectionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Raised when the window loads 
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.connectionState = false;

            // Initialize the connection Manager
            this.crmConnectionManager = new CrmConnectionManager
            {
                ParentControl = this.CrmLoginControl,
                UseUserLocalDirectoryForConfigStore = true
            };

            // Configure the Crm Control
            this.CrmLoginControl.SetGlobalStoreAccess(crmConnectionManager);
            this.CrmLoginControl.SetControlMode(ServerLoginConfigCtrlMode.FullLoginPanel);

            // Event registration
            this.CrmLoginControl.ConnectionCheckBegining += new EventHandler(CrmLoginControl_ConnectionCheckStarted);
            this.CrmLoginControl.ConnectErrorEvent += new EventHandler<ConnectErrorEventArgs>(CrmLoginControl_ConnectionErrorRaised);
            this.CrmLoginControl.ConnectionStatusEvent += new EventHandler<ConnectStatusEventArgs>(CrmLoginControl_ConnectionStatusEventRaised);
            this.CrmLoginControl.UserCancelClicked += new EventHandler(CrmLoginControl_UserCancelClicked);

            // Check if an auto login is possible
            if (!this.crmConnectionManager.RequireUserLogin())
            {
#pragma warning disable S1066 // Collapsible "if" statements should be merged
                if (MessageBox.Show("There are already Credentials saved in your configuration\nDo you want to load those?\nYes to Auto Login or No to Reset Credentials", "Auto Login possible", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
#pragma warning restore S1066 // Collapsible "if" statements should be merged
                {
                    // Credentials are cached
                    this.CrmLoginControl.IsEnabled = false;

                    this.crmConnectionManager.ServerConnectionStatusUpdate += new EventHandler<ServerConnectStatusEventArgs>(CrmConnectionManager_ServerConnectionStatusUpdateRaised);
                    this.crmConnectionManager.ConnectionCheckComplete += new EventHandler<ServerConnectStatusEventArgs>(CrmConnectionManager_ConnectionCheckCompleted);

                    this.crmConnectionManager.ConnectToServerCheck();

                    // Show message
                    this.CrmLoginControl.ShowMessageGrid();
                }
            }
        }

        #region events
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The <see cref="ServerConnectStatusEventArgs"/> instance containing the event data.</param>
        private void CrmConnectionManager_ConnectionCheckCompleted(object sender, ServerConnectStatusEventArgs e)
        {
            // The Status event will contain information about the current login process,  if Connected is false, then there is not yet a connection. 
            // Unwire events that we are not using anymore, this prevents issues if the user uses the control after a failed login. 
            ((CrmConnectionManager)sender).ConnectionCheckComplete -= CrmConnectionManager_ConnectionCheckCompleted;
            ((CrmConnectionManager)sender).ServerConnectionStatusUpdate -= CrmConnectionManager_ServerConnectionStatusUpdateRaised;

            if (!e.Connected)
            {
                // if its not connected pop the login screen here. 
                if (e.MultiOrgsFound)
                    MessageBox.Show("Unable to Login to CRM using cached credentials. Org Not found", "Login Failure");
                else
                    MessageBox.Show("Unable to Login to CRM using cached credentials", "Login Failure");

                resetUiFlag = true;
                this.CrmLoginControl.GoBackToLogin();

                // Bad Login Get back on the UI. 
                Dispatcher.Invoke(DispatcherPriority.Normal,
                       new System.Action(() =>
                       {
                           this.Title = "Failed to Login with cached credentials.";
                           MessageBox.Show(this.Title, "Notification from ConnectionManager", MessageBoxButton.OK, MessageBoxImage.Error);
                           this.CrmLoginControl.IsEnabled = true;
                       }
                        ));

                this.resetUiFlag = false;
            }
            else
            {
                // Good Login Get back on the UI 
                if (e.Connected && !this.connectionState)
                    ProcessSuccess();
            }
        }

        /// <summary>
        /// Will be raised on success
        /// </summary>
        private void ProcessSuccess()
        {
            this.resetUiFlag = true;
            this.connectionState = true;
            CrmServiceClient = crmConnectionManager.CrmSvc;
            this.CrmLoginControl.GoBackToLogin();
            Dispatcher.Invoke(DispatcherPriority.Normal,
               new Action(() =>
               {
                   this.Title = "Notification from Parent";
                   this.CrmLoginControl.IsEnabled = true;
               }));

            // Notify Caller that we are done with success. 
            ConnectionToCrmCompleted?.Invoke(this, EventArgs.Empty);

            this.resetUiFlag = false;
        }

        /// <summary>
        /// Will raised upon Status Events from the Auto Login Process
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The <see cref="ServerConnectStatusEventArgs"/> instance containing the event data.</param>
        private void CrmConnectionManager_ServerConnectionStatusUpdateRaised(object sender, ServerConnectStatusEventArgs e)
        {
            // The Status event will contain information about the current login process,  if Connected is false, then there is not yet a connection. 
            // Set the updated status of the loading process.
            Dispatcher.Invoke(priority: DispatcherPriority.Normal,
                                method: new Action(() =>
                                {
                                    this.Title = string.IsNullOrWhiteSpace(e.StatusMessage) ? e.ErrorMessage : e.StatusMessage;
                                }));
        }

        /// <summary>
        /// Login Control Cancel event raised. 
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CrmLoginControl_UserCancelClicked(object sender, EventArgs e)
        {
            if (!this.resetUiFlag)
                this.Close();
        }

        /// <summary>
        /// Login control connect check status event. 
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The <see cref="ConnectStatusEventArgs"/> instance containing the event data.</param>
        private void CrmLoginControl_ConnectionStatusEventRaised(object sender, ConnectStatusEventArgs e)
        {
            // Here we are using the loginState bool to check and make sure we only process this call once. 
            if (e.ConnectSucceeded && !this.connectionState)
                this.ProcessSuccess();
        }

        /// <summary>
        /// Will be raised on error, will display the error in a message box 
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The <see cref="ConnectErrorEventArgs"/> instance containing the event data.</param>
        private void CrmLoginControl_ConnectionErrorRaised(object sender, ConnectErrorEventArgs e)
        {
            MessageBox.Show(e.ErrorMessage, "Ups, an error occured", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
        }

        /// <summary>
        /// Will be raised on Connection status check starting
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void CrmLoginControl_ConnectionCheckStarted(object sender, EventArgs e)
        {
            this.connectionState = false;
            Dispatcher.Invoke(DispatcherPriority.Normal,
                               new Action(() =>
                               {
                                   this.Title = "Starting Login Process. ";
                                   this.CrmLoginControl.IsEnabled = true;
                               }));
        }

        #endregion
    }
}
