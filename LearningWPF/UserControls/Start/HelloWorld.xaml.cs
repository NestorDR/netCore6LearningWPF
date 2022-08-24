using LearningWPF.Common;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

        private void ResourceListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is Window parentWindow && ResourceListBox.SelectedItem != null)
                parentWindow.Title = ResourceListBox.SelectedItem.ToString();
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

        private void ClearListButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceListBox.Items.Clear();
        }
    }
}
