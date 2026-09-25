using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region SysObjects
  [Serializable]
  public class SysObjects : BaseDataControl, IDataControl
  {
    #region Property TableName
    private string _TableName;
    public string TableName
    {
      get => _TableName;
      set => _TableName = value;
    }
    #endregion
    #region Property ColumnName
    private string _ColumnName;
    public string ColumnName
    {
      get => _ColumnName;
      set => _ColumnName = value;
    }
    #endregion
    #region Property TypeName
    private string _TypeName;
    public string TypeName
    {
      get => _TypeName;
      set => _TypeName = value;
    }
    #endregion
    #region Property Length
    private int _Length;
    public int Length
    {
      get => _Length;
      set => _Length = value;
    }
    #endregion
    #region Property IsNullable
    private int _IsNullable;
    public int IsNullable
    {
      get => _IsNullable;
      set => _IsNullable = value;
    }
    #endregion
    #region Property CreateDate
    private DateTime _CreateDate;
    public DateTime CreateDate
    {
      get => _CreateDate;
      set => _CreateDate = value;
    }
    #endregion

    public new IBaseDao GetDaoSingleton()
    {
      IBaseDao _Dao = null;
      if (_Dao == null)
      {
        _Dao = (IBaseDao)MyDaoManager.Instance.GetDao(typeof(IBaseDao), ModeDB, this);
      }
      return _Dao;
    }
    #region Singleton
    public static List<SysObjects> _SysObjs = null;
    public static List<SysObjects> GetSysObjSingleton()
    {
      if ((_SysObjs == null) || (_SysObjs.Count == 0))
      {
        _SysObjs = new List<SysObjects>();

        SysObjects obj = new SysObjects();
        ISysObjectsDao _dao = (ISysObjectsDao)MyDaoManager.Instance.GetDao(typeof(ISysObjectsDao), obj.ModeDB, obj); // GetFuctions.GetDaoManager().GetDao(typeof(ISysObjectsDao));
        IList templist = _dao.QueryBy(BaseDataControl.ALL, obj, obj.ModeDB);
        if (templist != null)
        {

          foreach (SysObjects tempObj in templist)
          {
            _SysObjs.Add(tempObj);
          }
        }
      }
      return _SysObjs;
    }
    public static Hashtable _TableSysObjs = null;
    public static List<SysObjects> GetTableSysObjSingleton(BaseBO bo, string tname)
    {
      if (_TableSysObjs == null)
      {
        _TableSysObjs = new Hashtable();
      }
      if (_TableSysObjs[tname] == null)
      {
        List<SysObjects> _SysObjs = new List<SysObjects>();
        SysObjects obj = new SysObjects
        {
          ConnectionString = bo.ConnectionString
        };
        try
        {
          ISysObjectsDao _dao = (ISysObjectsDao)MyDaoManager.Instance.GetDao(typeof(ISysObjectsDao), obj.ModeDB, obj); // GetFuctions.GetDaoManager().GetDao(typeof(ISysObjectsDao));
          IList templist = _dao.QueryBy(BaseDataControl.ALL, obj, obj.ModeDB);
          if (templist != null)
          {

            foreach (SysObjects tempObj in templist)
            {
              _SysObjs.Add(tempObj);
            }
          }
          _TableSysObjs[tname] = _SysObjs;
        }
        catch (Exception ex)
        {
          UtilityBO.Log(obj, ex);
        }
        //List<SysObjects> subSysObj = GetSysObjSingleton().FindAll(o => o.TableName.Equals(tname.ToUpper()));
        //_TableSysObjs[tname] = subSysObj;
      }
      return (List<SysObjects>)_TableSysObjs[tname];
    }
    #endregion
    public SysObjects()
    {
      XMLName = "SysObjects";
    }
  }
  #endregion
  #region ISysObjectsDao
  public interface ISysObjectsDao : IBaseDao
  {
  }
  #endregion
  #region SysObjectsDao
  public class SysObjectsDao : BaseContextDao, ISysObjectsDao
  {
    #region BaseContextDao Members
    public new string GetTableName()
    {
      return "SysObjects";
    }
    #endregion BaseContextDao Members
  }
  #endregion

}

