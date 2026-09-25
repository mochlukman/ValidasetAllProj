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
  #region Ss00appversiconfigLookup
  [Serializable]
  public class Ss00appversiconfigLookupControl :  Ss00appversiconfigControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss00appversiconfigControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss00appversiconfigControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss00appversiconfigControl()).XMLName; ;
    //  List<Ss00appversiconfigControl> _ListData = (List<Ss00appversiconfigControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss00appversiconfigLookupControl dc = new Ss00appversiconfigLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss00appversiconfigControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss00appversiconfigControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss00appversiconfigControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss00appversiconfigLookupControl dc = new Ss00appversiconfigLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss00appversiconfigControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss00appversiconfigControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss00appversiconfigControl founddc = null;
      List<Ss00appversiconfigControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss00appversiconfigControl)_ListData.Find(o=>o.Idversiapp.Equals(dc.GetValue("Idversiapp")) && o.Par.Equals(dc.GetValue("Par")));
        if(founddc != null)
        {
          dc.SetValue("Idversiapp", founddc.Idversiapp);
          dc.SetValue("Par", founddc.Par);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idversiapp","Par"};
    }
    #endregion
    public Ss00appversiconfigLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPVERSICONFIG;
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
      Ss00appversiconfigLookupControl dclookup = new Ss00appversiconfigLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idversiapp","Par","Par" };
      string[] targets =  new String[] { "Idversiapp","Par","Par" };
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
  #endregion Ss00appversiconfigLookup
}

