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
  #region Ss20groupLookup
  [Serializable]
  public class Ss20groupLookupControl :  Ss20groupControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static Ss20groupLookupControl _Instance = null;
    public static Ss20groupLookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new Ss20groupLookupControl();
        }
        return _Instance;
      }
    }
    private static List<Ss20groupControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss20groupControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss20groupLookupControl dc = new Ss20groupLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss20groupControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss20groupControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss20groupControl founddc = null;
      List<Ss20groupControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss20groupControl)_ListData.Find(o=>o.Kdgroup.Equals(dc.GetValue("Kdgroup")));
        if(founddc != null)
        {
          dc.SetValue("Kdgroup", founddc.Kdgroup);
          dc.SetValue("Nmgroup", founddc.Nmgroup);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Kdgroup","Nmgroup"};
    }
    #endregion
    public Ss20groupLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUP;
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
      Ss20groupLookupControl dclookup = new Ss20groupLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Kdgroup","Nmgroup","Kdgroup" };
      string[] targets =  new String[] { "Kdgroup","Nmgroup","Kdgroup" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys,new int[] { 20, 75, 0 }, targets)
      {
        Label = title,
        VisibleControls = new bool[] { true, true, !entry },
        AllowRefresh = !entry,
        AllowEmpty = false,
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
  #endregion Ss20groupLookup
}

