using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
// --- App modules ---
using LearningWPF.Models;
using LearningWPF.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LearningWPF.UserControls.Start
{
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox : UserControl
    {
        private readonly ObservableCollection<TaskModel> _items;
        private string _list = string.Empty;

        public MultiSelectComboBox()
        {
            InitializeComponent();

            // Initialize combobox
            // Get item list to display into combobox
            _items = new ObservableCollection<TaskModel>(new TaskService().Get());
            // Bind combobox: set its data source
            TaskComboBox.ItemsSource = _items;
        }

        private void TaskComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (sender is not ComboBox { SelectedItem: TaskModel } cmb || cmb.SelectedIndex == -1) return;
            cmb.SelectedIndex = -1;
        }

        private void TaskComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TaskComboBox.IsTextSearchEnabled)
                TaskComboBox.ItemsSource = _items.Where(x => x.Name.StartsWith(TaskComboBox.Text.Trim()));
        }

        private void ItemCheckBox_CheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            IdentifySelectedItems();
        }

        private void TaskComboBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            DisplayCheckedList();

            // TODO: Limpiar param [KeyEventArgs e] en keydown

            // Prevent the edition depending on whether TextSearch is allowed
            e.Handled = !TaskComboBox.IsTextSearchEnabled;
        }

        private void TaskComboBox_DropDownClosed(object? sender, EventArgs e)
        {
            DisplayCheckedList();
        }

        private void TaskComboBox_OnDropDownOpened(object? sender, EventArgs e)
        {
            IdentifySelectedItems();
        }

        private void RandomSelectButton_Click(object sender, RoutedEventArgs e)
        {
            int upperBoundExclusive = _items.Count;
            int randomIndex = RandomNumberGenerator.GetInt32(upperBoundExclusive);

            SelectTask(randomIndex);
            IdentifySelectedItems();
        }

        private void SelectTask(int index)
        {
            for (int i = 0; i < _items.Count; i++) _items[i].Checked = i == index;
        }

        private void IdentifySelectedItems()
        {
            int checkedCount = 0;
            int selectedIndex = -1;

            checkedCount = 0;
            for (int i = 0; i < _items.Count; i++)
            {
                if (!_items[i].Checked) continue;
                checkedCount++;
                selectedIndex = i;
            }

            if (checkedCount != 1) selectedIndex = -1;

            TaskComboBox.SelectionChanged -= TaskComboBox_SelectionChanged;
            TaskComboBox.SelectedIndex = selectedIndex;
            TaskComboBox.SelectionChanged += TaskComboBox_SelectionChanged;

            DisplayCheckedList();
        }

        private void DisplayCheckedList()
        {
            _list = string.Empty;
            foreach (TaskModel task in _items)
                if (task.Checked) _list += (_list.Length > 0 ? ", " : "") + task.Name;

            SelectedTasksTextBox.Text = _list;

            if (TaskComboBox.IsEditable &&
                TaskComboBox.Template.FindName("PART_EditableTextBox", TaskComboBox) is TextBox txt)
                txt.Text = _list;

            TaskComboBox.Text = _list;
        }
    }
}
