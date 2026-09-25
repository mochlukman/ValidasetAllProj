using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Event
  [Serializable]
  public class EventUIControl : EventControl, IDataControl
  {

    #region Methods
    public EventUIControl()
    {
      XMLName = "Event";
      ModeDB = SQLDataSource.MODE_DB_LOG;
    }
    #region GetDaoSingleton()
    public new IBaseDao GetDaoSingleton()
    {
      IBaseDao _Dao = null;
      if (_Dao == null)
      {
        _Dao = (IBaseDao)MyDaoManager.Instance.GetDao(typeof(IBaseDao), ModeDB, this);
      }
      return _Dao;
    }
    #endregion
    #endregion Methods
  }
  #endregion Event
}

