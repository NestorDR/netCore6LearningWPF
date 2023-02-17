using System.Windows;
using System.Windows.Controls;

namespace LearningWPF.UserControls.Start
{
    /// <summary>
    /// Interaction logic for StackPanelAlignedContent.xaml
    /// </summary>
    public partial class StackPanelAlignedContent : UserControl
    {
        public StackPanelAlignedContent()
        {
            InitializeComponent();
        }

        private void ChangeOrientation(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = (ListBoxItem)(sender as ListBox)!.SelectedItem;

            ElementsStackPanel.Orientation =
                li.Content.ToString() switch
                {
                    "Horizontal" => Orientation.Horizontal,
                    "Vertical" => Orientation.Vertical,
                    _ => ElementsStackPanel.Orientation
                };
        }

        private void ChangeHorizontalAlignment(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = (ListBoxItem)(sender as ListBox)!.SelectedItem;

            ElementsStackPanel.HorizontalAlignment =
                li.Content.ToString() switch
                {
                    "Left" => HorizontalAlignment.Left,
                    "Right" => HorizontalAlignment.Right,
                    "Center" => HorizontalAlignment.Center,
                    "Stretch" => HorizontalAlignment.Stretch,
                    _ => ElementsStackPanel.HorizontalAlignment
                };
        }

        private void ChangeVerticalAlignment(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = (ListBoxItem)(sender as ListBox)!.SelectedItem;

            ElementsStackPanel.VerticalAlignment =
                li.Content.ToString() switch
                {
                    "Top" => VerticalAlignment.Top,
                    "Bottom" => VerticalAlignment.Bottom,
                    "Center" => VerticalAlignment.Center,
                    "Stretch" => VerticalAlignment.Stretch,
                    _ => ElementsStackPanel.VerticalAlignment
                };
        }
    }
}
