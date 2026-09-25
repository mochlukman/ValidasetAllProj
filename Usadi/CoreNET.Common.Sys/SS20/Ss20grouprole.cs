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
  #region Ss20grouprole
  [Serializable]
  public class Ss20grouproleControl : BaseDataControlSys, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Properties 
    public string Email { get; set; }
    public string Idrole { get; set; }
    public string Idst { get; set; }
    public string Kdrole { get; set; }
    public string Nama { get; set; }
    public string Nip { get; set; }
    public string Nipatasan { get; set; }
    public string Nmrole { get; set; }
    #endregion Properties 

    #region Methods 
    public Ss20grouproleControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPROLE;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Idst","Idrole" };
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
      return cViewListProperties;
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection();
       columns.Add(ExtFields.GetRatingField());
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdrole"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmrole"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nama"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nip"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Email"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nipatasan"), typeof(string), 10, HorizontalAlign.Left));
       columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stricon"), typeof(CommandColumn), 5, HorizontalAlign.Center));
      return columns;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
      }
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      //hpars.Add(TtdaftdokLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter));
      hpars.Add(new ParameterRowDate(this, ParameterRow.MODE_DATE_RANGE).SetEnable(enableFilter));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enableFilter));
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
      List<Ss20grouproleControl> ListData = new List<Ss20grouproleControl>();
      foreach(Ss20grouproleControl dc in list)
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
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Idst"), false, 70).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Idrole"), false, 70).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdrole"), false, 30).SetEnable(enable));
      hpars.Add(new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Nmrole"), false, 3).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nama"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nip"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Email"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nipatasan"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
        GetList(new DmlevelControl(this)), "Kdlevel=Nmlevel", 30).SetAllowRefresh(false).SetEnable(enable));
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
  #endregion Ss20grouprole
}

