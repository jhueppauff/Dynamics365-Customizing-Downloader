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
        public static readonly DependencyProperty HtmlProperty = DependencyProperty.RegisterAttached("Html", typeof(string), typeof(PatchNotes),new FrameworkPropertyMetadata(OnHtmlChanged));


        public PatchNotes()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string version = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion.ToString();
            // Issue with leading Zero
            

            Update.UpdateChecker updateChecker = new Update.UpdateChecker();
            Update.Release release = updateChecker.GetReleaseInfo(version.ToString());

            InitializeComponent();
            Tbx_PatchNotes.Text = release.Body; 
            Lbl_IsPreRelease.Content = release.Prerelease.ToString();
            Lbl_VersionNumber.Content = release.Name;
            Lbl_ReleaseDate.Content = release.Published_at.ToString();
        }

        [AttachedPropertyBrowsableForType(typeof(WebBrowser))]
        public static string GetHtml(WebBrowser d)
        {
            return (string)d.GetValue(HtmlProperty);
        }

        public static void SetHtml(WebBrowser d, string value)
        {
            d.SetValue(HtmlProperty, value);
        }

        static void OnHtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebBrowser wb = d as WebBrowser;
            if (wb != null)
                wb.NavigateToString(e.NewValue as string);
        }
    }
}
