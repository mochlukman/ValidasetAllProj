using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SsappconfigLookupControl, CoreNET.Common.BO
  [Serializable]
  public class SsappconfigLookupControl : SsappconfigControl, IDataControl
  {
    #region Singleton
    private static SsappconfigLookupControl _Instance = null;
    private Hashtable _Params = null;
    public static SsappconfigLookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new SsappconfigLookupControl();
        }
        return _Instance;
      }
    }
    private List<SsappconfigControl> _ListData = null;
    public void SetListDataNull()
    {
      _ListData = null;
    }
    public List<SsappconfigControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (_ListData.Count == 0))
      {
        SsappconfigLookupControl dc = new SsappconfigLookupControl();
        _ListData = (List<SsappconfigControl>)dc.View();
      }
      return _ListData;
    }

    public new IList View()
    {
      string template_sql = @"select * from {0}";

      string sql = string.Empty;
      sql = string.Format(template_sql, "SSAPPCONFIG");
      List<IDataControl> list = BaseDataAdapter.GetListDC(this, sql);
      if (list.Count == 0)
      {
        sql = string.Format(template_sql, "SS00APPCONFIG");
        list = BaseDataAdapter.GetListDC(this, sql);
      }
      List<SsappconfigControl> ListData = (List<SsappconfigControl>)list.ConvertAll(new Converter<IDataControl, SsappconfigControl>(delegate (IDataControl par) { return (SsappconfigControl)par; }));
      return ListData;
    }

    public SsappconfigControl FindAndSetValuesInto(IDataControl dc)
    {
      SsappconfigControl founddc = null;
      List<SsappconfigControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (SsappconfigControl)_ListData.Find(o => o.Idapp.Equals(dc.GetValue("Idapp")) && o.Par.Equals(dc.GetValue("Par")));
        if (founddc != null)
        {
          dc.SetValue("Idapp", founddc.Idapp);
          dc.SetValue("Par", founddc.Par);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Idapp", "Par" };
    }
    #endregion
    public SsappconfigLookupControl()
    {
      XMLName = "Ssappconfig";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
      ConnectionString = SQLDataSource.Instance.CS_Config;
    }
    public void ResetParams(string idapp)
    {
      Idapp = idapp;
      _Params = null;
    }
    public Hashtable Params
    {
      get
      {
        if (_Params == null)
        {
          string idapp = Idapp;
          _Params = new Hashtable();
          List<SsappconfigControl> list = (List<SsappconfigControl>)GetListDataSingleton().FindAll(o => o.Idapp.Equals(idapp));
          foreach (SsappconfigControl ctrl in list)
          {
            _Params[ctrl.Par] = ctrl.Value;
          }
        }
        return _Params;
      }
    }
  }
  #endregion SsappconfigLookup
}

