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
  #region Ss00appregLookup
  [Serializable]
  public class Ss00appregLookupControl :  Ss00appregControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss00appregControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss00appregControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss00appregControl()).XMLName; ;
    //  List<Ss00appregControl> _ListData = (List<Ss00appregControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss00appregLookupControl dc = new Ss00appregLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss00appregControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss00appregControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss00appregControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss00appregLookupControl dc = new Ss00appregLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss00appregControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss00appregControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss00appregControl founddc = null;
      List<Ss00appregControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss00appregControl)_ListData.Find(o=>o.Idreg.Equals(dc.GetValue("Idreg")));
        if(founddc != null)
        {
          dc.SetValue("Idreg", founddc.Idreg);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idreg"};
    }
    #endregion
    public Ss00appregLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPREG;
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
      Ss00appregLookupControl dclookup = new Ss00appregLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idreg","Idreg" };
      string[] targets =  new String[] { "Idreg","Idreg" };
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
  #endregion Ss00appregLookup
}

