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
    /// Interaction logic for CRM ConnectionDialog
    /// </summary>
    public partial class CrmConnectionDialog : Window
    {
        #region variables

        /// <summary>
        /// CRM Service Client
        /// </summary>
        private CrmServiceClient crmServiceClient = null;

        /// <summary>
        /// flag to determine if there is a connection
        /// </summary>
        private bool connectionState = false;

        /// <summary>
        /// CRM Connection Manager
        /// </summary>
        private CrmConnectionManager crmConnectionManager = null;

        /// <summary>
        /// Used to reset the UI without reopening
        /// </summary>
        private bool resetUiFlag = false;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CrmConnectionDialog"/> class.
        /// </summary>
        public CrmConnectionDialog()
        {
            this.InitializeComponent();
        }

        #region event

        /// <summary>
        /// Raised when the connection was established
        /// </summary>
        public event EventHandler ConnectionToCrmCompleted;
        #endregion

        #region properties

        /// <summary>
        /// Gets the CRM ConnectionManager
        /// </summary>
        public CrmConnectionManager CrmConnectionManager
        {
            get
            {
                return this.crmConnectionManager;
            }
        }

        #endregion

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
            this.CrmLoginControl.SetGlobalStoreAccess(this.crmConnectionManager);
            this.CrmLoginControl.SetControlMode(ServerLoginConfigCtrlMode.FullLoginPanel);

            // Event registration
            this.CrmLoginControl.ConnectionCheckBegining += new EventHandler(this.CrmLoginControl_ConnectionCheckStarted);
            this.CrmLoginControl.ConnectErrorEvent += new EventHandler<ConnectErrorEventArgs>(this.CrmLoginControl_ConnectionErrorRaised);
            this.CrmLoginControl.ConnectionStatusEvent += new EventHandler<ConnectStatusEventArgs>(this.CrmLoginControl_ConnectionStatusEventRaised);
            this.CrmLoginControl.UserCancelClicked += new EventHandler(this.CrmLoginControl_UserCancelClicked);

            // Check if an auto login is possible
            if (!this.crmConnectionManager.RequireUserLogin())
            {
#pragma warning disable S1066 // Collapsible "if" statements should be merged
                if (MessageBox.Show("There are already Credentials saved in your configuration\nDo you want to load those?\nYes to Auto Login or No to Reset Credentials", "Auto Login possible", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
#pragma warning restore S1066 // Collapsible "if" statements should be merged
                {
                    // Credentials are cached
                    this.CrmLoginControl.IsEnabled = false;

                    this.crmConnectionManager.ServerConnectionStatusUpdate += new EventHandler<ServerConnectStatusEventArgs>(this.CrmConnectionManager_ServerConnectionStatusUpdateRaised);
                    this.crmConnectionManager.ConnectionCheckComplete += new EventHandler<ServerConnectStatusEventArgs>(this.CrmConnectionManager_ConnectionCheckCompleted);

                    this.crmConnectionManager.ConnectToServerCheck();

                    // Show message
                    this.CrmLoginControl.ShowMessageGrid();
                }
            }
        }

        #region events
        /// <summary>
        /// Event will be raised if the Connection Check is completed
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="e">The <see cref="ServerConnectStatusEventArgs"/> instance containing the event data.</param>
        private void CrmConnectionManager_ConnectionCheckCompleted(object sender, ServerConnectStatusEventArgs e)
        {
            // The Status event will contain information about the current login process,  if Connected is false, then there is not yet a connection. 
            // Unwire events that we are not using anymore, this prevents issues if the user uses the control after a failed login. 
            ((CrmConnectionManager)sender).ConnectionCheckComplete -= this.CrmConnectionManager_ConnectionCheckCompleted;
            ((CrmConnectionManager)sender).ServerConnectionStatusUpdate -= this.CrmConnectionManager_ServerConnectionStatusUpdateRaised;

            if (!e.Connected)
            {
                // if its not connected pop the login screen here. 
                if (e.MultiOrgsFound)
                {
                    MessageBox.Show("Unable to Login to CRM using cached credentials. Org Not found", "Login Failure");
                }
                else
                {
                    MessageBox.Show("Unable to Login to CRM using cached credentials", "Login Failure");
                }

                this.resetUiFlag = true;
                this.CrmLoginControl.GoBackToLogin();

                // Bad Login Get back on the UI. 
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Title = "Failed to Login with cached credentials."; MessageBox.Show(this.Title, "Notification from ConnectionManager", MessageBoxButton.OK, MessageBoxImage.Error); this.CrmLoginControl.IsEnabled = true; }));
                
                this.resetUiFlag = false;
            }
            else
            {
                // Good Login Get back on the UI 
                if (e.Connected && !this.connectionState)
                {
                    this.ProcessSuccess();
                }
            }
        }

        /// <summary>
        /// Will be raised on success
        /// </summary>
        private void ProcessSuccess()
        {
            this.resetUiFlag = true;
            this.connectionState = true;
            this.crmServiceClient = this.crmConnectionManager.CrmSvc;
            this.CrmLoginControl.GoBackToLogin();
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Title = "Notification from Parent"; this.CrmLoginControl.IsEnabled = true; }));

            // Notify Caller that we are done with success. 
            this.ConnectionToCrmCompleted?.Invoke(this, EventArgs.Empty);

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
            Dispatcher.Invoke(
                priority: DispatcherPriority.Normal,
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
            {
                this.Close();
            }
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
            {
                this.ProcessSuccess();
            }
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
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => { this.Title = "Starting Login Process. "; this.CrmLoginControl.IsEnabled = true; }));
        }

        #endregion
    }
}
