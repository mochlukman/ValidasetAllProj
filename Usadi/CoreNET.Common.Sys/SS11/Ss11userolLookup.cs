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
  #region Ss11userolLookup
  [Serializable]
  public class Ss11userolLookupControl :  Ss11userolControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss11userolControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss10userControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss11userolControl()).XMLName; ;
    //  List<Ss10userControl> _ListData = (List<Ss10userControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss11userolLookupControl dc = new Ss11userolLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss10userControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss10userControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss10userControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss11userolLookupControl dc = new Ss11userolLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss10userControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss11userolControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss11userolControl founddc = null;
      List<Ss10userControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss11userolControl)_ListData.Find(o=>o.Idol.Equals(dc.GetValue("Idol")));
        if(founddc != null)
        {
          dc.SetValue("Idol", founddc.Idol);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idol"};
    }
    #endregion
    public Ss11userolLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS11USEROL;
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
      Ss11userolLookupControl dclookup = new Ss11userolLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idol","Idol" };
      string[] targets =  new String[] { "Idol","Idol" };
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
      return "Idol=Idol";
    }
  }
  #endregion Ss11userolLookup
}

