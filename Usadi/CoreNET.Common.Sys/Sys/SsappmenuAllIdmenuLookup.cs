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
  #region CoreNET.Common.BO.SsappmenuallLookupControl, CoreNET.Common.BO
  [Serializable]
  public class SsappmenuAllIdmenuLookupControl : Ss00appmenuControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static SsappmenuAllIdmenuLookupControl _Instance = null;
    public static SsappmenuAllIdmenuLookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new SsappmenuAllIdmenuLookupControl();
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
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        SsappmenuAllIdmenuLookupControl dc = new SsappmenuAllIdmenuLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss00appmenuControl>)dc.View(BaseDataControl.UNION);
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
    public static Ss00appmenuControl FindAndSetValuesIntoByIdmenu(IDataControlUI dc)
    {
      Ss00appmenuControl founddc = null;
      List<Ss00appmenuControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (Ss00appmenuControl)_ListData.Find(o => o.Idmenu.Equals(dc.GetValue("Idmenu")));
        if (founddc != null)
        {
          dc.SetValue("Idmenu", founddc.Idmenu);
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
    public SsappmenuAllIdmenuLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.CanModeTree = false;
      return cViewListProperties;
    }
    public new IList View()
    {
      IList list = base.View(BaseDataControl.UNION);
      return list;
    }


    public new void SetFilterKey(BaseBO bo)
    {
    }
    public new HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }

    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      SsappmenuAllIdmenuLookupControl dclookup = new SsappmenuAllIdmenuLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys = new String[] { "Typename" };
      string[] targets = new String[] { "Typename=Olist1" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 90, 0, 0 }, targets)
      {
        Label = title,
        VisibleControls = new bool[] { true, true, !entry },
        AllowRefresh = !entry,
        DCLookup = dclookup,
        IsTree = false,
        SelectionType = "D"
      };
      return par;
    }
    public string GetFieldValueMap()
    {
      return "Typename=Typename";
    }
  }
  #endregion SsappmenuLookup
}

