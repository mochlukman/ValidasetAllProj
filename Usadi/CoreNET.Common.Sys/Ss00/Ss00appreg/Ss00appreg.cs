using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region Ss00appreg
  [Serializable]
  public class Ss00appregControl : BaseDataControlSys, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Properties 
    public string Clalamat { get; set; }
    public string Clcomp { get; set; }
    public string Clip { get; set; }
    public string Csdb { get; set; }
    public string Csid { get; set; }
    public string Cskey { get; set; }
    public string Cspwd { get; set; }
    public string Csserver { get; set; }
    public string Csuser { get; set; }
    public string Email { get; set; }
    public new string Idapp { get; set; }
    public string Idreg { get; set; }
    public string Nama { get; set; }
    public string Nohp { get; set; }
    public string Nowa { get; set; }
    #endregion Properties 
    #region Methods 
    public Ss00appregControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPREG;
      //if (!SQLDataSource.CekAllowedServer())
      //{
      //  throw new Exception("Invalid License Server!!!");
      //}
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Idreg" };
      cViewListProperties.IDProperty = "Idreg";
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      if (MasterAppConstants.Instance.StatusAdmin)
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT;
      }
      else
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD;
      }
      return cViewListProperties;
    }
    public new void SetPrimaryKey()
    {
      Idreg = Guid.NewGuid().ToString();
      Csserver = SQLDataSource.GetSQLInstance();
      if (GlobalAsp.GetSessionApp().Equals(MasterAppConstants.ID_APP_PROGRAMMER))
      {
        if (Csserver.Equals("db2017"))
        {
          Csserver = "sql.usadi.id,14317";
        }
      }
      Csdb = SQLDataSource.GetOpDB(ConnectionString);
      Csuser = SQLDataSource.GetUserDB();
      Cspwd = SQLDataSource.GetPwdDB();
    }

    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)GlobalAsp.GetSessionUser()).GetUserID();
      }
    }
    public override DataControlFieldCollection GetColumns()
    {
      if (columns == null)
      {
        columns = new DataControlFieldCollection { ExtFields.GetStatusField() };
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Last_date"), typeof(DateTime), 13, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Last_by"), typeof(string), 20, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Clcomp"), typeof(string), 20, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Clip"), typeof(string), 15, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Csid"), typeof(string), 20, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Csserver"), typeof(string), 20, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Csdb"), typeof(string), 20, HorizontalAlign.Left));
      }
      return columns;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      if (hpars == null)
      {
        hpars = new HashTableofParameterRow
        {
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Csid"), true, 95).SetEnable(enable)
        };
        //if (MasterAppConstants.Instance.StatusAdmin && GlobalAsp.GetSessionApp().Equals(MasterAppConstants.Instance.MasterAppID))
        if (MasterAppConstants.Instance.StatusAdmin &&
          (GlobalAsp.GetSessionApp().Equals(MasterAppConstants.Instance.MasterAppID) ||
          GlobalAsp.GetSessionApp().Equals(MasterAppConstants.ID_APP_PROGRAMMER)))
        {
          hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Csserver"), true, 95).SetEnable(enable));
          hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Csdb"), true, 50).SetEnable(enable));
          hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Csuser"), true, 50).SetEnable(enable));
          hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Cspwd"), true, 50).SetEnable(enable));
        }
        hpars.Add(new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Cskey"), true, 3).SetEnable(enable));
        hpars.Add(new ParameterRowUploadFile(this, true));
        hpars.Add(new ParameterRowHelp(this, true));
        hpars.Add(new ParameterRowForum(this, true));
      }
      return hpars;
    }
    public new void Insert()
    {
      if (IsValid())
      {
        string cs_template = @"data source={0};Initial Catalog={1};user id={2};password={3};Asynchronous Processing=true";
        string cs = string.Format(cs_template, Csserver, Csdb, Csuser, Cspwd);
        Cskey = GlobalAsp.EncryptCSPublished(cs, Csid.Trim());
        base.Insert(BaseDataControl.DEFAULT);
      }
      else
      {
        throw new Exception(ConstantDict.Translate("LBL_INVALID_INSERT"));
      }
    }

    private bool IsValid()
    {
      bool valid = true;
      Csid = Csid.Trim();
      if ((Csid.Length != 16) || !GlobalAsp.IsBase64(Csid))
      {
        throw new Exception(ConstantDict.Translate("LBL_INVALID_CHALLENGE_KEY"));
      }
      Clcomp = ClientCompName;
      Clip = ClientCompIP;
      Idapp = GlobalAsp.GetSessionApp();
      return valid;
    }

    public new int Update()
    {
      if (IsValid())
      {
        string cs_template = @"data source={0};Initial Catalog={1};user id={2};password={3};Asynchronous Processing=true";
        string cs = string.Format(cs_template, Csserver, Csdb, Csuser, Cspwd);
        Cskey = GlobalAsp.EncryptCSPublished(cs, Csid);
        int n = base.Update(BaseDataControl.DEFAULT);
        return n;
      }
      else
      {
        throw new Exception(ConstantDict.Translate("LBL_INVALID_INSERT"));
      }
    }
    public new IList View()
    {
      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<Ss00appregControl> ListData = new List<Ss00appregControl>();
      foreach (Ss00appregControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
    #endregion Methods 
  }
  #endregion Ss00appreg
}

