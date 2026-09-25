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
  #region DmdokLookup
  [Serializable]
  public class DmdokLookupControl : DmdokControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static List<DmdokControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<DmdokControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        DmdokLookupControl dc = new DmdokLookupControl();
        dc.SetPageKey();
        _ListData = (List<DmdokControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static DmdokControl FindAndSetValuesInto(IDataControlUI dc)
    {
      DmdokControl founddc = null;
      List<DmdokControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (DmdokControl)_ListData.Find(o => o.Kddok.Equals(dc.GetValue("Kddok")));
        if (founddc != null)
        {
          dc.SetValue("Kddok", founddc.Kddok);
          dc.SetValue("Nmdok", founddc.Nmdok);
          if (dc.GetProperty("Lbldok") != null) dc.SetValue("Lbldok", founddc.Lbldok);
          if (dc.GetProperty("Kdpersppkd") != null) dc.SetValue("Kdpersppkd", founddc.Kdpersppkd);
          if (dc.GetProperty("Kdpersskpd") != null) dc.SetValue("Kdpersskpd", founddc.Kdpersskpd);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Kddok", "Lbldok" };
    }
    #endregion
    public DmdokLookupControl()
    {
      XMLName = ConstantTablesSys.XMLDMDOK;
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
      DmdokLookupControl dclookup = new DmdokLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName + GlobalExt.GetRequestId().Substring(0, 6));
      string[] keys = new String[] { "Kddok", "Nmdok", "Kddok" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys)
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
      return "Kddok=Nmdok";
    }
  }
  #endregion DmdokLookup
}

