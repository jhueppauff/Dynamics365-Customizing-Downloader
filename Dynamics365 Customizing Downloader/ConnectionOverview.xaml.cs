//-----------------------------------------------------------------------
// <copyright file="ConnectionOverview.xaml.cs" company="None">
// Copyright 2017 Jhueppauff
// MIT  
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------

namespace Dynamics365CustomizingDownloader
{
    using System.Collections.Generic;
    using System.Windows;

    /// <summary>
    /// Interaction logic for ConnectionOverview
    /// </summary>
    public partial class ConnectionOverview : Window
    {
        /// <summary>
        /// List of all CRM Connections <see cref="List{CRM Connections}"/>
        /// </summary>
        private List<Xrm.CrmConnection> crmConnections;

        /// <summary>
        /// CRM Connection
        /// </summary>
        private Xrm.CrmConnection crmConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionOverview"/> class.
        /// </summary>
        public ConnectionOverview()
        {
            this.InitializeComponent();
            this.LoadCRMConnections();
        }

        /// <summary>
        /// Loads every CRM Connection from Config
        /// </summary>
        private void LoadCRMConnections()
        {
            this.crmConnections = StorageExtensions.Load();
            Cbx_CRMConnections.Items.Clear();

            foreach (Xrm.CrmConnection connection in this.crmConnections)
            {
                Cbx_CRMConnections.Items.Add(connection.Name);
            }
        }

        /// <summary>
        /// Loads the selected Connection
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void Cbx_CRMConnections_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string connectionName = Cbx_CRMConnections.SelectedItem.ToString();

            this.crmConnection = this.crmConnections.Find(x => x.Name == connectionName);

            Tbx_ConnectionName.Text = this.crmConnection.Name;
            Tbx_ConnectionString.Text = this.crmConnection.ConnectionString;
        }

        /// <summary>
        /// Tests the Connection
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_TestConnection_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Xrm.ToolingConnector toolingConnector = new Xrm.ToolingConnector())
                {
                    toolingConnector.GetCrmServiceClient(Tbx_ConnectionString.Text);

                    Btn_SaveConnection.IsEnabled = true;
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Button Action, Save Connection
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_SaveConnection_Click(object sender, RoutedEventArgs e)
        {
            StorageExtensions.Update(this.crmConnection);
        }
    }
}