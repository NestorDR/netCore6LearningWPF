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
        /// Sets property if it does not equal existing value. Notifies listeners if change occurs.
        /// </summary>
        /// <typeparam name="T">Type of property.</typeparam>
        /// <param name="member">The property's backing field.</param>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">Name of the property used to notify listeners.  This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(member, value))
            {
                return false;
            }

            member = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }

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
