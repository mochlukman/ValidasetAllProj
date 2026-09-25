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
  #region Ss00appmenuconfigLookup
  [Serializable]
  public class Ss00appmenuconfigLookupControl :  Ss00appmenuconfigControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static List<Ss00appmenuconfigControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss00appmenuconfigControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        Ss00appmenuconfigLookupControl dc = new Ss00appmenuconfigLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss00appmenuconfigControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static Ss00appmenuconfigControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss00appmenuconfigControl founddc = null;
      List<Ss00appmenuconfigControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (Ss00appmenuconfigControl)_ListData.Find(o=>o.Idmenu.Equals(dc.GetValue("Idmenu")));
        if(founddc != null)
        {
          dc.SetValue("Idmenu", founddc.Idmenu);
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idmenu"};
    }
    #endregion
    public Ss00appmenuconfigLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPMENUCONFIG;
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
      Ss00appmenuconfigLookupControl dclookup = new Ss00appmenuconfigLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Idmenu","Idmenu" };
      string[] targets =  new String[] { "Idmenu","Idmenu" };
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
      return "Par=Par";
    }
  }
  #endregion Ss00appmenuconfigLookup
}

