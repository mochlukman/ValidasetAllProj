using System;
using IBatisNet.Common;
using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess.DaoSessionHandlers;
using IBatisNet.DataMapper;
using IBatisNet.Common.Exceptions;

namespace CoreNET.Common.Base
{
  public class LocalSqlMap
  {
    #region Members

    #region Property Instance
    //private static LocalSqlMap _Instance;
    //public static LocalSqlMap Instance
    //{
    //  get
    //  {
    //    if (_Instance == null)
    //    {
    //      _Instance = new LocalSqlMap();
    //    }
    //    return _Instance;
    //  }
    //}
    #endregion

    public static IDalSession GetContext(IDao dao)
    {
      IDaoManager Dao = DaoManager.GetInstance(dao);
      return (Dao.LocalDaoSession as DaoSession);
    }

    //public abstract ISqlMapper GetLocalSqlMap(IDao Dao, int mode);
    public static ISqlMapper GetLocalSqlMap(IDao Dao, int mode, BaseBO bo)
    {
      SqlMapDaoSession sqlMapDaoSession = (SqlMapDaoSession)GetContext(Dao);
      ISqlMapper mapper = sqlMapDaoSession.SqlMap;
      //mapper.DataSource = new SingleDataSource(mapper.DataSource);
      mapper.LocalSession.CloseConnection();//opsi ngga close connection, sqlmap dibedain
      mapper.DataSource = MyDaoManager.Instance.GetDataSource(mapper.DataSource, mode, bo);
      mapper.LocalSession.CreateConnection();//opsi ngga close connection
      return mapper;
    }
    #endregion
  }
}
