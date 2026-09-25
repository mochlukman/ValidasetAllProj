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
    #region Ss00appmenu
    [Serializable]
    public class Ss00appmenuControl : BaseDataControlMenu, IDataControlMenu, IDataControlMenuMapClass, IDataControlTreeGrid3, IHasJSScript, ICsv
    {
        #region Methods 
        public Ss00appmenuControl()
        {
            XMLName = ConstantTablesSys.XMLSS00APPMENU;
            SetMaxModePreviewIndex(1);
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
            Tahun = DateTime.Now.Year;
        }

        public new void SetFilterKey(BaseBO bo)
        {
            Ss00appmenuControl currentdc = (Ss00appmenuControl)HttpContext.Current.Session[XMLName];
            if (currentdc == null)
            {
                Idapp = GlobalAsp.GetSessionApp();
                SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
                HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1] = this;
                FilterClick("Idapp");
            }
            else
            {
                Idapp = currentdc.Idapp;
                Kdapp = currentdc.Kdapp;
                Nmapp = currentdc.Nmapp;
            }
            if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
            {
                Last_by = ((IDataControlAppuser)bo).GetUserID();
            }
            else
            {
                Idmenu = (string)HttpContext.Current.Request["val"];
                Load(BaseDataControl.ID);
            }
        }

        public new void FilterClick(string key)
        {
            Ss00appmenuControl dc = (Ss00appmenuControl)HttpContext.Current.Session[GlobalAsp.SESSION_OBJECT_FOR_LIST1];
            if (key.Contains("Idapp"))
            {
                dc.Idapp = Idapp;
                dc.Kdapp = Kdapp;
                dc.Nmapp = Nmapp;
            }
            else if (key.Contains("Idmenu"))
            {
                dc.Idmenu = Idmenu;
                dc.Kdmenu = Kdmenu;
                dc.Nmmenu = Nmmenu;
            }
            HttpContext.Current.Session[XMLName] = dc;
        }


        public new HashTableofParameterRow GetFilters()
        {
            HashTableofParameterRow hpars = new HashTableofParameterRow();
            return hpars;
        }

        public new IList View()
        {
            if (GlobalAsp.GetSessionUser().GetUserType().Equals(GlobalAsp.USER_ADMIN))
            {
                IList list = this.View(BaseDataControl.ALL);
                return list;
            }
            else
            {
                return new ArrayList();
            }
        }

        public new BaseBO Load()
        {
            BaseBO bo = Load(BaseDataControl.PK);
            return bo;
        }
        public new BaseBO Load(string label)
        {
            ((BaseDataControl)this).Load(label);
            //double
            //SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
            return this;
        }

        static List<Ss00appmenuControl> _ListData = null;
        public new IList View(string label)
        {
            if (_ListData == null)
            {
                IList list = ((BaseDataControl)this).View(label);
                _ListData = new List<Ss00appmenuControl>();
                foreach (Ss00appmenuControl dc in list)
                {
                    if (MasterAppConstants.Instance.StatusTesting || (dc.Status > -1))//Status==-1 hidden
                    {
                        DmstatusLookupControl.FindAndSetValuesInto(dc);
                        _ListData.Add(dc);
                    }
                }
            }
            return _ListData;
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
                   GlobalAsp.GetSessionApp(), "3534FCF7-06D1-432D-AF02-11592511E16F", GlobalAsp.GetRequestId());
                return url;
            }
        }
        public string URLMapClass
        {
            get
            {
                string url = string.Format("Page/PageForm.aspx?app={0}&id={1}",
                   GlobalAsp.GetSessionApp(), "6FA01169-1E40-49D9-B3CE-8CB53D77F9A0", GlobalAsp.GetRequestId());
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
        public string ParentKdmenu { get { return ParentKode(Kdmenu); } set { } }
        public static bool IsRootCondition(Ss00appmenuControl dc)
        {
            return (dc.Kdlevel == 1);
        }
        public Icon GetIcon()
        {
            //return Ext.Net.Icon.Table;
            return Ext.Net.Icon.Page;
        }
        public Ext.Net.TreeNode CreateMenu(IList list, int typetree, bool withroot)
        {
            string[] Keys = GetKeys();
            ExtTreePanelUtil util = new ExtTreePanelUtil();
            return util.CreateTree(ConstantDict.Translate("Daftar Menu"), list, Keys, new string[] { "Kdmenu", "." }, new string[] { "Nmmenu", ">" }, typetree, withroot);
        }
        public Ext.Net.TreeNode CreateRoot(IList list, int typetree, bool withroot)
        {
            string delim_menu = GlobalExt.DELIMITER_MENU;
            List<Ss00appmenuControl> localList = (List<Ss00appmenuControl>)list;
            List<Ss00appmenuControl> roots = localList.FindAll(i => IsRootCondition(i));

            Ext.Net.TreeNode root = new Ext.Net.TreeNode("Menu", "Menu", GetIcon());
            Ext.Net.TreeNodeCollection nodes = root.Nodes;
            foreach (Ss00appmenuControl ctrl in roots)
            {
                nodes.Add(CreateNodeWithOutChildren((List<Ss00appmenuControl>)list, ctrl, typetree, delim_menu));
            }
            return root;
        }
        public void LoadPages(IList inList, string nodeid, Ext.Net.TreeNodeCollection nodes, int typetree)
        {
            string delim_menu = ">";
            List<Ss00appmenuControl> list = (List<Ss00appmenuControl>)inList;
            if (list != null && list.Count > 0)
            {
                Ss00appmenuControl parent = list.Find(o => o.Kdmenu.Equals(nodeid));
                if ((parent != null) && (parent.Type.Equals("H")))
                {
                    List<Ss00appmenuControl> children = GetChildren(list, parent);
                    foreach (Ss00appmenuControl ctrl in children)
                    {
                        nodes.Add(CreateNodeWithOutChildren(list, ctrl, typetree, delim_menu));
                    }
                }
            }
        }
        public static List<Ss00appmenuControl> GetChildren(List<Ss00appmenuControl> domainset, Ss00appmenuControl parent)
        {
            List<Ss00appmenuControl> children = domainset.FindAll(o => o.Kdmenu.StartsWith(parent.Kdmenu) && (o.Kdlevel == parent.Kdlevel + 1));
            return children;
        }
        protected TreeNodeBase CreateNodeWithOutChildren(List<Ss00appmenuControl> list, Ss00appmenuControl parent, int typetree, string delim_menu)
        {
            TreeNodeBase treeNode;
            List<Ss00appmenuControl> children = GetChildren(list, parent);
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
        #region Toolbar Menu
        public IList GetListRef()
        {
            return null;
        }
        public Ext.Net.Menu GetMenu(IList list, short kdlevel)
        {
            //IList list, int typetree, bool withroot
            List<Ss00appmenuControl> localList = (List<Ss00appmenuControl>)list;
            List<Ss00appmenuControl> roots = localList.FindAll(i => (i.Kdlevel == kdlevel));
            Ext.Net.Menu menu = new Ext.Net.Menu();
            foreach (Ss00appmenuControl ctrl in roots)
            {
                menu.Items.Add(CreateMenuWithChildren((List<Ss00appmenuControl>)list, ctrl));
            }
            return menu;
        }
        private Ext.Net.MenuItem CreateMenuWithChildren(List<Ss00appmenuControl> list, Ss00appmenuControl parent)
        {
            Ext.Net.MenuItem menuNode = new Ext.Net.MenuItem(parent.Nmmenu);
            List<Ss00appmenuControl> children = GetChildren(list, parent);
            Ext.Net.Menu menu = new Ext.Net.Menu();
            if (children.Count > 0)
            {
                menuNode.Menu.Add(menu);
            }
            for (int i = 0; i < children.Count; i++)
            {
                Ss00appmenuControl child = children[i];
                menu.Items.Add(CreateMenuWithChildren(list, child));
            }
            return menuNode;
        }
        #endregion

        #region IDataControlMenu
        public void SetOtorisasiMenu(Page page, string idapp)
        {
            //idapp adalah ID APP Admin
            Ss10userappControl dc = new Ss10userappControl();
            dc.Userid = GlobalAsp.GetSessionUser().GetUserID();
            dc.Idapp = idapp;
            List<Ss10userControl> list = (List<Ss10userControl>)Ss10userappLookupControl.GetListDataSingleton();
            list = list.FindAll(o => o.Userid.Equals(dc.GetValue("Userid")));
            if (list.Count == 1)
            {
                Idapp = list[0].Idapp;
            }
            else
            {
                //Untuk otorisasi menu admin
                Idapp = string.Empty;
                foreach (Ss10userControl ctrl in list)
                {
                    if (ctrl.Idapp == GlobalAsp.GetRequestVal())//Idapp terpilih adalah otorisasi dari user tsb
                    {
                        Idapp = ctrl.Idapp;
                        break;
                    }
                }
                if (string.IsNullOrEmpty(Idapp))
                {
                    Idapp = GlobalAsp.GetSessionApp();
                }
            }
        }

        public IDataControlMenu FindObject(string kdmenu)
        {
            Ss00appmenuControl dc = ((List<Ss00appmenuControl>)GlobalExt.GetSessionListMenu()).Find(o => o.Kdmenu.Equals(kdmenu));
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
            //url = Ss01appmenuLookupControl.GetURL(Idapp, "01.");
            Ss01appmenuControl dcmenu = new Ss01appmenuControl();
            dcmenu.Idapp = MasterAppConstants.Instance.MasterAppID;
            dcmenu.Kdmenu = "01.";
            dcmenu.Load(BaseDataControl.PK);//Lebih Cepat, sdh union ss01appmenu dan ss00appmenu
            url = dcmenu.UrlFull;
        }
        //public string GetAppID()
        //{
        //  Ss10userappControl dc = new Ss10userappControl();
        //  dc.Userid = Userid;
        //  List<Ss10userControl> list = Ss10userappLookupControl.GetListDataSingleton().FindAll(o => o.Userid.Equals(Userid)
        //  && !o.Idapp.Equals(MasterAppConstants.AppID));
        //  if (list.Count > 0)
        //  {
        //    return (list[0]).Idapp;
        //  }
        //  return MasterAppConstants.AppID;
        //}
        public string GetAppName(string idapp)
        {
            Idapp = idapp;
            SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
            return Nmapp;
        }
        public string GetAppTitle(string idapp)
        {
            //Ss00appControl dc = new Ss00appControl();
            //dc.Idapp = GlobalAsp.GetSessionApp();
            //dc=Ss00appLookupControl.FindAndSetValuesInto(dc);
            //return dc.Nmapp;
            return "Modul Administrasi";
        }
        #endregion
        #region IDataControlMenuMapClass
        public void LoadByRoleID(string roleid)
        {
            Ss00appmenuControl dcmenu = new Ss00appmenuControl();
            dcmenu.Idmenu = roleid;
            dcmenu.Load(BaseDataControl.ID);//Lebih Cepat
                                            //Ss00appmenuControl dcmenu = SsappmenuAllIdmenuLookupControl.GetListDataSingleton().Find(o => o.Idmenu.Equals(roleid));
            Olist1 = dcmenu.Olist1;
            Olistdetil1 = dcmenu.Olistdetil1;
            Olistdetil2 = dcmenu.Olistdetil2;
            Olistdetil3 = dcmenu.Olistdetil3;
            Olistdetil4 = dcmenu.Olistdetil4;
        }
        public void LoadLookupByRoleID(string roleid)
        {
            Ss01appmenuconfigLookupControl dc = new Ss01appmenuconfigLookupControl();
            dc.Idmenu = roleid;
            Olookuplist1 = (string)dc.Params["OLIST1"];
            Olookuplistdetil1 = (string)dc.Params["OLISTDETIL1"];
            Olookuplistdetil2 = (string)dc.Params["OLISTDETIL2"];
            Olookuplistdetil3 = (string)dc.Params["OLISTDETIL3"];
            Olookuplistdetil4 = (string)dc.Params["OLISTDETIL4"];
        }

        #endregion
    }
    #endregion Ss00appmenu
}

