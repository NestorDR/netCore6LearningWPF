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

            ButtonsStackPanel.Orientation =
                li.Content.ToString() switch
                {
                    "Horizontal" => Orientation.Horizontal,
                    "Vertical" => Orientation.Vertical,
                    _ => ButtonsStackPanel.Orientation
                };
        }

        private void ChangeHorizontalAlignment(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = (ListBoxItem)(sender as ListBox)!.SelectedItem;

            ButtonsStackPanel.HorizontalAlignment =
                li.Content.ToString() switch
                {
                    "Left" => HorizontalAlignment.Left,
                    "Right" => HorizontalAlignment.Right,
                    "Center" => HorizontalAlignment.Center,
                    "Stretch" => HorizontalAlignment.Stretch,
                    _ => ButtonsStackPanel.HorizontalAlignment
                };
        }

        private void ChangeVerticalAlignment(object sender, SelectionChangedEventArgs args)
        {
            ListBoxItem li = (ListBoxItem)(sender as ListBox)!.SelectedItem;

            ButtonsStackPanel.VerticalAlignment =
                li.Content.ToString() switch
                {
                    "Top" => VerticalAlignment.Top,
                    "Bottom" => VerticalAlignment.Bottom,
                    "Center" => VerticalAlignment.Center,
                    "Stretch" => VerticalAlignment.Stretch,
                    _ => ButtonsStackPanel.VerticalAlignment
                };
        }
    }
}
