using System;
using System.Collections.ObjectModel;
using CommonLibrary.ViewModels;
using LearningWPF.Data;
using LearningWPF.Models;

namespace LearningWPF.ViewModels
{
    public class UserMaintenanceViewModel : ViewModelAddEditDeleteBase
    {
        private ObservableCollection<UserModel> _users = new ObservableCollection<UserModel>();

        public ObservableCollection<UserModel> Users
        {
            get => _users;
            set => SetProperty(ref _users, value);
        }

        public virtual void LoadUsers()
        {
            try
            {
                var db = new AppDbContext();
                Users = new ObservableCollection<UserModel>(db.Users!);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

    }
}
