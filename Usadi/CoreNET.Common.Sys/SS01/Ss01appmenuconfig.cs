using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuconfig
  [Serializable]
  public class Ss01appmenuconfigControl : BaseDataControlSys, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Properties 
    public string IdmenuPar => Idmenu + Par;
    public string Idmenu { get; set; }
    public string Kdmenu { get; set; }
    public string Nmmenu { get; set; }
    public string Par { get; set; }
    public string Value { get; set; }
    #endregion Properties 
    #region Methods 
    public Ss01appmenuconfigControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENUCONFIG;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Idmenu", "Par" };
      cViewListProperties.IDProperty = "Par";
      cViewListProperties.IDKey = "IdmenuPar";
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL_LOADCSV;
      return cViewListProperties;
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection
      {
        Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Par"), typeof(string), 20, HorizontalAlign.Left),
        Fields.Create(ConstantDict.GetColumnTitle("Value"), typeof(string), 70, HorizontalAlign.Left).SetEditable(true)
      };
      return columns;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        if (!string.IsNullOrEmpty(GlobalAsp.GetRequestVal()))
        {
          Idmenu = GlobalAsp.GetRequestVal();
        }
        SsappmenuLookupControl.FindAndSetValuesInto(this);
      }

    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev())&&
        string.IsNullOrEmpty(GlobalAsp.GetRequestVal());
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        Ss01appmenuLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter)
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
      List<Ss01appmenuconfigControl> ListData = new List<Ss01appmenuconfigControl>();
      foreach (Ss01appmenuconfigControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Par"), false, 90).SetEnable(enable));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Value"), false, 90).SetEnable(enable));
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
  #endregion Ss01appmenuconfig
}

