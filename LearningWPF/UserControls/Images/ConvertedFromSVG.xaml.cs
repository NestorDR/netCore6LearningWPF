using System.Diagnostics;
using System.Windows.Controls;

namespace LearningWPF.UserControls.Images
{
    /// <summary>
    /// Interaction logic for ConvertedFromSVG.xaml
    /// </summary>
    public partial class ConvertedFromSVG : UserControl
    {
        public ConvertedFromSVG()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            // For .NET Core you need to add { UseShellExecute = true }
            // Visit: https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
