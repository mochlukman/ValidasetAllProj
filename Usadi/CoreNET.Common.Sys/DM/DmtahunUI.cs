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
  #region Dmtahun
  [Serializable]
  public class DmtahunUIControl : DmtahunControl, IDataControlUIEntry, IHasJSScript
  {
    public DataControlFieldCollection columns = null;
    public void SetColumnsNull()
    {
      columns = null;
    }
    #region Methods 
    public DmtahunUIControl()
    {
      XMLName = ConstantTablesSys.XMLDMTAHUN;
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
      cViewListProperties.PrimaryKeys = new String[] { "Kdtahun" };
      cViewListProperties.IDKey = "Kdtahun";
      cViewListProperties.IDProperty = "Kdtahun";
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
      return cViewListProperties;
    }
    public bool IsEditable()
    {
      return Editable;
    }
    public DataControlFieldCollection GetColumns()
    {
      columns = new DataControlFieldCollection();
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdtahun"), typeof(int), 15, HorizontalAlign.Center));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmtahun"), typeof(string), 30, HorizontalAlign.Left));
      return columns;
    }
    public DataControlFieldCollection GetColumnsFilter()
    {
      return GetColumns();
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
      }
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
      int year = DateTime.Now.Year;
      IList list = ((BaseDataControl)this).View(label);
      List<DmtahunControl> ListData = new List<DmtahunControl>();
      foreach (DmtahunControl dc in list)
      {
        if (((dc.Kdtahun > (year - 4)) && (dc.Kdtahun < (year + 1))) || (dc.Kdtahun == 0))
        {
          DmstatusLookupControl.FindAndSetValuesInto(dc);
          ListData.Add(dc);
        }
      }
      return ListData;
    }
    public HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowNumeric(this, ConstantDict.GetColumnTitle("Kdtahun"), false, 10).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nmtahun"), true, 95).SetEnable(enable));
      //hpars.Add(new ParameterRowHelp(this, true));
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
  }
  #endregion Dmtahun
}

