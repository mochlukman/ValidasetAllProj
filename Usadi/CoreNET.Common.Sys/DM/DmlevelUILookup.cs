using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;
using CoreNET.Common.BO;

namespace CoreNET.Common.BO
{
  #region DmlevelLookup
  [Serializable]
  public class DmlevelUILookupControl : DmlevelUIControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new DmlevelUIControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<DmlevelUIControl> GetListDataSingleton()
    //{
    //  string session_name = (new DmlevelUIControl()).XMLName; ;
    //  List<DmlevelUIControl> _ListData = (List<DmlevelUIControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    DmlevelUIControl dc = new DmlevelUIControl();
    //    dc.SetPageKey();
    //    _ListData = (List<DmlevelUIControl>)dc.View();
    //     HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<DmlevelUIControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<DmlevelUIControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        DmlevelUIControl dc = new DmlevelUIControl();
        dc.SetPageKey();
        _ListData = (List<DmlevelUIControl>)dc.View();
      }
      return _ListData;
    }
    public static void FindAndSetValuesInto(IDataControlUI dc)
    {
      DmlevelUIControl founddc = (DmlevelUIControl)GetListDataSingleton().Find(o => o.Tablename.Equals(dc.GetValue("Tablename")) && o.Kdlevel.Equals(dc.GetValue("Kdlevel")));
      if (founddc != null)
      {
        dc.SetValue("Tablename", founddc.Tablename);
        dc.SetValue("Kdlevel", founddc.Kdlevel);
        dc.SetValue("Nmlevel", founddc.Nmlevel);
      }
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Tablename", "Kdlevel", "Nmlevel" };
    }
    #endregion
    public DmlevelUILookupControl()
    {
      XMLName = ConstantTablesSys.XMLDMLEVEL;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    //public override DataControlFieldCollection GetColumns()
    //{
    //  DataControlFieldCollection columns = new DataControlFieldCollection();
    //  return columns;
    //}
    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      DmlevelUILookupControl dclookup = new DmlevelUILookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys = new String[] { "Kdlevel", "Nmlevel", "Kdlevel" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys)
      {
        Label = title,
        VisibleControls = new bool[] { true, true, !entry },
        AllowRefresh = !entry,
        DCLookup = dclookup,
        IsTree = true,
        SelectionType = "D"
      };
      return par;
    }
    public string GetFieldValueMap()
    {
      return "Kdlevel=Nmlevel";
    }
  }
  #endregion DmlevelLookup
}

