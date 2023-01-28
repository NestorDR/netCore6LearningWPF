using System;

namespace LearningWPF.Models
{
    internal interface IEntityInterface
    {
        // Property signatures:
        public int Id { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
