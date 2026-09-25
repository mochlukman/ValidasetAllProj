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
  #region Ss01appmenuLookup
  [Serializable]
  public class Ss01appmenuLookupControl : Ss01appmenuControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static Ss01appmenuLookupControl _Instance = null;
    public static Ss01appmenuLookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new Ss01appmenuLookupControl();
        }
        return _Instance;
      }
    }
    private static List<Ss01appmenuControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss01appmenuControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss01appmenuLookupControl dc = new Ss01appmenuLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss01appmenuControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static Ss01appmenuControl FindAndSetValuesIntoByIdmenu(IDataControlUI dc)
    {
      Ss01appmenuControl founddc = null;
      List<Ss01appmenuControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (Ss01appmenuControl)_ListData.Find(o => o.Idmenu.Equals(dc.GetValue("Idmenu")));
        //founddc = (Ss01appmenuControl)_ListData.Find(o => o.Idapp.Equals(dc.GetValue("Idapp")) && o.Kdmenu.Equals(dc.GetValue("Kdmenu")));
        if (founddc != null)
        {
          dc.SetValue("Idapp", founddc.Idapp);
          dc.SetValue("Kdmenu", founddc.Kdmenu);
          dc.SetValue("Nmmenu", founddc.Nmmenu);
          dc.SetValue("Url", founddc.Url);
          dc.SetValue("UrlFull", founddc.UrlFull);
        }
      }
      return founddc;
    }
    public static Ss01appmenuControl FindAndSetValuesIntoByKdmenu(IDataControlUI dc)
    {
      Ss01appmenuControl founddc = null;
      List<Ss01appmenuControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (Ss01appmenuControl)_ListData.Find(o => o.Idapp.Equals(dc.GetValue("Idapp")) && o.Kdmenu.Equals(dc.GetValue("Kdmenu")));
        if (founddc != null)
        {
          dc.SetValue("Idapp", founddc.Idapp);
          dc.SetValue("Kdmenu", founddc.Kdmenu);
          dc.SetValue("Nmmenu", founddc.Nmmenu);
          dc.SetValue("Url", founddc.Url);
          dc.SetValue("UrlFull", founddc.UrlFull);
        }
      }
      return founddc;
    }
    public static string GetURL(string idapp, string kdmenu)
    {
      Ss01appmenuControl dc = new Ss01appmenuControl() { Idapp = idapp, Kdmenu = kdmenu };
      Ss01appmenuControl founddc = FindAndSetValuesIntoByKdmenu(dc);
      if (founddc != null)
      {
        return founddc.UrlFull;
      }
      else
      {
        return string.Empty;
      }
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Idapp", "Kdmenu", "Nmmenu" };
    }
    #endregion
    public Ss01appmenuLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      Ss01appmenuLookupControl dclookup = new Ss01appmenuLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys = new String[] { "Kdmenu", "Nmmenu", "Idmenu" };
      string[] targets = new String[] { "Kdmenu", "Nmmenu", "Idmenu" };
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
      return "Idmenu=Kdmenu";
    }
  }
  #endregion Ss01appmenuLookup
}

