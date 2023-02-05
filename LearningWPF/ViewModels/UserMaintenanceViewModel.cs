using System;
using System.Collections.ObjectModel;
using System.Linq;
// App modules
using CommonLibrary.ViewModels;
using CommonLibrary.Troubles;
using LearningWPF.Helper;
using LearningWPF.Models;
using LearningWPF.Services;
using static LearningWPF.Helper.EnumHelper;

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
                ServiceBase service = new();
                UserRoles = new ObservableCollection<UserRoleModel>(service.Get<UserRoleModel>());

                if (!UserRoles.Any())
                {
                    this.Clear();

                    // The UserRole data table is empty, initialize it.
                    foreach (int id in Enum.GetValues(typeof(UserRole)))
                    {
                        UserRoles.Add(
                            new UserRoleModel
                            {
                                Id = id, 
                                Name = $"{Enum.GetName(typeof(UserRole), id)}"
                            });
                    }

                    if (!service.Save(UserRoles.ToList(), isAddMode:true))
                    {
                        TroubleMessages =
                            new ObservableCollection<TroubleMessage>(service.TroubleMessages);
                        IsTroubleVisible = true;
                    }

                    //UserRoles.Add(new UserRoleModel{Id = (int)EnumHelper.UserRole.Guest, Name = "Guest"});
                    //UserRoles.Add(new UserRoleModel{Id = (int)EnumHelper.UserRole.Admin, Name = "Admin"});
                    //UserRoles.Add(new UserRoleModel{Id = (int)EnumHelper.UserRole.User, Name = "User"});
                }
                
                if (!HasTroubles) Users = new ObservableCollection<UserModel>(service.Get<UserModel>());
            }
            catch (Exception ex)
            {
                PublishException(ex);
                throw;
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
                        user.UserRoleId = UserSelectedItem.UserRoleId;
                        user.Active = UserSelectedItem.Active;
                        user.EmailAddress = UserSelectedItem.EmailAddress;
                        if (string.IsNullOrWhiteSpace(user.Password)) user.Password = UserModel.EXAMPLE_PASSWORD;
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
                ServiceBase service = new();
                if (service.Delete(UserSelectedItem))
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
                        new ObservableCollection<TroubleMessage>(service.TroubleMessages);
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
