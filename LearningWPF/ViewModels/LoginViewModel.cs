using System;
using System.Linq;
using CommonLibrary.MessageBroker;
using CommonLibrary.ViewModels;
//using LearningWPF.Data;
using LearningWPF.Models;

namespace LearningWPF.ViewModels
{
    public sealed class LoginViewModel : ViewModelBase
    {
        public LoginViewModel() : base()
        {
            DisplayStatusMessage("Login to Application");

            Entity = new UserModel
            {
                UserName = Environment.UserName
            };
        }

        private UserModel _entity;

        public UserModel Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                RaisePropertyChanged(nameof(Entity));
            }
        }
        /*
        public bool Validate()
        {
            Entity.IsLoggedIn = false;
            ValidationMessages.Clear();
            if (string.IsNullOrEmpty(Entity.UserName))
            {
                AddValidationMessage("UserName", "User Name Must Be Filled In");
            }

            if (string.IsNullOrEmpty(Entity.Password))
            {
                AddValidationMessage("Password", "Password Must Be Filled In");
            }

            var result = (ValidationMessages.Count == 0);

            return result;
        }

        public bool ValidateCredentials()
        {
            bool result = false;

            try
            {
                var db = new AppDbContext();

                // NOTE: Not using password here, but in production you would. I intentionally leave it out so as not having to go into hashing and securing your password.
                if (db.Users.Any(u => u.UserName == Entity.UserName))
                {
                    result = true;
                }
                else
                {
                    AddValidationMessage("LoginFailed",
                        "Invalid User Name and/or Password.");
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

            if (!Validate()) return false;
            // Check Credentials in User Table
            
            if (!ValidateCredentials()) return false;
            
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
                    MessageBrokerMessages.DISPLAY_TIMEOUT_INFO_MESSAGE_TITLE,
                    "User NOT Logged In.");
            }

            base.Close(wasCancelled);
        }
        */
    }
}
