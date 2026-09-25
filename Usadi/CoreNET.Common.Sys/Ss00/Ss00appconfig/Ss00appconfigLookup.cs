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
  #region CoreNET.Common.BO.Ss00appconfigLookupControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appconfigLookupControl :  Ss00appconfigControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    public static void SetSessionListDataNull()
    {
      string session_name = (new Ss00appconfigControl()).XMLName; ;
      HttpContext.Current.Session[session_name] = null;
    }
    public static List<Ss00appconfigControl> GetSessionListDataSingleton()
    {
      string session_name = (new Ss00appconfigControl()).XMLName; ;
      List<Ss00appconfigControl> _ListData = (List<Ss00appconfigControl>)HttpContext.Current.Session[session_name];
      if (_ListData == null)
      {
        try
        {
          Ss00appconfigLookupControl dc = new Ss00appconfigLookupControl();
          dc.SetPageKey();
          _ListData = (List<Ss00appconfigControl>)dc.View(BaseDataControl.ALL);
        }
        catch (Exception) { }
        HttpContext.Current.Session[session_name] = _ListData;
      }
      return _ListData;
    }
    public static Ss00appconfigControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss00appconfigControl founddc = null;
      List<Ss00appconfigControl> _ListData = GetSessionListDataSingleton();
      if (_ListData != null)
      {
        founddc = (Ss00appconfigControl)_ListData.Find(o => o.Idapp.Equals(dc.GetValue("Idapp")) && o.Par.Equals(dc.GetValue("Par")));
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
      return new string[]{ "Idapp","Par"};
    }
    #endregion
    public Ss00appconfigLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPCONFIG;
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
      Ss00appconfigLookupControl dclookup = new Ss00appconfigLookupControl();
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
      return "Par=Par";
    }
  }
  #endregion Ss00appconfigLookup
}

