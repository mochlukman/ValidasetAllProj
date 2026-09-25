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
  #region Ss00appregconfigLookup
  [Serializable]
  public class Ss00appregconfigLookupControl :  Ss00appregconfigControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss00appregconfigControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss00appregconfigControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss00appregconfigControl()).XMLName; ;
    //  List<Ss00appregconfigControl> _ListData = (List<Ss00appregconfigControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss00appregconfigLookupControl dc = new Ss00appregconfigLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss00appregconfigControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss00appregconfigControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss00appregconfigControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss00appregconfigLookupControl dc = new Ss00appregconfigLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss00appregconfigControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss00appregconfigControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss00appregconfigControl founddc = null;
      List<Ss00appregconfigControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss00appregconfigControl)_ListData.Find(o=>o.Idreg.Equals(dc.GetValue("Idreg")) && o.Par.Equals(dc.GetValue("Par")));
        if(founddc != null)
        {
          dc.SetValue("Idreg", founddc.Idreg);
          dc.SetValue("Par", founddc.Par);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idreg","Par"};
    }
    #endregion
    public Ss00appregconfigLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPREGCONFIG;
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
      Ss00appregconfigLookupControl dclookup = new Ss00appregconfigLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idreg","Par","Par" };
      string[] targets =  new String[] { "Idreg","Par","Par" };
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
  #endregion Ss00appregconfigLookup
}

