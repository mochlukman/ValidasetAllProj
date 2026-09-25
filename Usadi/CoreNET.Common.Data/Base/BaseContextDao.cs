using System;
using System.Collections;
using System.Configuration;
using System.Text;
using IBatisNet.Common;
using IBatisNet.DataAccess;
using IBatisNet.DataAccess.Interfaces;
using IBatisNet.DataAccess.DaoSessionHandlers;
using IBatisNet.DataMapper;
using IBatisNet.Common.Exceptions;

namespace CoreNET.Common.Base
{
  public class BaseContextDao : IDao, IBaseDao
  {
    #region Constant
    public const string DEFAULT = "";
    public const string PREFIX_INSERT = "Insert";
    public const string PREFIX_UPDATE = "UpdateBy";
    public const string PREFIX_DELETE = "DeleteBy";
    public const string PREFIX_LOAD = "LoadBy";
    public const string PREFIX_QUERY = "QueryBy";
    public const string PREFIX_EXEC = "Exec";
    public const string SYSTEM_ERR_CANT_INSERT_DUPLICATE = "Cannot insert duplicate key";
    public const string SYSTEM_ERR_CHILDREN_STILL_EXIST = "Children still exist";
    public const string SYSTEM_ERR_CONVERT_NUM_TO_MONEY = "Arithmetic overflow error converting numeric to data type money";

    #endregion
    #region Abstract Method
    //public abstract string GetClassName(BO);
    #endregion

    #region IBaseDao Members
    string _TableName = "";
    public string TableName
    {
      get { return _TableName; }
      set { _TableName = value; }
    }
    public string GetClassName(Object o)
    {
      return o.GetType().Name.Replace("Control", "");
    }
    public string GetTableName()
    {
      return TableName;
    }
    public void Insert(string label, BaseBO BO, int mode)
    {
      string cname = ((BaseDataControl)BO).XMLName;
      String SqlMapStatement = PREFIX_INSERT + label + cname;
      #region Ibatis Code
      try
      {
        ISqlMapper mapper = LocalSqlMap.GetLocalSqlMap(this, mode, BO);
        using (mapper.LocalSession.Connection)
        {
          Object o = mapper.Insert(SqlMapStatement, BO);
        }
      }
      catch (Exception ex)
      {
        if (MasterAppConstants.Instance.StatusTesting)
        {
          string msg = string.Format(@"Testing: Error executing query '{0}'. Cause: {1}; In: {2} ", SqlMapStatement, ex.Message, ex.StackTrace);
          ex = new Exception(msg, ex);
          throw ex;
        }
        else
        {
          //UtilityBO.Log(BO, ex, EventControl.EVENT_INSERT);
          if (ex.Message.Contains(SYSTEM_ERR_CANT_INSERT_DUPLICATE) || ex.Message.Contains("The statement has been terminated."))
          {
            ex = new Exception("Data yang ditambahkan sudah ada di tabel ini!");
          }
          if (ex.Message.Contains(SYSTEM_ERR_CONVERT_NUM_TO_MONEY))
          {
            ex = new Exception("Nilai tidak boleh melebihi batas tipe money SQL ( (+922.337.203.685.477,5807))!");
          }

          string[] msgs = ex.Message.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
          string msg = (msgs.Length > 0) ? msgs[msgs.Length - 1].Trim() : ex.Message;
          msgs = msg.Split(new string[] { "cause : " }, StringSplitOptions.RemoveEmptyEntries);
          throw new Exception((msgs.Length > 1) ? msgs[1] : msgs[0]);
        }
      }
      #endregion
    }

