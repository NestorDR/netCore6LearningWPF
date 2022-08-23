using System.Windows;

namespace LearningWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Downloads for .NET, including ASP.NET Core: https://dotnet.microsoft.com/en-us/download
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, 
                "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
