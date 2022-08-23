using System.Timers;
using CommonLibrary.ViewModels;

namespace LearningWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Properties
        
        private string _loginMenuHeader = "Login";
        public string LoginMenuHeader
        {
            get => _loginMenuHeader;
            set
            {
                _loginMenuHeader = value;
                RaisePropertyChanged(nameof(LoginMenuHeader));
            }
        }

        private string? _statusMessage = string.Empty;
        public string? StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                RaisePropertyChanged(nameof(StatusMessage));
            }
        }

        /// <summary>
        /// Allow to control the visibility of the message via a border
        /// </summary>
        private bool _isInfoMessageVisible = true;
        public bool IsInfoMessageVisible
        {
            get => _isInfoMessageVisible;
            set
            {
                _isInfoMessageVisible = value;
                RaisePropertyChanged(nameof(IsInfoMessageVisible));
            }
        }

        /// <summary>
        ///  The small message
        /// </summary>
        private string? _infoMessage = string.Empty;
        public string? InfoMessage
        {
            get => _infoMessage;
            set
            {
                _infoMessage = value;
                RaisePropertyChanged(nameof(InfoMessage));
            }
        }

        /// <summary>
        /// A message for the large title message
        /// </summary>
        private string? _infoMessageTitle = string.Empty;
        public string? InfoMessageTitle
        {
            get => _infoMessageTitle;
            set
            {
                _infoMessageTitle = value;
                RaisePropertyChanged(nameof(InfoMessageTitle));
            }
        }

        private int _infoMessageTimeout;
        public int InfoMessageTimeout
        {
            get => _infoMessageTimeout;
            set
            {
                _infoMessageTimeout = value;
                RaisePropertyChanged(nameof(InfoMessageTimeout));
            }
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
