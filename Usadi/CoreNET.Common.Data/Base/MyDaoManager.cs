using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Configuration;
using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Configuration;
using System.Security.Cryptography;

namespace CoreNET.Common.Base
{
  public class MyDaoManager
  {
    #region Property DataSource
    private IbatisSQLDataSource _DataSource = new IbatisSQLDataSource();
    private IbatisSQLDataSource DataSource
    {
      get { return _DataSource; }
      set { _DataSource = value; }
    }
    #endregion
    #region Property Instance
    private static MyDaoManager _Instance;
    /*Klo disetup sebagai aplikasi harusnya tidak masalah, klo dalam satu aplikasi, user bisa multi akses ke DB, gunakan session*/
    public static MyDaoManager Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new MyDaoManager();
        }
        return _Instance;
      }
    }
    #endregion

    static MyDaoManager()
    {
    }
    private const string DAO_CONTEXT_ID = "SqlMapDao";
    public IBaseDao GetDao(Type type, int mode, BaseBO bo)
    {
      IDaoManager cDaoManager = null;// DaoManager.GetInstance(DAO_CONTEXT_ID);
      try
      {
        switch (mode)
        {
          case SQLDataSource.MODE_DB_CONFIG:
            cDaoManager = DaoManager.GetInstance("SqlMapDaoSys");
            cDaoManager.LocalDataSource.ConnectionString = DataSource.CS_Config;
            break;
          case SQLDataSource.MODE_DB_LOG:
            cDaoManager = DaoManager.GetInstance("SqlMapDaoLog");
            cDaoManager.LocalDataSource.ConnectionString = DataSource.CS_Log;
            break;
          case SQLDataSource.MODE_DB_DM:
            cDaoManager = DaoManager.GetInstance("SqlMapDaoDM");
            cDaoManager.LocalDataSource.ConnectionString = DataSource.CS_DM;
            break;
          case SQLDataSource.MODE_DB_OPERATIONAL:
            cDaoManager = DaoManager.GetInstance("SqlMapDaoOp");
            if (string.IsNullOrEmpty(bo.ConnectionString))
            {
              throw new Exception("Empty operational connection string!");
            }
            cDaoManager.LocalDataSource.ConnectionString = bo.ConnectionString;// DataSource.CS_Operational;
            break;
          case SQLDataSource.MODE_DB_OPERATIONAL2:
            cDaoManager = DaoManager.GetInstance("SqlMapDaoOp2");
            cDaoManager.LocalDataSource.ConnectionString = DataSource.CS_Operational2;
            break;
          default://case MODE_DB_OPERATIONAL:
            cDaoManager = DaoManager.GetInstance("SqlMapDaoOp");
            cDaoManager.LocalDataSource.ConnectionString = DataSource.CS_Operational;
            break;
        }
        return (IBaseDao)cDaoManager.GetDao(type);
      }
      catch (Exception ex) {
        UtilityBO.Log(ex);}
      return null;
    }

    private Dictionary<string, SQLDataSource> datasources = new Dictionary<string, SQLDataSource>();
    public IBatisNet.Common.IDataSource GetDataSource(IBatisNet.Common.IDataSource innerDataSource, int mode, BaseBO bo)
    {
      string cs;
      switch (mode)
      {
        case SQLDataSource.MODE_DB_CONFIG:
          cs = DataSource.CS_Config;
          break;
        case SQLDataSource.MODE_DB_LOG:
          cs = DataSource.CS_Log;
          break;
        case SQLDataSource.MODE_DB_DM:
          cs = DataSource.CS_DM;
          break;
        case SQLDataSource.MODE_DB_OPERATIONAL:
          cs = bo.ConnectionString; //DataSource.CS_Operational;
          break;
        case SQLDataSource.MODE_DB_OPERATIONAL2:
          cs = DataSource.CS_Operational2;
          break;
        default://case MODE_DB_OPERATIONAL:
          cs = DataSource.CS_Operational;
          break;
      }
      DataSource.ConnectionString = cs;
      DataSource.DbProvider = innerDataSource.DbProvider;
      DataSource.DbProvider.DbCommandTimeout = 0;//Special for SQL Server, 0 means No Timeout
      return DataSource;
    }
    class IbatisSQLDataSource : IBatisNet.Common.DataSource
    {
      #region Property CS_Config
      public string CS_Config
      {
        get { return SQLDataSource.Instance.CS_Config; }
      }
      #endregion
      #region Property CS_Operational
      public string CS_Operational
      {
        get { return SQLDataSource.Instance.CS_Operational; }
      }
      #endregion
      #region Property CS_Operational2
      public string CS_Operational2
      {
        get { return SQLDataSource.Instance.CS_Operational2; }
      }
      #endregion
      #region Property CS_Log
      public string CS_Log
      {
        get { return SQLDataSource.Instance.CS_Log; }
      }
      #endregion
      #region Property CS_DM
      public string CS_DM
      {
        get { return SQLDataSource.Instance.CS_DM; }
      }
      #endregion
    }
  }
}
