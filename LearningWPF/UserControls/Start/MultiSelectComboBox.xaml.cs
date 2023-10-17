using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
// --- App modules ---
using LearningWPF.Models;
using LearningWPF.Services;
using Key = System.Windows.Input.Key;

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

            // Initialize combobox
            // Get item list to display into combobox
            _items = new ObservableCollection<TaskModel>(new TaskService().Get());
            // Bind combobox: set its data source
            TaskComboBox.ItemsSource = _items;
        }

        /// <summary>
        /// Avoid the usual behavior of the built-in combo box
        /// </summary>
        private void TaskComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (sender is not ComboBox { SelectedItem: TaskModel } cmb || cmb.SelectedIndex == -1) return;
            cmb.SelectedIndex = -1;
        }

        /// <summary>
        /// Reduces the items source based on user requirements 
        /// </summary>
        private void TaskComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine("TaskComboBox_TextChanged");
            if (TaskComboBox.IsTextSearchEnabled)
                TaskComboBox.ItemsSource = _items.Where(x => x.Name.StartsWith(TaskComboBox.Text.Trim()));
        }

        /// <summary>
        /// Selects or deselects items based on user requirements 
        /// </summary>
        private void ItemCheckBox_CheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            SetSelectedIndex(GetSelectedIndex(out _));
            DisplaySelectedItems();
        }
        /// <summary>
        /// Toggle check value and prevents the combobox from closing after the selection
        /// </summary>
        private void ItemCheckBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Visit https://stackoverflow.com/questions/65671234/how-can-i-disable-the-selection-event-on-this-wpf-combobox-without-disabling-it
            if (sender is not CheckBox checkBox) return;
            checkBox.IsChecked = !checkBox.IsChecked;
            e.Handled = true;
        }

        /// <summary>
        /// Prevents the edition depending on whether TextSearch is allowed
        /// </summary>
        private void TaskComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("TaskComboBox_PreviewKeyDown");
            Debug.WriteLine($"  e.Key .......: {e.Key}");
            if (e.Key is Key.Tab or Key.Escape or Key.System or >= Key.F1 and <= Key.RightAlt) return;

            // Prevent the edition depending on whether TextSearch is allowed
            e.Handled = !TaskComboBox.IsTextSearchEnabled;
        }

        /// <summary>
        /// If there is only one item selected in the combobox, it emulates the behavior of the arrows to select the Previous/Next item
        /// </summary>
        private void TaskComboBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            Debug.WriteLine("TaskComboBox_PreviewKeyUp");
            Debug.WriteLine($"  e.Key .......: {e.Key}");
            if (e.Key is Key.Tab or Key.Escape) return;

            int index = GetSelectedIndex(out int selectedCount);
            Debug.WriteLine($"  selectedCount: {index} de {selectedCount}");

            if (selectedCount <= 1 && e.Key < Key.Help)
            {
                int increment = e.Key switch
                {
                    Key.Down or Key.Right when index < _items.Count - 1 => 1,
                    Key.Up or Key.Left when index > 0 => -1,
                    _ => 0
                };
                
                Debug.WriteLine($"  increment ...: {increment}");
                if (increment != 0) 
                    SelectOneItem(index + increment);
                else
                    DisplaySelectedItems();

                // Prevent the edition depending on whether TextSearch is allowed
                e.Handled = !TaskComboBox.IsTextSearchEnabled;
            }
            else
            {
                DisplaySelectedItems();
            }
        }

        /// <summary>
        /// Refresh combobox text
        /// </summary>
        private void TaskComboBox_DropDownClosed(object? sender, EventArgs e)
        {
            DisplaySelectedItems();
        }

        /// <summary>
        /// Selects properly item in combobox
        /// </summary>
        private void TaskComboBox_DropDownOpened(object? sender, EventArgs e)
        {
            SetSelectedIndex(GetSelectedIndex(out _));
        }

        /// <summary>
        /// Randomly selects one and only one item 
        /// </summary>
        private void RandomSelectButton_Click(object sender, RoutedEventArgs e)
        {
            int upperBoundExclusive = _items.Count;
            int randomIndex = RandomNumberGenerator.GetInt32(upperBoundExclusive);
            SelectOneItem(randomIndex);
        }

        /// <summary>
        /// Select one and only one item in the combobox
        /// </summary>
        private void SelectOneItem(int index)
        {
            // Select one and only one item in the item list
            for (int i = 0; i < _items.Count; i++) _items[i].Selected = i == index;
            SetSelectedIndex(index);
            DisplaySelectedItems();
        }

        /// <summary>
        /// Get the index of the item that should be selected in the combo box.
        /// If there is more than one item selected then the index will be -1.
        /// </summary>
        /// <param name="selectedCount">Selected items counter</param>
        private int GetSelectedIndex(out int selectedCount)
        {
            selectedCount = 0;
            int index = -1;
            for (int i = 0; i < _items.Count; i++)
            {
                if (!_items[i].Selected) continue;
                selectedCount++;
                index = i;
            }

            if (selectedCount != 1) index = -1;
            return index;
        }

        /// <summary>
        /// Select item in combobox through index
        /// </summary>
        private void SetSelectedIndex(int index)
        {
            if (TaskComboBox.SelectedIndex == index) return;
            TaskComboBox.SelectionChanged -= TaskComboBox_SelectionChanged;
            TaskComboBox.SelectedIndex = index;
            TaskComboBox.SelectionChanged += TaskComboBox_SelectionChanged;
        }

        /// <summary>
        /// Display combobox text
        /// </summary>
        private void DisplaySelectedItems()
        {
            string list = string.Join(", ", _items.Where(item => item.Selected).Select(i => i.Name));

            SelectedItemsControlTextBox.Text = list;

            if (TaskComboBox.IsEditable &&
                TaskComboBox.Template.FindName("PART_EditableTextBox", TaskComboBox) is TextBox txt)
                txt.Text = list;

            TaskComboBox.Text = list;
        }
    }
}
