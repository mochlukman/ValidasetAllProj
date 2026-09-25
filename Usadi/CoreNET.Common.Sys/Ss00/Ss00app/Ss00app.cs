using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region Ss00app
  [Serializable]
  public class Ss00appControl : SsappControl, IDataControlApp, IDataControlTreeGrid3, IExtLoadCsv
  {
    #region Properties 
    private static ImageCommand _EditCmd = null;
    public static ImageCommand EditCmd
    {
      get
      {
        if (_EditCmd == null)
        {
          _EditCmd = new ImageCommand
          {
            CommandName = "EditForm",
            Icon = Ext.Net.Icon.Pencil
          };
          _EditCmd.ToolTip.Text = "Klik untuk menampilkan form edit";
        }
        return _EditCmd;
      }
    }
    public ImageCommand[] Cmds
    {
      get
      {
        ImageCommand cmd1 = new ImageCommand()
        {
          CommandName = "ViewMenu",
          Icon = Ext.Net.Icon.TableAdd
        };
        cmd1.ToolTip.Text = "Klik tombol ini untuk menampilkan rincian menu";
        ImageCommand cmd2 = new ImageCommand()
        {
          CommandName = "ViewUser",
          Icon = Ext.Net.Icon.UserAdd
        };
        cmd2.ToolTip.Text = "Klik tombol ini untuk menampilkan daftar pengguna";
        ImageCommand cmd3 = new ImageCommand()
        {
          CommandName = "ViewConfig",
          Icon = Ext.Net.Icon.Wrench
        };
        cmd2.ToolTip.Text = "Klik tombol ini untuk menampilkan konfigurasi";
        return new ImageCommand[] { cmd1, cmd2, cmd3 };
      }
    }

    #endregion Properties 
    #region Property Debug
    public string GetDefaultDebug()
    {
      return Debug;
    }
    public static string GetStaticDefaultDebug(IDataControl dc)
    {
      try
      {
        IDataControlUI dcmaster = UtilityUI.GetDataControl(1);
        IDataControlUI dcdetil1 = null;
        IDataControlUI dcdetil2 = null;
        IDataControlUI dcdetil3 = null;
        IDataControlUI dcdetil4 = null;
        string strdcdetil = string.Empty;
        try
        {
          dcdetil1 = UtilityUI.GetDataControl(11);
          strdcdetil += dcdetil1.GetType().AssemblyQualifiedName;
          dcdetil2 = UtilityUI.GetDataControl(12);
          strdcdetil += "; " + dcdetil2.GetType().AssemblyQualifiedName;
          dcdetil3 = UtilityUI.GetDataControl(13);
          strdcdetil += "; " + dcdetil3.GetType().AssemblyQualifiedName;
          dcdetil4 = UtilityUI.GetDataControl(14);
          strdcdetil += "; " + dcdetil4.GetType().AssemblyQualifiedName;
        }
        catch (Exception)
        {
          //StackOverFlow
          //UtilityBO.Log(dc, ex);
        }

        string idmenu = GlobalAsp.GetRequestId();

        string str = @"
                [BaseDataControlUI]<ul>
                   <li>dcMaster = {0}</li>
                   <li>dcDetil = {1}</li>
                   <li>url = " + HttpUtility.HtmlDecode(HttpContext.Current.Request.UrlReferrer.OriginalString) + @"</li>
                   <li>UserClass=" + GlobalAsp.GetSessionAppValue(MasterAppConstants.USERDC) + @"</li>
                   <li>MenuClass=" + GlobalAsp.GetSessionAppValue(MasterAppConstants.MENUDC) + @"</li>
                   <li>idApp = " + GlobalAsp.GetSessionApp() + @"</li>
                   <li>idMenu = " + GlobalAsp.GetRequestId() + @"</li>
                   <li>user = " + ((GlobalAsp.GetSessionUser() != null) ? GlobalAsp.GetSessionUser().GetUserID() : string.Empty) + @"</li>
                   <li>usertype = " + ((GlobalAsp.GetSessionUser() != null) ? GlobalAsp.GetSessionUser().GetUserType() : string.Empty) + @"</li>
                   <li>userdebug = " + (GlobalAsp.GetSessionUser() != null ? GlobalAsp.GetSessionUser().GetUserDebugInfo() : string.Empty) + @"</li>
                   <li>kdgroup = " + ((GlobalAsp.GetSessionGroup() != null) ? GlobalAsp.GetSessionGroup().GetGroupID() : string.Empty) + @"</li>
                </ul>[/BaseDataControlUI] <br/>
              " + BaseBO.GetStaticDefaultDebug((BaseBO)dc);
        return string.Format(str
          , (dcmaster != null) ? dcmaster.GetType().AssemblyQualifiedName : string.Empty
          , strdcdetil
          );
      }
      catch (Exception ex)
      {
        return "<br/>[BaseDataContrlUI]<br/>" + ex.Message + "<br/>[/BaseDataContrlUI]<br/>";
      }
    }
    public new string Debug
    {
      get => _Debug + GetStaticDefaultDebug(this);
      set => _Debug = value;
    }
    #endregion
    public DataControlFieldCollection columns = null;
    public void SetColumnsNull()
    {
      columns = null;
    }

    #region Methods 
    public Ss00appControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APP;
      SetMaxModePreviewIndex(1);
    }

    private ViewListProperties cViewListProperties = null;
    public IProperties GetProperties()
    {
      if (cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)ViewListProperties.CreateDefaultProperties();
        cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
        cViewListProperties.PrimaryKeys = new String[] { "Kdapp" };
        cViewListProperties.IDProperty = "Kdapp";
        cViewListProperties.IDKey = "Idapp";
        cViewListProperties.ReadOnlyFields = new String[] { "Userid" };
        cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
        cViewListProperties.AllowKeepFormExpanded = true;
        if (MasterAppConstants.Instance.StatusAdmin)
        {
          cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
        }
        else
        {
          cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
        }
      }
      return cViewListProperties;
    }
    public DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection
      {
        Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center)
      };
      if (MasterAppConstants.Instance.StatusAdmin)
      {
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Stricon"), typeof(CommandColumn), Cmds, 10, HorizontalAlign.Center));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdapp"), typeof(string), EditCmd, 20, HorizontalAlign.Left));
      }
      else
      {
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdapp"), typeof(string), 20, HorizontalAlign.Left));
      }
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmapp"), typeof(string), 35, HorizontalAlign.Left).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Urapp"), typeof(string), 40, HorizontalAlign.Left).SetEditable(true));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Idapp"), typeof(string), 40, HorizontalAlign.Left));
      if (ModePreviewIndex == 1)
      {
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdlevel"), typeof(int), 7, HorizontalAlign.Center).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Type"), typeof(string), 7, HorizontalAlign.Center).SetEditable(true));
      }
      return columns;
    }
    public new void SetPageKey()
    {
      Last_by = GlobalAsp.GetSessionUser().GetUserID();
      Last_date = DateTime.Now;
    }

    public new void SetCopyRowKey()
    {
      Idapp = Guid.NewGuid().ToString();
    }
    public new void SetPrimaryKey()
    {
      Idapp = Guid.NewGuid().ToString();
      if (UtilityUI.IsModePageTree())
      {
        Kdapp = ((Ss00appControl)GlobalAsp.GetEditingObject()).Kdapp + "XX.";
        Kdlevel = Kdapp.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries).Length;
        Type = "D";
        Status = 0;
      }
    }

    public void SetFilterKey(BaseBO bo)
    {
      if (typeof(Ss10userappControl).IsInstanceOfType(bo))
      {
        Kdapp = ((Ss10userappControl)bo).Kdapp;
        if (typeof(Ss10userappByUserControl).IsInstanceOfType(bo))
        {
          Userid = ((Ss10userappControl)bo).Userid;
        }
      }
    }
    public HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }
    public new IList View()
    {
      SsappLookupControl.SetListDataNull();

      List<SsappControl> list = null;
      string url = HttpContext.Current.Request.Url.OriginalString;
      list = (List<SsappControl>)View(BaseDataControl.ALL);
      if (url.Contains("PageTabular.aspx"))
      {
        if (MasterAppConstants.Instance.StatusAdmin)
        {
          list = list.FindAll(o => o.Type.Equals("D"));
        }
        else
        {
          list = list.FindAll(o => o.Type.Equals("D") && o.Last_by.Equals(GlobalAsp.GetSessionUser().GetUserID()));
        }
      }
      else
      {
        if (!MasterAppConstants.Instance.StatusAdmin)
        {
          list = new List<SsappControl>();
        }
      }
      return list;
    }
    public new IList View(string label)
    {
      IList list = base.View(label);
      List<SsappControl> ListData = new List<SsappControl>();
      foreach (SsappControl dc in list)
      {
        //string url = dc.Url;
        //dc.UrlFull = string.Format(url + "?app={0}&key{1}&sub=1", dc.Idapp, GlobalAsp.GetRequestKey());
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }

    public new void Insert()
    {
      base.Insert();
      #region Update Level Parent
      Ss00appControl dcParent = (Ss00appControl)GlobalAsp.GetEditingObject();
      if (Kdlevel == dcParent.Kdlevel + 1)
      {
        dcParent.Type = "H";
        dcParent.Update();
      }
      #endregion

      string sql = $@"exec RevalidateSS00AppConfig '{Idapp}'";
      BaseDataAdapter.ExecuteCmd(this, sql);
      SsappLookupControl.SetListDataNull();
      Ss00appconfigLookupControl.SetSessionListDataNull();
    }

    public new int Update()
    {
      int n = 0;
      n = base.Update();
      SsappLookupControl.SetListDataNull();
      return n;
    }

    public string ViewMenu
    {
      get
      {
        Dictionary<string, object> Props = GetPageMasterProperties();
        return ConstantDict.Translate("Erpappmenu") + " " + Kdapp + ":" + ((string[])Props[PageMasterProperties.URLDETILS])[0];
      }
    }
    public string ViewUser
    {
      get
      {
        Dictionary<string, object> Props = GetPageMasterProperties();
        return ConstantDict.Translate("Erpuserapp") + " " + Kdapp + ":" + ((string[])Props[PageMasterProperties.URLDETILS])[1];
      }
    }
    public string ViewConfig
    {
      get
      {
        Dictionary<string, object> Props = GetPageMasterProperties();
        return ConstantDict.Translate("Erpappconfig") + " " + Kdapp + ":" + ((string[])Props[PageMasterProperties.URLDETILS])[2];
      }
    }
    public HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdapp"), true, 30).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nmapp"), true, 95).SetEnable(enable),
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Urapp"), true, 3).SetEnable(enable),
        new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Idapp"), true, 70).SetEnable(false),
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
    public Dictionary<string, object> GetPageMasterProperties()
    {
      string app = GlobalAsp.GetRequestApp();
      string id = GlobalAsp.GetRequestId();
      string idprev = GlobalAsp.GetRequestIdPrev();
      string kode = GlobalAsp.GetRequestKode();
      string idx = GlobalAsp.GetRequestIndex();
      string strenable = "&enable=" + ((Status == 0) ? 1 : 0);

      Dictionary<string, object> Props = new Dictionary<string, object>
      {
        [PageMasterProperties.URLMASTER] = string.Format("PageTabular.aspx?passdc=1&app={0}&i={1}&id={2}&idprev={3}&kode={4}&idx={5}&child=1&val={5}", app, 1, id, idprev, kode, idx, GlobalAsp.GetRequestVal()),
        [PageMasterProperties.URLDETILS] = new string[] {
          string.Format("PageTabular.aspx?passdc=1&app={0}&i={1}&id={2}&idprev={3}&kode={4}&idx={5}"+strenable, app, 11, id, idprev,kode,idx)
          ,string.Format("PageTabular.aspx?passdc=1&app={0}&i={1}&id={2}&idprev={3}&kode={4}&idx={5}"+strenable, app, 12, id, idprev,kode,idx)
          ,string.Format("PageTabular.aspx?passdc=1&app={0}&i={1}&id={2}&idprev={3}&kode={4}&idx={5}"+strenable, app, 13, id, idprev,kode,idx)
        }
      };
      return Props;
    }
    #endregion Methods 
    #region BaseDataControlUI
    public string GetURLReport(object config)
    {
      return string.Empty;//Default HTML Report
    }
    public new int Delete()
    {
      if (Nchild > 0)
      {
        throw new Exception(ConstantDict.Translate("LBL_HAVE_CHILDREN"));
      }
      if (Rating > 0)
      {
        throw new Exception(ConstantDict.Translate("LBL_APPROVED_DATA"));
      }
      if (Status > 0)
      {
        throw new Exception(ConstantDict.Translate("LBL_STATUS_CANNOT_DELETED"));
      }
      if (Valid)
      {
        throw new Exception(ConstantDict.Translate("LBL_STATUS_VALID"));
      }
      return Delete(BaseDataControl.DEFAULT);
    }
    public new String[] GetFields()
    {
      ArrayList arListNotFields = new ArrayList(GetNotFields());
      return GetFields(arListNotFields);
    }
    public void FilterClick(string key)
    {

    }
    #endregion
    #region BaseDataControlUIEntry Members
    public void AfterInsert()
    {

    }
    public void AfterUpdate()
    {

    }
    public void AfterDelete()
    {

    }
    public string GetDestinationFile(string filename)
    {
      string destfile = "File\\" + filename;
      return destfile;
    }
    public string GetURLFile(string filename, string versi)
    {
      string url = "File/" + filename;
      return url;
    }
    public Exception ValidateTotal()
    {
      return null;
    }
    public bool IsEditable()
    {
      return Editable;
    }
    #endregion
    #region IDataControlTreeGrid3
    public void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdapp"), Width = 200, DataIndex = "Kdapp", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmapp"), Width = 500, DataIndex = "Nmapp", Align = Ext.Net.TextAlign.Left });
    }
    public static bool IsRootCondition(Ss00appControl dc)
    {
      return (dc.Kdlevel == 1);
    }
    public Icon GetIcon()
    {
      return Ext.Net.Icon.Table;
    }
    public Ext.Net.TreeNode CreateRoot(IList list, int typetree, bool withroot)
    {
      string delim_menu = GlobalExt.DELIMITER_MENU;
      List<SsappControl> localList = (List<SsappControl>)list;
      List<SsappControl> roots = localList.FindAll(i => IsRootCondition(i));

      Ext.Net.TreeNode root = new Ext.Net.TreeNode("Menu", "Menu", GetIcon());
      Ext.Net.TreeNodeCollection nodes = root.Nodes;
      foreach (SsappControl ctrl in roots)
      {
        nodes.Add(CreateNodeWithOutChildren((List<SsappControl>)list, ctrl, typetree, delim_menu));
      }
      return root;
    }
    public void LoadPages(IList inList, string nodeid, Ext.Net.TreeNodeCollection nodes, int typetree)
    {
      string delim_menu = ">";
      List<SsappControl> list = (List<SsappControl>)inList;
      if (list != null && list.Count > 0)
      {
        SsappControl parent = list.Find(o => o.Kdapp.Equals(nodeid));
        if ((parent != null) && (parent.Type.Equals("H")))
        {
          List<SsappControl> children = GetChildren(list, parent);
          foreach (SsappControl ctrl in children)
          {
            nodes.Add(CreateNodeWithOutChildren(list, ctrl, typetree, delim_menu));
          }
        }
      }
    }
    public static new List<SsappControl> GetChildren(List<SsappControl> domainset, SsappControl parent)
    {
      List<SsappControl> children = domainset.FindAll(o => o.Kdapp.StartsWith(parent.Kdapp) && (o.Kdlevel == parent.Kdlevel + 1));
      return children;
    }
    protected TreeNodeBase CreateNodeWithOutChildren(List<SsappControl> list, SsappControl parent, int typetree, string delim_menu)
    {
      TreeNodeBase treeNode;
      List<SsappControl> children = GetChildren(list, parent);
      if (children != null && children.Count > 0)
      {
        treeNode = (AsyncTreeNode)ExtTreeNode.GetExtTreeNode(parent.Kdapp,
          ExtTreePanelUtil.GetUraian(parent.Nmapp, new string[] { "Nmapp", delim_menu }),
          typetree, GetKeys(), parent, true, Ext.Net.Icon.Folder);
      }
      else
      {
        treeNode = ExtTreeNode.GetExtTreeNode(parent.Kdapp,
          ExtTreePanelUtil.GetUraian(parent.Nmapp, new string[] { "Nmapp", delim_menu }),
          typetree, GetKeys(), parent, false, Ext.Net.Icon.Table);
      }
      return treeNode;
    }
    #endregion IDataControlTreeGrid3
    #region IExtLoadCsv
    public string[] GetLoadCsvColumns()
    {
      string[] csvcolumns = SysgetkeysLookupControl.GetLoadCSVCols(this);
      return csvcolumns;
    }

    public string[] GetCsvColumns(int mode)
    {
      string[] keys = SysgetkeysLookupControl.GetCSVCols(this);
      if (keys.Length == 0)
      {
        keys = GetKeys();
      }
      return keys;
    }

    public IList GetList(int id, int mode)
    {
      return GlobalAsp.GetSessionListRows(id);
    }
    public Window GetLoadCsvWindow()
    {
      return new WindowLoadCSV("0", "Load Menu", string.Format("~/Page/CSVUpload.aspx"), 400, 200);
    }
    public static void ExecutedSQL(IDataControlUI dc, string tname, DataTable csvData, int counter, bool isdelete)
    {
      if (csvData.Rows.Count > 0)
      {
        DataRow dr = csvData.Rows[counter];
        #region Insert Row
        try
        {
          string[] cols = ((ILoadCsv)dc).GetLoadCsvColumns();
          for (int i = 0; i < cols.Length; i++)
          {
            string[] maps = cols[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
            if (maps.Length > 1)
            {
              dc.SetValue(maps[0], dr[maps[1]]);
            }
            else
            {
              dc.SetValue(maps[0], dr[maps[0]]);
            }
          }
          dc.SetPageKey();
          //IDataControl dctes = (IDataControl)((BaseBO)dc).Clone();
          IDataControl dctes = (IDataControl)BaseBO.Clone(dc);
          if (dctes.Load() == null)
          {
            dc.SetPrimaryKey();
            dc.Insert();
          }
          else
          {
            if (isdelete)
            {
              dc.SetValue("STATUS", -1);
              dc.Delete();
              dc.SetPageKey();
              dc.SetPrimaryKey();
              dc.Insert();
            }
            else
            {
              dc.Update();
            }
          }
        }
        catch (Exception ex)
        {
          try
          {
            UtilityBO.Log(dc, ex);
            dc.Update();
          }
          catch (Exception ex1)
          {
            UtilityBO.Log(dc, ex1);
          }
        }
        #endregion
      }
    }
    #endregion
    #region IDataControlMenu
    public void SetOtorisasiMenu(Page page, string idapp)
    {
      Idapp = idapp;
      SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
    }

    public Ext.Net.TreeNode CreateMenu(IList list, int typetree, bool withroot)
    {
      List<SsappControl> localList = (List<SsappControl>)list;
      List<SsappControl> roots = localList.FindAll(i => IsRootCondition(i));

      Ext.Net.TreeNode root = new Ext.Net.TreeNode("Menu", "Menu", GetIcon());
      Ext.Net.TreeNodeCollection nodes = root.Nodes;
      foreach (SsappControl ctrl in roots)
      {
        nodes.Add(CreateNodeWithChildren((List<SsappControl>)list, ctrl));
      }
      return root;
    }
    private Ext.Net.TreeNode CreateNodeWithChildren(List<SsappControl> list, SsappControl parent)
    {
      Ext.Net.TreeNode treeNode = null;
      if (UtilityUI.GetModePage() == UtilityUI.MAIN_MENU)
      {
        treeNode = (Ext.Net.TreeNode)ExtTreeNode.GetExtTreeNode(parent.Kdapp,
          ExtTreePanelUtil.GetUraian(parent.Kdnmapp),
          ExtTreePanelUtil.TYPE_TREEMENU, GetKeys(), parent, Ext.Net.Icon.Table);
      }
      else
      {
        treeNode = (Ext.Net.TreeNode)ExtTreeNode.GetExtTreeNode(parent.Kdapp,
          ExtTreePanelUtil.GetUraian(parent.Nmapp),
          ExtTreePanelUtil.TYPE_TREEMENU, GetKeys(), parent, Ext.Net.Icon.Table);
      }

      string url = parent.UrlFull;
      if (UtilityUI.GetModePage() == UtilityUI.PAGE_MENU)
      {
        url = url.Replace("Page/", "");
      }
      treeNode.Href = url;

      List<SsappControl> children = GetChildren(list, parent);
      for (int i = 0; i < children.Count; i++)
      {
        SsappControl child = children[i];
        treeNode.Nodes.Add(CreateNodeWithChildren(list, child));
      }
      return treeNode;
    }

    public IList GetListRef()
    {
      return null;
    }

    public Ext.Net.Menu GetMenu(IList list, short kdlevel)
    {
      return null;
    }

    public string GetRoleid()
    {
      return Kdapp;
    }

    public string GetURL()
    {
      return Url;
    }

    public string GetURLReal()
    {
      return UrlFull;
    }

    public string GetAppTitle(string idapp)
    {
      return Urapp;
    }

    public string GetAppName(string idapp)
    {
      return Nmapp;
    }

    private Ss00appmenuControl dcmenu = null;
    public void LoadByRoleID(string roleid)
    {
      Ss01appmenuControl dcmenu1 = new Ss01appmenuControl
      {
        Idmenu = roleid
      };
      BaseBO dcresult = dcmenu1.Load(BaseDataControl.ID);//Lebih Cepat
      if (dcresult == null)
      {
        Ss00appmenuControl dcmenu0 = new Ss00appmenuControl
        {
          Idmenu = roleid
        };
        dcresult = dcmenu0.Load(BaseDataControl.ID);//Lebih Cepat
      }
      dcmenu.CopyPropertyBOFrom(dcresult);
      //dcmenu = SsappmenuAllIdmenuLookupControl.GetListDataSingleton().Find(o => o.Idmenu.Equals(roleid));
    }

    public void LoadLookupByRoleID(string roleid)
    {
      dcmenu = SsappmenuAllIdmenuLookupControl.GetListDataSingleton().Find(o => o.Idmenu.Equals(roleid));
    }

    public string GetOList1()
    {
      return dcmenu.Olist1;
    }

    public string GetOListDetil1()
    {
      return dcmenu.Olistdetil1;
    }

    public string GetOListDetil2()
    {
      return dcmenu.Olistdetil2;
    }

    public string GetOListDetil3()
    {
      return dcmenu.Olistdetil3;
    }

    public string GetOListDetil4()
    {
      return dcmenu.Olistdetil4;
    }

    public string GetLookupOList1()
    {
      return dcmenu.Olookuplist1;
    }

    public string GetLookupOListDetil1()
    {
      return dcmenu.Olookuplistdetil1;
    }

    public string GetLookupOListDetil2()
    {
      return dcmenu.Olookuplistdetil2;
    }

    public string GetLookupOListDetil3()
    {
      return dcmenu.Olookuplistdetil3;
    }

    public string GetLookupOListDetil4()
    {
      return dcmenu.Olookuplistdetil4;
    }

    #endregion
  }
  #endregion Ss00app
}

