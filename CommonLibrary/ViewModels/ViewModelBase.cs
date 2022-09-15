using System.Collections.ObjectModel;
using CommonLibrary.Exceptions;
using CommonLibrary.MessageBroker;
using CommonLibrary.Validation;

namespace CommonLibrary.ViewModels
{
    public class ViewModelBase : CommonBase
    {
        #region Public Properties

        private ObservableCollection<ValidationMessage> _validationMessages = new();
        public ObservableCollection<ValidationMessage> ValidationMessages
        {
            get => _validationMessages;
            set => SetProperty(ref _validationMessages, value);
        }

        private bool _isValidationVisible;
        public bool IsValidationVisible
        {
            get => _isValidationVisible;
            set => SetProperty(ref _isValidationVisible, value);
        }

        #endregion

        #region AddBusinessRuleMessage Method

        public virtual void AddValidationMessage(string propertyName, string message)
        {
            _validationMessages.Add(new ValidationMessage { Message = message, PropertyName = propertyName });
            IsValidationVisible = true;
        }

        #endregion

        #region Clear Method

        public virtual void Clear()
        {
            ValidationMessages.Clear();
            IsValidationVisible = false;
        }

        #endregion

        #region DisplayStatusMessage Method

        public virtual void DisplayStatusMessage(string msg = "")
        {
            MessageBroker.MessageBroker.Instance.SendMessage(MessageBrokerMessages.DISPLAY_STATUS_MESSAGE, msg);
        }

        #endregion

        #region PublishException Method

        public void PublishException(Exception ex)
        {
            // Publish Exception
            ExceptionManager.Instance.Publish(ex);
        }

        #endregion

        #region Close Method

        public virtual void Close(bool wasCancelled = true)
        {
            MessageBroker.MessageBroker.Instance.SendMessage(MessageBrokerMessages.CLOSE_USER_CONTROL, wasCancelled);
        }

        #endregion

        #region Dispose Method

        public virtual void Dispose()
        {
        }

        #endregion
    }
}
