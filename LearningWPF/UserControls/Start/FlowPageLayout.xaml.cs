using System.Windows;
using System.Windows.Controls;

namespace LearningWPF.UserControls.Start
{
    /// <summary>
    /// Interaction logic for FlowPageLayout.xaml
    /// </summary>
    public partial class FlowPageLayout : UserControl
    {
        public FlowPageLayout()
        {
            InitializeComponent();
        }

        private void ReaderWidthText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.Reader != null) this.Width = this.Reader.Width + 80;
        }
    }
}
