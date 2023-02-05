using CommonLibrary;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LearningWPF.Models
{
    internal class UserRoleModel : CommonBase, IEntityInterface
    {
        private int _id;
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name = string.Empty;
        [Column(TypeName = "nvarchar")]
        [MaxLength(30)]
        [Required]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string? CreatedBy { get; set; } = string.Empty;

        public DateTime? CreatedOn { get; set; } = DateTime.Now;

        public string? UpdatedBy { get; set; } = string.Empty;

        public DateTime? UpdatedOn { get; set; } = null;
    }
}
