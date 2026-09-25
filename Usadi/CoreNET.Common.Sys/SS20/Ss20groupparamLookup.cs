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
  #region Ss20groupparamLookup
  [Serializable]
  public class Ss20groupparamLookupControl :  Ss20groupparamControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss20groupparamControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss20groupparamControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss20groupparamControl()).XMLName; ;
    //  List<Ss20groupparamControl> _ListData = (List<Ss20groupparamControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss20groupparamLookupControl dc = new Ss20groupparamLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss20groupparamControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss20groupparamControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss20groupparamControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss20groupparamLookupControl dc = new Ss20groupparamLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss20groupparamControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss20groupparamControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss20groupparamControl founddc = null;
      List<Ss20groupparamControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss20groupparamControl)_ListData.Find(o=>o.Par.Equals(dc.GetValue("Par")));
        if(founddc != null)
        {
          dc.SetValue("Par", founddc.Par);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Par"};
    }
    #endregion
    public Ss20groupparamLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPPARAM;
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
      Ss20groupparamLookupControl dclookup = new Ss20groupparamLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Par","Par" };
      string[] targets =  new String[] { "Par","Par" };
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
  #endregion Ss20groupparamLookup
}

