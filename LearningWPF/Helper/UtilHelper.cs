using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace LearningWPF.Helper
{
    internal static class UtilHelper
    {
        /// <summary>
        /// After click the [Edit] data row button, focus to the 1º editable cell/column of the data row and starts editing.
        /// </summary>
        /// <param name="depObj">Dependency object</param>
        /// <param name="dgColumn">Data grid column name</param>
        public static void EditOnDataGridCell(DependencyObject depObj, DataGridColumn dgColumn)
        {
            // Visit: https://blog.magnusmontin.net/2013/11/08/how-to-programmatically-select-and-focus-a-row-or-cell-in-a-datagrid-in-wpf/comment-page-1/
            //        https://stackoverflow.com/questions/10279092/how-to-get-children-of-a-wpf-container-by-type/10279201#10279201

            // 1.Gets data row
            while (depObj is not DataGridRow)
                depObj = VisualTreeHelper.GetParent(depObj)!;
            DataGridRow row = (DataGridRow)depObj;

            // 2.Gets 1º editable cell
            DataGridCellsPresenter? presenter = GetChildOfType<DataGridCellsPresenter>(row);
            if (presenter?.ItemContainerGenerator.ContainerFromIndex(dgColumn.DisplayIndex)
                is not DataGridCell cell) return;

            // 3.Starts edition
            cell.Focus();
            SendKey(new[] { Key.F2, Key.End });
        }

        /// <summary>
        /// Generic method that recursively searches for a user interface element of a specific type among the descendants of a
        /// visual "object or tree" of type System.Windows.DependencyObject
        /// </summary>
        /// <typeparam name="T">UI Element type</typeparam>
        /// <param name="depObj">Dependency object</param>
        /// <returns>An UI Element</returns>
        public static T? GetChildOfType<T>(DependencyObject? depObj) where T : DependencyObject
        {
            // Visit: https://stackoverflow.com/questions/10279092/how-to-get-children-of-a-wpf-container-by-type/10279201#10279201

            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);

                T? result = child as T ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }

            return null;
        }

        /// <summary>
        /// Sends the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public static void SendKey(Key key)
        {
            // Visit: https://michlg.wordpress.com/2013/02/05/wpf-send-keys/
            //        https://learn.microsoft.com/en-us/dotnet/api/system.windows.input.key?view=windowsdesktop-6.0

            if (Keyboard.PrimaryDevice == null || Keyboard.PrimaryDevice.ActiveSource == null) return;

            var e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
            {
                RoutedEvent = Keyboard.KeyDownEvent
            };
            InputManager.Current.ProcessInput(e);

            // Note: Based on your requirements you may also need to fire events for:
            // RoutedEvent = Keyboard.PreviewKeyDownEvent
            // RoutedEvent = Keyboard.KeyUpEvent
            // RoutedEvent = Keyboard.PreviewKeyUpEvent
        }

        /// <summary>
        /// Send a sequence of keys
        /// </summary>
        /// <param name="keys">Key sequence.</param>
        public static void SendKey(Key[] keys)
        {
            foreach (Key key in keys) SendKey(key);
        }

    }
}
