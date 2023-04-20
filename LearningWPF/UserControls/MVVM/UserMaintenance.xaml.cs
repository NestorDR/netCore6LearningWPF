using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using CommonLibrary.Exceptions;
using System.Windows.Threading;
// App modules
using LearningWPF.Helper;
using LearningWPF.Models;
using LearningWPF.ViewModels;
using System.Windows.Data;

namespace LearningWPF.UserControls.MVVM
{
    /// <summary>
    /// Interaction logic for UserMaintenance.xaml
    /// </summary>
    public partial class UserMaintenance : UserControl
    {
        #region Private variables

        // User control's view model class
        private readonly UserMaintenanceViewModel _viewModel;

        #endregion

        public UserMaintenance()
        {
            InitializeComponent();

            // Connect to instance of the view model created by the XAML
            _viewModel = (UserMaintenanceViewModel)Resources["ViewModel"];
        }

        private void UserMaintenanceControl_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadUsers();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel.IsReadOnly)
            {
                // Form button in [Add New] mode 
                _viewModel.BeginEdit(true);
            }
            else
            {
                // Form button in [Save] mode 
                if (ValidationHelper.IsValid(this)) _viewModel.Save();
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.IsReadOnly)
            {
                // Form button in [Cancel] mode 
                _viewModel.CancelEdit();
                return;
            }

            if (_viewModel.UserSelectedItem == null) return;

            // Button in [Edit] mode
            _viewModel.BeginEdit();

            if ((string?)((Button)sender).Tag == "FormButton")
            {
                // Clicked the [Edit] form button. Focus to the 1º editable field and starts editing.
                UserNameTextBox.Focus();
            }
            else
            {
                // Clicked the [Edit] data row button. Focus to the 1º editable cell/column of the data row and starts editing.
                UtilHelper.EditOnDataGridCell((DependencyObject)e.OriginalSource, this.UserNameDataGridColumn);
            }

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Ask if the user wants to delete the entity
            string caption = "Confirm";
            string text = $"Are you sure you want to Delete the User {_viewModel.UserSelectedItem!.UserName}?";

            // Confirm
            if (MessageBox.Show(text, caption, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                _viewModel.Delete();
        }

        /// <summary>
        /// Begins a drag and drop operation to reorder the rows
        /// </summary>
        private void ReorderIcon_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed
                || _viewModel.UserSelectedItem is not { } draggedItem) return;
            DragDrop.DoDragDrop(UsersDataGrid, draggedItem, DragDropEffects.Move);
        }

        /// <summary>
        /// Reorders rows when a drop event occurs with the data grid as the target
        /// </summary>
        private void UsersDataGrid_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (_viewModel.UserSelectedItem is not { } draggedItem) return;
                int previousIndex = _viewModel.Users.IndexOf(draggedItem);

                int droppedIndex;
                int lastIndex = _viewModel.Users.Count - 1;
                if (((FrameworkElement)e.OriginalSource).DataContext is UserModel targetItem)
                    droppedIndex = _viewModel.Users.IndexOf(targetItem);
                else if (_viewModel.Users[0].DragRowEffect != DragRowEffect.None)
                    // Item dropped above the first row bounds, take first row
                    droppedIndex = 0;
                else if (_viewModel.Users[lastIndex].DragRowEffect != DragRowEffect.None)
                    // Item dropped below the last row bounds, take last row
                    droppedIndex = lastIndex;
                else return;

                if (droppedIndex == previousIndex) return;

                // Reposition dropped item
                _viewModel.Users.Remove(draggedItem);
                if (droppedIndex > lastIndex)
                    _viewModel.Users.Add(draggedItem);
                else
                    _viewModel.Users.Insert(droppedIndex, draggedItem);

                // Select dropped Item
                _viewModel.UserSelectedItem = draggedItem;
                e.Handled = true;

                // Reorder the position numbers
                // _viewModel.RenumberPositions();
            }
            catch (Exception ex)
            {
                ExceptionManager.LogToDebug(ex);
            }
        }

        /// <summary>
        /// Occurs continuously while an item is being dragged within the bounds of a row that can be a drop target
        /// </summary>
        private void Row_DragOver(object sender, DragEventArgs e)
        {
            _viewModel.RemoveDragRowEffects();

            if (_viewModel.UserSelectedItem is not { } draggedItem) return;

            int targetIndex =
                _viewModel.Users.IndexOf((UserModel)((FrameworkElement)e.OriginalSource).DataContext);
            if (targetIndex < 0) return;

            int draggedIndex = _viewModel.Users.IndexOf(draggedItem);
            _viewModel.Users[targetIndex].DragRowEffect =
                targetIndex == 0 || draggedIndex > targetIndex
                    ? DragRowEffect.Before
                    : DragRowEffect.After;
        }

        /// <summary>
        /// Occurs when the dragged item is dropped within the grid bounds
        /// </summary>
        private void UsersDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.DragOver += Row_DragOver;
        }

        /// <summary>
        /// When a key is pressed while the focus is on the row under editing, prevent the row change
        /// </summary>
        private void UsersDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (_viewModel.IsReadOnly) return;
            // While data grid is in edit mode, prevent row change
            ModifierKeys modifier = e.KeyboardDevice.Modifiers;
            switch (e.Key)
            {
                case Key.Enter or Key.Up or Key.Down:
                case Key.Tab when UsersDataGrid.CurrentColumn == LastDataGridColumn && modifier != ModifierKeys.Shift:
                case Key.Tab when UsersDataGrid.CurrentColumn == UserNameDataGridColumn && modifier == ModifierKeys.Shift:
                    e.Handled = true;
                    break;
            }
        }

        /// <summary>
        /// When any mouse button is pressed while a row is in editing, prevent the row change
        /// </summary>
        private void UsersDataGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_viewModel.IsReadOnly) return;
            // While data grid is in edit mode, prevent row change
            // But the right mouse button is not trapped, so you must also code the SelectionChanged event.
            var nextDataRow = (FrameworkElement)e.OriginalSource;
            e.Handled = nextDataRow.DataContext != UsersDataGrid.SelectedItem;
        }

        /// <summary>
        /// When the selected row changes and the previous row is in edit, undo the row change
        /// </summary>
        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_viewModel.IsReadOnly || e.RemovedItems.Count == 0 || e.RemovedItems[0] == null 
                || e.OriginalSource != sender || sender is not DataGrid dg) return;

            // If the PreviewKey and PreviewMouse methods failed to trap the row change in the data grid during editing,
            // undo the row change by returning to the item being edited.
            // The event handler is momentarily removed to prevent recursive calls and a StackOverflow exception. Then it is attached again.
            // Visit: https://social.msdn.microsoft.com/Forums/vstudio/en-US/bb0a6458-b073-433c-b98b-4b18aa31e0f3/how-to-stop-datagrid-selectionchange-from-selectionchanged-event?forum=wpf
            dg.SelectionChanged -= UsersDataGrid_SelectionChanged;
            DispatcherOperation operation = dg.Dispatcher.BeginInvoke(
                new Action(() => dg.SelectedItem = e.RemovedItems[0]), DispatcherPriority.ContextIdle);
            operation.Completed +=
                (s, ea) => dg.SelectionChanged += UsersDataGrid_SelectionChanged;
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is not TextBox txt) return;

            // Capitalize: Proper Case
            txt.Text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(txt.Text.ToLower());
        }
    }
}
