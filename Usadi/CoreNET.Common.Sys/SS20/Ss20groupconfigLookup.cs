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
  #region CoreNET.Common.BO.Ss20groupconfigLookupControl, CoreNET.Common.BO
  [Serializable]
  public class Ss20groupconfigLookupControl :  Ss20groupconfigControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static  Ss20groupconfigLookupControl _Instance = null;
    public static  Ss20groupconfigLookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new  Ss20groupconfigLookupControl();
        }
        return _Instance;
      }
    }
    private static List<Ss20groupconfigControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss20groupconfigControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        Ss20groupconfigLookupControl dc = new Ss20groupconfigLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss20groupconfigControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    #endregion
    public Ss20groupconfigLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPCONFIG;
    }
    ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if(cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
      }
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
      return cViewListProperties;
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.LOOKUP);
      return list;
    }
    public override DataControlFieldCollection GetColumns()
    {
      if (columns == null)
      {
        columns = new DataControlFieldCollection();
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdgroup"), typeof(string), 30, HorizontalAlign.Left).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Par"), typeof(string), 10, HorizontalAlign.Left).SetEditable(true));
      }
      return columns;
    }
    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      Ss20groupconfigLookupControl dclookup = new Ss20groupconfigLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys =  new String[] { "Kdgroup","Nmgroup","Par","Par" };
      string[] targets =  new String[] { "Kdgroup=Kdgroup","Nmgroup=Nmgroup","Par=Par" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys,new int[] { 20, 75, 0 }, targets)
      {
        Label = title,
        VisibleControls = new bool[] { true, true, !entry },
        AllowRefresh = !entry,
        DCLookup = dclookup,
        IsTree = true,
        SelectionCriteria = ParameterRow.SELECTION_CRITERIA_TYPE,
        SelectionType = "D"
      };
      return par;
    }
    public string GetFieldValueMap()
    {
      return "Nmgroup=Nmgroup";
    }
  }
  #endregion Ss20groupconfigLookup
}