    public int UpdateBy(string label, BaseBO BO, int mode)
    {
      string cname = ((BaseDataControl)BO).XMLName;
      String SqlMapStatement = "";
      int lintRowEffected = 0;
      if (cname == "")
      {
        SqlMapStatement = PREFIX_UPDATE + label + GetClassName(BO);
      }
      else
      {
        SqlMapStatement = PREFIX_UPDATE + label + cname;
      }
      #region Ibatis Code
      try
      {
        ISqlMapper mapper = LocalSqlMap.GetLocalSqlMap(this, mode, BO);
        using (mapper.LocalSession.Connection)
        {
          lintRowEffected = mapper.Update(SqlMapStatement, BO);
        }
      }
      catch (Exception ex)
      {
        if (MasterAppConstants.Instance.StatusTesting)
        {
          string msg = string.Format(@"Testing: Error executing query '{0}'. Cause: {1}; In: {2} ", SqlMapStatement, ex.Message, ex.StackTrace);
          ex = new Exception(msg, ex);
          throw ex;
        }
        else
        {
          //UtilityBO.Log(BO, ex, EventControl.EVENT_UPDATE);
          if (ex.Message.Contains(SYSTEM_ERR_CONVERT_NUM_TO_MONEY))
          {
            ex = new Exception("Nilai tidak boleh melebihi batas tipe money SQL ( (+922.337.203.685.477,5807))!");
          }

          string[] msgs = ex.Message.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
          string msg = (msgs.Length > 0) ? msgs[msgs.Length - 1].Trim() : ex.Message;
          msgs = msg.Split(new string[] { "cause : " }, StringSplitOptions.RemoveEmptyEntries);
          throw new Exception((msgs.Length > 1) ? msgs[1] : msgs[0]);
        }
      }
      #endregion
      return lintRowEffected;
    }

    public int DeleteBy(string label, BaseBO BO, int mode)
    {
      string cname = ((BaseDataControl)BO).XMLName;
      String SqlMapStatement = PREFIX_DELETE + label + cname;
      int lintRowEffected = 0;
      #region Ibatis Code
      try
      {
        ISqlMapper mapper = LocalSqlMap.GetLocalSqlMap(this, mode, BO);
        using (mapper.LocalSession.Connection)
        {
          lintRowEffected = mapper.Delete(SqlMapStatement, BO);
        }
      }
      catch (Exception ex)
      {
        if (MasterAppConstants.Instance.StatusTesting)
        {
          string msg = string.Format(@"Testing: Error executing query '{0}'. Cause: {1}; In: {2} ", SqlMapStatement, ex.Message, ex.StackTrace);
          ex = new Exception(msg, ex);
          throw ex;
        }
        else
        {
          //UtilityBO.Log(BO, ex, EventControl.EVENT_DELETE);
          if (ex.Message.Contains(SYSTEM_ERR_CHILDREN_STILL_EXIST))
          {
            int id = ex.Message.IndexOf("Children still exist in ");
            string tab = ex.Message.Substring(id);
            id = tab.IndexOf("\"");
            tab = tab.Substring(id + 1);
            tab = tab.Substring(0, tab.IndexOf("\""));
            ex = new Exception("Data tidak bisa dihapus karena masih digunakan di [" + tab + "]!");
          }

          string[] msgs = ex.Message.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
          string msg = (msgs.Length > 0) ? msgs[msgs.Length - 1].Trim() : ex.Message;
          msgs = msg.Split(new string[] { "cause : " }, StringSplitOptions.RemoveEmptyEntries);
          throw new Exception((msgs.Length > 1) ? msgs[1] : msgs[0]);
        }
      }
      #endregion
      return lintRowEffected;
    }

    public BaseBO LoadBy(string label, BaseBO BO, int mode)
    {
      string cname = ((BaseDataControl)BO).XMLName;
      String SqlMapStatement = SqlMapStatement = PREFIX_LOAD + label + cname;
      #region Ibatis Code
      try
      {
        ISqlMapper mapper = LocalSqlMap.GetLocalSqlMap(this, mode, BO);
        using (mapper.LocalSession.Connection)
        {
          return (BaseBO)mapper.QueryForObject(SqlMapStatement, BO);
        }
      }
      catch (Exception ex)
      {
        if (MasterAppConstants.Instance.StatusTesting)
        {
          string msg = string.Format(@"Testing: Error executing query '{0}'. Cause: {1}; In: {2} ", SqlMapStatement, ex.Message, ex.StackTrace);
          ex = new Exception(msg, ex);
          throw ex;
        }
        else
        {
          //UtilityBO.Log(BO, ex, EventControl.EVENT_LOAD);
          if (ex.Message.Contains("DaoManager could not CloseConnection(). Cause :SqlMap could not invoke CloseConnection()"))
          {
            throw new Exception("Query gagal! Refresh halaman utama!");
          }

          string[] msgs = ex.Message.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
          string msg = (msgs.Length > 0) ? msgs[msgs.Length - 1].Trim() : ex.Message;
          msgs = msg.Split(new string[] { "cause : " }, StringSplitOptions.RemoveEmptyEntries);
          throw new Exception((msgs.Length > 1) ? msgs[1] : msgs[0]);
        }
      }
      #endregion
    }

