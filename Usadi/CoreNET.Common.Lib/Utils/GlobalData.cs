using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoreNET.Common.Base
{
  public class GlobalData
  {

    #region Property DataSource
    private static string _DataSource;
    public static string DataSource
    {
      get { return _DataSource; }
      set { _DataSource = value; }
    }
    #endregion
    #region Property DataSourceSys
    private static string _DataSourceSys;
    public static string DataSourceSys
    {
      get { return _DataSourceSys; }
      set { _DataSourceSys = value; }
    }
    #endregion
    #region Property DataSourceDM
    private static string _DataSourceDM;
    public static string DataSourceDM
    {
      get { return _DataSourceDM; }
      set { _DataSourceDM = value; }
    }
    #endregion
    #region Property DataSourceOP
    private static string _DataSourceOP;
    public static string DataSourceOP
    {
      get { return _DataSourceOP; }
      set { _DataSourceOP = value; }
    }
    #endregion
    #region Property DataSourceOP2
    private static string _DataSourceOP2;
    public static string DataSourceOP2
    {
      get { return _DataSourceOP2; }
      set { _DataSourceOP2 = value; }
    }
    #endregion
    #region Property DataSourceLog
    private static string _DataSourceLog;
    public static string DataSourceLog
    {
      get { return _DataSourceLog; }
      set { _DataSourceLog = value; }
    }
    #endregion


  }
}
