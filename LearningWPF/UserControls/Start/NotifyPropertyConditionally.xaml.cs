using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace LearningWPF.UserControls.Start
{
    /// <summary>
    /// Interaction logic for NotifyPropertyConditionally.xaml
    /// </summary>
    public partial class NotifyPropertyConditionally : UserControl
    {

        private readonly ObservableCollection<char> _alphabet = new(Enumerable.Range('A', 26).Select(x => (char)x));

        private readonly LetterContext _letterContext;

        public NotifyPropertyConditionally()
        {
            InitializeComponent();

            // Initialize data context
            _letterContext = new LetterContext(_alphabet[0]);
            DataContext = _letterContext;

            // Initialize view
            FirstAlphabetComboBox.ItemsSource = _alphabet;
            FirstAlphabetComboBox.SelectedIndex = 0;

            SecondAlphabetComboBox.ItemsSource = _alphabet;
            SecondAlphabetComboBox.SelectedIndex = 0;

        }

        private void FirstAlphabetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _letterContext.FirstLetter.Value = (char)((ComboBox)sender).SelectedItem;
            _letterContext.FirstLetterCharValue = _letterContext.FirstLetter.Value;
            MessageBox.Show($"1º letter:\nObj is {_letterContext.FirstLetter.Value}\nChar is {_letterContext.FirstLetterCharValue}", "1º Letter");

            if (_letterContext.FirstLetterCharValue != 'A' && _letterContext.FirstLetterCharValue != 'M') return;
            _letterContext.SecondLetter = new Letter { Value = _letterContext.FirstLetterCharValue };
            _letterContext.SecondLetterCharValue = _letterContext.FirstLetterCharValue;
            SecondAlphabetComboBox.SelectedIndex = FirstAlphabetComboBox.SelectedIndex;
        }

        private void SecondAlphabetComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _letterContext.SecondLetter.Value = (char)((ComboBox)sender).SelectedItem;
            _letterContext.SecondLetterCharValue = _letterContext.SecondLetter.Value;
            MessageBox.Show($"2º letter:\nObj is {_letterContext.SecondLetter.Value}\nChar is {_letterContext.SecondLetterCharValue}", "2º Letter");
        }
    }

    /*
        Implementing the INotifyPropertyChanged interface, allows LetterContext objects to be able 
          to alert the UI layer of changes in their properties.
        Visit https://wpf-tutorial.com/data-binding/responding-to-changes/
    */
    internal class LetterContext : INotifyPropertyChanged
    {
        // Threshold for 
        public static char ThresholdLetter => 'F';

        #region Properties

        // First letter like an object
        private Letter _firstLetter;
        public Letter FirstLetter           // The property must be public to be accessed from XAML
        {
            get => _firstLetter;
            set
            {
                if (_firstLetter == value) return;
                _firstLetter = value;
                NotifyPropertyChanged();    // Alternatively with property name NotifyPropertyChanged(nameof(FirstLetter));
            }
        }

        // First letter like an simple char variable
        private char _firstLetterCharValue;
        public char FirstLetterCharValue    // The property must be public to be accessed from XAML
        {
            get => _firstLetterCharValue;
            set
            {
                if (_firstLetterCharValue == value) return;
                _firstLetterCharValue = value;
                NotifyPropertyChanged();    // Alternatively with property name NotifyPropertyChanged(nameof(FirstLetterCharValue));
            }
        }

        // Second letter like an object
        private Letter _secondLetter;
        public Letter SecondLetter          // The property must be public to be accessed from XAML
        {
            get => _secondLetter;
            set
            {
                if (_secondLetter == value) return;
                _secondLetter = value;
                if (_secondLetter.Value <= ThresholdLetter)
                    // Changes below the Threshold Letter, are not notified to the View
                    NotifyPropertyChanged();      // Alternatively with property name NotifyPropertyChanged(nameof(SecondLetter));
            }
        }

        // Second letter like an simple char variable
        private char _secondLetterCharValue;
        public char SecondLetterCharValue   // The property must be public to be accessed from XAML
        {
            get => _secondLetterCharValue;
            set
            {
                if (_secondLetterCharValue == value) return;
                _secondLetterCharValue = value;
                if (_secondLetterCharValue <= ThresholdLetter)
                    // Changes below the Threshold Letter, are not notified to the View
                    NotifyPropertyChanged();   // Alternatively with property name NotifyPropertyChanged(nameof(SecondLetterCharValue));
            }
        }

        #endregion

        #region Constructor

        internal LetterContext(char letter)
        {
            _firstLetter = new Letter { Value = letter };
            FirstLetterCharValue = FirstLetter.Value;

            _secondLetter = new Letter { Value = letter };
            SecondLetterCharValue = SecondLetter.Value;
        }

        #endregion

        #region Notify property changes
        
        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 

        #endregion
    }

    internal class Letter : INotifyPropertyChanged
    {
        #region Property
        
        private char _value;
        public char Value               // The property must be public to be accessed from XAML
        {
            get => _value;
            set
            {
                if (_value == value) return;
                _value = value;
                NotifyPropertyChanged();
            }
        } 

        #endregion

        #region Notify property changes

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        } 

        #endregion
    }
}
