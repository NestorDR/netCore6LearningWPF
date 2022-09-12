using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommonLibrary.MessageBroker;
using LearningWPF.ViewModels;

namespace LearningWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Private variables
        // Main window's view model class
        private readonly MainWindowViewModel _viewModel;
        // Hold the main window's original status message
        private readonly string? _originalMessage;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

            // Connect to instance of the view model created by the XAML
            _viewModel = (MainWindowViewModel)Resources["ViewModel"];

            // Get the original status message
            _originalMessage = _viewModel.StatusMessage;

            // Initialize the Message Broker Events
            MessageBroker.Instance.MessageReceived += Instance_MessageReceived;

        }
        #endregion

        private async void MainWindow_OnContentRendered(object? sender, EventArgs e)
        {
            // Call method to load resources application
            await LoadApplication();

            // Turn off informational message area
            _viewModel.ClearInfoMessages();
        }

        private void Instance_MessageReceived(object sender, MessageBrokerEventArgs e)
        {
            switch (e.MessageName)
            {
                case MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE_TITLE:
                    _viewModel.InfoMessageTitle = e.MessagePayload?.ToString();
                    _viewModel.CreateInfoMessageTimer();
                    break;

                case MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE:
                    _viewModel.InfoMessage = e.MessagePayload?.ToString();
                    _viewModel.CreateInfoMessageTimer();
                    break;

                case MessageBrokerMessages.DISPLAY_STATUS_MESSAGE:
                    // Set new status message
                    _viewModel.StatusMessage = e.MessagePayload?.ToString();
                    break;

                case MessageBrokerMessages.CLOSE_USER_CONTROL:
                    CloseUserControl();
                    break;
            }
        }

        /// <summary>
        /// Determines whether to process a command or load a user control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            var option = (ListBoxItem)sender;

            // The Tag property contains a command or the name of a user control to load
            if (option.Tag == null) return;
            var cmd = option.Tag.ToString();

            if (cmd!.Contains('.'))
            {
                // Display a user control
                LoadUserControl(cmd);
                MenuToggleButton.IsChecked = false;
            }
            else if (cmd!.EndsWith("ToggleMenu"))
            {
                // Expand/Collapse menu group
                ToggleMenuGroup(cmd);
            }
            else
            {
                // Process special commands
                ProcessMenuCommands(cmd);
                MenuToggleButton.IsChecked = false;
            }

        }

        /// <summary>
        /// Loads and displays a control in content area.
        /// </summary>
        /// <param name="controlName">Name of the control to display.</param>
        private void LoadUserControl(string controlName)
        {
            if (!ShouldLoadUserControl(controlName)) return;

            // Create a Type from controlName parameter
            var ucType = Type.GetType(controlName);
            if (ucType == null)
            {
                MessageBox.Show($"The control: {controlName} does not exist.");
            }
            else
            {
                // Close current user control in content area
                // NOTE: Optionally add current user control to a list 
                //       so you can restore it when you close the newly added one
                CloseUserControl();

                // Create an instance of this control
                if (Activator.CreateInstance(ucType) is UserControl uc)
                {
                    // Display control in content area
                    DisplayUserControl(uc);
                }
            }
        }

        /// <summary>
        /// Adds the new user control to the content area to display it.
        /// </summary>
        /// <param name="uc"></param>
        public void DisplayUserControl(UserControl uc)
        {
            ContentArea.Children.Add(uc);
        }

        /// <summary>
        /// Handles any expand/collapse toggle within the Tag property of the menu items.
        /// </summary>
        /// <param name="menuGroupToggle">Command found in Tag property.</param>
        private void ToggleMenuGroup(string menuGroupToggle)
        {
            menuGroupToggle = menuGroupToggle.Replace("ToggleMenu", "");

            static Visibility ToggleVisibility(Visibility currentVisibility) 
                => currentVisibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;

            switch (menuGroupToggle.ToLower())
            {
                case "start":
                    _viewModel.StartMenuGroupVisibility = ToggleVisibility(_viewModel.StartMenuGroupVisibility);
                    break;
                case "mvvm":
                    _viewModel.MvvmMenuGroupVisibility = ToggleVisibility(_viewModel.MvvmMenuGroupVisibility);
                    //;
                    break;
            }
        }

        /// <summary>
        /// Handles any commands within the Tag property of the menu items.
        /// </summary>
        /// <param name="command">Command found in Tag property.</param>
        private void ProcessMenuCommands(string command)
        {
            switch (command.ToLower())
            {
                case "exit":
                    this.Close();
                    break;

                case "login":
                    /*
                    // TODO: Login/Logout
                    if (_viewModel.UserEntity.IsLoggedIn)
                    {
                        // Logging out, so close any open windows
                        // Reset the user object
                        // Make menu display Login
                    }
                    else
                    {
                        // Display the login screen
                    }
                    */
                    break;
            }
        }

        /// <summary>
        /// Make sure you don't reload a control already loaded.
        /// </summary>
        /// <param name="controlName">Name of the control to load.</param>
        /// <returns>True if the user control is loaded and displayed yet, otherwise False.</returns>
        private bool ShouldLoadUserControl(string controlName)
        {
            bool result = true;

            if (ContentArea.Children.Count <= 0) return result;

            // To check whether the current user control loaded into the content area is the same one the user is trying to load
            if (((UserControl)ContentArea.Children[0]).GetType().FullName == controlName)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Allow you close the user control displayed.
        /// </summary>
        private void CloseUserControl()
        {
            // Remove current user control
            ContentArea.Children.Clear();

            // Restore the original status message
            _viewModel.StatusMessage = _originalMessage;
        }

        #region LoadApplication Method
        public async Task LoadApplication()
        {
            static async Task Delay(float seconds) => await Task.Delay((int) (seconds * 1000));

            _viewModel.InfoMessage = "Simulating the loading of some kind of object...";
            // Use the System.Windows.Threading.Dispatcher class to call the LoadObjectTypes() method in the view model.
            // await Dispatcher.BeginInvoke(new Action(() => { _viewModel.LoadObjectTypes(); }), DispatcherPriority.Background);
            await Delay(1);
            _viewModel.InfoMessage = "Simulating another load...";
            await Delay(1);
        }
        #endregion

        private void MenuToggleButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
