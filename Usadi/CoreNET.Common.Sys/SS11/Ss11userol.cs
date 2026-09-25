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
  #region Ss11userol
  [Serializable]
  public class Ss11userolControl : Ss10userControl, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {

    #region Methods 
    public Ss11userolControl()
    {
      XMLName = ConstantTablesSys.XMLSS11USEROL;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Idol" };
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
      return cViewListProperties;
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection();
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Userid"), typeof(string), 20, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Usertype"), typeof(string), 10, HorizontalAlign.Center));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Usernama"), typeof(string), 30, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Userkey"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Usercomp"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Userip"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Userbrowser"), typeof(string), 10, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Userblock"), typeof(string), 10, HorizontalAlign.Center));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Usernip"), typeof(string), 20, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Userhp"), typeof(string), 20, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Useremail"), typeof(string), 35, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Useruraian"), typeof(string), 35, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stricon"), typeof(CommandColumn), 5, HorizontalAlign.Center));
      return columns;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Userid"), false, 30));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Usertype"),
        GetList(new Ss10usertypesControl()), "Usertype=Uturaian", 30).SetAllowRefresh(true).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Usernama"), true, 90).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Userkey"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Usercomp"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Userip"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Userbrowser"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowNumeric(this, ConstantDict.GetColumnTitle("Userblock"), true, 30).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Usernip"), true, 30));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Userhp"), true, 50));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Useremail"), true, 50));
      hpars.Add(new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Useruraian"), true, 3));
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle("Userstatus"),
        GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable));
      hpars.Add(new ParameterRowUploadFile(this, true));
      hpars.Add(new ParameterRowHelp(this, true));
      hpars.Add(new ParameterRowForum(this, true));
      return hpars;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
        Idapp = GlobalAsp.GetSessionApp();
        SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      }
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev());
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
      List<Ss10userControl> ListData = new List<Ss10userControl>();
      foreach (Ss10userControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
    #endregion Methods 
  }
  #endregion Ss11userol
}

