using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgetkeysUIControl, CoreNET.Common.Sys
  [Serializable]
  public class SysgetkeysUIControl : SysgetkeysControl, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Methods 
    public SysgetkeysUIControl()
    {
      XMLName = ConstantTablesSys.XMLSYSGETKEYS;
    }
    public new void Insert()
    {
      base.Insert();
      SysgetkeysLookupControl.SetListDataNull();
    }
    public new int Update()
    {
      int n = base.Update(BaseDataControl.DEFAULT);
      SysgetkeysLookupControl.SetListDataNull();
      return n;
    }
    public new int Delete()
    {
      int n = ((BaseDataControlUI)this).Delete(BaseDataControl.DEFAULT);
      SysgetkeysLookupControl.SetListDataNull();
      return n;
    }
    public new IList View()
    {
      IList list = View(BaseDataControl.ALL);
      if ((list.Count == 0) && !string.IsNullOrEmpty(Typename))
      {
        InsertDefault(Typename);
      }
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<SysgetkeysControl> ListData = new List<SysgetkeysControl>();
      foreach (SysgetkeysControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
    public void InsertDefault(string typename)
    {
      IDataControlUIEntry ctrl = null;
      try
      {
        ctrl = (IDataControlUIEntry)UtilityBO.Create(typename);
      }
      catch (Exception) { }
      if (ctrl != null)
      {
        SysgetkeysUIControl dc = new SysgetkeysUIControl
        {
          Typename = Typename,
          Cols = ctrl.GetKeys().ToString(),
          Csvcols = ctrl.GetKeys().ToString(),
          Loadcsvcols = ctrl.GetKeys().ToString()
        };
        dc.Insert();
      }
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        if (!string.IsNullOrEmpty(GlobalAsp.GetRequestVal()))
        {
          Idmenu = GlobalAsp.GetRequestVal();
        }
        SsappmenuLookupControl.FindAndSetValuesInto(this);
      }
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev()) &&
        string.IsNullOrEmpty(GlobalAsp.GetRequestVal());

      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        Ss01appmenuLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Kode"),
          ParamControl.GetListOTypes(), "Kdpar=Nmpar", 30).SetAllowRefresh(true).SetEnable(true),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Typename"), true, 95).SetEnable(false)

    };
      return hpars;
    }

    public new void FilterClick(string key)
    {
      if (key.Equals("Kode"))
      {
        Ss00appmenuControl dc = new Ss00appmenuControl() { Idmenu = GlobalAsp.GetRequestVal() };
        dc = SsappmenuAllIdmenuLookupControl.FindAndSetValuesIntoByIdmenu(dc);
        switch (Kode)
        {
          case "1": Typename = dc.Olist1; break;
          case "11": Typename = dc.Olistdetil1; break;
          case "12": Typename = dc.Olistdetil2; break;
          case "13": Typename = dc.Olistdetil3; break;
          case "14": Typename = dc.Olistdetil4; break;
        }
      }
    }

    public override DataControlFieldCollection GetColumns()
    {
      columns = new DataControlFieldCollection
      {
        Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center)
      };
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Typename"), typeof(string), 40, HorizontalAlign.Left).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Cols"), typeof(string), 15, HorizontalAlign.Left).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Csvcols"), typeof(string), 15, HorizontalAlign.Left).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Loadcsvcols"), typeof(string), 15, HorizontalAlign.Left).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Loadconfig"), typeof(string), 15, HorizontalAlign.Left).SetEditable(true));
      return columns;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      hpars = new HashTableofParameterRow
      {
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Cols"), true, 3).SetEnable(enable),
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Csvcols"), true, 3).SetEnable(enable),
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Loadcsvcols"), true, 3).SetEnable(enable),
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Loadconfig"), true, 3).SetEnable(enable),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
        DmstatusLookupControl.GetListDataSingleton(), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable),
        //hpars.Add(new ParameterRowCek(this, true));
        new ParameterRowUploadFile(this, true),
        new ParameterRowHelp(this, true),
        new ParameterRowForum(this, true)
      };
      return hpars;
    }
    #endregion
  }
  #endregion CoreNET.Common.BO.SysgetkeysUIControl, CoreNET.Common.Sys
}

