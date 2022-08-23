using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;

namespace LearningWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Downloads for .NET, including ASP.NET Core: https://dotnet.microsoft.com/en-us/download
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration? Configuration { get; private set; }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        /// <summary>
        /// WPF lets you handle all unhandled exceptions globally, through the DispatcherUnhandledException event on the Application class.
        /// Visit: https://wpf-tutorial.com/wpf-application/handling-exceptions/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, 
                "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }

    }
}
