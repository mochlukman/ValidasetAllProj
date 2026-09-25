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
  #region Ss00appmenuLookup
  [Serializable]
  public class Ss00appmenuLookupControl : Ss00appmenuControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static Ss00appmenuLookupControl _Instance = null;
    public static Ss00appmenuLookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new Ss00appmenuLookupControl();
        }
        return _Instance;
      }
    }
    private static List<Ss00appmenuControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss00appmenuControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        Ss00appmenuLookupControl dc = new Ss00appmenuLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss00appmenuControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static Ss00appmenuControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss00appmenuControl founddc = null;
      List<Ss00appmenuControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (Ss00appmenuControl)_ListData.Find(o => o.Idapp.Equals(dc.GetValue("Idapp")) && o.Kdmenu.Equals(dc.GetValue("Kdmenu")));
        if (founddc != null)
        {
          dc.SetValue("Idapp", founddc.Idapp);
          dc.SetValue("Kdmenu", founddc.Kdmenu);
          dc.SetValue("Nmmenu", founddc.Nmmenu);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Idapp", "Kdmenu", "Nmmenu" };
    }
    #endregion
    public Ss00appmenuLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPMENU;
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

    public new IList View()
    {
      /*Filter By Idapp*/
      IList list = base.View(BaseDataControl.ALL);
      return list;
    }
    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      Ss00appmenuLookupControl dclookup = new Ss00appmenuLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys = new String[] { "Kdmenu", "Nmmenu", "Olist1"};
      string[] targets = new String[] { "Kdmenu", "Nmmenu", "Olist1" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 20, 75, 0 }, targets)
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
      return "Kdmenu=Nmmenu";
    }
  }
  #endregion Ss00appmenuLookup
}

