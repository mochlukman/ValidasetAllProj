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
  #region CoreNET.Common.BO.Ss20groupconfigControl, CoreNET.Common.BO
  [Serializable]
  public class Ss20groupconfigControl : BaseDataControlSys, IDataControlUIEntry, IHasJSScript, IExtLoadCsv
  {
    #region Properties 
    public string Kdgroup { get; set; }
    public string Par { get; set; }
    public string Value { get; set; }
    #endregion Properties 

    #region Methods 
    public Ss20groupconfigControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPCONFIG;
    }
    ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if(cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
      }
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Kdgroup","Par" };
      cViewListProperties.IDKey = "Par";//IDKey for ID Notes
      cViewListProperties.IDProperty = "Par";//UniqueKey in gridview
      cViewListProperties.ReadOnlyFields = new String[] {};//Key in GetFilters should put here
      cViewListProperties.SortFields = new String[] {"Par"};//
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
        //columns.Add(ExtFields.GetRatingField());
        //columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stricon"), typeof(CommandColumn), Cmds, 5, HorizontalAlign.Center));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdgroup"), typeof(string), 30, HorizontalAlign.Left).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Par"), typeof(string), 10, HorizontalAlign.Left).SetEditable(true));
        //columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Value"), typeof(string), 10, HorizontalAlign.Left).SetEditable(true));
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
      //hpars.Add(DmtahunLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter));
      //hpars.Add(new ParameterRowDate(this, ParameterRow.MODE_DATE_RANGE).SetEnable(enableFilter));
      //hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle("Status"),
      //    DmstatusLookupControl.GetListDataSingleton(string.Empty), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enableFilter));
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
      List<Ss20groupconfigControl> ListData = new List<Ss20groupconfigControl>();
      foreach(Ss20groupconfigControl dc in list)
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
        hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdgroup"), false, 30).SetEnable(enable));
        hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Par"), true, 95).SetEnable(enable));
        //hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Value"), true, 95).SetEnable(enable));
        //hpars.Add(DmtahunLookupControl.Instance.GetLookupParameterRow(this, true).SetEnable(enable));
        //hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
        //  DmlevelLookupControl.GetListDataSingleton(), "Kdlevel=Nmlevel", 30).SetAllowRefresh(false).SetEnable(enable));
        //hpars.Add(new ParameterRowType(this, true));
        hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          DmstatusLookupControl.GetListDataSingleton(string.Empty), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable));
        //hpars.Add(new ParameterRowCek(this, true));
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
      return new string[]{"Kdgroup","Par","Value","Kdlevel","Type"};
    }
    public new string[] GetCsvColumns(int mode)
    {
      return new string[]{"Kdgroup","Par","Value","Kdlevel","Type"};
    }
    public new string[] GetLoadCsvColumns()
    {
      return new string[]{"Kdgroup","Par","Value","Kdlevel","Type"};
    }
    #endregion Methods 
  }
  #endregion Ss20groupconfig
}

