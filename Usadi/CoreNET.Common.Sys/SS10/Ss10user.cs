using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
    #region Ss10user
    [Serializable]
    public class Ss10userControl : BaseDataControlSys, IDataControlAppuserUI, IExtLoadCsv, IHasJSScript
    {
        #region Constant
        public const string USER_LEVEL_1 = "1";
        public const string USER_LEVEL_2 = "2";
        public const string USER_LEVEL_3 = "3";
        public const string USER_LEVEL_4 = "4";
        public const string USER_LEVEL_5 = "5";
        #endregion
        #region Properties Ss10user
        public string Nip { get; set; }
        public string Userblock { get; set; }
        public string Userket { get; set; }
        public string Usernama { get; set; }
        public string Usernip { get; set; }
        public int Userno { get; set; }
        public string Userpwd { get; set; }
        public int Userstatus { get; set; }
        public string Usertype { get; set; }
        public string Userhp { get; set; }
        public string Useremail { get; set; }
        public string Useruraian { get; set; }
        public string Uturaian { get; set; }
        #endregion
        #region Properties Ss10userapp
        private string _UseridForEntry = string.Empty;
        public string UseridForEntry
        {
            get => string.IsNullOrEmpty(_UseridForEntry) ? Userid : _UseridForEntry;
            set => _UseridForEntry = value;
        }
        public string Idrole { get; set; }
        public string Nmrole { get; set; }
        #endregion Properties Ss10userapp 
        #region Ss10userLoginControl
        public string Nodok { get; set; }//Not DB Field
        public string Uraian { get; set; }//Not DB Field
        public string DefaultNodok { get; set; }//Not DB Field
        public string DefaultUraian { get; set; }//Not DB Field
        public string DefaultKdunit { get; set; }//Not DB Field
        public string DefaultNmunit { get; set; }//Not DB Field
        public string Kdpemda { get; set; }//Not DB Field
        public string Nmpemda { get; set; }//Not DB Field
        public string Idunit { get; set; }//Not DB Field
        public string Unitkey { get; set; }//Not DB Field
        public string Kdunit { get; set; }//Not DB Field
        public string Nmunit { get; set; }//Not DB Field
        public string Kdgroup { get; set; }//Not DB Field
        public int ModeEditable { get; set; }//Not DB Field
        #endregion Properties
        #region Properties Ss11userol
        public string Idol { get; set; }
        public string Userbrowser { get; set; }
        public string Usercomp { get; set; }
        public string Userip { get; set; }
        public string Userkey { get; set; }
        #endregion Properties 
        public ImageCommand[] Cmds
        {
            get
            {
                ImageCommand cmd1 = new ImageCommand()
                {
                    CommandName = "ViewApp",
                    Icon = Ext.Net.Icon.TableGear,
                    ToolTip =
          {
            Text = "Klik tombol ini untuk menampilkan daftar modul"
          }
                };

                return new ImageCommand[] { cmd1 };
            }
        }
        public string ViewApp
        {
            get
            {
                string app = GlobalAsp.GetRequestApp();
                string id = GlobalAsp.GetRequestId();
                string idprev = GlobalAsp.GetRequestId();
                string kode = GlobalAsp.GetRequestKode();
                string idx = GlobalAsp.GetRequestIndex();
                string strenable = "&enable=" + ((Status == 0) ? 1 : 0);
                string url = string.Format("PageTabular.aspx?passdc=1&i=11&app={0}&id={1}&idprev={2}&kode={3}&idx={4}"
                  + strenable, app, id, idprev, kode, idx);
                return "Daftar Modul:" + url;
            }
        }
        #region Methods
        public Ss10userControl()
        {
            XMLName = ConstantTablesSys.XMLSS10USER;
            Tahun = 0;
        }
        public new IProperties GetProperties()
        {
            ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
            cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
            cViewListProperties.PrimaryKeys = new String[] { "Userid" };
            cViewListProperties.IDKey = "Userid";
            cViewListProperties.IDProperty = "Userid";
            cViewListProperties.ReadOnlyFields = new String[] { "Kdpemda" };
            cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
            if (MasterAppConstants.Instance.StatusAdmin)
            {
                cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_DEL;
            }
            else
            {
                cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
            }
            return cViewListProperties;
        }

        public void InsertDefaultApp()
        {
            try
            {
                string sql = $@"exec InsertDefaultUserApp '{Userid}'";
                BaseDataAdapter.ExecuteCmd(this, sql);

            }
            catch (Exception) { }
        }

        public new void SetPageKey()
        {
            //!SysUtils.GetEnableFilter() && 
            if (GlobalExt.GetSessionGroup() != null)
            {
                if (!string.IsNullOrEmpty(GlobalExt.GetSessionGroup().GetGroupID()))
                {
                    DefaultNodok = SysUtils.GetDefaultNodok();
                }
            }
        }

        public new void SetPrimaryKey()
        {
            Userblock = "0";
        }

        public new void SetFilterKey(BaseBO bo)
        {
            if (typeof(IDataControlMenu).IsInstanceOfType(bo) && !GlobalAsp.GetSessionApp().Equals(MasterAppConstants.Instance.MasterAppID))
            {
                Kdpemda = !string.IsNullOrEmpty(Kdpemda) ? Kdpemda : ((string)GlobalAsp.GetSessionUser().GetValue("Kdpemda"));
                Nmpemda = !string.IsNullOrEmpty(Nmpemda) ? Nmpemda : ((string)GlobalAsp.GetSessionUser().GetValue("Nmpemda"));
            }
        }
        public void UpdatePwdReg(string pwd)
        {
            if (System.Web.HttpContext.Current.Request.Url.OriginalString.Contains("Register"))
            {
                Userpwd = UtilityBO.GetMD5HexStringForStorePwd(pwd);//Baru diencrypt 
                Userket = GlobalAsp.GetHashData(this);
                Update("Pwd");
            }
        }
        public void UpdatePwd(string chipper)
        {
            Load();
            Userpwd = chipper;//Sdh diencrypt GlobalAsp.En(pwd);
            Userket = GlobalAsp.GetHashData(this);
            Update("Pwd");
        }
        public new void Insert()
        {
            if (IsValid())
            {
                if (!Usertype.Equals(GlobalAsp.USER_ADMIN))
                {
                    Useruraian = UtilityBO.GetRandomPassword(Userid);
                    //Userpwd = GlobalAsp.En(Useruraian);
                    base.Insert();
                    Userpwd = UtilityBO.GetMD5HexStringForStorePwd(Useruraian);
                    Userket = GlobalAsp.GetHashData(this);
                    UpdatePwd(Userpwd);
                }
                else
                {
                    throw new Exception(ConstantDict.Translate("LBL_INVALID_USERTYPE"));
                }
            }
            else
            {
                throw new Exception(ConstantDict.Translate("LBL_INVALID_INSERT"));
            }
        }
        public new int Update()
        {
            int n = 0;
            if (IsValid())
            {
                if (!Usertype.Equals(GlobalAsp.USER_ADMIN))
                {
                    if (int.Parse(Userblock) > 3)//Reset Password
                    {
                        Useruraian = UtilityBO.GetRandomPassword(Userid);
                        Userpwd = UtilityBO.GetMD5HexStringForStorePwd(Useruraian);
                        Userket = GlobalAsp.GetHashData(this);
                        UpdatePwd(Userpwd);
                    }
                    Userket = GlobalAsp.GetHashData(this);
                    n = base.Update();
                }
                else
                {
                    Userket = GlobalAsp.GetHashData(this);
                    n = base.Update();
                    //throw new Exception(ConstantDict.Translate("LBL_INVALID_USERTYPE"));
                }
            }
            else
            {
                throw new Exception(ConstantDict.Translate("LBL_INVALID_UPDATE"));
            }
            return n;
        }
        private bool IsValid()
        {
            bool valid = true;
            if (Usertype.Equals(GlobalAsp.USER_ADMIN))
            {
                valid = (GlobalAsp.IsClientSuperCreator());
            }
            else
            {
                valid = string.IsNullOrEmpty(Kdpemda);
            }
            return valid;
        }

        public new HashTableofParameterRow GetFilters()
        {
            IDataControlAppuser dcUser = ((IDataControlAppuser)GlobalAsp.GetSessionUser());
            bool isAdmin = (dcUser.GetUserType().Equals(GlobalAsp.USER_ADMIN));
            bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev())
              && isAdmin;
            HashTableofParameterRow hpars = new HashTableofParameterRow{
                DmpemdaLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter)
            };
            return hpars;
        }
        public new int Delete()
        {
            return base.Delete();
        }

        public new IList View()
        {
            IList list = null;
            if (GlobalAsp.GetSessionApp() == MasterAppConstants.Instance.MasterAppID)
            {
                list = View(BaseDataControl.LOOKUP);
            }
            else
            {
                list = View(BaseDataControl.ALL);
            }
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
        public new BaseBO Load()
        {
            var dcRoot = Ss10userLookupControl.GetListDataSingleton().Find(o => o.Userid.Equals(Userid));
            if (dcRoot != null)
            {
                dcRoot.CopyPropertyBOTo(this);
            }
            return this;
            //return base.Load();
        }

        public new void ExecutedSQL(string tname, DataTable csvData, int counter, bool isdelete)
        {
            ReadDataTable(csvData, counter);
            SetPrimaryKey();
            Insert();
        }
        public override DataControlFieldCollection GetColumns()
        {
            DataControlFieldCollection columns = new DataControlFieldCollection
            {
                Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Ext.Net.Icon), 5, HorizontalAlign.Center),
                Fields.Create(ConstantDict.GetColumnTitle("Stricon"), typeof(CommandColumn), Cmds, 5, HorizontalAlign.Center),
                Fields.Create(ConstantDict.GetColumnTitle("Userid"), typeof(string), EditCmd, 35, HorizontalAlign.Left).SetEditable(false),
                Fields.Create(ConstantDict.GetColumnTitle("Usernama"), typeof(string), 20, HorizontalAlign.Left).SetEditable(true),
                Fields.Create(ConstantDict.GetColumnTitle("Userblock"), typeof(string), 8, HorizontalAlign.Center).SetEditable(true),
                Fields.Create(ConstantDict.GetColumnTitle("Userhp"), typeof(string), 30, HorizontalAlign.Center).SetEditable(true),
                Fields.Create(ConstantDict.GetColumnTitle("Useremail"), typeof(string), 30, HorizontalAlign.Center).SetEditable(true),
                Fields.Create(ConstantDict.GetColumnTitle("Useruraian"), typeof(string), 20, HorizontalAlign.Left).SetEditable(true)
            };
            return columns;
        }

        public override HashTableofParameterRow GetEntries()
        {
            bool enable = true;
            bool modeedit = UtilityExt.IsModeEdit();
            HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Userid"), false, 70).SetEnable(enable),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Usertype"),
        GetList(new Ss10usertypesControl()), "Usertype=Uturaian", 30).SetAllowRefresh(false).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Usernama"), true, 95).SetEnable(enable),
        new ParameterRowNumeric(this, ConstantDict.GetColumnTitle("Userblock"), true, 10).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Userhp"), true, 50).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Useremail"), true, 50).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Useruraian"), true, 95).SetEnable(modeedit),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(true)
      };
            return hpars;
        }

        public new string[] GetKeys()
        {
            return new string[] { "Userid", "Kdpemda"
        , "Usernip", "Userno", "Usertype", "Usernama","Userpwd"
        ,"Idapp","Kdapp","Nmapp"
        , "Status", "Statusicon", "Statusname", "Stricon", "Rating"
        , "Userblock", "Useruraian", "Userhp", "Useremail", "Userstatus", "Userket"};
        }
        public new string[] GetCsvColumns(int mode)
        {
            return new string[] { "Userid", "Kdpemda", "Usernip", "Usertype", "Usernama"
        , "Useruraian", "Userhp", "Useremail", "Userket"};
        }
        public new string[] GetLoadCsvColumns()
        {
            return new string[] { "Userid", "Kdpemda", "Usernip", "Usertype", "Usernama"
        , "Useruraian", "Userhp", "Useremail", "Userket"};
        }
        #endregion Methods
        #region IDataControlAppuser
        private Dictionary<string, object> Params = new Dictionary<string, object>();
        public object GetValueProperty(string par)
        {
            if (Params.ContainsKey(par))
            {
                return Params[par];
            }
            else
            {
                return null;
            }
        }

        public void SetValueProperty(string par, object val)
        {
            Params[par] = val;
        }
        public bool IsAdmin()
        {
            return (Usertype.Equals(MasterAppConstants.STR_ADMIN));
        }
        public bool IsEnableLookup()
        {
            return true;
        }
        public bool IsVisibleLookup()
        {
            return true;
        }
        public string GetAppName(string idapp)
        {
            Idapp = idapp;
            SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
            return Nmapp;
        }
        public string GetUserID()
        {
            return Userid;
        }
        public string GetGroupingDataID()
        {
            return Kdpemda;
        }
        public string GetUserPwd()
        {
            return Userpwd;
        }
        public string GetUserType()
        {
            return Usertype;
        }
        public string GetLabelUser() { return "User ID = " + Userid; }
        public string GetLabelNip() { return "NIP = " + Usernip; }
        public string GetLabelNama() { return "Nama = " + Usernama; }
        public string GetLabelEmail() { return "Email = " + Useremail; }
        public string GetLabelNomor() { return "Nomor =" + Nodok; }
        public string GetLabelUraian() { return "Uraian = " + Userket; }
        public string GetLabelRole() { return "Role=" + Uturaian; }
        public string GetLabelIP() { return "IP = " + UtilityUI.GetIPAddress(); }

        public void SetUserID(string appID, string userID)
        {
            Idapp = appID;
            Userid = userID;
        }
        public void SetUserOL(string userID, string userkey)
        {
            Ss11userolControl dc = new Ss11userolControl
            {
                Idol = Guid.NewGuid().ToString(),
                Userid = userID,
                Userkey = userkey,
                Last_by = userID,
                Last_date = DateTime.Now
            };
            ((BaseDataControl)dc).Insert();
        }
        public string GetUserDebugInfo()
        {
            //string kdmenu = GlobalExt.GetSessionCurrentRoleid();
            //string strModeEditableFromGroup = string.Empty;
            //if (GlobalExt.GetSessionGroup() != null)
            //{
            //  strModeEditableFromGroup = ((SswebgroupControl)GlobalExt.GetSessionGroup()).GetGroupConfigs("MODE_EDITABLE");
            //}
            //int modeEditablefromGroup = -1;
            //if (!string.IsNullOrEmpty(strModeEditableFromGroup))
            //{
            //  modeEditablefromGroup = int.Parse(strModeEditableFromGroup);
            //}
            //int modeEditablefromUser = ((Ss10userControl)GlobalExt.GetSessionUser()).ModeEditable;

            //int modeEditablefromMenu = -1;
            //if (GlobalExt.GetSessionMenu() != null)
            //{
            //  modeEditablefromMenu = ((SswebmenuControl)GlobalExt.GetSessionMenu()).Modeeditable;
            //}

            //string info = @"
            //  Kdpemda = " + Kdpemda + @"<br/>
            //  Tahun = " + AppUtils.GetCurrentYear() + @"<br/>
            //  Kdunit = " + Kdunit + @"<br/>
            //  Nip = " + Nip + @"<br/>
            //  /* ModeEditable, urutan prioritas ValidUnit,User,Group,Menu (klo tidak -1) */<br/>
            //  modeEditablefromMenu = " + modeEditablefromMenu + @"<br/>
            //  modeEditablefromGroup = " + modeEditablefromGroup + @"<br/>
            //  modeEditablefromUser = " + modeEditablefromUser + @"<br/>
            //  FILTER_DOK = " + AppUtils.GetFilterKddok() + @"<br/>
            //  DEFAULT_DOK = " + Nodok + @"<br/>
            //";

            return string.Empty;
        }
        #endregion
    }
    #endregion Ssappuser
}

