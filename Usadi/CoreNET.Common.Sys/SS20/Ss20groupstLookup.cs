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
  #region Ss20groupstLookup
  [Serializable]
  public class Ss20groupstLookupControl :  Ss20groupstControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss20groupstControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss20groupstControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss20groupstControl()).XMLName; ;
    //  List<Ss20groupstControl> _ListData = (List<Ss20groupstControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss20groupstLookupControl dc = new Ss20groupstLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss20groupstControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss20groupstControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss20groupstControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss20groupstLookupControl dc = new Ss20groupstLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss20groupstControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss20groupstControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss20groupstControl founddc = null;
      List<Ss20groupstControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss20groupstControl)_ListData.Find(o=>o.Idst.Equals(dc.GetValue("Idst")));
        if(founddc != null)
        {
          dc.SetValue("Idst", founddc.Idst);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idst"};
    }
    #endregion
    public Ss20groupstLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPST;
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
      Ss20groupstLookupControl dclookup = new Ss20groupstLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idst","Idst" };
      string[] targets =  new String[] { "Idst","Idst" };
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
      return "Idconfig=Idconfig";
    }
  }
  #endregion Ss20groupstLookup
}

