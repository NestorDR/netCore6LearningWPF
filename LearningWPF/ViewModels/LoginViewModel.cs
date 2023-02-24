using System;
using System.Linq;
using System.Security.Principal;
// --- App modules ---
using CommonLibrary.MessageBroker;
using CommonLibrary.ViewModels;
using LearningWPF.Data;
using LearningWPF.Helper;
using LearningWPF.Models;
using LearningWPF.Services;

namespace LearningWPF.ViewModels
{
    internal sealed class LoginViewModel : ViewModelBase
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

            TroubleMessages.Clear();
            if (string.IsNullOrWhiteSpace(Entity.UserName))
                AddTroubleMessage("UserName", "User Name Must Be Filled In");
            if (string.IsNullOrWhiteSpace(Entity.Password))
                AddTroubleMessage("Password", "Password Must Be Filled In");

            return (TroubleMessages.Count == 0);
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
                if (db.Users!.Any(u => u.UserName == Entity.UserName && u.Password == Entity.Password))
                {
                    // Set the application security
                    SetAppIdentity();

                    // Success
                    result = true;
                }
                else if (Entity.Password == UserModel.EXAMPLE_PASSWORD)
                {
                    // Complete user data
                    Entity.FirstName = Entity.UserName;
                    Entity.UserRoleId = (int)UserRole.User;
                    Entity.EmailAddress = "";

                    // Set the application security
                    SetAppIdentity();       

                    // Save new user if UserRole data table has saved records
                    ServiceBase service = new(db);
                    result = !service.Get<UserRoleModel>().Any() || service.Save(Entity, isAddMode: true);
                }
                else
                {
                    AddTroubleMessage("LoginFailed", "Invalid User Name and/or Password.");
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw;
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

        /// <summary>
        /// Set the application security entity based on values from the current logged user and WindowsIdentity.
        /// </summary>
        public void SetAppIdentity()
        {
            // Get values from the current WindowsIdentity.
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();

            // Create a generic user identity object based on the logged user and
            // the current Windows identity name and authentication type.
            string fullyQualifiedUserName = $"{Environment.UserDomainName}\\{Entity.UserName}";
            string userName;
            string authenticationType;
            if (fullyQualifiedUserName == windowsIdentity.Name && windowsIdentity.AuthenticationType != null)
            {
                userName = fullyQualifiedUserName;
                authenticationType = windowsIdentity.AuthenticationType;

            }
            else
            {
                authenticationType = UtilHelper.GetApplicationName();
                userName = $"{authenticationType}\\{Entity.UserName}";
            }

            GenericIdentity applicationIdentity = new(userName, authenticationType);

            // Create a generic security entity for the application
            GenericPrincipal applicationPrincipal = new(applicationIdentity, new[] { Entity.RoleName });

            // Set the generic security entity for the application with extra info (as Roles)
            AppDomain.CurrentDomain.SetThreadPrincipal(applicationPrincipal);
        }

    }
}
