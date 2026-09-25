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
  #region Ss01appmenuconfigLookup
  [Serializable]
  public class Ss01appmenuconfigLookupControl : Ss01appmenuconfigControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static List<Ss01appmenuconfigControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss01appmenuconfigControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss01appmenuconfigLookupControl dc = new Ss01appmenuconfigLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss01appmenuconfigControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public Ss01appmenuconfigLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENUCONFIG;
      ResetParams();
    }
    private Hashtable _Params = null;
    public Hashtable Params
    {
      get
      {
        if (_Params == null)
        {
          _Params = new Hashtable();
          List<Ss01appmenuconfigControl> list = (List<Ss01appmenuconfigControl>)GetListDataSingleton().FindAll(o => o.Idmenu.Equals(Idmenu));
          foreach (Ss01appmenuconfigControl ctrl in list)
          {
            _Params[ctrl.Par] = ctrl.Value;
          }
        }
        return _Params;
      }
    }
    public void ResetParams()
    {
      _Params = null;
    }

    public static Ss01appmenuconfigControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss01appmenuconfigControl founddc = null;
      List<Ss01appmenuconfigControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (Ss01appmenuconfigControl)_ListData.Find(o => o.Idmenu.Equals(dc.GetValue("Idmenu")) && o.Par.Equals(dc.GetValue("Par")));
        if (founddc != null)
        {
          dc.SetValue("Idmenu", founddc.Idmenu);
          dc.SetValue("Par", founddc.Par);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Idmenu", "Par" };
    }
    #endregion
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public static decimal GetProgress(string idmenu)
    {
      Ss01appmenuconfigControl dc = new Ss01appmenuconfigControl();
      dc.Idmenu = idmenu;
      dc.Par = "PROGRESS";
      if (dc.Load() != null)
      {
        return decimal.Parse(dc.Value);
      }
      else
      {
        return 0;
      }
    }
    //public override DataControlFieldCollection GetColumns()
    //{
    //  DataControlFieldCollection columns = new DataControlFieldCollection();
    //  return columns;
    //}
    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      Ss01appmenuconfigLookupControl dclookup = new Ss01appmenuconfigLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys = new String[] { "Idmenu", "Par", "Par" };
      string[] targets = new String[] { "Idmenu", "Par", "Par" };
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
      return "Par=Par";
    }
  }
  #endregion Ss01appmenuconfigLookup
}

