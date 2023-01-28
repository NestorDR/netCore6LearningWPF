using System.Collections.ObjectModel;
using CommonLibrary.Exceptions;
using CommonLibrary.MessageBroker;
using CommonLibrary.Troubles;

namespace CommonLibrary.ViewModels
{
    public class ViewModelBase : CommonBase
    {
        #region Public Properties

        private ObservableCollection<TroubleMessage> _troubleMessages = new();
        public ObservableCollection<TroubleMessage> TroubleMessages
        {
            get => _troubleMessages;
            set => SetProperty(ref _troubleMessages, value);
        }

        public bool HasTroubles => _troubleMessages.Any();

        private bool _isTroubleVisible;
        public bool IsTroubleVisible
        {
            get => _isTroubleVisible;
            set => SetProperty(ref _isTroubleVisible, value);
        }

        #endregion

        #region Business Rule Message Methods

        public virtual void AddTroubleMessage(string propertyName, string message)
        {
            _troubleMessages.Add(new TroubleMessage { Message = message, PropertyName = propertyName });
            IsTroubleVisible = true;
        }

        public virtual bool IsValid() => TroubleMessages.Count == 0;

        #endregion

        #region Clear Method

        public virtual void Clear()
        {
            TroubleMessages.Clear();
            IsTroubleVisible = false;
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
