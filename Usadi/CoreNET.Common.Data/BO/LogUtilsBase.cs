using CoreNET.Common.Base;
using System;
using System.IO;

namespace CoreNET.Common.BO
{
  #region Event
  public class LogUtilsBase
  {
    #region Static
    public const string STR_EVENT_LOGIN = "01.";
    public const int EVENT_INSERT = 11;
    public const int EVENT_UPDATE = 12;
    public const int EVENT_DELETE = 13;
    public const int EVENT_VIEW = 14;
    public const int EVENT_LOAD = 15;
    public const int EVENT_EXECSP = 16;
    public static void Log(BaseBO bo, int jevent)
    {
      try
      {
        //Hanya untuk ngetes. Yang dilog, CRUD aja
        //UtilityBO.Log((IDataControl)bo, new Exception("Success..."));
        //@todo
        //Harusnya Thread
        IDataControl dc = (IDataControl)bo;


        ((BaseBO)dc).Last_by = bo.Userid;
        ((BaseBO)dc).Last_date = DateTime.Now;
      }
      catch (Exception ex)
      {
        UtilityBO.Log((IDataControl)bo, ex);
      }
    }
    public static void Log(BaseBO bo, string kdtype)
    {
      try
      {
        //@todo
        //Harusnya Thread
        IDataControl dc = (IDataControl)bo;

        EventControl ctrl = new EventControl
        {
          Idlog = Guid.NewGuid().ToString(),
          Userid = bo.Userid,
          Tgllog = DateTime.Now,
          App_name = bo.Idapp,
          Kdtype = kdtype,
          Logdata = bo.Debug
        };
        ctrl.Insert();
      }
      catch (Exception ex)
      {
        UtilityBO.Log((IDataControl)bo, ex);
      }
    }
    #endregion
  }
  #endregion Event
}

