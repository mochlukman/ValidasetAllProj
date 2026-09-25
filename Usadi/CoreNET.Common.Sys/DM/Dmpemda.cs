using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.DmpemdaControl, CoreNET.Common.BO
  [Serializable]
  public class DmpemdaControl : BaseDataControlSys, IDataControlTreeGrid3, IHasJSScript
  {
    #region Properties 
    public string Akropemda { get; set; }
    public bool Aktif { get; set; }
    public string Almtpemda { get; set; }
    public string Emailpemda { get; set; }
    public string Kdblok { get; set; }
    public string Kdpack { get; set; }
    public string Nmpack { get; set; }
    public string Kdpemda { get; set; }
    public string Nmpemda { get; set; }
    public string Satker { get; set; }
    public DateTime Tglakhir { get; set; }
    public DateTime Tglawal { get; set; }
    public string Tglawalstr
    {
      get
      {
        if (Tglawal == new DateTime())
        {
          return string.Empty;
        }
        else
        {
          return Tglawal.ToString("dd/MM/yyyy");
        }
      }
    }
    public string Tglakhirstr
    {
      get
      {
        if (Tglakhir == new DateTime())
        {
          return string.Empty;
        }
        else
        {
          return Tglakhir.ToString("dd/MM/yyyy");
        }
      }
    }
    public string Tlpnpemda { get; set; }
    public string Urpemda { get; set; }
    public string Webpemda { get; set; }
    #endregion Properties 

    #region Methods 
    public DmpemdaControl()
    {
      XMLName = ConstantTablesSys.XMLDMPEMDA;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Kdpemda" };
      cViewListProperties.IDKey = "Kdpemda";//IDKey for ID Notes
      cViewListProperties.IDProperty = "Kdpemda";//UniqueKey in gridview
      cViewListProperties.ReadOnlyFields = new String[] { };//Key in GetFilters should put here
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      if (GlobalAsp.GetSessionApp().Equals(MasterAppConstants.Instance.MasterAppID))
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL_LOADCSV;
      }
      else
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      }
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        Last_by = ((BaseBO)bo).Userid;
      }
    }
    public new void SetPrimaryKey()
    {
      //Id = Guid.NewGuid();
    }
    public new IList View()
    {
      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<DmpemdaControl> ListData = new List<DmpemdaControl>();
      foreach (DmpemdaControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        if (((Ss01appmenuControl)GlobalExt.GetSessionMenu()).Kdapp.StartsWith("01"))
        {
          if (dc.Kdpemda.StartsWith("1"))
          {
            ListData.Add(dc);
          }
        }
        else
        {
          ListData.Add(dc);
        }
      }
      //Update(ListData);
      return ListData;
    }
    public override DataControlFieldCollection GetColumns()
    {
      bool isDev = (GlobalAsp.GetSessionApp().Equals(MasterAppConstants.Instance.MasterAppID));
      if (columns == null)
      {
        columns = new DataControlFieldCollection
        {
          Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center)
        };
        if (isDev)
        {
          columns.Add(ExtFields.GetRatingField());
        }
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdpemda"), typeof(string), 10, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Satker"), typeof(string), 10, HorizontalAlign.Left));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmpemda"), typeof(string), 30, HorizontalAlign.Left).SetEditable(isDev));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Urpemda"), typeof(string), 10, HorizontalAlign.Left).SetEditable(isDev));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Akropemda"), typeof(string), 10, HorizontalAlign.Left).SetEditable(isDev));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Almtpemda"), typeof(string), 10, HorizontalAlign.Left).SetEditable(isDev));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Tlpnpemda"), typeof(string), 10, HorizontalAlign.Left).SetEditable(isDev));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Webpemda"), typeof(string), 10, HorizontalAlign.Left).SetEditable(isDev));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Emailpemda"), typeof(string), 10, HorizontalAlign.Left).SetEditable(isDev));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdblok"), typeof(string), 5, HorizontalAlign.Left).SetEditable(isDev));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Tglawalstr"), typeof(string), 13, HorizontalAlign.Center));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Tglakhirstr"), typeof(string), 13, HorizontalAlign.Center));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Aktif"), typeof(bool), 5, HorizontalAlign.Left).SetEditable(isDev));
      }
      return columns;
    }
    public const string GROUP_INFO = "Info Pemda";
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      if (hpars == null)
      {
        hpars = new HashTableofParameterRow
        {
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdpemda"), false, 30).SetEnable(false),
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nmpemda"), true, 95).SetEnable(false),
          new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Urpemda"), true, 3).SetEnable(enable).SetGroup(GROUP_INFO),
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Akropemda"), true, 50).SetEnable(enable).SetGroup(GROUP_INFO),
          new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Almtpemda"), true, 3).SetEnable(enable).SetGroup(GROUP_INFO),
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Tlpnpemda"), true, 50).SetEnable(enable).SetGroup(GROUP_INFO),
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Webpemda"), true, 50).SetEnable(enable).SetGroup(GROUP_INFO),
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Emailpemda"), true, 50).SetEnable(enable).SetGroup(GROUP_INFO),
          new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Kdblok"), false, 30).SetEnable(enable).SetGroup(GROUP_INFO),
          new ParameterRowDate(this, ConstantDict.GetColumnTitle("Tglawal"), true).SetEnable(enable).SetGroup(GROUP_INFO),
          new ParameterRowDate(this, ConstantDict.GetColumnTitle("Tglakhir"), true).SetEnable(enable).SetGroup(GROUP_INFO),
          new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          DmstatusLookupControl.GetListDataSingleton(string.Empty), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable),
          new ParameterRowUploadFile(this, true),
          new ParameterRowHelp(this, true),
          new ParameterRowForum(this, true)
        };
      }
      return hpars;
    }
    public new string[] GetKeys()
    {
      return new string[] { "Kdpemda", "Satker", "Urpemda", "Akropemda", "Almtpemda"
        , "Tlpnpemda", "Webpemda", "Emailpemda", "Kdblok"
        , "Tglawal", "Tglakhir", "Tglawalstr", "Tglakhirstr", "Aktif"
        , "Status", "Last_by", "Last_date", "Statusicon", "Nmpemda", "Url", "Kdlevel", "Type" };
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdpemda"), Width = 150, DataIndex = "Kdpemda", Align = Ext.Net.TextAlign.Left });
      //Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Satker"), Width = 100, DataIndex = "Satker", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmpemda"), Width = 500, DataIndex = "Nmpemda", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Akropemda"), Width = 150, DataIndex = "Akropemda", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Tlpnpemda"), Width = 150, DataIndex = "Tlpnpemda", Align = Ext.Net.TextAlign.Left });
      //Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdblok"), Width = 100, DataIndex = "Kdblok", Align = Ext.Net.TextAlign.Left });
      //Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Tglawalstr"), Width = 75, DataIndex = "Tglawal", Align = Ext.Net.TextAlign.Left });
      //Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Tglakhirstr"), Width = 75, DataIndex = "Tglakhir", Align = Ext.Net.TextAlign.Left });
      //Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Aktif"), Width = 50, DataIndex = "Aktif", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Status"), Width = 50, DataIndex = "Status", Align = Ext.Net.TextAlign.Center });
      if (GetModePreviewIndex() == 1)
      {
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdlevel"), Width = 300, DataIndex = "Kdlevel", Align = Ext.Net.TextAlign.Left });
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Type"), Width = 300, DataIndex = "Type", Align = Ext.Net.TextAlign.Left });
      }
    }
    #endregion Methods 
    #region IDataControlTreeGrid3
    public string ParentKdpemda { get => ParentKode(Kdpemda); set { } }
    public static bool IsRootCondition(DmpemdaControl dc)
    {
      return (dc.Kdlevel == 1);
    }
    public Icon GetIcon()
    {
      return Ext.Net.Icon.Table;
    }
    public Ext.Net.TreeNode CreateRoot(IList list, int typetree, bool withroot)
    {
      string delim_menu = GlobalAsp.DELIMITER_MENU;
      List<DmpemdaControl> localList = (List<DmpemdaControl>)list;
      List<DmpemdaControl> roots = localList.FindAll(i => IsRootCondition(i));

      Ext.Net.TreeNode root = new Ext.Net.TreeNode("Root", "Root", GetIcon());
      Ext.Net.TreeNodeCollection nodes = root.Nodes;
      foreach (DmpemdaControl ctrl in roots)
      {
        nodes.Add(CreateNodeWithOutChildren((List<DmpemdaControl>)list, ctrl, typetree, delim_menu));
      }
      return root;
    }
    public void LoadPages(IList inList, string nodeid, Ext.Net.TreeNodeCollection nodes, int typetree)
    {
      string delim_menu = ">";
      List<DmpemdaControl> list = (List<DmpemdaControl>)inList;
      if (list != null && list.Count > 0)
      {
        DmpemdaControl parent = list.Find(o => o.Kdpemda.Equals(nodeid));
        if ((parent != null) && (parent.Type.Equals("H")))
        {
          List<DmpemdaControl> children = GetChildren(list, parent);
          foreach (DmpemdaControl ctrl in children)
          {
            nodes.Add(CreateNodeWithOutChildren(list, ctrl, typetree, delim_menu));
          }
        }
      }
    }
    public static List<DmpemdaControl> GetChildren(List<DmpemdaControl> domainset, DmpemdaControl parent)
    {
      List<DmpemdaControl> children = domainset.FindAll(o => o.Kdpemda.StartsWith(parent.Kdpemda) && (o.Kdlevel == parent.Kdlevel + 1));
      return children;
    }
    protected TreeNodeBase CreateNodeWithOutChildren(List<DmpemdaControl> list, DmpemdaControl parent, int typetree, string delim_menu)
    {
      TreeNodeBase treeNode;
      List<DmpemdaControl> children = GetChildren(list, parent);
      if (children != null && children.Count > 0)
      {
        treeNode = (AsyncTreeNode)ExtTreeNode.GetExtTreeNode(parent.Kdpemda,
          ExtTreePanelUtil.GetUraian(parent.Nmpemda, new string[] { "Nmpemda", delim_menu }),
          typetree, GetKeys(), parent, true, Ext.Net.Icon.Folder);
      }
      else
      {
        treeNode = ExtTreeNode.GetExtTreeNode(parent.Kdpemda,
          ExtTreePanelUtil.GetUraian(parent.Nmpemda, new string[] { "Nmpemda", delim_menu }),
          typetree, GetKeys(), parent, false, Ext.Net.Icon.Table);
      }
      return treeNode;
    }
    #endregion IDataControlTreeGrid3
    #region Sum All
    public static void Update(List<DmpemdaControl> domainset)
    {
      List<DmpemdaControl> roots = domainset.FindAll(i => IsRootCondition(i));
      if (roots.Count > 0)
      {
        foreach (DmpemdaControl r in roots)
        {
          UpdatedRecFields bobotProgress = SumChildren(domainset, r);
          r.SetValue(bobotProgress);
        }
      }
    }

    private static UpdatedRecFields SumChildren(List<DmpemdaControl> domainsetParent, DmpemdaControl parent)
    {
      UpdatedRecFields localBobotProgress = new UpdatedRecFields();
      List<DmpemdaControl> localDomainset = domainsetParent.FindAll(o => o.Kdpemda.StartsWith(parent.Kdpemda));

      List<DmpemdaControl> children = GetChildren(localDomainset, parent);
      int localNumChildren = 0;
      if ((children.Count > 0) && (parent.Type == "H"))
      {
        localBobotProgress.Nchild = children.Count;
        foreach (DmpemdaControl o in children)
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

      public void SetValue(DmpemdaControl rec)
      {
        Nchild = rec.Nchild;
      }
    }
    #endregion
  }
  #endregion Dmpemda
}

