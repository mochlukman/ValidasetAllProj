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
  #region Ss00appversiLookup
  [Serializable]
  public class Ss00appversiLookupControl :  Ss00appversiControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss00appversiControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss00appversiControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss00appversiControl()).XMLName; ;
    //  List<Ss00appversiControl> _ListData = (List<Ss00appversiControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss00appversiLookupControl dc = new Ss00appversiLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss00appversiControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss00appversiControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss00appversiControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss00appversiLookupControl dc = new Ss00appversiLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss00appversiControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss00appversiControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss00appversiControl founddc = null;
      List<Ss00appversiControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss00appversiControl)_ListData.Find(o=>o.Idapp.Equals(dc.GetValue("Idapp")) && o.Kdversi.Equals(dc.GetValue("Kdversi")));
        if(founddc != null)
        {
          dc.SetValue("Idapp", founddc.Idapp);
          dc.SetValue("Kdversi", founddc.Kdversi);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idapp","Kdversi","Nmversi"};
    }
    #endregion
    public Ss00appversiLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPVERSI;
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
      Ss00appversiLookupControl dclookup = new Ss00appversiLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idapp","Kdversi","Nmversi","Kdversi" };
      string[] targets =  new String[] { "Idapp","Kdversi","Nmversi","Kdversi" };
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
  #endregion Ss00appversiLookup
}

