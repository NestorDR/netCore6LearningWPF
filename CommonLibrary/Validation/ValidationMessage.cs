namespace CommonLibrary.Validation
{
  public class ValidationMessage : CommonBase
  {
    #region Public Properties

    private string _propertyName = string.Empty;
    public string PropertyName
    {
      get => _propertyName;
      set {
        _propertyName = value;
        NotifyPropertyChanged();
      }
    }

    private string _message = string.Empty;
    public string Message
    {
      get => _message;
      set {
        _message = value;
        NotifyPropertyChanged();
      }
    }

    #endregion
  }
}
