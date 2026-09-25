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
  #region Ss20grouproleLookup
  [Serializable]
  public class Ss20grouproleLookupControl :  Ss20grouproleControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss20grouproleControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss20grouproleControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss20grouproleControl()).XMLName; ;
    //  List<Ss20grouproleControl> _ListData = (List<Ss20grouproleControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss20grouproleLookupControl dc = new Ss20grouproleLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss20grouproleControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss20grouproleControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss20grouproleControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss20grouproleLookupControl dc = new Ss20grouproleLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss20grouproleControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss20grouproleControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss20grouproleControl founddc = null;
      List<Ss20grouproleControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss20grouproleControl)_ListData.Find(o=>o.Idst.Equals(dc.GetValue("Idst")) && o.Idrole.Equals(dc.GetValue("Idrole")));
        if(founddc != null)
        {
          dc.SetValue("Idst", founddc.Idst);
          dc.SetValue("Idrole", founddc.Idrole);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idst","Idrole"};
    }
    #endregion
    public Ss20grouproleLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPROLE;
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
      Ss20grouproleLookupControl dclookup = new Ss20grouproleLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idst","Idrole","Idrole" };
      string[] targets =  new String[] { "Idst","Idrole","Idrole" };
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
  #endregion Ss20grouproleLookup
}

