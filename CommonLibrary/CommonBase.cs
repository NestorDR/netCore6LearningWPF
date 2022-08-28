using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CommonLibrary
{
    /// <summary>
    /// This class implements the INotifyPropertyChanged Event Procedure
    /// </summary>
    public class CommonBase : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        /// <summary>
        /// The PropertyChanged Event to raise to any UI object
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notifies to the listeners that a property value has changed, using PropertyChanged Event to raise to any UI object.
        /// [CallerMemberName] allows to get the name of the calling property avoiding writing it explicitly.
        /// This method is often also named: RaisePropertyChange or OnPropertyChanged
        /// </summary>
        /// <param name="propertyName">The property name that is changing and is used for notifying listeners.</param>
        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null) =>
            // Raise the PropertyChanged event, if handler is connected
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        #region Clone Method

        public void Clone<T>(T original, T cloneTo)
        {
            if (original == null || cloneTo == null) return;
            // Use reflection so the NotifyPropertyChanged event is fired for each property
            foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = prop.GetValue(original, null);
                prop.SetValue(cloneTo, value, null);
            }
        }

        #endregion
    }
}
