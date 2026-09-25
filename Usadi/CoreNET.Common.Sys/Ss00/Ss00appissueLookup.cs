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
  #region Ss00appissueLookup
  [Serializable]
  public class Ss00appissueLookupControl :  Ss00appissueControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    //public static void SetSessionListDataNull()
    //{
    //  string session_name = (new Ss00appissueControl()).XMLName; ;
    //  HttpContext.Current.Session[session_name] = null;
    //}
    //public static List<Ss00appissueControl> GetListDataSingleton()
    //{
    //  string session_name = (new Ss00appissueControl()).XMLName; ;
    //  List<Ss00appissueControl> _ListData = (List<Ss00appissueControl>)HttpContext.Current.Session[session_name];
    //  if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
    //  {
    //    try
    //    {
    //      Ss00appissueLookupControl dc = new Ss00appissueLookupControl();
    //      dc.SetPageKey();
    //      _ListData = (List<Ss00appissueControl>)dc.View(BaseDataControl.ALL);
    //    }
    //    catch (Exception ex) 
    //    { 
    //      UtilityBO.Log(dc, ex);
    //    }
    //    HttpContext.Current.Session[session_name] = _ListData;
    //  }
    //  return _ListData;
    //}
    private static List<Ss00appissueControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss00appissueControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss00appissueLookupControl dc = new Ss00appissueLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss00appissueControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss00appissueControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss00appissueControl founddc = null;
      List<Ss00appissueControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss00appissueControl)_ListData.Find(o=>o.Idissue.Equals(dc.GetValue("Idissue")));
        if(founddc != null)
        {
          dc.SetValue("Idissue", founddc.Idissue);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idissue"};
    }
    #endregion
    public Ss00appissueLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPISSUE;
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
      Ss00appissueLookupControl dclookup = new Ss00appissueLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idissue","Idissue" };
      string[] targets =  new String[] { "Idissue","Idissue" };
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
      return "Idissue=Idissue";
    }
  }
  #endregion Ss00appissueLookup
}

