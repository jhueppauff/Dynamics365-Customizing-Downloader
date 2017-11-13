using System;
using System.Collections.Generic;
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

namespace Dynamics365CustomizingDownloader
{
    /// <summary>
    /// Interaction logic for DownloadMultiple.xaml
    /// </summary>
    public partial class DownloadMultiple : Window
    {
        public List<Xrm.CrmSolution> CRMSolutions;
        public Xrm.CrmConnection CRMConnection;
        private int downloadIndex = 0;

        public DownloadMultiple()
        {
            InitializeComponent();
            this.Btn_close.IsEnabled = false;
        }

        /// <summary>
        /// Closes the Form on Button Action
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.RoutedEventArgs"/> instance containing the event data.</param>
        private void Btn_close_Click(object sender, RoutedEventArgs e)
        {
            if (downloadIndex == 0)
            {
                this.Close();
            }
        }
    }
}
