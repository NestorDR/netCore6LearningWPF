using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LearningWPF.Models
{
    /*
        Implementing the INotifyPropertyChanged interface, allows TaskModel objects to be able 
          to alert the UI layer of changes in their properties.
        Visit https://wpf-tutorial.com/data-binding/responding-to-changes/
    */
    internal class TaskModel : INotifyPropertyChanged
    {
        [Key]
        public int Id { get; set; }

        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
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

        private bool _checked = false;
        public bool Checked
        {
            get => _checked;
            set
            {
                if (_checked == value) return; 
                _checked = value;
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
