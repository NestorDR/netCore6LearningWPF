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
      // TODO: Implement an exception publisher here
      System.Diagnostics.Debug.WriteLine(ex.ToString());
    }
    #endregion
  }
}
