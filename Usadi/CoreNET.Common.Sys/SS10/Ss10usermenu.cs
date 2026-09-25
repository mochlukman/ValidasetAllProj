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
  #region CoreNET.Common.BO.Ss10usermenuControl, CoreNET.Common.BO
  [Serializable]
  public class Ss10usermenuControl : BaseDataControlSys, IDataControlUIEntry, IHasJSScript, IExtLoadCsv
  {
    #region Properties 
    public string Idmenu { get; set; }
    public bool Stdelete { get; set; }
    public bool Stfilter { get; set; }
    public bool Stinsert { get; set; }
    public bool Stload { get; set; }
    public bool Stupdate { get; set; }
    #endregion Properties 

    #region Methods 
    public Ss10usermenuControl()
    {
      XMLName = ConstantTablesSys.XMLSS10USERMENU;
    }
    ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if(cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
      }
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Userid","Idmenu" };
      cViewListProperties.IDKey = "Idmenu";//IDKey for ID Notes
      cViewListProperties.IDProperty = "Idmenu";//UniqueKey in gridview
      cViewListProperties.ReadOnlyFields = new String[] {};//Key in GetFilters should put here
      cViewListProperties.SortFields = new String[] {"Idmenu"};//
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_DEL;
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      //cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_LOOKUP;
      //cViewListProperties.LookupDC = "CoreNET.Common.BO.DmstatusLookupControl, CoreNET.Common.Sys";
      //cViewListProperties.LookupLabelQuery = BaseDataControl.LOOKUP;
      return cViewListProperties;
    }
    public override DataControlFieldCollection GetColumns()
    {
      if (columns == null)
      {
        columns = new DataControlFieldCollection
        {
          Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center)
        };
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdmenu"), typeof(string), 15, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmmenu"), typeof(string), 30, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stinsert"), typeof(bool), 10, HorizontalAlign.Left).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stupdate"), typeof(bool), 10, HorizontalAlign.Left).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stdelete"), typeof(bool), 10, HorizontalAlign.Left).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stload"), typeof(bool), 10, HorizontalAlign.Left).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stfilter"), typeof(bool), 10, HorizontalAlign.Left).SetEditable(true));
      }
      return columns;
    }
    public new void SetPageKey()
    {
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        //This Is Sample.For more than 2 filter, must checking if property have had value
        Last_by = !string.IsNullOrEmpty(Last_by) ? Last_by : ((BaseBO)bo).Userid;
      }
    }
    public new void SetPrimaryKey()
    {
      //Id = Guid.NewGuid().ToString();
      //string sql = string.Format("select top 1 NOTR from TR order by NOTR desc");
      //string[] fields = new string[] { "Notr" };
      //int length = 5;
      //string prefix = string.Empty;
      //string sufix = string.Empty;
      //UtilityUI.GetNoUrut(this, sql, fields, length, prefix, sufix);
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(Ss10userLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter));
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
      List<Ss10usermenuControl> ListData = new List<Ss10usermenuControl>();
      foreach(Ss10usermenuControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      //Update(ListData);
      return ListData;
    }
    //Unuk ParameterLookup2, pastikan parameter entry is true
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      if (hpars == null)
      {
        hpars = new HashTableofParameterRow();
        hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdmenu"), false, 30).SetEnable(enable));
        hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nmmenu"), false, 100).SetEnable(enable));
        //hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Stinsert"), true, 95).SetEnable(enable));
        //hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Stupdate"), true, 95).SetEnable(enable));
        //hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Stdelete"), true, 95).SetEnable(enable));
        //hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Stload"), true, 95).SetEnable(enable));
        //hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Stfilter"), true, 95).SetEnable(enable));
        hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          DmstatusLookupControl.GetListDataSingleton(string.Empty), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable));
        hpars.Add(new ParameterRowUploadFile(this, true));
        hpars.Add(new ParameterRowHelp(this, true));
        hpars.Add(new ParameterRowForum(this, true));
      }
      return hpars;
    }
    public new string[] GetScript()
    {
      string script = @"
        var ShowHidden = function (type) {
          if (type=='D') {
            return 'visible';/**/
          }else{
            return 'hidden';/**/
          }
        };
        var prepareNchildstrCommand = function(grid, command, record, row, col, value)
        {
          command.command = command.command.replace('Detil', 'Link');
        };
      ";
      return new string[] { script };
    }
    public new string[] GetKeys()
    {
      return new string[]{"Userid","Idmenu","Stinsert","Stupdate","Stdelete","Stload","Stfilter","Kdlevel","Type"};
    }
    public new string[] GetCsvColumns(int mode)
    {
      return new string[]{"Userid","Idmenu","Stinsert","Stupdate","Stdelete","Stload","Stfilter","Kdlevel","Type"};
    }
    public new string[] GetLoadCsvColumns()
    {
      return new string[]{"Userid","Idmenu","Stinsert","Stupdate","Stdelete","Stload","Stfilter","Kdlevel","Type"};
    }
    #endregion Methods 
  }
  #endregion Ss10usermenu
}

