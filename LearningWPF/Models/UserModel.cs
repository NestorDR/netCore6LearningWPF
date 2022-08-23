using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CommonLibrary;

namespace LearningWPF.Models
{
    // Visit https://odetocode.com/blogs/scott/archive/2011/06/29/manual-validation-with-data-annotations.aspx

    [Table("User", Schema = "dbo")]
    public class UserModel : CommonBase
    {
        private int _id;
        [Required]
        [Key]
        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChanged(nameof(Id));
            }
        }

        private string _userName = string.Empty;
        [Column(TypeName = "nvarchar")]
        [Required]
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }

        private string _password = string.Empty;
        [Required]
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        private string _firstName = string.Empty;
        [Column(TypeName = "nvarchar")]
        [Required]
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        private string _lastName = string.Empty;
        [Column(TypeName = "nvarchar")]
        [Required]
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        private string _emailAddress = string.Empty;
        // Visit https://odetocode.com/blogs/scott/archive/2011/06/29/manual-validation-with-data-annotations.aspx
        [EmailAddress(ErrorMessage = "Invalid Email.")]
        [MaxLength(100)]
        [Required]
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                RaisePropertyChanged(nameof(EmailAddress));
            }
        }

        private bool _isLoggedIn = false;
        [NotMapped]
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                RaisePropertyChanged(nameof(IsLoggedIn));
            }
        }
    }
}