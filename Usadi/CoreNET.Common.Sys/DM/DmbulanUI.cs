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
  #region Dmbulan
  [Serializable]
  public class DmbulanUIControl : DmbulanControl, IDataControlUIEntry, IHasJSScript
  {
    public DataControlFieldCollection columns = null;
    public void SetColumnsNull()
    {
      columns = null;
    }
    #region Methods 
    public DmbulanUIControl()
    {
      XMLName = ConstantTablesSys.XMLDMBULAN;
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
      cViewListProperties.PrimaryKeys = new String[] { "Bulan" };
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
      return cViewListProperties;
    }
    public DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection();
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdbulan"), typeof(string), 20, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmbulan"), typeof(string), 50, HorizontalAlign.Left));
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
      IList list = ((BaseDataControl)this).View(label);
      List<DmbulanControl> ListData = new List<DmbulanControl>();
      foreach (DmbulanControl dc in list)
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
      hpars.Add(new ParameterRowNumeric(this, ConstantDict.GetColumnTitle("Bulan"), true, 30).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdbulan"), true, 30).SetEnable(enable));
      hpars.Add(new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Nmbulan"), true, 3).SetEnable(enable));
      return hpars;
    }
    public string[] GetScript()
    {
      string script = @"
        var ShowHidden = function (type) {
          if (type=='D') {
            return 'visible';/**/
          }else{
            return 'hidden';/**/
          }
        };
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

    public bool IsEditable()
    {
      return Editable;
    }
    #endregion Methods 
  }
  #endregion Dmbulan
}

