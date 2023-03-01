using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
// --- App modules ---
using LearningWPF.Dialogs;
using LearningWPF.Models;
using LearningWPF.Services;

namespace LearningWPF.UserControls.Start
{
    /// <summary>
    /// Interaction logic for ListBoxOnObservableCollection.xaml
    /// </summary>
    public partial class ListBoxOnObservableCollection : UserControl
    {
        
        private Window? _parentWindow;
        private string _originalWindowTitle = "";

        // Before: private readonly List<TaskModel> items;
        private readonly ObservableCollection<TaskModel> _items;

        public ListBoxOnObservableCollection()
        {
            InitializeComponent();

            // Initialize listbox setting its data source
            _items = new ObservableCollection<TaskModel>(new TaskService().Get());
            /*
            {
                new () { Name = "Learning how to develop", Completion = 30 },
                new () { Name = "Learn C#", Completion = 50 },
                new () { Name = "Learning WPF", Completion = 75 },
                new () { Name = "Wash the car", Completion = 85 },
                new () { Name = "Buy beer", Completion = 90 },
                new () { Name = "Enjoy it!", Completion = 100 }
            };
            */
            TaskListBox.ItemsSource = _items;
        }

        private void ListBoxSelection_Loaded(object sender, RoutedEventArgs e)
        {
            _parentWindow = Window.GetWindow(this)!;

            _originalWindowTitle = _parentWindow.Title;
        }

        private void ListBoxSelection_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_parentWindow == null) return;
            _parentWindow.Title = _originalWindowTitle;
        }

        private void TaskListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_parentWindow != null && TaskListBox.SelectedItem != null)
                _parentWindow.Title = ((TaskModel)TaskListBox.SelectedItem).Name;
        }

        #region Selection

        private void ShowSelectedItemButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var o in TaskListBox.SelectedItems)
                MessageBox.Show(((TaskModel)o).Name, _parentWindow?.Title);
        }

        private void SelectNextButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItems.Count > 1)
            {
                var previousIndex = TaskListBox.SelectedIndex;
                TaskListBox.SelectedItems.Clear();
                TaskListBox.SelectedIndex = previousIndex;
            }
            TaskListBox.SelectedIndex =
                TaskListBox.SelectedIndex >= 0 && TaskListBox.SelectedIndex < TaskListBox.Items.Count - 1
                    ? TaskListBox.SelectedIndex + 1
                    : 0;
        }

        private void SelectLastButton_Click(object sender, RoutedEventArgs e)
        {
            TaskListBox.SelectedItems.Clear();
            TaskListBox.SelectedIndex = TaskListBox.Items.Count - 1;
        }

        private void SelectCSharpButton_Click(object sender, RoutedEventArgs e)
        {
            TaskListBox.SelectedItems.Clear();
            foreach (var o in TaskListBox.Items)
            {
                if (o is not TaskModel item || !item.Name.Contains("C#")) continue;
                TaskListBox.SelectedItem = item;
                break;
            }
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var o in TaskListBox.Items)
                TaskListBox.SelectedItems.Add(o);
        }

        #endregion

        #region Actions
        
        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            Task taskDialog = new(new TaskModel { Id = _items.Count + 1 } );
            taskDialog.ShowDialog();        // Show it as a modal dialog
            if (taskDialog.DataContext is not TaskModel taskModel) return;
            // Add new item
            _items.Add(taskModel);
        }

        private void ChangeItemButton_Click(object sender, RoutedEventArgs e)
        {
            // Before: ((TaskModel)TaskListBox.SelectedItem).Name = "Changed TaskModel";
            if (TaskListBox.SelectedItem is not TaskModel selectedTask) return;
            
            Task taskDialog = new(new TaskModel { Id = selectedTask.Id, Name = selectedTask.Name, Completion = selectedTask.Completion });
            taskDialog.ShowDialog();        // Show it as a modal dialog
            if (taskDialog.DataContext is not TaskModel taskModel) return;
            // Modify item
            selectedTask.Name = taskModel.Name;
            selectedTask.Completion = taskModel.Completion;
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem != null)
                _items.Remove((TaskModel)TaskListBox.SelectedItem);
        }

        #endregion
    }
}
