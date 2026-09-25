using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss01appmenuMonControl, CoreNET.Common.Sys
  /**
   * 
   * 
   * 
   * */
  [Serializable]
  public class Ss01appmenuMonControl : Ss01appmenuRegControl, IDataControlTreeGrid3, IHasJSScript
  {
    public Ss01appmenuMonControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }

    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      if (UtilityUI.GetModePage() == UtilityUI.PAGE_TABULAR)
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
      }
      else
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      }
      return cViewListProperties;
    }

    public new void Insert()
    {
      if (IsValid())
      {
        base.Insert();
        UpdateProgress();
      }
      else
      {
        throw new Exception(ConstantDict.Translate("LBL_INVALID_INSERT"));
      }
    }

    public new int Update()
    {
      int n = 0;
      if (IsValid())
      {
        n = base.Update();
        UpdateProgress();
      }
      else
      {
        throw new Exception(ConstantDict.Translate("LBL_INVALID_UPDATE"));
      }
      return n;
    }

    private void UpdateProgress()
    {
      try
      {
        Ss01appmenuconfigControl dc = new Ss01appmenuconfigControl
        {
          Idmenu = Idmenu,
          Par = "PROGRESS",
          Value = Progressstr,
          Status=0
        };
        dc.Delete();
        dc.Insert();
      }
      catch (Exception ex)
      {
        UtilityBO.Log(ex);
      }
    }

    private bool IsValid()
    {
      bool valid = true;
      return valid;
    }
    public new IList View()
    {
      IList list = null;
      list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<Ss01appmenuControl> ListData = new List<Ss01appmenuControl>();
      foreach (Ss01appmenuControl dc in list)
      {
        if (MasterAppConstants.Instance.StatusTesting || (dc.Status > -1))//Status==-1 hidden
        {
          string nmmenu = ConstantDict.Translate("Menu" + dc.Kdapp + dc.Kdmenu + "=" + dc.Nmmenu);
          dc.Nmmenu = string.IsNullOrEmpty(nmmenu) ? dc.Nmmenu : nmmenu;
          DmstatusLookupControl.FindAndSetValuesInto(dc);
          if (MasterAppConstants.Instance.StatusTesting)//Normalisasi Kdlevel
          {
            dc.Kdlevel = dc.Kdmenu.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length;
            dc.Update("Kdlevel");
          }
          dc.Progress = Ss01appmenuconfigLookupControl.GetProgress(dc.Idmenu);
          ListData.Add(dc);
        }
      }
      Update(ListData);
      return ListData;
    }

    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdmenu"), Width = 150, DataIndex = "Kdmenu", Align = Ext.Net.TextAlign.Left });
      if (GetModePreviewIndex() == 1)
      {
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kddok"), Width = 100, DataIndex = "Kddok", Align = Ext.Net.TextAlign.Center });
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Idxdok"), Width = 100, DataIndex = "Idxdok", Align = Ext.Net.TextAlign.Center });
      }
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmmenu"), Width = 350, DataIndex = "Nmmenu", Align = Ext.Net.TextAlign.Left });
      if (GetModePreviewIndex() == 1)
      {
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("UrlFull"), Width = 200, DataIndex = "UrlFull", Align = Ext.Net.TextAlign.Left });
      }
      Columns.Add(TreeColStatus);
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Progress"), Width = 70, DataIndex = "Progressstr", Align = Ext.Net.TextAlign.Center });
      Columns.Add(TreeColProgress);
      if (GetModePreviewIndex() == 1)
      {
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdlevel"), Width = 50, DataIndex = "Kdlevel", Align = Ext.Net.TextAlign.Center });
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Type"), Width = 50, DataIndex = "Type", Align = Ext.Net.TextAlign.Center });
      }
      #region Command
      //TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 250, Align = Ext.Net.TextAlign.Center };
      //col1.XTemplate.Html = @"<a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""URL"",""URL"", ""{[GetURLEditor(values.Idmenu)]}"");'>URL</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Link"",""Link"", ""{[GetURLMapClass(values.Idmenu)]}"");'>Link</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Entry"",""Entry"", ""{[GetURLEntry(values.Idmenu)]}"");'>Entry</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Grid"",""Grid"", ""{[GetURLColGrid(values.Idmenu)]}"");'>Grid</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Tree"",""Tree"", ""{[GetURLColTree(values.Idmenu)]}"");'>Tree</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Filter"",""Filter"", ""{[GetURLFilter(values.Idmenu)]}"");'>Filter</a> | <a style='visibility:true;'
      //  onclick = 'parent.loadPageByID(""Pages"",""Csv"",""Csv"", ""{[GetURLCsv(values.Idmenu)]}"");'>CSV</a>
      //";
      //Columns.Add(col1);
      #endregion
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection
      {
        Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Kdmenu"), typeof(string), EditCmd, 15, HorizontalAlign.Left).SetEditable(true),
        Fields.Create(ConstantDict.GetColumnTitle("Nmmenu"), typeof(string), 40, HorizontalAlign.Left).SetEditable(true),
        Fields.Create(ConstantDict.GetColumnTitle("Progress"), typeof(decimal), 15, HorizontalAlign.Center).SetEditable(true),
      };
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Url"), typeof(string), 45, HorizontalAlign.Left).SetEditable(true));
      if (ModePreviewIndex == 1)
      {
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdlevel"), typeof(int), 7, HorizontalAlign.Center).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Type"), typeof(string), 7, HorizontalAlign.Center).SetEditable(true));
      }
      return columns;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdmenu"), true, 30).SetEnable(enable),
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Nmmenu"), true, 3).SetEnable(enable),
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Url"), true, 3).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Progressstr"), true, 50).SetEnable(enable),
        new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
        GetList(new DmlevelControl(this)), "Kdlevel=Nmlevel", 30).SetEnable(enable),
        new ParameterRowType(this, true),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
        GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(true).SetEnable(enable),
        new ParameterRowCek(this, true),
        new ParameterRowUploadFile(this, true),
        new ParameterRowHelp(this, true),
        new ParameterRowForum(this, true)
      };
      return hpars;
    }
  }
  #endregion Ss01appmenuMon
}

