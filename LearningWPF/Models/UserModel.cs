﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// App modules
using CommonLibrary;
using LearningWPF.Helper;

namespace LearningWPF.Models
{
    // Visit https://odetocode.com/blogs/scott/archive/2011/06/29/manual-validation-with-data-annotations.aspx
    [Table("Users", Schema = "dbo")]
    public class UserModel : CommonBase, IEntityInterface
    {
        private int _id;
        [Required]
        [Key]
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _userName = string.Empty;
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password = string.Empty;
        [MaxLength(20)]
        [Required]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _firstName = string.Empty;
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName = string.Empty;
        [Column(TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        private bool _active = true;
        [Required]
        public bool Active
        {
            get => _active;
            set => SetProperty(ref _active, value);
        }

        private string _emailAddress = string.Empty;
        // Visit https://odetocode.com/blogs/scott/archive/2011/06/29/manual-validation-with-data-annotations.aspx
        [EmailAddress(ErrorMessage = "Invalid Email.")]
        [MaxLength(100)]
        [Required]
        public string EmailAddress
        {
            get => _emailAddress;
            set => SetProperty(ref _emailAddress, value);
        }

        public string? CreatedBy { get; set; } = string.Empty;

        public DateTime? CreatedOn { get; set; } = DateTime.Now;

        public string? UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedOn { get; set; } = null;

        private bool _isLoggedIn = false;
        [NotMapped]
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }

        private bool _isReadOnly = true;
        [NotMapped]
        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => SetProperty(ref _isReadOnly, value);
        }

        private EnumHelper.DragRowEffect _dragRowEffect;

        [NotMapped]
        internal EnumHelper.DragRowEffect DragRowEffect
        {
            get => _dragRowEffect;
            set => SetProperty(ref _dragRowEffect, value);
        }

    }
}