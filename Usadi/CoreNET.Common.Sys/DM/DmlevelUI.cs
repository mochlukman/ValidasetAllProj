using System;
using System.Text;
using System.Data;
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
  #region Dmlevel
  [Serializable]
  public class DmlevelUIControl : DmlevelControl, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    public DataControlFieldCollection columns = null;
    public void SetColumnsNull()
    {
      columns = null;
    }
    #region Methods 
    public DmlevelUIControl()
    {
      XMLName = ConstantTablesSys.XMLDMLEVEL;
      ConnectionString = SQLDataSource.Instance.CS_Config;
    }
    public string GetDefaultDebug()
    {
      return BaseDataControlUI.GetStaticDefaultDebug(this);
    }

    public IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = ViewListProperties.CreateDefaultProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Tablename", "Kdlevel" };
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
      return cViewListProperties;
    }
    public DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection();
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Tablename"), typeof(string), 20, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdlevel"), typeof(string), 20, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmlevel"), typeof(string), 50, HorizontalAlign.Left));
      return columns;
    }
    public DataControlFieldCollection GetColumnsFilter()
    {
      return GetColumns();
    }
    public void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
      }
    }
    public bool IsEditable()
    {
      return Editable;
    }
    public HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<DmlevelControl> ListData = new List<DmlevelControl>();
      foreach (DmlevelControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
    public HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Tablename"), true, 90).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdlevel"), true, 3).SetEnable(enable));
      hpars.Add(new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Nmlevel"), true, 3).SetEnable(enable));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
        GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable));
      hpars.Add(new ParameterRowCek(this, true));
      hpars.Add(new ParameterRowUploadFile(this, true));
      hpars.Add(new ParameterRowHelp(this, true));
      hpars.Add(new ParameterRowForum(this, true));
      return hpars;
    }
    public string[] GetScript()
    {
      string script = @"
        var prepareCommands = function (grid, commands, record, row) {
          GetCommandStatus(commands,record.get('Status'));
        };
        var prepareCommand = function (grid, command, record, row) 
        {
        };
        var prepareCellCommands = function (grid, commands, record, row, col, value) 
        {
        };
        var prepareCellCommand = function (grid, command, record, row, col, value)  
        {
          command.command = command.command.replace('Detil','Link');
        };
      ";
      return new string[] { script };
    }
    #endregion Methods 
    #region IExtLoadCsv
    public string[] GetLoadCsvColumns()
    {
      string[] csvcolumns = SysgetkeysLookupControl.GetLoadCSVCols(this);
      return csvcolumns;
    }

    public string[] GetCsvColumns(int mode)
    {
      string[] csvcolumns = SysgetkeysLookupControl.GetCSVCols(this);
      return csvcolumns;
    }

    public IList GetList(int id, int mode)
    {
      List<SysgetrowsControl> list = (List<SysgetrowsControl>)GlobalAsp.GetSessionListRows();
      return list;
    }
    public Window GetLoadCsvWindow()
    {
      return new WindowLoadCSV("0", "Load Menu", string.Format("~/Page/CSVUpload.aspx"), 400, 150);
    }
    #endregion Methods 
  }
  #endregion Dmlevel
}

