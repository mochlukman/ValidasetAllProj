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
  #region Dmdok
  [Serializable]
  public class DmdokControl : BaseDataControlSys, IExtLoadCsv
  {
    #region Properties

    public string Kddok { get; set; }
    public string Nmdok { get; set; }
    public string Lbldok { get; set; }
    public string Kdpersppkd { get; set; }
    public string Kdpersskpd { get; set; }
    #endregion Properties
    #region Methods
    public DmdokControl()
    {
      XMLName = ConstantTablesSys.XMLDMDOK;
      ConnectionString = SQLDataSource.Instance.CS_Config;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Kddok" };
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
      return cViewListProperties;
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection();
      columns.Add(ExtFields.GetRatingField());
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kddok"), typeof(string), 20, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmdok"), typeof(string), 65, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Lbldok"), typeof(string), 10, HorizontalAlign.Center));
      return columns;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
      }
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<DmdokControl> ListData = new List<DmdokControl>();
      foreach (DmdokControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = false;
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kddok"), false, 30).SetEnable(enable));
      hpars.Add(new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Nmdok"), false, 3).SetEnable(enable));
      hpars.Add(new ParameterRowNumeric(this, ConstantDict.GetColumnTitleEntry("Status"), false, 30).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdlevel"), false, 30).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Type"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
        GetList(new DmdokControl()), "Kdlevel=Nmlevel", 30).SetAllowRefresh(false).SetEnable(enable));
      hpars.Add(new ParameterRowType(this, true));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
        GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable));
      hpars.Add(new ParameterRowCek(this, true));
      hpars.Add(new ParameterRowUploadFile(this, true));
      hpars.Add(new ParameterRowHelp(this, true));
      hpars.Add(new ParameterRowForum(this, true));
      return hpars;
    }
    #endregion Methods
  }
  #endregion Dmdok
}

