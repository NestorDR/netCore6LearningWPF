using System.Timers;
using System.Windows;
using CommonLibrary.ViewModels;
using LearningWPF.Models;
using Newtonsoft.Json.Linq;

namespace LearningWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Properties
        
        private string _loginMenuHeader = "Login";
        public string LoginMenuHeader
        {
            get => _loginMenuHeader;
            set => SetProperty(ref _loginMenuHeader, value);
        }

        private string? _statusMessage = string.Empty;
        public string? StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        /// <summary>
        /// Allow to control the visibility of the message via a border
        /// </summary>
        private bool _isInfoMessageVisible = true;
        public bool IsInfoMessageVisible
        {
            get => _isInfoMessageVisible;
            set => SetProperty(ref _isInfoMessageVisible, value);
        }

        /// <summary>
        ///  The small message
        /// </summary>
        private string? _infoMessage = string.Empty;
        public string? InfoMessage
        {
            get => _infoMessage;
            set => SetProperty(ref _infoMessage, value);
        }

        /// <summary>
        /// A message for the large title message
        /// </summary>
        private string? _infoMessageTitle = string.Empty;
        public string? InfoMessageTitle
        {
            get => _infoMessageTitle;
            set => SetProperty(ref _infoMessageTitle, value);
        }

        private int _infoMessageTimeout;
        public int InfoMessageTimeout
        {
            get => _infoMessageTimeout;
            set => SetProperty(ref _infoMessageTimeout, value);
        }

        private UserModel _userEntity = new();
        public UserModel UserEntity
        {
            get => _userEntity;
            set => SetProperty(ref _userEntity, value);
        }

        #endregion

        #region Menu Toggles

        private Visibility _startMenuGroupVisibility = Visibility.Collapsed;
        public Visibility StartMenuGroupVisibility
        {
            get => _startMenuGroupVisibility;
            set => SetProperty(ref _startMenuGroupVisibility, value);
        }

        private Visibility _imagesMenuGroupVisibility = Visibility.Collapsed;
        public Visibility ImagesMenuGroupVisibility
        {
            get => _imagesMenuGroupVisibility;
            set => SetProperty(ref _imagesMenuGroupVisibility, value);
        }
        
        private Visibility _mvvmMenuGroupVisibility = Visibility.Collapsed;
        public Visibility MvvmMenuGroupVisibility
        {
            get => _mvvmMenuGroupVisibility;
            set => SetProperty(ref _mvvmMenuGroupVisibility, value);
        }

        #endregion

        #region ClearInfoMessage Method

        public void ClearInfoMessages()
        {
            InfoMessage = string.Empty;
            InfoMessageTitle = string.Empty;
            IsInfoMessageVisible = false;
        }
        
        #endregion

        #region Message Timer Methods
        
        private Timer? _infoMessageTimer;
        public virtual void CreateInfoMessageTimer()
        {
            if (_infoMessageTimer == null)
            {
                // Create informational message timer
                _infoMessageTimer = new Timer(_infoMessageTimeout);
                // Connect to an Elapsed event
                _infoMessageTimer.Elapsed += MessageTimer_Elapsed;
            }
            _infoMessageTimer.AutoReset = false;
            _infoMessageTimer.Enabled = true;
            IsInfoMessageVisible = true;
        }

        private void MessageTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            IsInfoMessageVisible = false;
        }
        
        #endregion
    }
}
