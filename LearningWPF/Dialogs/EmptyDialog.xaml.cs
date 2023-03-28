using System.Windows;
using System.Windows.Controls;
using CommonLibrary.ViewModels;

namespace LearningWPF.Dialogs
{
    /// <summary>
    /// Interaction logic for EmptyDialog.xaml
    /// </summary>
    public partial class EmptyDialog : UserControl
    {
        #region Private variables
        private ViewModelBase _viewModel;
        #endregion

#pragma warning disable CS8618      // Allows private fields uninitialized
        public EmptyDialog()
#pragma warning restore CS8618
        {
            InitializeComponent();
        }

        private void EmptyDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel = (ViewModelBase)DataContext;
        }
    }
}
