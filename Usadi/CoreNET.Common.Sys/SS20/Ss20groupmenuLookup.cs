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
  #region Ss20groupmenuLookup
  [Serializable]
  public class Ss20groupmenuLookupControl :  Ss20groupmenuControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss20groupmenuControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss20groupmenuControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss20groupmenuControl()).XMLName; ;
    //  List<Ss20groupmenuControl> _ListData = (List<Ss20groupmenuControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss20groupmenuLookupControl dc = new Ss20groupmenuLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss20groupmenuControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss20groupmenuControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss20groupmenuControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss20groupmenuLookupControl dc = new Ss20groupmenuLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss20groupmenuControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss20groupmenuControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss20groupmenuControl founddc = null;
      List<Ss20groupmenuControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss20groupmenuControl)_ListData.Find(o=>o.Kdgroup.Equals(dc.GetValue("Kdgroup")) && o.Idmenu.Equals(dc.GetValue("Idmenu")));
        if(founddc != null)
        {
          dc.SetValue("Kdgroup", founddc.Kdgroup);
          dc.SetValue("Idmenu", founddc.Idmenu);
        }
      }
      return founddc;
    }
    public static string GetURL(string kdgroup, string kdmenu)
    {
      Ss20groupmenuControl dc = new Ss20groupmenuControl() { Kdgroup = kdgroup, Kdmenu = kdmenu };
      Ss20groupmenuControl founddc = FindAndSetValuesInto(dc);
      if (founddc != null)
      {
        return founddc.Url;
      }
      else
      {
        return string.Empty;
      }
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idgroup","Idmenu"};
    }
    #endregion
    public Ss20groupmenuLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPMENU;
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
      Ss20groupmenuLookupControl dclookup = new Ss20groupmenuLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idgroup","Idmenu","Idmenu" };
      string[] targets =  new String[] { "Idgroup","Idmenu","Idmenu" };
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
  #endregion Ss20groupmenuLookup
}

