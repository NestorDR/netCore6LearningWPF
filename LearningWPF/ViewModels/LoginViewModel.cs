using System;
using System.Linq;
// --- App modules ---
using CommonLibrary.MessageBroker;
using CommonLibrary.ViewModels;
using LearningWPF.Data;
using LearningWPF.Models;

namespace LearningWPF.ViewModels
{
    public sealed class LoginViewModel : ViewModelBase
    {
        private UserModel _entity = null!;

        public UserModel Entity
        {
            get => _entity;
            set => SetProperty(ref _entity, value);
        }

        public LoginViewModel() : base()
        {
            DisplayStatusMessage("Login to Application");

            Entity = new UserModel
            {
                UserName = Environment.UserName
            };
        }

        /// <summary>
        /// Validate non-empty values
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            Entity.IsLoggedIn = false;

            ValidationMessages.Clear();
            if (string.IsNullOrWhiteSpace(Entity.UserName))
                AddValidationMessage("UserName", "User Name Must Be Filled In");
            if (string.IsNullOrWhiteSpace(Entity.Password))
                AddValidationMessage("Password", "Password Must Be Filled In");

            return (ValidationMessages.Count == 0);
        }

        /// <summary>
        /// Check Credentials in User Table
        /// </summary>
        /// <returns></returns>
        public bool ValidateCredentials()
        {
            bool result = false;

            try
            {
                AppDbContext db = new();

                // NOTE: It will be necessary to hash and secure your password.
                if (Entity.Password == "1234" 
                    || db.Users.Where(u => u.UserName == Entity.UserName && u.Password == Entity.Password).Count() > 0)
                {
                    result = true;
                }
                else
                {
                    AddValidationMessage("LoginFailed", "Invalid User Name and/or Password.");
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
            }

            return result;
        }

        public bool Login()
        {
            if (!Validate() || !ValidateCredentials()) return false;

            // Mark as logged in
            Entity.IsLoggedIn = true;

            // Send message that login was successful
            MessageBroker.Instance.SendMessage(MessageBrokerMessages.LOGIN_SUCCESS, Entity);

            // Close the user control
            Close(false);

            return true;
        }

        public override void Close(bool wasCancelled = true)
        {
            if (wasCancelled)
            {
                // Display Informational Message
                MessageBroker.Instance.SendMessage(
                    MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE_TITLE, "User NOT logged In.");
            }

            base.Close(wasCancelled);
        }
    }
}
