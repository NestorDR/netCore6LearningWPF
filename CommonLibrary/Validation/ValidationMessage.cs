namespace CommonLibrary.Validation
{
  public class ValidationMessage : CommonBase
  {
    #region Public Properties

    private string _propertyName = string.Empty;
    public string PropertyName
    {
      get => _propertyName;
      set => SetProperty(ref _propertyName, value);
    }

    private string _message = string.Empty;
    public string Message
    {
      get => _message;
      set => SetProperty(ref _message, value);
    }

    #endregion
  }
}
