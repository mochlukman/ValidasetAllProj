using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;
using CoreNET.Common.BO;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.DmpemdaLookupControl, CoreNET.Common.BO
  [Serializable]
  public class DmpemdaLookupControl : DmpemdaControl, IDataControlLookup, IDataControlTreeGrid3,IHasJSScript
  {
    #region Singleton
    private static DmpemdaLookupControl _Instance = null;
    public static DmpemdaLookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new DmpemdaLookupControl();
        }
        return _Instance;
      }
    }
    private static List<DmpemdaControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<DmpemdaControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        DmpemdaLookupControl dc = new DmpemdaLookupControl();
        dc.SetPageKey();
        _ListData = (List<DmpemdaControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static DmpemdaControl FindAndSetValuesInto(IDataControlUI dc)
    {
      DmpemdaControl founddc = null;
      List<DmpemdaControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (DmpemdaControl)_ListData.Find(o => o.Kdpemda.Equals(dc.GetValue("Kdpemda")));
        if (founddc != null)
        {
          if (typeof(DmpemdaControl).IsInstanceOfType(dc))
          {
            founddc.CopyPropertyBOTo(dc);
          }
          else
          {
            dc.SetValue("Kdpemda", founddc.Kdpemda);
            dc.SetValue("Nmpemda", founddc.Nmpemda);
          }
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Kdpemda", "Nmpemda" };
    }
    #endregion
    public DmpemdaLookupControl()
    {
      XMLName = ConstantTablesSys.XMLDMPEMDA;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (bo.GetProperty("FilterKdpemda")!=null)
      {
        Kdpemda = (string)bo.GetValue("FilterKdpemda");
      }
      else
      {
        Kdpemda = string.Empty;
      }
    }
    public new DataControlFieldCollection GetColumns()
    {
      if (columns == null)
      {
        columns = new DataControlFieldCollection
        {
          Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center)
        };
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdpemda"), typeof(string), 30, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Satker"), typeof(string), 10, HorizontalAlign.Center));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmpemda"), typeof(string), 50, HorizontalAlign.Left));
      }
      return columns;
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdpemda"), Width = 150, DataIndex = "Kdpemda", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmpemda"), Width = 300, DataIndex = "Nmpemda", Align = Ext.Net.TextAlign.Left });
    }
    public new IList View()
    {
      List<DmpemdaControl> list = (List<DmpemdaControl>)GetListDataSingleton();
      list = list.FindAll(o => o.Kdpemda.StartsWith(Kdpemda));
      return list;
    }
    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      DmpemdaLookupControl dclookup = new DmpemdaLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys = new String[] { "Kdpemda", "Nmpemda", "Kdpemda" };
      string[] targets = new String[] { "Kdpemda", "Nmpemda"};
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 20, 75, 0 }, targets)
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
    public ParameterRow GetLookupParameterRowFilter(IDataControl callerCtr, bool entry)
    {
      DmpemdaLookupControl dclookup = new DmpemdaLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys = new String[] { "Kdpemda", "Nmpemda", "Kdpemda" };
      string[] targets = new String[] { "Kdpemda", "Nmpemda", "Kdunit=ID", "Nmunit=ID", "Idunit=ID" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 20, 75, 0 }, targets)
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

    public ParameterRow GetLookupParameterRowFilter(IDataControl callerCtr)
    {
      ParameterRow par = (ParameterRow)GetLookupParameterRowFilter(callerCtr, false);
      par = par.SetAllowRefresh(true);
      bool enable = false; //(System.Configuration.ConfigurationManager.AppSettings["AllowMultiData"] == "1");
      par = par.SetEnable(enable);
      return par;
    }
    public string GetFieldValueMap()
    {
      return "Kdpemda=Nmpemda";
    }
  }
  #endregion DmpemdaLookup
}

