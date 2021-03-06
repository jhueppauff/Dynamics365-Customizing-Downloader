﻿//-----------------------------------------------------------------------
// <copyright file="LoadingPanel.xaml.cs" company="https://github.com/jhueppauff/Dynamics365-Customizing-Downloader">
// Copyright 2018 Jhueppauff
// Mozilla Public License Version 2.0 
// For licence details visit https://github.com/jhueppauff/Dynamics365-Customizing-Downloader/blob/master/LICENSE
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader.Controls
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for LoadingPanel
    /// </summary>
    public partial class LoadingPanel
    {
        /// <summary>
        /// IsLoading Property
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingPanel), new UIPropertyMetadata(false));

        /// <summary>
        /// Message Property
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(LoadingPanel), new UIPropertyMetadata("Loading..."));

        /// <summary>
        /// SubMessage Property
        /// </summary>
        public static readonly DependencyProperty SubMessageProperty =
            DependencyProperty.Register("SubMessage", typeof(string), typeof(LoadingPanel), new UIPropertyMetadata(string.Empty));

        /// <summary>
        /// ClosePanelCommand Property
        /// </summary>
        public static readonly DependencyProperty ClosePanelCommandProperty =
            DependencyProperty.Register("ClosePanelCommand", typeof(ICommand), typeof(LoadingPanel));

        /// <summary>
        /// Initializes a new instance of the <see cref="LoadingPanel"/> class.
        /// </summary>
        public LoadingPanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is loading; otherwise, <c>false</c>.
        /// </value>
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the sub message.
        /// </summary>
        /// <value>The sub message.</value>
        public string SubMessage
        {
            get { return (string)GetValue(SubMessageProperty); }
            set { SetValue(SubMessageProperty, value); }
        }

        /// <summary>
        /// Gets or sets the close panel command.
        /// </summary>
        /// <value>The close panel command.</value>
        public ICommand ClosePanelCommand
        {
            get { return (ICommand)GetValue(ClosePanelCommandProperty); }
            set { SetValue(ClosePanelCommandProperty, value); }
        }

        /// <summary>
        /// Called when [close click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            if (this.ClosePanelCommand != null)
            {
                this.ClosePanelCommand.Execute(null);
            }
        }
    }
}