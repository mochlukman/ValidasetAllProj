using CoreNET.Common.Base;
using System;
using System.IO;

namespace CoreNET.Common.BO
{
  #region Event
  public class LogUtils : LogUtilsBase
  {
    #region Static
    public static DateTime Start = DateTime.Now;
    public static string Path
    {
      get
      {
        string datadir = GlobalAsp.GetDataDir();
        if (!string.IsNullOrEmpty(GlobalAsp.GetSessionApp()))
        {
          datadir += "Log/" + GlobalAsp.GetSessionApp() + "/";
        }
        else
        {
          datadir += "Log/";
        }
        if (!Directory.Exists(datadir))
        {
          System.IO.Directory.CreateDirectory(datadir);
        }
        return datadir;
      }
    }
    public static string LogFileName => Path + "{0}_" + Start.ToString("ddMMyyyy_HHmmss") + ".txt";
    public static new void Log(BaseBO bo, string kdtype)
    {
      //Ibatis ngga bisa pake Thread
      //Thread T = new Thread(LogUtils.RunLog);
      //T.Start(new LogObject(bo, kdtype));

      //Klo mau ada kondisi, jangan di sini
      //if (MasterAppConstants.Instance.StatusTesting)
      //{
      //  RunLog(new LogObject(bo, kdtype));
      //}
      RunLog(new LogObject(bo, kdtype));
    }

    public class LogObject
    {
      public EventControl BO { get; set; }
      public string Kdtype { get; set; }
      public LogObject(BaseBO bo, string kdtype)
      {
        BO = new EventControl
        {
          Idlog = Guid.NewGuid().ToString(),
          Userid = bo.Userid,
          Tgllog = DateTime.Now,
          Computer_name = UtilityUI.GetClientCompName(),
          Computer_ip = UtilityUI.GetClientCompIP(),
          App_name = bo.Kdapp + bo.Nmapp,
          User_app = bo.Idapp,
          User_db = bo.ConnectionString,
          User_menu = bo.Url,
          Kdtype = kdtype,
          Status = 0,
          Logdata = bo.Debug
        };

        Kdtype = kdtype;
      }
    }
    public static void RunLog(object data)
    {
      LogObject obj = (LogObject)data;
      EventControl ctrl = obj.BO;
      try
      {
        string kdtype = obj.Kdtype;
        ctrl.Insert();
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ctrl, ex);
      }
    }
    #endregion
  }
  #endregion Event
}

