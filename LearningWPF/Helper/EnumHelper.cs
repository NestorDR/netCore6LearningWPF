namespace LearningWPF.Helper
{
    internal class EnumHelper
    {
        /// <summary>
        /// A list of dragging effects for data grid rows, which can be applied above or below the dragged row.
        /// </summary>
        public enum DragRowEffect
        {
            None,
            Before,
            After
        }

        public enum UserRole
        {
            Admin,
            User
        }
    }
}
