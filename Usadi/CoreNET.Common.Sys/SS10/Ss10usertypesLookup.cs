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
  #region Ss10usertypesLookup
  [Serializable]
  public class Ss10usertypesLookupControl :  Ss10usertypesControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss10usertypesControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss10userControl> GetSessionListDataSingleton()
    //{
    //  string session_name = (new Ss10usertypesControl()).XMLName; ;
    //  List<Ss10userControl> _ListData = (List<Ss10userControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss10usertypesLookupControl dc = new Ss10usertypesLookupControl();
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
        Ss10usertypesLookupControl dc = new Ss10usertypesLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss10userControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss10usertypesControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss10usertypesControl founddc = null;
      List<Ss10userControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss10usertypesControl)_ListData.Find(o=>o.Usertype.Equals(dc.GetValue("Usertype")));
        if(founddc != null)
        {
          dc.SetValue("Usertype", founddc.Usertype);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Usertype"};
    }
    #endregion
    public Ss10usertypesLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS10USERTYPES;
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
      Ss10usertypesLookupControl dclookup = new Ss10usertypesLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Usertype","Usertype" };
      string[] targets =  new String[] { "Usertype","Usertype" };
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
      return "Usertype=Usertype";
    }
  }
  #endregion Ss10usertypesLookup
}

