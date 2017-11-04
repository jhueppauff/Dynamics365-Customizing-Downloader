

namespace Dynamics365CustomizingDownloader
{
    using System;
    using System.Collections.Generic;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    /// <summary>
    /// Interactionlogic for ConnectionManger.xaml
    /// </summary>
    public partial class ConnectionManger : Window
    {

        public ConnectionManger()
        {
            InitializeComponent();
        }

        // Button Click Action, Connect and add CRM Organisation
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            xrm.ToolingConnector toolingConnector = new xrm.ToolingConnector();
            var crmServiceClient = toolingConnector.GetCrmServiceClient(tbx_connectionString.Text);

            if (crmServiceClient != null)
            {
                try
                {
                    List<xrm.CrmConnection> crmConnections = StorageExtensions.Load();

                    foreach (xrm.CrmConnection crmTempConnection in crmConnections)
                    {
                        if (crmTempConnection.Name != crmServiceClient.ConnectedOrgFriendlyName)
                        {
                            xrm.CrmConnection crmConnection = new xrm.CrmConnection
                            {
                                ConnectionString = tbx_connectionString.Text,
                                Name = crmServiceClient.ConnectedOrgFriendlyName
                            };
                            StorageExtensions.Save(crmConnection);
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show($"Connection {crmTempConnection.Name} does already exist!", "Connection already exists", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
                catch (System.IO.FileNotFoundException)
                {
                    // Ignor
                }
                finally
                {
                    xrm.CrmConnection crmConnection = new xrm.CrmConnection
                    {
                        ConnectionString = tbx_connectionString.Text,
                        Name = crmServiceClient.ConnectedOrgFriendlyName
                    };
                    StorageExtensions.Save(crmConnection);
                    this.Close();
                }
            }
        }
    }
}
