using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Notes
  [Serializable]
  public class NotesControl : BaseDataControl, IDataControl
  {
    #region Constant
    public const int RELEASE_NOTES = 0;
    public const int DESCRIPTION = 1;
    public const int FORUM = 2;
    public const int HTMLREPORT = 11;
    #endregion
    #region Properties
    public string Idnotes { get; set; }
    public string Notes { get; set; }
    public int Notestype { get; set; }
    public string Refdb { get; set; }
    public string Refid { get; set; }
    public string Reftabel { get; set; }
    public DateTime Tglnotes { get; set; }
    #endregion Properties
    #region Methods
    public NotesControl()
    {
      ModeDB = SQLDataSource.MODE_DB_LOG;
      ConnectionString = SQLDataSource.Instance.CS_Log;
      Idnotes = Guid.NewGuid().ToString();
      XMLName = "Notes";
      Tglnotes = DateTime.Now;
    }
    public new IBaseDao GetDaoSingleton()
    {
      IBaseDao _Dao = null;
      if (_Dao == null)
      {
        _Dao = (IBaseDao)MyDaoManager.Instance.GetDao(typeof(IBaseDao), ModeDB, this);
      }
      return _Dao;
    }

    public new void Insert()//SMART_APP_LOG
    {
      ((BaseDataControl)this).Insert(BaseDataControl.DEFAULT);
      //try
      //{
      //  ModeDB = MyDaoManager.MODE_DB_LOG;
      //  ((BaseDataControl)this).Insert(BaseDataControl.DEFAULT);
      //}
      //catch (Exception ex)
      //{
      //  throw ex;
      //}
      //finally
      //{
      //  ModeDB = MyDaoManager.MODE_DB_OPERATIONAL;
      //}
    }
    public new IList View()
    {
      IList list = null;
      list = ((BaseDataControl)this).View(BaseDataControl.FILTER);
      //try
      //{
      //  ModeDB = MyDaoManager.MODE_DB_LOG;
      //  list = ((BaseDataControl)this).View(BaseDataControl.FILTER);
      //}
      //catch (Exception ex)
      //{
      //  throw ex;
      //}
      //finally
      //{
      //  ModeDB = MyDaoManager.MODE_DB_OPERATIONAL;
      //}
      return list;
    }
    public new BaseBO Load()
    {
      return ((BaseDataControl)this).Load(BaseDataControl.FILTER);
      //try
      //{
      //  ModeDB = MyDaoManager.MODE_DB_LOG;
      //  return ((BaseDataControl)this).Load(BaseDataControl.FILTER);
      //}
      //catch (Exception ex) 
      //{ 
      //  throw ex; 
      //}
      //finally
      //{
      //  ModeDB = MyDaoManager.MODE_DB_OPERATIONAL;
      //}
    }
    #endregion Methods
    #region static

    #endregion
  }
  #endregion Notes
}

