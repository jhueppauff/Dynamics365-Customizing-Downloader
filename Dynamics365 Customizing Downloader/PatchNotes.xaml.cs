using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for PatchNotes.xaml
    /// </summary>
    public partial class PatchNotes : Window
    {
        public PatchNotes()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            // Issue with leading Zero
            var version = new Version(fvi.FileVersion);

            Update.UpdateChecker updateChecker = new Update.UpdateChecker();
            updateChecker.GetReleaseInfo(version.ToString());
            InitializeComponent();
        }
    }
}
