using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgetcolsUIControl, CoreNET.Common.Sys
  [Serializable]
  public class SysgetcolsUIControl : SysgetcolsControl, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Methods 
    public SysgetcolsUIControl()
    {
      XMLName = ConstantTablesSys.XMLSYSGETCOLS;
    }
    public new void Insert()
    {
      base.Insert();
      SysgetcolsLookupControl.SetListDataNull();
    }
    public new int Update()
    {
      SysgetcolsControl dc = (SysgetcolsControl)GlobalExt.GetEditingObject();
      Colnameprev = dc.Colname;
      int n = base.Update(BaseDataControl.DEFAULT);
      SysgetcolsLookupControl.SetListDataNull();
      return n;
    }
    public new int Delete()
    {
      int n = ((BaseDataControlUI)this).Delete(BaseDataControl.DEFAULT);
      SysgetcolsLookupControl.SetListDataNull();
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
      List<SysgetcolsControl> ListData = new List<SysgetcolsControl>();
      foreach (SysgetcolsControl dc in list)
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
        DataControlFieldCollection hpars = ctrl.GetColumns();
        if (hpars.Count > 0)
        {
          try
          {
            foreach (MyBoundField field in hpars)
            {
              SysgetcolsUIControl dc = new SysgetcolsUIControl
              {
                Typename = Typename,
                Colname = field.DataField,
                Coltype = field.FooterText,
                Colwidth = (short)field.ItemStyle.Width.Value,
                Colalign = (field.ItemStyle.HorizontalAlign == HorizontalAlign.Left ? "left" :
                  field.ItemStyle.HorizontalAlign == HorizontalAlign.Right ? "right" : "center")
              };
              dc.Insert();
            }
          }
          catch (Exception ex)
          {
            UtilityBO.Log(ctrl, ex);
          }
        }
        else
        {
          ctrl = (IDataControlUIEntry)UtilityBO.Create(typename);
          BaseDataAdapter.GetListDC(ctrl, string.Format("InsertDefaultcols '{0}','{1}'",
            Typename, ((BaseBO)ctrl).XMLName, 0));
        }
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
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Typename"), typeof(string), 45, HorizontalAlign.Left).SetEditable(false));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nourut"), typeof(short), 7, HorizontalAlign.Center).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Colname"), typeof(string), 25, HorizontalAlign.Left).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Coltype"), typeof(string), 10, HorizontalAlign.Center).SetEditable(true)
        .SetAvailableValues(ParamControl.GetListTypes(), new string[] { "Kdpar", "Nmpar" }));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Colwidth"), typeof(short), 10, HorizontalAlign.Center).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Colalign"), typeof(bool), 10, HorizontalAlign.Center).SetEditable(true)
        .SetAvailableValues(ParamControl.GetListAlign(), new string[] { "Kdpar", "Nmpar" }));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Coleditable"), typeof(bool), 10, HorizontalAlign.Center).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Colavaillist"), typeof(string), 25, HorizontalAlign.Left).SetEditable(true));
      return columns;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      hpars = new HashTableofParameterRow
      {
        //new ParameterRowNumeric(this, ConstantDict.GetColumnTitle("Nourut"), true, 30).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Colname"), true, 95).SetEnable(enable),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Coltype"),
          ParamControl.GetListTypes(), "Kdpar=Nmpar", 30).SetAllowRefresh(false).SetEnable(enable),
        new ParameterRowNumeric(this, ConstantDict.GetColumnTitle("Colwidth"), true, 30).SetEnable(enable),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Colalign"),
          ParamControl.GetListAlign(), "Kdpar=Nmpar", 30).SetAllowRefresh(false).SetEnable(enable),
        new ParameterRowCek(this, ConstantDict.GetColumnTitle("Coleditable"), true).SetEnable(enable),
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Colavaillist"), true, 3).SetEnable(enable),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          DmstatusLookupControl.GetListDataSingleton(), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable),
        new ParameterRowUploadFile(this, true),
        new ParameterRowHelp(this, true),
        new ParameterRowForum(this, true)
      };
      return hpars;
    }
    #endregion
  }
  #endregion CoreNET.Common.BO.SysgetcolsUIControl, CoreNET.Common.Sys
}

