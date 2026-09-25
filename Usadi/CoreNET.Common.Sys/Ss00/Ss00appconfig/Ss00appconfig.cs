using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss00appconfigControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appconfigControl : BaseDataControlSys, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Properties 
    public string Par { get; set; }
    public string Value { get; set; }
    #endregion Properties 
    #region Methods 
    public Ss00appconfigControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPCONFIG;
      //if (MasterAppConstants.Instance.StatusTesting)
      //{
      //  BaseDataAdapter.ExecuteCmd(ConnectionString, string.Format("exec RevalidateSS00AppConfig '{0}'", Idapp));
      //}
      //if (!string.IsNullOrEmpty(GlobalAsp.GetRequestApp()))
      //{
      //  Idapp = GlobalAsp.GetRequestApp();
      //  SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      //}

    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Idapp", "Par" };
      cViewListProperties.IDProperty = "Par";
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
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
          Idapp = GlobalAsp.GetRequestVal();
        }
        else
        {
          Idapp = GlobalAsp.GetSessionApp();
        }
        SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      }
      else if (typeof(IDataControlLookup).IsInstanceOfType(bo))
      {
        //Set dari Lookup
        Idapp = (string)bo.GetValue("Idapp");
        SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      }
    }

    public new void SetPageKey()
    {
      if ((!string.IsNullOrEmpty(Par)) && (Par.Equals("AppID")))
      {
        Value = Idapp;
      }
    }

    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev()) &&
        string.IsNullOrEmpty(GlobalAsp.GetRequestVal());
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        Ss00appLookupControl.Instance.GetLookupParameterRowDetil(this, false).SetEnable(enableFilter)
      };
      return hpars;
    }
    public new IList View()
    {
      string sql = $@"
        insert into SS00APPCONFIG (IDAPP,PAR,VALUE)
        select '{Idapp}' as IDAPP,PAR,DEFAULTVALUE 
        from SS00APPCONFIGPAR
        where PAR not in (select PAR from SS00APPCONFIG where IDAPP='{Idapp}')
      ";
      sql = string.Format(sql, XMLName, ID);
      BaseDataAdapter.ExecuteCmd(this, sql);

      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      string sql = string.Format(@"select * from SS00APPCONFIG where IDAPP='{0}'", Idapp);
      List<IDataControl> list = BaseDataAdapter.GetListDC(this, sql);
      List<Ss00appconfigControl> ListData = new List<Ss00appconfigControl>();
      foreach (Ss00appconfigControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }

    public new int Delete()
    {
      Status = -1;
      int n = ((BaseDataControlUI)this).Delete(BaseDataControl.DEFAULT);
      Ss00appconfigLookupControl.SetSessionListDataNull();
      return n;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Par"), false, 30).SetEnable(enable),
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Value"), true, 3).SetEnable(enable),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
        GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(true).SetEnable(enable),
        new ParameterRowUploadFile(this, true),
        new ParameterRowHelp(this, true),
        new ParameterRowForum(this, true)
      };
      return hpars;
    }
    #endregion Methods 
  }
  #endregion Ss00appconfig
}

