using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LearningWPF.ViewModels;

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

        private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.BeginEdit(true);
        }

        private void EditButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.BeginEdit(false);
        }

        private void DeleteButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //listControl.DeleteUser();
        }

        private void UndoButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.CancelEdit();
        }

        private void SaveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _viewModel.Save();
        }

        private void ReorderIcon_OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if (e.LeftButton != MouseButtonState.Pressed
            //    || _viewModel.RollerSelectedItem is not { } draggedItem) return;
            //DragDrop.DoDragDrop(RollersDataGrid, draggedItem, DragDropEffects.Move);
        }

    }
}
