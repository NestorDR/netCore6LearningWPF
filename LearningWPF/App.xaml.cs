using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.Configuration;
using LearningWPF.Common;

namespace LearningWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// Downloads for .NET, including ASP.NET Core: https://dotnet.microsoft.com/en-us/download
    /// </summary>
    public partial class App : Application
    {
        public IConfiguration? Configuration { get; private set; }

        public App()
        {
            // Load Application Settings
            AppSettings.Instance.LoadSettings();

            /* Previously, using System.Windows.Application.Properties
            var builder = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Application.Properties Property, visit: https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.properties?view=windowsdesktop-6.0
            this.Properties["DefaultConnectionSting"] = Configuration.GetConnectionString("DefaultConnection");
            */
        }

        /// <summary>
        /// WPF lets you handle all unhandled exceptions globally, through the DispatcherUnhandledException event on the Application class.
        /// Visit: https://wpf-tutorial.com/wpf-application/handling-exceptions/
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Extract the exception that was raised while executing code
            Exception? ex = e.Exception;

            // Identify the line and method where the exception originated
            string sourceLine = "";
            if (ex.StackTrace is { Length: > 0 })
            {
                string[] stackTrace = ex.StackTrace.Split('\n');
                foreach (string stackItem in stackTrace)
                {
                    sourceLine = stackItem.Trim();
                    if (sourceLine.StartsWith("at") &&  sourceLine.Contains(":line ")) break;
                }
            }
            if (!sourceLine.EndsWith(".")) sourceLine += ".";

            // Restore normal mouse cursor (preemptively)
            Mouse.OverrideCursor = null;
            string message = $"{ex.Message + (ex.Message.EndsWith(".") ? "" : ".")}\n\n{sourceLine}";
            // Show exception message
            MessageBox.Show("An unhandled exception just occurred: " + message, 
                "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            // All is done. Prevent the base classes from doing any further handling of the event.
            e.Handled = true;
        }
    }
}
