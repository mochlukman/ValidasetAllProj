using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss20groupuserControl, CoreNET.Common.BO
  [Serializable]
  public class Ss20groupuserControl : BaseDataControlSys, IDataControlUIEntry, IHasJSScript, IExtLoadCsv
  {
    #region Properties 
    public string Kdgroup { get; set; }
    public string Nmgroup { get; set; }
    public string Usernip { get; set; }
    public string Usernama { get; set; }
    public string Userhp { get; set; }
    public string Useremail { get; set; }
    public string Useruraian { get; set; }
    #endregion Properties 

    #region Methods 
    public Ss20groupuserControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPUSER;
    }

    private ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if (cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
      }
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Userid", "Kdgroup" };
      cViewListProperties.IDKey = "Userid";//IDKey for ID Notes
      cViewListProperties.IDProperty = "Userid";//UniqueKey in gridview
      cViewListProperties.ReadOnlyFields = new String[] { "Kdgroup" };//Key in GetFilters should put here
      cViewListProperties.SortFields = new String[] { "Kdgroup" };//
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_DEL;
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_LOOKUP;
      cViewListProperties.LookupDC = "CoreNET.Common.BO.Ss20groupuserLookupForSelfControl, CoreNET.Common.Sys";
      cViewListProperties.LookupLabelQuery = BaseDataControl.LOOKUP_FOR_SELF;
      return cViewListProperties;
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection
      {
        Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Ext.Net.Icon), 5, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Userid"), typeof(string), 20, HorizontalAlign.Left),
        Fields.Create(ConstantDict.GetColumnTitle("Usernama"), typeof(string), 20, HorizontalAlign.Left),
        Fields.Create(ConstantDict.GetColumnTitle("Userhp"), typeof(string), 30, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Useremail"), typeof(string), 30, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Useruraian"), typeof(string), 50, HorizontalAlign.Left)
      };
      return columns;
    }
    public new void SetPageKey()
    {
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        Last_by = !string.IsNullOrEmpty(Last_by) ? Last_by : ((BaseBO)bo).Userid;
      }
      else if (typeof(Ss20groupControl).IsInstanceOfType(bo))
      {
        Kdgroup = ((Ss20groupControl)bo).Kdgroup;
        Nmgroup = ((Ss20groupControl)bo).Nmgroup;
      }
      else if (typeof(Ss20groupuserLookupForSelfControl).IsInstanceOfType(bo))
      {
        Userid = ((Ss20groupuserLookupForSelfControl)bo).Userid;
      }
    }
    public new void SetPrimaryKey()
    {
      Status = 0;
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        Ss20groupLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter).SetAllowEmpty(false)
      };
      return hpars;
    }
    public new IList View()
    {
      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<Ss20groupuserControl> ListData = new List<Ss20groupuserControl>();
      foreach (Ss20groupuserControl dc in list)
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
        hpars = new HashTableofParameterRow
        {
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Userid"), true, 95).SetEnable(enable),
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdgroup"), false, 30).SetEnable(enable),
          //hpars.Add(DmtahunLookupControl.Instance.GetLookupParameterRow(this, true).SetEnable(enable));
          //hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
          //  DmlevelLookupControl.GetListDataSingleton(), "Kdlevel=Nmlevel", 30).SetAllowRefresh(false).SetEnable(enable));
          //hpars.Add(new ParameterRowType(this, true));
          new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          DmstatusLookupControl.GetListDataSingleton(string.Empty), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable),
          //hpars.Add(new ParameterRowCek(this, true));
          new ParameterRowUploadFile(this, true),
          new ParameterRowHelp(this, true),
          new ParameterRowForum(this, true)
        };
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
      return new string[]{"Userid","Kdgroup", "Nmgroup"
        , "Usernip", "Userid", "Usernama","Userhp", "Useremail", "Useruraian"
        , "Status", "Statusicon", "Statusname", "Stricon", "Rating"
        };
    }
    public new string[] GetCsvColumns(int mode)
    {
      return new string[] { "Userid", "Kdgroup", "Kdlevel", "Type" };
    }
    public new string[] GetLoadCsvColumns()
    {
      return new string[] { "Userid", "Kdgroup", "Kdlevel", "Type" };
    }
    #endregion Methods 
  }
  #endregion Ss20groupuser
}

