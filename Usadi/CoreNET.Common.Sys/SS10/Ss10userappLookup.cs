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
  #region Ss10userappLookup
  [Serializable]
  public class Ss10userappLookupControl :  Ss10userappControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss10userappControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss10userControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss10userappControl()).XMLName; ;
    //  List<Ss10userControl> _ListData = (List<Ss10userControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss10userappLookupControl dc = new Ss10userappLookupControl();
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
      if (_ListData == null)
      {
        Ss10userappLookupControl dc = new Ss10userappLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss10userControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static Ss10userappControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss10userappControl founddc = null;
      List<Ss10userControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss10userappControl)_ListData.Find(o=>o.Idapp.Equals(dc.GetValue("Idapp")) && o.Userid.Equals(dc.GetValue("Userid")));
        if(founddc != null)
        {
          dc.SetValue("Idapp", founddc.Idapp);
          dc.SetValue("Userid", founddc.Userid);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idapp","Userid"};
    }
    #endregion
    public Ss10userappLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS10USERAPP;
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
      Ss10userappLookupControl dclookup = new Ss10userappLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Userid" };
      string[] targets =  new String[] { "Userid" };
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
      return "Userid=Userid";
    }
  }
  #endregion Ss10userappLookup
}

