using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LearningWPF.UserControls.Start
{
    /// <summary>
    /// Interaction logic for ComboBoxSelection.xaml
    /// Visit: https://wpf-tutorial.com/list-controls/combobox-control/
    /// </summary>
    public partial class ComboBoxSelection : UserControl
    {
        public ComboBoxSelection()
        {
            InitializeComponent();
            ColorsComboBox.ItemsSource = typeof(Colors).GetProperties();

            Brush? selectedColor = this.Background;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (ColorsComboBox.SelectedIndex > 0)
                ColorsComboBox.SelectedIndex--;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (ColorsComboBox.SelectedIndex < ColorsComboBox.Items.Count - 1)
                ColorsComboBox.SelectedIndex++;
            else
                ColorsComboBox.SelectedIndex = 0;
        }

        private void BlueButton_Click(object sender, RoutedEventArgs e)
        {
            ColorsComboBox.SelectedItem = typeof(Colors).GetProperty("Blue");
        }

        private void ColorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ColorsComboBox is bound to a property list, each being a color, instead of a simple list of colors,
            //  thus we must use GetValue
            Color selectedColor = (Color)((PropertyInfo)ColorsComboBox.SelectedItem).GetValue(null, null);
            this.Background = new SolidColorBrush(selectedColor);
        }
    }
}
