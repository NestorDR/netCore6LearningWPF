namespace CommonLibrary.Exceptions
{
    public class ExceptionManager : CommonBase
    {
        #region Instance Property

        private static ExceptionManager? _instance;

        public static ExceptionManager Instance
        {
            get => _instance ??= new ExceptionManager();
            set => _instance = value;
        }

        #endregion

        #region Publish Methods

        public virtual void Publish(Exception ex)
        {
            LogToDebug(ex);
        }

        public static void LogToDebug(Exception ex)
        {
            string format = $"\n{{0}}\tError: {{1}}\n{{2}}\n{new string('-', 80)}\n";
            System.Diagnostics.Debug.WriteLine(format, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), ex.GetType().FullName,
                ex.ToString());
        }

        #endregion Publish Methods
    }
}
