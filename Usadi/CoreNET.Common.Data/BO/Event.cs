using CoreNET.Common.Base;
using System;

namespace CoreNET.Common.BO
{
  #region Event
  [Serializable]
  public class EventControl : BaseDataControl, IDataControl
  {
    #region Properties
    public string App_name { get; set; }
    public string Computer_ip { get; set; }
    public string Computer_name { get; set; }
    public string Idlog { get; set; }
    public string Logdata { get; set; }
    public string Kdtype { get; set; }
    public DateTime Tgllog { get; set; }
    public string User_db { get; set; }
    public string User_app { get; set; }
    public string User_menu { get; set; }
    #endregion Properties

    #region Methods
    public EventControl()
    {
      XMLName = "Event";
      ModeDB = SQLDataSource.MODE_DB_LOG;
      Computer_name = ClientCompName;
      Computer_ip = ClientCompIP;

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
    #endregion
    #region Static
    public const int EVENT_INSERT = 11;
    public const int EVENT_UPDATE = 12;
    public const int EVENT_DELETE = 13;
    public const int EVENT_VIEW = 14;
    public const int EVENT_LOAD = 15;
    public const int EVENT_EXECSP = 16;
    public static void Log(IDataControl dc, int jevent)
    {
      try
      {
        if (typeof(IDataControlAppuser).IsInstanceOfType(dc))
        {
          //Event Login
        }


        //IDataControlAppuser user = (IDataControlAppuser)GlobalApp.GetSessionUser();
        //if (user != null)
        //{
        //  ((BaseBO)dc).Last_by = user.GetUserID();
        //}
        ((BaseBO)dc).Last_date = DateTime.Now;
      }
      catch (Exception ex)
      {
        UtilityBO.Log(dc, ex);
      }
    }
    public static void Log(IDataControl dc, Exception ex, int jevent)
    {
    }
    public static void Log(Exception ex)
    {
    }
    #endregion
  }
  #endregion Event
}

