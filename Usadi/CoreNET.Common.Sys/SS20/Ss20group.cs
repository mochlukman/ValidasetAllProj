using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss20groupControl, CoreNET.Common.BO
  [Serializable]
  public class Ss20groupControl : BaseDataControlSys, IDataControlTreeGrid3, IDataControlWebgroup, IHasJSScript, IExtLoadCsv
  {
    #region Properties 
    public string Kdgroup { get; set; }
    public string Nmgroup { get; set; }
    public string Urgroup { get; set; }
    public new int Kdlevel
    {
      get
      {
        base.Kdlevel = (string.IsNullOrEmpty(Kdgroup) ? 0 : Kdgroup.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length);
        return base.Kdlevel;
      }
      set => base.Kdlevel = value;
    }
    public ImageCommand[] Cmds
    {
      get
      {
        ImageCommand cmd1 = new ImageCommand()
        {
          CommandName = "ViewApp",
          Icon = Ext.Net.Icon.Table,
          ToolTip =
          {
            Text = "Klik tombol ini untuk menampilkan rincian modul"
          }
        };
        ImageCommand cmd2 = new ImageCommand()
        {
          CommandName = "ViewMenu",
          Icon = Ext.Net.Icon.TableAdd,
          ToolTip =
          {
            Text = "Klik tombol ini untuk menampilkan rincian menu"
          }
        };
        ImageCommand cmd3 = new ImageCommand()
        {
          CommandName = "ViewUser",
          Icon = Ext.Net.Icon.UserAdd,
          ToolTip =
          {
            Text = "Klik tombol ini untuk menampilkan rincian pengguna"
          }
        };
        return new ImageCommand[] { cmd1, cmd2, cmd3 };
      }
    }
    public string ViewApp => "Daftar Modul:" + UrlApp(string.Empty);
    public string ViewMenu => "Daftar Menu:" + UrlMenu(string.Empty);
    public string ViewUser => "Daftar Pengguna:" + UrlUser(string.Empty);
    public string UrlApp(string prefix)
    {
      string app = GlobalAsp.GetRequestApp();
      string id = "0582A3B4-3984-4306-9E0B-90DB20F86BA8";// GlobalAsp.GetRequestId();
      string idprev = GlobalAsp.GetRequestId();
      string kode = GlobalAsp.GetRequestKode();
      string idx = GlobalAsp.GetRequestIndex();
      string strenable = "&enable=" + ((Status == 0) ? 1 : 0);
      string url = $"{prefix}PageTabular.aspx?i=1&app={app}&id={id}&idprev={idprev}&kode={kode}&idx={idx}{strenable}";
      return url;
    }
    public string UrlMenu(string prefix)
    {
      string app = GlobalAsp.GetRequestApp();
      string id = "CEB0626A-9997-4315-9767-EC67925E4B62";
      string idprev = GlobalAsp.GetRequestId();
      string kode = GlobalAsp.GetRequestKode();
      string idx = GlobalAsp.GetRequestIndex();
      string strenable = "&enable=" + ((Status == 0) ? 1 : 0);
      string url = $"{prefix}PageTabular.aspx?i=1&app={app}&id={id}&idprev={idprev}&kode={kode}&idx={idx}{strenable}";
      return url;
    }
    public string UrlUser(string prefix)
    {
      string app = GlobalAsp.GetRequestApp();
      string id = "E733F1DD-4EAE-47E3-B8F1-ECA583F3146C";// GlobalAsp.GetRequestId();
      string idprev = GlobalAsp.GetRequestId();
      string kode = GlobalAsp.GetRequestKode();
      string idx = GlobalAsp.GetRequestIndex();
      string strenable = "&enable=" + ((Status == 0) ? 1 : 0);
      string url = $"{prefix}PageTabular.aspx?i=1&app={app}&id={id}&idprev={idprev}&kode={kode}&idx={idx}{strenable}";
      return url;
    }
    #endregion Properties 
    #region Methods 
    public Ss20groupControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUP;
    }

    private ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if (cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
      }
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Kdgroup" };
      cViewListProperties.IDKey = "Kdgroup";//IDKey for ID Notes
      cViewListProperties.IDProperty = "Kdgroup";//UniqueKey in gridview
      cViewListProperties.ReadOnlyFields = new String[] { };//Key in GetFilters should put here
      cViewListProperties.SortFields = new String[] { "Kdgroup" };//
      if (UtilityUI.GetModePage().Equals(UtilityUI.PAGE_TABULAR))
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_DEL;
      }
      else
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
      }
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
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
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stricon"), typeof(CommandColumn), Cmds, 10, HorizontalAlign.Center));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdgroup"), typeof(string), EditCmd, 30, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmgroup"), typeof(string), 30, HorizontalAlign.Left).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Urgroup"), typeof(string), 50, HorizontalAlign.Left).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdlevel"), typeof(int), 7, HorizontalAlign.Center).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Type"), typeof(string), 7, HorizontalAlign.Center).SetEditable(true));
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
      List<Ss20groupControl> ListData = new List<Ss20groupControl>();
      foreach (Ss20groupControl dc in list)
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
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdgroup"), false, 30).SetEnable(enable),
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nmgroup"), true, 95).SetEnable(enable),
          new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Urgroup"), true, 3).SetEnable(enable),
          new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          DmstatusLookupControl.GetListDataSingleton(string.Empty), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable),
          new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
            DmlevelLookupControl.GetData(string.Empty), "Kdlevel=Nmlevel", 30).SetAllowRefresh(false).SetEnable(false),
          new ParameterRowType(this, true),
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
        var GetApp = function (val) {
            return '" + UrlApp("Page/") + @"&val={0}'.format(val);
        };
        var GetMenu = function (val) {
            return '" + UrlMenu("Page/") + @"&val={0}'.format(val);
        };
        var GetUser = function (val) {
            return '" + UrlUser("Page/") + @"&val={0}'.format(val);
        };
      ";
      return new string[] { script };
    }
    public new string[] GetKeys()
    {
      return new string[] { "Kdgroup", "Urgroup"
        ,"Idapp","Kdapp","Nmapp"
        , "Status", "Statusicon", "Statusname", "Stricon"
        , "Last_by", "Last_date"
        , "Nmgroup", "Url", "Kdlevel", "Type" };
    }
    public new string[] GetCsvColumns(int mode)
    {
      return new string[] { "Kdgroup", "Nmgroup", "Urgroup", "Kdlevel", "Type" };
    }
    public new string[] GetLoadCsvColumns()
    {
      return new string[] { "Kdgroup", "Nmgroup", "Urgroup", "Kdlevel", "Type" };
    }
    #endregion Methods 
    #region IDataControlTreeGrid3
    public string ParentKdgroup { get => ParentKode(Kdgroup); set { } }
    public static bool IsRootCondition(Ss20groupControl dc)
    {
      return (dc.Kdlevel == 1);
    }
    public Icon GetIcon()
    {
      return Icon.Table;
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdgroup"), Width = 200, DataIndex = "Kdgroup", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmgroup"), Width = 300, DataIndex = "Nmgroup", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Urgroup"), Width = 480, DataIndex = "Urgroup", Align = Ext.Net.TextAlign.Left });
      #region Command
      TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 50, Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"
        <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Modul {Kdgroup}"",""Modul {Kdgroup}"", ""{[GetApp(values.Kdgroup)]}"");'>
          <img style='width: 16px; height: 16px; title='Rincian Modul' src='/icons/table-png/ext.axd'/>
        </a>
        <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Menu {Kdgroup}"",""Menu {Kdgroup}"", ""{[GetMenu(values.Kdgroup)]}"");'>
          <img style='width: 16px; height: 16px; title='Rincian Menu' src='/icons/table_go-png/ext.axd'/>
        </a>
        <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""User {Kdgroup}"",""User {Kdgroup}"", ""{[GetUser(values.Kdgroup)]}"");'>
          <img style='width: 16px; height: 16px; title='Rincian User' src='/icons/user_go-png/ext.axd'/>
        </a>
      ";
      Columns.Add(col1);
      #endregion
    }
    public Ext.Net.TreeNode CreateRoot(IList list, int typetree, bool withroot)
    {
      string delim_menu = GlobalAsp.DELIMITER_MENU;
      List<Ss20groupControl> localList = (List<Ss20groupControl>)list;
      List<Ss20groupControl> roots = localList.FindAll(i => IsRootCondition(i));

      Ext.Net.TreeNode root = new Ext.Net.TreeNode("Root", "Root", GetIcon());
      Ext.Net.TreeNodeCollection nodes = root.Nodes;
      foreach (Ss20groupControl ctrl in roots)
      {
        nodes.Add(CreateNodeWithOutChildren((List<Ss20groupControl>)list, ctrl, typetree, delim_menu));
      }
      return root;
    }
    public void LoadPages(IList inList, string nodeid, Ext.Net.TreeNodeCollection nodes, int typetree)
    {
      string delim_menu = ">";
      List<Ss20groupControl> list = (List<Ss20groupControl>)inList;
      if (list != null && list.Count > 0)
      {
        Ss20groupControl parent = list.Find(o => o.Kdgroup.Equals(nodeid));
        if ((parent != null) && (parent.Type.Equals("H")))
        {
          List<Ss20groupControl> children = GetChildren(list, parent);
          foreach (Ss20groupControl ctrl in children)
          {
            nodes.Add(CreateNodeWithOutChildren(list, ctrl, typetree, delim_menu));
          }
        }
      }
    }
    public static List<Ss20groupControl> GetChildren(List<Ss20groupControl> domainset, Ss20groupControl parent)
    {
      List<Ss20groupControl> children = domainset.FindAll(o => o.Kdgroup.StartsWith(parent.Kdgroup) && (o.Kdlevel == parent.Kdlevel + 1));
      return children;
    }
    protected TreeNodeBase CreateNodeWithOutChildren(List<Ss20groupControl> list, Ss20groupControl parent, int typetree, string delim_menu)
    {
      TreeNodeBase treeNode;
      List<Ss20groupControl> children = GetChildren(list, parent);
      if (children != null && children.Count > 0)
      {
        treeNode = (AsyncTreeNode)ExtTreeNode.GetExtTreeNode(parent.Kdgroup,
          ExtTreePanelUtil.GetUraian(parent.Nmgroup, new string[] { "Nmgroup", delim_menu }),
          typetree, GetKeys(), parent, true, Icon.Folder);
      }
      else
      {
        treeNode = ExtTreeNode.GetExtTreeNode(parent.Kdgroup,
          ExtTreePanelUtil.GetUraian(parent.Nmgroup, new string[] { "Nmgroup", delim_menu }),
          typetree, GetKeys(), parent, false, Icon.Table);
      }
      return treeNode;
    }
    #endregion IDataControlTreeGrid3
    #region Sum All
    public static void Update(List<Ss20groupControl> domainset)
    {
      List<Ss20groupControl> roots = domainset.FindAll(i => IsRootCondition(i));
      if (roots.Count > 0)
      {
        foreach (Ss20groupControl r in roots)
        {
          UpdatedRecFields bobotProgress = SumChildren(domainset, r);
          r.SetValue(bobotProgress);
        }
      }
    }

    private static UpdatedRecFields SumChildren(List<Ss20groupControl> domainsetParent, Ss20groupControl parent)
    {
      UpdatedRecFields localBobotProgress = new UpdatedRecFields();
      List<Ss20groupControl> localDomainset = domainsetParent.FindAll(o => o.Kdgroup.StartsWith(parent.Kdgroup));

      List<Ss20groupControl> children = GetChildren(localDomainset, parent);
      int localNumChildren = 0;
      if ((children.Count > 0) && (parent.Type == "H"))
      {
        localBobotProgress.Nchild = children.Count;
        foreach (Ss20groupControl o in children)
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
    }

    private class UpdatedRecFields
    {
      public int Nchild = 0;

      public void Sum(UpdatedRecFields rec)
      {
        Nchild += rec.Nchild;
      }

      public void SetValue(Ss20groupControl rec)
      {
        Nchild = rec.Nchild;
      }
    }
    #endregion
    #region IDataControlWebgroup
    public IList GetListData()
    {
      return Ss20groupLookupControl.GetListDataSingleton();
    }

    public IDataControlWebgroup GetSelectionObject(string groupid)
    {
      Kdgroup = groupid;
      return Ss20groupLookupControl.FindAndSetValuesInto(this);
    }
    //Available Parameters
    public IList GetAvailableParams()
    {
      return null;// SsconfigparamsLookupControl.GetListDataSingleton();
    }
    public bool IsValidParams(string par)
    {
      bool valid = false;// SsconfigparamsLookupControl.GetListDataSingleton().Exists(o => o.Par.Equals(par));
      if (!valid)
      {
        string msg = ConstantDict.Translate("LBL_UNDEFINED_PARAMETER");
        if (!msg.Contains("{0}"))
        {
          msg += " => {0}";
        }
        throw new Exception(string.Format(msg, par));
      }
      return valid;
    }
    public string GetGroupConfigs(string par)
    {
      if (IsValidParams(par))
      {
        string kdmenu = string.Empty;
        Ss20groupconfigControl dc = null;
        //dc = Ss20groupconfigLookupControl.GetListDataSingleton().Find(o => o.Kdgroup.Equals(Kdgroup) && o.Prefixkdmenu.Equals(kdmenu) && o.Par.Equals(par));
        if (dc != null)
        {
          return dc.Value;
        }
        else
        {
          return string.Empty;
        }
      }
      return string.Empty;
    }
    public string GetGroupConfigs(string kdmenu, string par)
    {
      if (IsValidParams(par))
      {
        Ss20groupconfigControl dc = null;
        //dc = Ss20groupconfigControl.GetListDataSingleton().Find(o => o.Kdgroup.Equals(Kdgroup) && o.Prefixkdmenu.Equals(kdmenu) && o.Par.Equals(par));
        if (dc == null)
        {
          //dc = Ss20groupconfigControl.GetListDataSingleton().Find(o => o.Kdgroup.Equals(Kdgroup) && kdmenu.StartsWith(o.Prefixkdmenu) && o.Par.Equals(par));
        }
        if (dc != null)
        {
          return dc.Value;
        }
        else
        {
          return string.Empty;
        }
      }
      return string.Empty;
    }
    public string GetGroupID()
    {
      return Kdgroup;
    }

    public void SetGroupID(string groupid)
    {
      Kdgroup = groupid;
    }

    public void SetUserID(string userid)
    {
      Userid = userid;
      //Ss20groupLookupControl.SetSessionListData(Userid);
    }
    public void GetDefaultUrl(out string url, out string kdmenu)
    {
      string par = "DEFAULT_MENU";
      string groupid = GlobalExt.GetSessionGroup().GetGroupID();

      kdmenu = string.Empty;
      if (!string.IsNullOrEmpty(groupid))
      {
        Ss20groupconfigControl dc = Ss20groupconfigLookupControl.GetListDataSingleton().Find(o => o.Kdgroup.Equals(groupid) && o.Par.Equals(par));

        if (dc != null)
        {
          kdmenu = dc.Value;
          url = Ss20groupmenuLookupControl.GetURL(groupid, kdmenu);
        }
        else
        {
          X.Msg.Alert(ConstantDict.Translate(GlobalExt.LBL_INFO),
            string.Format(ConstantDict.Translate("LBL_NOCONFIG"), par, groupid)).Show();
          url = GlobalExt.GetBlankURL();
        }
      }
      else
      {
        url = GlobalExt.GetBlankURL();
      }
    }

    public IDataControlUI GetObjTahun()
    {
      string kdtahun = DateTime.Now.Year.ToString();
      DmtahunUIControl dc = DmtahunUILookupControl.GetListDataSingleton().Find(o => o.Kdtahun.Equals(kdtahun));
      return dc;
    }

    public IList GetListTahun()
    {
      return DmtahunLookupControl.GetListDataSingleton();
    }

    public string[] GetFieldValueTahun()
    {
      return DmtahunLookupControl.GetFieldValueProps();
    }

    public string[] GetFieldValueGroup()
    {
      return Ss20groupLookupControl.GetFieldValueProps();
    }

    public string GetMenuTemplate()
    {
      string html = @"
      <tpl for=""."">
	      <tpl if=""[xindex] == 1"">
		      <table class=""cbStates-list"">
			      <tr>
				      <th width=""70px""><center>Kode</center></th>
				      <th><center>Status</center></th>
				      <th width=""110px""><center>Uraian</center></th>
				      <th width=""70px""><center>Pemda</center></th>
				      <th width=""250px""><center>SKPD</center></th>
			      </tr>
	      </tpl>
	      <tr class=""list-item"">
		      <td style=""padding:3px 0px;"">{[values.Kdgroup]}</td>
		      <td><center>{Status}</center></td>
		      <td>{Nmgroup}</td>
		      <td>{Kdpemda}</td>
		      <td>{Kdnmunit}</td>
	      </tr>
	      <tpl if=""[xcount-xindex]==0"">
		      </table>
	      </tpl>
      </tpl>
    ";
      return html;
    }
    #endregion  }
  }
  #endregion Ss20group
}

