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
  #region Rpt00datavolLookup
  [Serializable]
  public class Rpt00datavolLookupControl :  Rpt00datavolControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Rpt00datavolControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Rpt00datavolControl> GetSessionListDataSingleton()
    //{
    //  string session_name = (new Rpt00datavolControl()).XMLName; ;
    //  List<Rpt00datavolControl> _ListData = (List<Rpt00datavolControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Rpt00datavolLookupControl dc = new Rpt00datavolLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Rpt00datavolControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Rpt00datavolControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Rpt00datavolControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Rpt00datavolLookupControl dc = new Rpt00datavolLookupControl();
        dc.SetPageKey();
        _ListData = (List<Rpt00datavolControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Rpt00datavolControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Rpt00datavolControl founddc = null;
      List<Rpt00datavolControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Rpt00datavolControl)_ListData.Find(o=>o.Nmobj.Equals(dc.GetValue("Nmobj")));
        if(founddc != null)
        {
          dc.SetValue("Nmobj", founddc.Nmobj);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Nmobj"};
    }
    #endregion
    public Rpt00datavolLookupControl()
    {
      XMLName = ConstantTablesSys.XMLRPT00DATAVOL;
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
      Rpt00datavolLookupControl dclookup = new Rpt00datavolLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Nmobj","Nmobj" };
      string[] targets =  new String[] { "Nmobj","Nmobj" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys,new int[] { 20, 75, 0 }, targets)
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
      return "Nmobj=Nmobj";
    }
  }
  #endregion Rpt00datavolLookup
}

