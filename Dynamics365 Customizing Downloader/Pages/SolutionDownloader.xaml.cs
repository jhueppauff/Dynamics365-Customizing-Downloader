namespace Dynamics365CustomizingDownloader.Pages
{
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Interaction logic for SolutionDownloader.xaml
    /// </summary>
    public partial class SolutionDownloader : Page
    {
        /// <summary>
        /// Log4Net Logger
        /// </summary>
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public SolutionDownloader()
        {
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {
                Log.Error(ex.Message, ex);
                Diagnostics.ErrorReport errorReport = new Diagnostics.ErrorReport(ex);
                errorReport.Show();
            }
        }

        private void btn_download_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Reload_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
