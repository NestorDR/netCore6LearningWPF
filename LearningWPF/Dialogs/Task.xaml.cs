using System;
using System.Windows;
using LearningWPF.Models;

namespace LearningWPF.Dialogs
{
    /// <summary>
    /// Interaction logic for Task.xaml
    /// </summary>
    public partial class Task : Window
    {

        public Task(TaskModel taskModel)
        {
            InitializeComponent();

            DataContext = taskModel;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e) => Close();

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            DataContext = null;
        }
    }
}
