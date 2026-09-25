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
  #region CoreNET.Common.BO.Ss00appconfigparLookupControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appconfigparLookupControl :  Ss00appconfigparControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss00appconfigparControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss00appconfigparControl> GetSessionListDataSingleton()
    //{
    //  string session_name = (new Ss00appconfigparControl()).XMLName; ;
    //  List<Ss00appconfigparControl> _ListData = (List<Ss00appconfigparControl>)HttpContext.Current.Session[session_name];
    //  if (_ListData == null)
    //  {
    //    try
    //    {
    //      Ss00appconfigparLookupControl dc = new Ss00appconfigparLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss00appconfigparControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss00appconfigparControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss00appconfigparControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        Ss00appconfigparLookupControl dc = new Ss00appconfigparLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss00appconfigparControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static Ss00appconfigparControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss00appconfigparControl founddc = null;
      List<Ss00appconfigparControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss00appconfigparControl)_ListData.Find(o=>o.Kdpar.Equals(dc.GetValue("Kdpar")));
        if(founddc != null)
        {
          dc.SetValue("Kdpar", founddc.Kdpar);
          dc.SetValue("Nmpar", founddc.Nmpar);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Kdpar","Nmpar"};
    }
    #endregion
    public Ss00appconfigparLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPCONFIGPAR;
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
      Ss00appconfigparLookupControl dclookup = new Ss00appconfigparLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Kdpar","Nmpar","Kdpar" };
      string[] targets =  new String[] { "Kdpar","Nmpar","Kdpar" };
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
      return "Kdpar=Nmpar";
    }
  }
  #endregion Ss00appconfigparLookup
}