    public IList QueryBy(string label, BaseBO BO, int mode)
    {
      string cname = ((BaseDataControl)BO).XMLName;
      String SqlMapStatement = PREFIX_QUERY + label + cname;
      ISqlMapper mapper = null;
      string cs = string.Empty;
      try
      {
        mapper = LocalSqlMap.GetLocalSqlMap(this, mode, BO);
        using (mapper.LocalSession.Connection)
        {
          cs = mapper.LocalSession.Connection.ConnectionString;
          return mapper.QueryForList(SqlMapStatement, BO);
        }
      }
      catch (Exception ex)
      {
        BO.Debug = "\n"+string.Format(@"
          Object Type = {0}
          Calling Method = {1}"
        , BO.GetType().FullName, UtilityBO.GetCurrentMethod(1));
        UtilityBO.Log((IDataControl)BO, ex);
        if (MasterAppConstants.Instance.StatusTesting)//@TODO
        {
          //ex.StackTrace
          string msg = string.Format(@"Testing: Error executing query '{0}'. Cause: {1}; cs: {2} ", SqlMapStatement, ex.Message, "*******");
          ex = new Exception(msg, ex);
          throw ex;
        }
        else
        {
          //UtilityBO.Log(BO, ex, EventControl.EVENT_VIEW);
          if (ex.Message.Contains(SYSTEM_ERR_CONVERT_NUM_TO_MONEY))
          {
            ex = new Exception("Error convert tipe numerik ke tipe money!");
          }

          string[] msgs = ex.Message.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
          string msg = (msgs.Length > 0) ? msgs[msgs.Length - 1].Trim() : ex.Message;
          msgs = msg.Split(new string[] { "cause : " }, StringSplitOptions.RemoveEmptyEntries);
          throw new Exception((msgs.Length > 1) ? msgs[1] : msgs[0]);
        }
      }
    }

    //public IList QueryByHashtable(string label, Hashtable param, int mode)
    //{
    //  String SqlMapStatement = PREFIX_QUERY + label;// +GetClassName(BO);
    //  try
    //  {
    //    ISqlMapper mapper = LocalSqlMap.GetLocalSqlMap(this, mode, BO);
    //    using (mapper.LocalSession.Connection)
    //    {
    //      return mapper.QueryForList(SqlMapStatement, param);
    //    }
    //  }
    //  catch (Exception ex)
    //  {
    //    if (MasterAppConstants.Instance.StatusTesting)
    //    {
    //      string msg = string.Format(@"Testing: Error executing query '{0}'. Cause: {1}; In: {2} ", SqlMapStatement, ex.Message, ex.StackTrace);
    //      ex = new Exception(msg, ex);
    //      throw ex;
    //    }
    //    else
    //    {
    //      ////UtilityBO.Log(BO, ex, EventControl.EVENT_VIEW);
    //      string[] msgs = ex.Message.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
    //      string msg = (msgs.Length > 0) ? msgs[msgs.Length - 1].Trim() : ex.Message;
    //      msgs = msg.Split(new string[] { "cause : " }, StringSplitOptions.RemoveEmptyEntries);
    //      throw new Exception((msgs.Length > 1) ? msgs[1] : msgs[0]);
    //    }
    //  }
    //}

    public IList ExecSP(string label, BaseBO BO, int mode)
    {
      String SqlMapStatement = PREFIX_EXEC + label;
      try
      {
        ISqlMapper mapper = LocalSqlMap.GetLocalSqlMap(this, mode, BO);
        using (mapper.LocalSession.Connection)
        {
          return mapper.QueryForList(SqlMapStatement, BO);
        }
      }
      catch (Exception ex)
      {
        if (MasterAppConstants.Instance.StatusTesting)
        {
          string msg = string.Format(@"Testing: Error executing query '{0}'. Cause: {1}; In: {2} ", SqlMapStatement, ex.Message, ex.StackTrace);
          ex = new Exception(msg, ex);
          throw ex;
        }
        else
        {
          //UtilityBO.Log(BO, ex, EventControl.EVENT_EXECSP);
          string[] msgs = ex.Message.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
          string msg = (msgs.Length > 0) ? msgs[msgs.Length - 1].Trim() : ex.Message;
          msgs = msg.Split(new string[] { "cause : " }, StringSplitOptions.RemoveEmptyEntries);
          throw new Exception((msgs.Length > 1) ? msgs[1] : msgs[0]);
        }
      }
    }

    #endregion

  }
}
