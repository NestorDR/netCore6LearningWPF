namespace CommonLibrary.ViewModels
{
  public class ViewModelAddEditDeleteBase : ViewModelBase
  {
    #region Public Properties

    private bool _isAddMode;
    public bool IsAddMode
    {
      get => _isAddMode;
      set => SetProperty(ref _isAddMode, value);
    }

    private bool _isReadOnly = true;
    public virtual bool IsReadOnly
    {
        get => _isReadOnly;
        set => SetProperty(ref _isReadOnly, value);
    }

    #endregion

    #region BeginEdit Method

    public virtual void BeginEdit(bool isAddMode = false)
    {
        IsAddMode = isAddMode;
    }

    #endregion

    #region CancelEdit Method

    public virtual void CancelEdit()
    {
        base.Clear();
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
