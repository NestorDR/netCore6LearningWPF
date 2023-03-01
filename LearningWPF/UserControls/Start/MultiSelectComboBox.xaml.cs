using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
// --- App modules ---
using LearningWPF.Models;
using LearningWPF.Services;

namespace LearningWPF.UserControls.Start
{
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private readonly ObservableCollection<TaskModel> _items;

        public MultiSelectComboBox()
        {
            InitializeComponent();
            // Initialize listbox setting its data source
            _items = new ObservableCollection<TaskModel>(new TaskService().Get());

        }

        private void TaskComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TaskComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TaskComboBox.ItemsSource = _items.Where(x => x.Name.StartsWith(TaskComboBox.Text.Trim()));
        }

        private void TaskComboBox_CheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            BindText();
        }

        private void BindText()
        {
            //testListbox.Items.Clear();
            foreach (var item in _items.Where(taskModel => taskModel.Checked))
            {
                Debug.WriteLine(item.Name);
                //testListbox.Items.Add(item.Name);
            }
        }
    }
}
