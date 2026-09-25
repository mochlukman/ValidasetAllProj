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
  #region DmtahunLookup
  [Serializable]
  public class DmtahunUILookupControl : DmtahunUIControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static DmtahunUILookupControl _Instance = null;
    public static DmtahunUILookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new DmtahunUILookupControl();
        }
        return _Instance;
      }
    }
    private static List<DmtahunUIControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<DmtahunUIControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        DmtahunUIControl dc = new DmtahunUIControl();
        dc.SetPageKey();
        _ListData = (List<DmtahunUIControl>)dc.View();
      }
      return _ListData;
    }
    public static void FindAndSetValuesInto(IDataControlUI dc)
    {
      DmtahunUIControl founddc = (DmtahunUIControl)GetListDataSingleton().Find(o => o.Kdtahun.Equals(dc.GetValue("Kdtahun")));
      if (founddc != null)
      {
        dc.SetValue("Kdtahun", founddc.Kdtahun);
        dc.SetValue("Nmtahun", founddc.Nmtahun);
      }
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Kdtahun", "Nmtahun" };
    }
    #endregion
    public DmtahunUILookupControl()
    {
      XMLName = ConstantTablesSys.XMLDMTAHUN;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.IDKey = "Kdtahun";
      cViewListProperties.IDProperty = "Kdtahun";
      return cViewListProperties;
    }
    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      string[] keys = new String[] { "Kdtahun", "Nmtahun", "Kdtahun" };
      string[] targets = new String[] { "Idapp=Kdtahun", "Nmapp=Nmtahun" };
      return GetLookupParameterRow(callerCtr, entry, keys, targets);
    }
    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry, string[] keys, string[] targets)
    {
      DmtahunUILookupControl dclookup = new DmtahunUILookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 20, 75, 0 }, targets)
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
      return "Kdtahun=Nmtahun";
    }
  }
  #endregion DmtahunLookup
}

