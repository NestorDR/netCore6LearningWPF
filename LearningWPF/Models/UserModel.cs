using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// App modules
using CommonLibrary;
using LearningWPF.Helper;
using static LearningWPF.Helper.EnumHelper;

namespace LearningWPF.Models
{
    // Visit https://odetocode.com/blogs/scott/archive/2011/06/29/manual-validation-with-data-annotations.aspx
    [Table("Users", Schema = "dbo")]
    internal class UserModel : CommonBase, IEntityInterface
    {
        // Generic password to get extremely simple example code
        // NOTE: It will be necessary to hash and secure your password.
        public const string EXAMPLE_PASSWORD = "1234";

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

        // Fully defined One-to-Many relationship at both ends
        // Visit: topic "Convention 4" in
        //        https://www.entityframeworktutorial.net/efcore/one-to-many-conventions-entity-framework-core.aspx
        //        https://www.entityframeworktutorial.net/code-first/configure-one-to-many-relationship-in-code-first.aspx#conventions-for-one-to-many-ef6
        private int _userRoleId = (int) EnumHelper.UserRole.User;
        /// <summary>
        /// Fully defined One-to-Many relationship at both ends (dependent entity is Relationships, and principal entity is UserRoleModel)
        /// </summary>
        public int UserRoleId
        {
            get => _userRoleId;
            set => SetProperty(ref _userRoleId, value);
        }

        // Reference Navigation property: is a property of another entity type
        /// <summary>
        /// Navigation property of type UserRole Reference. UserRole is the principal entity.
        /// </summary>
        public UserRoleModel? UserRole { get; set; }    // One-to-Many relationship

        [NotMapped]
        public string RoleName => UserRole != null ? UserRole.Name : $"{Enum.GetName(typeof(UserRole), UserRoleId)}";

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

        private DragRowEffect _dragRowEffect;
        [NotMapped]
        public DragRowEffect DragRowEffect
        {
            get => _dragRowEffect;
            set => SetProperty(ref _dragRowEffect, value);
        }

    }
}