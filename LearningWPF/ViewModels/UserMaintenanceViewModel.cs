using System;
using System.Collections.ObjectModel;
using System.Linq;
// App modules
using CommonLibrary.ViewModels;
using CommonLibrary.Troubles;
using LearningWPF.Data;
using LearningWPF.Helper;
using LearningWPF.Models;
using LearningWPF.Services;

namespace LearningWPF.ViewModels
{
    public class UserMaintenanceViewModel : ViewModelAddEditDeleteBase
    {
        #region Properties

        private ObservableCollection<UserModel> _users = new();

        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        private UserModel? _userSelectedItem;
        public UserModel? UserSelectedItem
        {
            get => _userSelectedItem;
            set
            {
                if (SetProperty(ref _userSelectedItem, value))
                    NotifyPropertyChanged(nameof(this.HasEditable));
                RemoveDragRowEffects();
            }
        }

        private UserModel? _originalUser;

        public bool HasEditable => UserSelectedItem != null;

        private ObservableCollection<UserRoleModel> _userRoles = new();
        public ObservableCollection<UserRoleModel> UserRoles
        {
            get => _userRoles;
            set => SetProperty(ref _userRoles, value);
        }

        #endregion Properties

        public virtual void LoadUsers()
        {
            try
            {
                var db = new AppDbContext();
                UserRoles = new ObservableCollection<UserRoleModel>(db.UserRoles!);
                Users = new ObservableCollection<UserModel>(db.Users!);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Reset the drag indicator effect to None
        /// </summary>
        public void RemoveDragRowEffects()
        {
            foreach (UserModel user in Users.Where(r => r.DragRowEffect != EnumHelper.DragRowEffect.None))
                user.DragRowEffect = EnumHelper.DragRowEffect.None;
        }

        /// <summary>
        /// Validate entity properties before saving.
        /// </summary>
        /// <returns>True if the entered data passes all checks, otherwise False.</returns>
        public override bool IsValid()
        {
            TroubleMessages.Clear();

            if (UserSelectedItem == null)
            {
                AddTroubleMessage(nameof(UserSelectedItem), "User: Null or missing entity!");
                return false;
            }

            // TODO: Add here validation rules for entity properties

            return base.IsValid();
        }
        public override bool Save()
        {
            bool result = false;

            try
            {
                if (this.IsValid())
                {
                    ServiceBase userService = new();

                    // Get latest data from the database 
                    int userId = this.UserSelectedItem!.Id;
                    UserModel? user;
                    if (userId > 0)
                    {
                        user = userService.GetById<UserModel>(userId);
                        if (user == null)
                        {
                            // Entity to update no longer exists in the database
                            AddTroubleMessage(nameof(UserSelectedItem), "The user you want to update no longer exists in the database.");
                            goto endSaving;
                        }

                        // Update editable properties in this view
                        user.UserName = UserSelectedItem.UserName;
                        user.FirstName = UserSelectedItem.FirstName;
                        user.LastName = UserSelectedItem.LastName;
                        user.Active = UserSelectedItem.Active;
                        user.EmailAddress = UserSelectedItem.EmailAddress;
                    }
                    else
                    {
                        // New User
                        user = UserSelectedItem;
                    }

                    // Save
                    if (userService.Save(user, IsAddMode))
                    {
                        // Set Original Entity equal to changed entity
                        _originalUser = user;

                        // If is new entity, add to view model entities collection
                        if (IsAddMode) Users.Add(user);

                        // Return success
                        result = true;

                        // Set mode back to normal display
                        CancelEdit();
                    }
                    else
                    {
                        TroubleMessages = new ObservableCollection<TroubleMessage>(userService.TroubleMessages);
                    }
                }

                endSaving:
                if (this.HasTroubles) IsTroubleVisible = true;
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw;
            }

            return result;
        }

        public override bool Delete()
        {
            if (UserSelectedItem == null) return true;

            bool result = false;

            try
            {
                ServiceBase UserRoleService = new();
                if (UserRoleService.Delete(UserSelectedItem))
                {
                    result = true;

                    // Remove from view model collection
                    int index = Users.IndexOf(UserSelectedItem);
                    Users.RemoveAt(index);

                    // Get the selected entity after deleting
                    if (Users.Count > 0)
                    {
                        if (index == Users.Count) --index;
                        UserSelectedItem = Users[index];
                    }
                    else
                    {
                        UserSelectedItem = null;
                    }
                }
                else
                {
                    TroubleMessages =
                        new ObservableCollection<TroubleMessage>(UserRoleService.TroubleMessages);
                    IsTroubleVisible = true;
                }
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw;
            }

            return result;
        }
        public override void BeginEdit(bool isAddMode = false)
        {

            // Guard original data of Entity object to allow cancel
            if (isAddMode)
            {
                // Copy and create new
                _originalUser = UserSelectedItem;
                UserSelectedItem = new UserModel();
            }
            else
            {
                // Clone to safeguard
                _originalUser = new UserModel();
                Clone(UserSelectedItem, _originalUser);
            }

            IsReadOnly = false;
            base.BeginEdit(isAddMode);
        }

        public override void CancelEdit()
        {
            if (IsAddMode)
                // Recover original Entity object copy
                UserSelectedItem = _originalUser;
            else
                // Clone original data safeguarded to Entity object
                Clone(_originalUser, UserSelectedItem);

            base.CancelEdit();

            IsReadOnly = true;
        }

    }
}
