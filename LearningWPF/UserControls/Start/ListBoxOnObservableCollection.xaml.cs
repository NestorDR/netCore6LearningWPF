using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LearningWPF.Dialogs;
using LearningWPF.Models;

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
            _items = new ObservableCollection<TaskModel>
            {
                new () { Title = "Learning how to develop", Completion = 30 },
                new () { Title = "Learn C#", Completion = 50 },
                new () { Title = "Learning WPF", Completion = 75 },
                new () { Title = "Wash the car", Completion = 85 },
                new () { Title = "Buy beer", Completion = 90 },
                new () { Title = "Enjoy it!", Completion = 100 }
            };
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
                _parentWindow.Title = ((TaskModel)TaskListBox.SelectedItem).Title;
        }

        #region Selection

        private void ShowSelectedItemButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var o in TaskListBox.SelectedItems)
                MessageBox.Show(((TaskModel)o).Title, _parentWindow?.Title);
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
                if (o is not TaskModel item || !item.Title.Contains("C#")) continue;
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
            Task taskDialog = new(new TaskModel());
            taskDialog.ShowDialog();        // Show it as a modal dialog
            if (taskDialog.DataContext is TaskModel taskModel) _items.Add(taskModel);
        }

        private void ChangeItemButton_Click(object sender, RoutedEventArgs e)
        {
            // Before: ((TaskModel)TaskListBox.SelectedItem).Title = "Changed TaskModel";
            if (TaskListBox.SelectedItem is not TaskModel selectedTask) return;
            
            Task taskDialog = new(new TaskModel { Title = selectedTask.Title, Completion = selectedTask.Completion});
            taskDialog.ShowDialog(); // Show it as a modal dialog
            if (taskDialog.DataContext is TaskModel taskModel)
            {
                selectedTask.Title = taskModel.Title;
                selectedTask.Completion = taskModel.Completion;
            }
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem != null)
                _items.Remove((TaskModel)TaskListBox.SelectedItem);
        }

        #endregion
    }
}
