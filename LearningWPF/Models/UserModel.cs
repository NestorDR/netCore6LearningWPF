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
            set => SetProperty(ref _id, value);
        }

        private string _userName = string.Empty;
        [Column(TypeName = "nvarchar")]
        [Required]
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }

        private string _password = string.Empty;
        [Required]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _firstName = string.Empty;
        [Column(TypeName = "nvarchar")]
        [Required]
        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        private string _lastName = string.Empty;
        [Column(TypeName = "nvarchar")]
        [Required]
        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
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

        private bool _isLoggedIn = false;
        [NotMapped]
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }
    }
}