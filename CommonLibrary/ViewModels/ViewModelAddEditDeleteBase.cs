namespace CommonLibrary.ViewModels
{
  public class ViewModelAddEditDeleteBase : ViewModelBase
  {
    #region Private Variables
    private bool _isListEnabled = true;
    private bool _isDetailEnabled;
    private bool _isAddMode;
    #endregion

    #region Public Properties
    public bool IsListEnabled
    {
      get => _isListEnabled;
      set {
        _isListEnabled = value;
        RaisePropertyChanged(nameof(IsListEnabled));
      }
    }

    public bool IsDetailEnabled
    {
      get => _isDetailEnabled;
      set {
        _isDetailEnabled = value;
        RaisePropertyChanged(nameof(IsDetailEnabled));
      }
    }

    public bool IsAddMode
    {
      get => _isAddMode;
      set {
        _isAddMode = value;
        RaisePropertyChanged(nameof(IsAddMode));
      }
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
