using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Configuration;
using CoreNET.Common.BO;
using System.Web;
using Ext.Net;
using Ext.Net.Utilities;

namespace CoreNET.Common.Base
{
  [Serializable]
  public abstract class BaseDataControl : BaseExtBO, IDataControl
  {
    #region Constant
    public const int NORMAL = 0;
    public const int CAN_BE_INSERTED = 1;
    public const int CAN_BE_UPDATED = 2;
    public const int ADD_CHILD = 3;
    public const int ADD_NODE = 4;

    public const string ALL = "All";
    public const string LAST = "Last";
    public const string LIST = "List";
    public const string RANGE = "Range";
    public const string TOTAL = "Total";
    public const string DEFAULT = "";
    public const string DETIL = "Detil";
    public const string TYPE = "Type";
    public const string PARAM = "Param";
    public const string FILTER = "Filter";
    public const string LOOKUP = "Lookup";
    public const string PARENT = "Parent";
    public const string PK = "PK";
    public const string ID = "ID";
    public const string KEY = "Key";
    public const string SYSTEM_ERR_CANT_INSERT_DUPLICATE = "Cannot insert duplicate key";
    public const string SYSTEM_ERR_CHILDREN_STILL_EXIST = "Children still exist";
    public const string SYSTEM_ERR_CONVERT_NUM_TO_MONEY = "Arithmetic overflow error converting numeric to data type money";
    #endregion
    #region IDataControl Members
    #region GetDao()

    #region Property ModeDB
    private int _ModeDB = 0;
    public int ModeDB
    {
      get
      {
        if (_ModeDB <= 0)
        {
          return MyDaoManager.MODE_DB_OPERATIONAL;
        }
        else
        {
          return _ModeDB;
        }
      }
      set { _ModeDB = value; }
    }
    #endregion

    public IBaseDao GetDaoSingleton()
    {
      IBaseDao _Dao = (IBaseDao)GlobalApp.GetSessionDaoManager().GetDao(typeof(IBaseDao), ModeDB);
      if (_Dao == null)
      {
        //HttpContext.Current.Response.Redirect(GlobalApp.GetHomeURL());/*susah didebug nantinya*/
        /*bikin crash klo pake ConstantDict */
        try
        {
          HttpContext.Current.Application[GlobalUtils.DAO] = null; GlobalApp.SetSessionDataSourceConfig();
          GlobalApp.SetSessionDataSourceOperational();
          GlobalApp.SetSessionDataSourceOperational2();
          GlobalApp.SetSessionDataSourceLog();
          IBatisNet.DataAccess.Configuration.DomDaoManagerBuilder builder = new IBatisNet.DataAccess.Configuration.DomDaoManagerBuilder();
          builder.Configure(GlobalUtils.DAO);
         _Dao = (IBaseDao)GlobalApp.GetSessionDaoManager().GetDao(typeof(IBaseDao), ModeDB);
          HttpContext.Current.Application[GlobalUtils.DAO] = "1";
        }
        catch (Exception)
        {
          throw new Exception("Error get dao manager, try to refresh main page");//new Exception(ConstantDict.Translate("LBL_ERROR_GET_DAO"));
        }
      }
      return _Dao;
    }
    #endregion

    //private string _dao;
    //public string Dao
    //{
    //  get { return _dao; }
    //  set { _dao = value; }
    //}
    private string _XMLName;
    public string XMLName
    {
      get { return _XMLName; }
      set { _XMLName = value; }
    }
    private string[] _Keys = new string[] { "Key", "Field", "Value", "Type" };
    public string[] Keys
    {
      get { return _Keys; }
      set { _Keys = value; }
    }

    public string GetTableName()
    {
      return XMLName;
    }
    public int GetRowCount()
    {
      return 0;
    }

    public void Insert(String label)
    {
      if (GlobalApp.GetSessionUser() != null)
      {
        Cre_by = GlobalApp.GetSessionUser().GetUserID();
      }
      Cre_date = DateTime.Now;
      Last_by = Cre_by;
      Last_date = Cre_date;
      GetDaoSingleton().Insert(label, this, ModeDB);
      LogUtils.Log(this, EventControl.EVENT_INSERT);
    }

    public int Update(String label)
    {
      /*Ada kemungkinan, sdh diapprove tp tetep bisa diupdate*/
      //if (Rating > 0)
      //{
      //  throw new Exception(ConstantDict.Translate("LBL_APPROVED_DATA"));
      //}
      Mod_by = GlobalApp.GetSessionUser().GetUserID();
      Mod_date = DateTime.Now;
      Last_by = Mod_by;
      Last_date = Mod_date;
      int n = GetDaoSingleton().UpdateBy(label, this, ModeDB);
      LogUtils.Log(this, EventControl.EVENT_UPDATE);
      return n;
    }

