using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LearningWPF.Models
{
    /*
        Implementing the INotifyPropertyChanged interface, allows TaskModel objects to be able 
          to alert the UI layer of changes in their properties.
        Visit https://wpf-tutorial.com/data-binding/responding-to-changes/
    */
    public class TaskModel : INotifyPropertyChanged
    {
        private string _title = "";

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                NotifyPropertyChanged();
            }
        }

        private int _completion;
        public int Completion
        {
            get => _completion;
            set
            {
                if (_completion == value) return;
                _completion = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        //public void NotifyPropertyChanged(string propertyName)
        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
