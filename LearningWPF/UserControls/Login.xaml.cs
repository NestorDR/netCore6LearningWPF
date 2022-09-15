using LearningWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace LearningWPF.UserControls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : UserControl
    {
        #region Private variables
        // Login view model class
        private readonly LoginViewModel _viewModel;
        #endregion

        public Login()
        {
            InitializeComponent();

            // Connect to instance of the view model created by the XAML
            _viewModel = (LoginViewModel)this.Resources["ViewModel"];
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Add the Password manually because data binding does not work
            _viewModel.Entity.Password = PasswordTextBox.Password;

            _viewModel.Login();
        }
    }
}
