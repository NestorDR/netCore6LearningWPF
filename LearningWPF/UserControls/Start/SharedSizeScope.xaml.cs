using System.Windows.Controls;

namespace LearningWPF.UserControls.Start
{
    /// <summary>
    /// Interaction logic for SharedSizeScope.xaml
    /// </summary>
    public partial class SharedSizeScope : UserControl
    {
        public SharedSizeScope()
        {
            InitializeComponent();
        }

        private void SetTrue(object sender, System.Windows.RoutedEventArgs e)
        {
            Grid.SetIsSharedSizeScope(MainPanel, true);
            TextBlock.Text = "IsSharedSizeScope Property is set to " + Grid.GetIsSharedSizeScope(MainPanel).ToString();
        }

        private void SetFalse(object sender, System.Windows.RoutedEventArgs e)
        {
            Grid.SetIsSharedSizeScope(MainPanel, false);
            TextBlock.Text = "IsSharedSizeScope Property is set to " + Grid.GetIsSharedSizeScope(MainPanel).ToString();
        }
    }
}
