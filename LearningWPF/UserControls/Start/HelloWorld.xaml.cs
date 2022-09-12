using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
// --- App modules ---
using LearningWPF.Common;

namespace LearningWPF.UserControls.Start
{
    /// <summary>
    /// Interaction logic for HelloWorld.xaml
    /// </summary>
    public partial class HelloWorld : UserControl
    {

        private Window? _originalWindowSettings;

        public HelloWorld()
        {
            InitializeComponent();
            ColorsComboBox.ItemsSource = typeof(Colors).GetProperties();
        }

        private void HelloWorld_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this)!;

            DataContext = parentWindow;
         
            // Safeguard relevant properties
            _originalWindowSettings = new Window
            {
                Title = ((Window)DataContext).Title,
                Height = parentWindow.ActualHeight,
                Width = parentWindow.ActualWidth
            };
        }

        private void HelloWorld_Unloaded(object sender, RoutedEventArgs e)
        {
            // throw new NotImplementedException();
            if (DataContext is not Window parentWindow || _originalWindowSettings == null) return;

            // Restore the original settings of the parent window
            parentWindow.Title = _originalWindowSettings.Title;
            parentWindow.Height = _originalWindowSettings.Height;
            parentWindow.Width = _originalWindowSettings.Width;

            // Release resource, if it is not done, the application does not end
            _originalWindowSettings.Close();
        }

        private void UpdateTitleButton_Click(object sender, RoutedEventArgs e)
        {
            BindingExpression? bindingExpression = WindowTitleTextBox.GetBindingExpression(TextBox.TextProperty);
            bindingExpression?.UpdateSource();
        }

        private void WidthHeightTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (!int.TryParse(textBox.Text, out int value) || value < 200)
                textBox.Text = "500";
        }

        private void ColorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ColorsComboBox is bound to a property list, each being a color, instead of a simple list of colors,
            //  thus we must use GetValue
            Color selectedColor = (Color)((PropertyInfo)ColorsComboBox.SelectedItem).GetValue(null, null)!;
            this.Background = new SolidColorBrush(selectedColor);
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

        private void LoadMeButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceListBox.Items.Add(ResourceListBox.FindResource("ListBoxResourceString").ToString());
            ResourceListBox.Items.Add(UserControlMainPanel.FindResource("PanelResourceString").ToString());
            ResourceListBox.Items.Add(this.FindResource("UserControlResourceString").ToString());
            ResourceListBox.Items.Add(((Window) DataContext).FindResource("WindowResourceString").ToString());
            ResourceListBox.Items.Add(Application.Current.FindResource("ApplicationResourceString")?.ToString());
            ResourceListBox.Items.Add(AppSettings.Instance.ConnectionString);
        }

        private void ResourceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is Window parentWindow && ResourceListBox.SelectedItem != null)
                parentWindow.Title = ResourceListBox.SelectedItem.ToString();
        }

        private void ClearListButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceListBox.Items.Clear();
        }
    }
}
