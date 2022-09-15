namespace CommonLibrary.ViewModels
{
  public class ViewModelAddEditDeleteBase : ViewModelBase
  {
    #region Public Properties

    private bool _isListEnabled = true;
    public bool IsListEnabled
    {
      get => _isListEnabled;
      set => SetProperty(ref _isListEnabled, value);
    }

    private bool _isDetailEnabled;
    public bool IsDetailEnabled
    {
      get => _isDetailEnabled;
      set => SetProperty(ref _isDetailEnabled, value);
    }

    private bool _isAddMode;
    public bool IsAddMode
    {
      get => _isAddMode;
      set => SetProperty(ref _isAddMode, value);
    }

    #endregion

    #region BeginEdit Method
    public virtual void BeginEdit(bool isAddMode = false)
    {
      IsListEnabled = false;
      IsDetailEnabled = true;
      IsAddMode = isAddMode;
    }
    #endregion

    #region CancelEdit Method
    public virtual void CancelEdit()
    {
      base.Clear();

      IsListEnabled = true;
      IsDetailEnabled = false;
      IsAddMode = false;
    }
    #endregion

    #region Save Method
    public virtual bool Save()
    {
      return true;
    }
    #endregion

    #region Delete Method
    public virtual bool Delete()
    {
      return true;
    }
    #endregion
  }
}