    public int Delete(String label)
    {
      if (Rating > 0)
      {
        throw new Exception(ConstantDict.Translate("LBL_APPROVED_DATA"));
      }
      int n = GetDaoSingleton().DeleteBy(label, this, ModeDB);
      LogUtils.Log(this, EventControl.EVENT_DELETE);
      return n;
    }
    public void Insert()
    {
      Insert(BaseDataControl.DEFAULT);
    }
    public int Update()
    {
      return Update(BaseDataControl.DEFAULT);
    }
    public int Delete()
    {
      return Delete(BaseDataControl.DEFAULT);
    }
    public IList View()
    {
      return View(BaseDataControl.ALL);
    }
    public BaseBO Load()
    {
      return Load(BaseDataControl.PK);
    }
    public IList View(String label)
    {
      IList list = GetDaoSingleton().QueryBy(label, this, ModeDB);
      LogUtils.Log(this, EventControl.EVENT_VIEW);
      return list;
    }

    public IList ExecSP(String label)
    {
      IList list = GetDaoSingleton().ExecSP(label, this, ModeDB);
      LogUtils.Log(this, EventControl.EVENT_EXECSP);
      return list;
    }

    public BaseBO Load(String label)
    {
      IBaseDao dao = GetDaoSingleton();
      if (dao != null)
      {
        BaseBO bo = dao.LoadBy(label, this, ModeDB);
        if (bo != null)
        {
          CopyPropertyBOFrom(bo);
          LogUtils.Log(this, EventControl.EVENT_LOAD);
          return this;
        }
        else
        {
          return bo;
        }
      }
      return null;
    }
    public void CopyPropertyBOTo(IDataControl dest_bo)//this is source, dest_bo is destination BO
    {
      ((BaseDataControl)dest_bo).CopyPropertyBOFrom(this);
    }
    public void CopyPropertyBOFrom(BaseBO source_bo)//this is destination, source_bo is source BO
    {
      CopyPropertyBO(source_bo);
    }
    public void CopyPropertyBOExcludeROFrom(BaseBO source_bo)//Exclue ReadOnly
    {
      IDataControlUI dc = (IDataControlUI)this;
      Dictionary<string, object> ro = new Dictionary<string, object>();
      for (int i = 0; i < ((ViewListProperties)dc.GetProperties()).ReadOnlyFields.Length; i++)
      {
        string key = ((ViewListProperties)dc.GetProperties()).ReadOnlyFields[i];
        ro.Add(key, GetValue(key));
      }
      this.CopyPropertyBOFrom(source_bo);
      for (int i = 0; i < ((ViewListProperties)dc.GetProperties()).ReadOnlyFields.Length; i++)
      {
        string key = ((ViewListProperties)dc.GetProperties()).ReadOnlyFields[i];
        SetValue(key, ro[key]);
      }
    }
    public void CopyPropertyBO(BaseBO source_bo)//this is destination, source_bo is source BO
    {
      if (source_bo != null)
      {
        //Mode dan Dao ga boleh ditimpa, cukup property databasenya aja
        int mode = Mode;
        int modeDB = ModeDB;
        //string dao = Dao;
        string xmlname = XMLName;
        source_bo.Mirror(this);
        Mode = mode;
        ModeDB = modeDB;
        //Dao = dao;
        XMLName = xmlname;
      }
    }

    public void CekFields(string[] _Fields)
    {
      for (int i = 0; i < _Fields.Length; i++)
      {
        PropertyInfo prop = this.GetType().GetProperty(_Fields[i]);
        if (prop == null)
        {
          throw new Exception("Field " + _Fields[i] + " pada Fields tidak terdefinisi!");
        }
      }
    }

    public new String[] GetNotFields()
    {
      string[] temps = base.GetNotFields();
      int n = temps.Length;
      string[] notfields = new string[n + 4];
      for (int i = 0; i < n; i++)
      {
        notfields[i] = temps[i];
      }
      notfields[n++] = "Mode";
      notfields[n++] = "Dao";
      notfields[n++] = "XMLName";
      notfields[n++] = "Keys";
      return notfields;
    }
    public new String[] GetFields()
    {
      ArrayList arListNotFields = new ArrayList(GetNotFields());
      return GetFields(arListNotFields);
    }
    public static IList GetList(IDataControl dc)
    {
      return ((BaseDataControl)dc).View(BaseDataControl.ALL);
    }
    /// <summary>
    /// Dieksekusi sebelum menampilkan form entry, untuk menset nilai default (NextKey, NoUrut, dll)
    /// Next Rename : SetDefaultKey
    /// </summary>
    public void SetPrimaryKey() { }
    #endregion
  }
}
