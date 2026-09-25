using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
    #region Ss01appmenu
    /**
     * 
     * 
     * */
    [Serializable]
    public class Ss01appmenuControl : BaseDataControlMenu, IDataControlMenu, IDataControlMenuMapClass, IDataControlTreeGrid3, IHasJSScript, ICsv
    {
        #region Methods 
        public Ss01appmenuControl()
        {
            XMLName = ConstantTablesSys.XMLSS01APPMENU;
            SetMaxModePreviewIndex(2);
        }
        public new IProperties GetProperties()
        {
            ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
            cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
            cViewListProperties.PrimaryKeys = new String[] { "Idapp", "Kdmenu" };
            cViewListProperties.IDKey = "Idmenu";
            cViewListProperties.IDProperty = "Kdmenu";
            cViewListProperties.ReadOnlyFields = new String[] { };
            cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
            cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
            return cViewListProperties;
        }

        public new void SetPageKey()
        {
            if (!GlobalAsp.GetHomeURL().ToLower().Contains("portal"))
            {
                Idapp = GlobalAsp.GetSessionApp();
                Idmenu = GlobalAsp.GetRequestId();
                Userid = GlobalAsp.GetSessionUser().GetUserID();
            }
        }

        public new void SetFilterKey(BaseBO bo)
        {
            if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
            {
                Idapp = bo.Idapp;
                SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
            }
            else if (typeof(IDataControlMenu).IsInstanceOfType(bo))
            {
                try
                {
                    Idmenu = (string)HttpContext.Current.Request["val"];
                    if (!string.IsNullOrEmpty(Idmenu))
                    {
                        Load(BaseDataControl.ID);
                    }
                }
                catch (Exception ex)
                {
                    UtilityBO.Log(this, ex);
                }
            }
        }

        public new void FilterClick(string key)
        {
            if (key.Contains("Idapp"))
            {
                SsappControl dcapp = new SsappControl() { Idapp = Idapp, Kdapp = Kdapp, Nmapp = Nmapp };
                GlobalAsp.SetSessionData(dcapp);
            }
            else if (key.Contains("Idmenu"))
            {
                GlobalAsp.SetSessionData(XMLName, this);
            }
        }

        public new HashTableofParameterRow GetFilters()
        {
            HashTableofParameterRow hpars = new HashTableofParameterRow();
            return hpars;
        }

        public new void Insert()
        {
            base.Insert();
            SsappmenuLookupControl.SetListDataNull();
        }

        public new int Update()
        {
            int n = 0;
            n = base.Update();
            SsappmenuLookupControl.SetListDataNull();
            return n;
        }

        public new BaseBO Load()
        {
            BaseBO bo = Load(BaseDataControl.PK);
            return bo;
        }
        static Dictionary<string, Ss01appmenuControl> Cache = new Dictionary<string, Ss01appmenuControl>();
        public new BaseBO Load(string label)
        {
            string key = $"{Idapp}|{Kdmenu}";
            Ss01appmenuControl bo = null;
            if (Cache.ContainsKey(key))
            {
                bo = Cache[key];
            }
            if (bo == null)
            {
                bo = (Ss01appmenuControl)((BaseDataControl)this).Load(label);
                if (bo == null)
                {
                    return null;
                }
                else
                {
                    Cache[key] = bo;
                    return bo;
                }
            }
            else
            {
                return bo;
            }
            //Ngga efisien, datanya banyak
            //var dcRoot = Ss01appmenuLookupControl.GetListDataSingleton().Find(o => o.Idapp.Equals(Idapp) && o.Kdmenu.Equals(Kdmenu));
            //if (dcRoot != null)
            //{
            //    dcRoot.CopyPropertyBOTo(this);
            //}
            //return this;

        }

        public new IList View()
        {
            IList list = null;
            list = View(BaseDataControl.ALL);
            return list;
        }
        public new IList View(string label)
        {
            IList list = ((BaseDataControl)this).View(label);
            List<Ss01appmenuControl> ListData = new List<Ss01appmenuControl>();
            foreach (Ss01appmenuControl dc in list)
            {
                if (MasterAppConstants.Instance.StatusTesting || (dc.Status > -1))//Status==-1 hidden
                {
                    string nmmenu = ConstantDict.Translate("Menu" + dc.Kdapp + dc.Kdmenu + "=" + dc.Nmmenu);
                    dc.Nmmenu = string.IsNullOrEmpty(nmmenu) ? dc.Nmmenu : nmmenu;
                    DmstatusLookupControl.FindAndSetValuesInto(dc);
                    if (MasterAppConstants.Instance.StatusTesting)//Normalisasi Kdlevel
                    {
                        dc.Kdlevel = dc.Kdmenu.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length;
                        dc.Update("Kdlevel");
                    }
                    ListData.Add(dc);
                }
            }
            Update(ListData);
            return ListData;
        }
        public override DataControlFieldCollection GetColumns()
        {
            DataControlFieldCollection columns = new DataControlFieldCollection
      {
        Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Kdmenu"), typeof(string), EditCmd, 15, HorizontalAlign.Left).SetEditable(true),
        Fields.Create(ConstantDict.GetColumnTitle("Nmmenu"), typeof(string), 40, HorizontalAlign.Left).SetEditable(true)
      };
            if (ModePreviewIndex == 2)
            {
                columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Idxdok"), typeof(string), 25, HorizontalAlign.Center).SetEditable(true));
                columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdpack"), typeof(string), 20, HorizontalAlign.Left));
                columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kddok"), typeof(string), 20, HorizontalAlign.Center));
            }
            columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Url"), typeof(string), 45, HorizontalAlign.Left));
            if (ModePreviewIndex == 1)
            {
                columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdlevel"), typeof(int), 7, HorizontalAlign.Center).SetEditable(true));
                columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Type"), typeof(string), 7, HorizontalAlign.Center).SetEditable(true));
            }
            return columns;
        }
        public override HashTableofParameterRow GetEntries()
        {
            bool enable = true;
            HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdmenu"), true, 30).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nmmenu"), true, 95).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Idmenu"), true, 70).SetEnable(false),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Url"), true, 95).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olist1"), true, 95).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olistdetil1"), true, 95).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olistdetil2"), true, 95).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olistdetil3"), true, 95).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Olistdetil4"), true, 95).SetEnable(enable),
        new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
        GetList(new DmlevelControl(this)), "Kdlevel=Nmlevel", 30).SetEnable(enable),
        new ParameterRowType(this, true),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
        GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(true).SetEnable(enable),
        new ParameterRowCek(this, true),
        new ParameterRowUploadFile(this, true),
        new ParameterRowHelp(this, true),
        new ParameterRowForum(this, true)
      };
            return hpars;
        }

        public string URLEditor
        {
            get
            {
                string url = string.Format("Page/PageForm.aspx?app={0}&id={1}",
                   GlobalAsp.GetSessionApp(), "559492D9-A935-4E75-B603-D3A138EDA7A3", GlobalAsp.GetRequestId());
                return url;
            }
        }
        public string URLMapClass
        {
            get
            {
                string url = string.Format("Page/PageForm.aspx?app={0}&id={1}",
                   GlobalAsp.GetSessionApp(), "637317A5-5DA3-4991-9CC2-468037264102", GlobalAsp.GetRequestId());
                return url;
            }
        }

        public new string[] GetScript()
        {
            ArrayList listScript = new ArrayList(base.GetScript());
            string script = @"
        var GetURLEditor = function (val) {
            return '" + URLEditor + @"&idprev={0}'.format(val);
        };
        var GetURLMapClass = function (val) {
            return '" + URLMapClass + @"&idprev={0}'.format(val);
        };
      ";
            listScript.Add(script);
            string[] scripts = new string[listScript.Count];
            listScript.CopyTo(scripts);
            return scripts;
        }
        #endregion Methods 
        #region IDataControlTreeGrid3
        public string ParentKdmenu { get => ParentKode(Kdmenu); set { } }
        public static bool IsRootCondition(Ss01appmenuControl dc)
        {
            return (dc.Kdlevel == 1);
        }
        public Icon GetIcon()
        {
            //return Ext.Net.Icon.Table;
            return Ext.Net.Icon.Page;
        }
        public new string[] GetKeys()
        {
            return new string[] { "Idapp","Kdmenu","Idmenu","Kdpack","Kddok","Idxdok","Nodok","Nfiles"
        ,"Olist1","Olistdetil1","Olistdetil2","Olistdetil3","Olistdetil4"
        , "Stinsert", "Stupdate", "Stdelete", "Stload", "Stfilter"
        ,"Progress","Progressicon","Progressstr"
        , "Status", "Statusicon", "Statusname", "Stricon", "Rating"
        ,"Last_by","Last_date","Url"
        ,"Nmmenu","UrlFull","Kdlevel","Type" };
        }
        public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
        {
            Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdmenu"), Width = 200, DataIndex = "Kdmenu", Align = Ext.Net.TextAlign.Left });
            Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kddok"), Width = 100, DataIndex = "Kddok", Align = Ext.Net.TextAlign.Center });
            Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Idxdok"), Width = 100, DataIndex = "Idxdok", Align = Ext.Net.TextAlign.Center });
            Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmmenu"), Width = 500, DataIndex = "Nmmenu", Align = Ext.Net.TextAlign.Left });
            if (GetModePreviewIndex() == 1)
            {
                Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Url"), Width = 200, DataIndex = "UrlFull", Align = Ext.Net.TextAlign.Left });
            }
            Columns.Add(TreeColStatus);
            if (GetModePreviewIndex() == 1)
            {
                Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdlevel"), Width = 50, DataIndex = "Kdlevel", Align = Ext.Net.TextAlign.Center });
                Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Type"), Width = 50, DataIndex = "Type", Align = Ext.Net.TextAlign.Center });
            }
        }
        public Ext.Net.TreeNode CreateMenu(IList list, int typetree, bool withroot)
        {
            string[] Keys = new string[] { "Kdmenu", "Nmmenu", "UrlFull", "Kdlevel", "Type" };
            ExtTreePanelUtil util = new ExtTreePanelUtil();
            return util.CreateTree(ConstantDict.Translate("Daftar Menu"), list, Keys, new string[] { "Kdmenu", "." }, new string[] { "Nmmenu", ">" }, typetree, withroot);
        }
        public Ext.Net.TreeNode CreateRoot(IList list, int typetree, bool withroot)
        {
            string delim_menu = GlobalExt.DELIMITER_MENU;
            List<Ss01appmenuControl> localList = (List<Ss01appmenuControl>)list;
            List<Ss01appmenuControl> roots = localList.FindAll(i => IsRootCondition(i));

            Ext.Net.TreeNode root = new Ext.Net.TreeNode("Menu", "Menu", GetIcon());
            Ext.Net.TreeNodeCollection nodes = root.Nodes;
            foreach (Ss01appmenuControl ctrl in roots)
            {
                nodes.Add(CreateNodeWithOutChildren((List<Ss01appmenuControl>)list, ctrl, typetree, delim_menu));
            }
            return root;
        }
        public void LoadPages(IList inList, string nodeid, Ext.Net.TreeNodeCollection nodes, int typetree)
        {
            string delim_menu = ">";
            List<Ss01appmenuControl> list = (List<Ss01appmenuControl>)inList;
            if (list != null && list.Count > 0)
            {
                Ss01appmenuControl parent = list.Find(o => o.Kdmenu.Equals(nodeid));
                if ((parent != null) && (parent.Type.Equals("H")))
                {
                    List<Ss01appmenuControl> children = GetChildren(list, parent);
                    foreach (Ss01appmenuControl ctrl in children)
                    {
                        nodes.Add(CreateNodeWithOutChildren(list, ctrl, typetree, delim_menu));
                    }
                }
            }
        }
        public static List<Ss01appmenuControl> GetChildren(List<Ss01appmenuControl> domainset, Ss01appmenuControl parent)
        {
            List<Ss01appmenuControl> children = domainset.FindAll(o => o.Kdmenu.StartsWith(parent.Kdmenu) && (o.Kdlevel == parent.Kdlevel + 1));
            return children;
        }
        protected TreeNodeBase CreateNodeWithOutChildren(List<Ss01appmenuControl> list, Ss01appmenuControl parent, int typetree, string delim_menu)
        {
            TreeNodeBase treeNode;
            List<Ss01appmenuControl> children = GetChildren(list, parent);
            if (children != null && children.Count > 0)
            {
                treeNode = (AsyncTreeNode)ExtTreeNode.GetExtTreeNode(parent.Kdmenu,
                  ExtTreePanelUtil.GetUraian(parent.Nmmenu, new string[] { "Nmmenu", delim_menu }),
                  typetree, GetKeys(), parent, true, Ext.Net.Icon.Folder);
            }
            else
            {
                treeNode = ExtTreeNode.GetExtTreeNode(parent.Kdmenu,
                  ExtTreePanelUtil.GetUraian(parent.Nmmenu, new string[] { "Nmmenu", delim_menu }),
                  typetree, GetKeys(), parent, false, Ext.Net.Icon.Table);
            }
            return treeNode;
        }

        #endregion IDataControlTreeGrid3
        #region IDataControlMenu
        public void SetOtorisasiMenu(Page page, string idapp)
        {
            Url = HttpContext.Current.Request.Url.OriginalString;
            Idapp = GlobalAsp.GetSessionApp();
            SsappLookupControl.FindAndSetValuesIntoByIdapp(this);

            Ss10userControl user = (Ss10userControl)GlobalAsp.GetSessionUser();

            SetAboutValue(page, "userid", string.Format("Userid = {0}", user.Userid));
            SetAboutValue(page, "nama", string.Format("Nama = {0}", user.Usernama));
            SetAboutValue(page, "nip", string.Format("NIP = {0}", user.Usernip));
            SetAboutValue(page, "email", string.Format("Email = {0}", user.Useremail));
            SetAboutValue(page, "nohp", string.Format("Mobile No.={0}", user.Userhp));
            SetAboutValue(page, "role", string.Format("Jabatan = {0}", user.Uturaian));
            SetAboutValue(page, "uraian", string.Format("{0}", user.Nmpemda));
            SetAboutValue(page, "ipaddr", string.Format("IP Adress = {0}", UtilityUI.GetIPAddress())); //UtilityUI.GetClientCompIP();
        }

        public IDataControlMenu FindObject(string kdmenu)
        {
            Ss01appmenuControl dc = ((List<Ss01appmenuControl>)GlobalExt.GetSessionListMenu()).Find(o => o.Kdmenu.Equals(kdmenu));
            return dc;
        }
        public string GetRoleid()
        {
            return Kdmenu;
        }
        public string GetURLReal()
        {
            return Url;
        }
        public string GetURL()
        {
            return UrlFull;
        }
        public void GetDefaultURL(out string roleid, out string url)
        {
            roleid = "01.";
            //Idapp = Idproject
            string idapp = GlobalAsp.GetSessionApp();
            //url = Ss01appmenuLookupControl.GetURL(idapp, "01.");

            Ss01appmenuControl dcmenu = new Ss01appmenuControl
            {
                Idapp = idapp,
                Kdmenu = "01."
            };
            dcmenu.Load(BaseDataControl.PK);//Lebih Cepat, sdh union ss01appmenu dan ss00appmenu
            url = dcmenu.UrlFull;
        }
        public string GetAppName(string idapp)
        {
            Idapp = idapp;
            SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
            return Nmapp;
        }
        public string GetAppTitle(string idapp)
        {
            Idapp = idapp;
            SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
            return Nmapp;
        }
        #endregion
        #region Toolbar Menu
        public IList GetListRef()
        {
            return null;
        }
        public Ext.Net.Menu GetMenu(IList list, short kdlevel)
        {
            //IList list, int typetree, bool withroot
            List<Ss01appmenuControl> localList = (List<Ss01appmenuControl>)list;
            List<Ss01appmenuControl> roots = localList.FindAll(i => (i.Kdlevel == kdlevel));
            Ext.Net.Menu menu = new Ext.Net.Menu();
            foreach (Ss01appmenuControl ctrl in roots)
            {
                menu.Items.Add(CreateMenuWithChildren((List<Ss01appmenuControl>)list, ctrl));
            }
            return menu;
        }
        private Ext.Net.MenuItem CreateMenuWithChildren(List<Ss01appmenuControl> list, Ss01appmenuControl parent)
        {
            Ext.Net.MenuItem menuNode = new Ext.Net.MenuItem(parent.Nmmenu);
            menuNode.Listeners.Click.Handler =
              string.Format("e.stopEvent(); loadPage(#{0}, '{1}','{1}','{2}');", "{Pages}", parent.Nmmenu, parent.UrlFull);
            List<Ss01appmenuControl> children = GetChildren(list, parent);
            Ext.Net.Menu menu = new Ext.Net.Menu();
            if (children.Count > 0)
            {
                menuNode.Menu.Add(menu);
            }
            for (int i = 0; i < children.Count; i++)
            {
                Ss01appmenuControl child = children[i];
                menu.Items.Add(CreateMenuWithChildren(list, child));
            }
            return menuNode;
        }
        #endregion
        #region Sum All
        public static void Update(List<Ss01appmenuControl> domainset)
        {
            List<Ss01appmenuControl> roots = domainset.FindAll(i => IsRootCondition(i));
            if (roots.Count > 0)
            {
                foreach (Ss01appmenuControl r in roots)
                {
                    UpdatedRecFields bobotProgress = SumChildren(domainset, r);
                    r.SetValue(bobotProgress);
                }
            }
        }

        private static UpdatedRecFields SumChildren(List<Ss01appmenuControl> domainsetParent, Ss01appmenuControl parent)
        {
            UpdatedRecFields localBobotProgress = new UpdatedRecFields();
            List<Ss01appmenuControl> localDomainset = domainsetParent.FindAll(o => o.Kdmenu.StartsWith(parent.Kdmenu));

            List<Ss01appmenuControl> children = GetChildren(localDomainset, parent);
            int localNumChildren = 0;
            if ((children.Count > 0) && (parent.Type == "H"))
            {
                localBobotProgress.Nchild = children.Count;
                foreach (Ss01appmenuControl o in children)
                {
                    localNumChildren++;
                    UpdatedRecFields childrenBobotProgress = SumChildren(localDomainset, o);
                    localBobotProgress.Sum(childrenBobotProgress);
                }
                parent.SetValue(localBobotProgress);
                return localBobotProgress;
            }
            else
            {
                localBobotProgress.SetValue(parent);
                localBobotProgress.Nchild = 0;
                return localBobotProgress;
            }
        }

        private void SetValue(UpdatedRecFields rec)
        {
            Nchild = rec.Nchild;
            Progress = (rec.Nchild == 0) ? Progress : rec.Progress / rec.Nchild;
        }

        private class UpdatedRecFields
        {
            public int Nchild = 0;
            public decimal Progress = 0;
            public int Nmsgs = 0;

            public void Sum(UpdatedRecFields rec)
            {
                Nchild += rec.Nchild;
                Progress += rec.Progress;
            }

            public void SetValue(Ss01appmenuControl rec)
            {
                Nchild = rec.Nchild;
                Progress = rec.Progress;
            }
        }
        #endregion
        #region IDataControlMenuMapClass
        public void LoadByRoleID(string roleid)
        {
            //Ss01appmenuControl dcmenu = new Ss01appmenuControl
            //{
            //    Idmenu = roleid
            //};
            //dcmenu.Load(BaseDataControl.ID);//Lebih Cepat, sdh union ss01appmenu dan ss00appmenu
            Ss00appmenuControl dcmenu = SsappmenuAllIdmenuLookupControl.GetListDataSingleton().Find(o => o.Idmenu.Equals(roleid));
            Olist1 = dcmenu.Olist1;
            Olistdetil1 = dcmenu.Olistdetil1;
            Olistdetil2 = dcmenu.Olistdetil2;
            Olistdetil3 = dcmenu.Olistdetil3;
            Olistdetil4 = dcmenu.Olistdetil4;
        }
        public void LoadLookupByRoleID(string roleid)
        {
            Ss01appmenuconfigLookupControl ctrl = new Ss01appmenuconfigLookupControl
            {
                Idmenu = roleid
            };
            Olookuplist1 = (string)ctrl.Params["OLIST1"];
            Olookuplistdetil1 = (string)ctrl.Params["OLISTDETIL1"];
            Olookuplistdetil2 = (string)ctrl.Params["OLISTDETIL2"];
            Olookuplistdetil3 = (string)ctrl.Params["OLISTDETIL3"];
            Olookuplistdetil4 = (string)ctrl.Params["OLISTDETIL4"];
        }

        #endregion
    }
    #endregion Ss01appmenu
}

