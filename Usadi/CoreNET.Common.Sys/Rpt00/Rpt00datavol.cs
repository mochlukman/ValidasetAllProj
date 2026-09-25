using CoreNET.Common.Base;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region Rpt00datavol
  [Serializable]
  public class Rpt00datavolControl : BaseDataControlSys, IDataControlTreeGrid3, IHasJSScript
  {
    #region Properties 
    public string Nmobj { get; set; }
    public int Row_count { get; set; }
    #endregion Properties 

    #region Methods 
    public Rpt00datavolControl()
    {
      XMLName = ConstantTablesSys.XMLRPT00DATAVOL;
      SetMaxModePreviewIndex(1);
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Nmobj" };
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_READONLY;
      return cViewListProperties;
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection
      {
        ExtFields.GetRatingField(),
        Fields.Create(ConstantDict.GetColumnTitle("Row_count"), typeof(int), 10, HorizontalAlign.Right),
        Fields.Create(ConstantDict.GetColumnTitle("Stricon"), typeof(CommandColumn), 5, HorizontalAlign.Center)
      };
      if (ModePreviewIndex == 1)
      {
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdlevel"), typeof(int), 7, HorizontalAlign.Center).SetEditable(true));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Type"), typeof(string), 7, HorizontalAlign.Center).SetEditable(true));
      }
      return columns;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
      }
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      //hpars.Add(TtdaftdokLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter));
      //hpars.Add(new ParameterRowDate(this, ParameterRow.MODE_DATE_RANGE).SetEnable(enableFilter));
      //hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
      //    GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enableFilter));
      return hpars;
    }
    public new IList View()
    {
      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      string sql = @"exec ListAllDBTablesRowCount";
      sql = string.Format(sql, XMLName, ID);
      BaseDataAdapter.ExecuteCmd(this, sql);


      IList list = ((BaseDataControl)this).View(label);
      List<Rpt00datavolControl> ListData = new List<Rpt00datavolControl>();
      foreach (Rpt00datavolControl dc in list)
      {
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      Update(ListData);
      return ListData;
    }
    public override HashTableofParameterRow GetEntries()
    {
      bool enable = false;
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        new ParameterRowMemo(this, ConstantDict.GetColumnTitle("Nmobj"), false, 3).SetEnable(enable),
        new ParameterRowNumeric(this, ConstantDict.GetColumnTitle("Row_count"), false, 30).SetEnable(enable),
        new ParameterRowSelect(ConstantDict.GetColumnTitle("Kdlevel"),
        GetList(new DmlevelControl(this)), "Kdlevel=Nmlevel", 30).SetAllowRefresh(false).SetEnable(enable),
        new ParameterRowType(this, true),
        new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
        GetList(new DmstatusControl()), "Idstatus=Nmstatus", 30).SetAllowRefresh(false).SetEnable(enable),
        new ParameterRowCek(this, true),
        new ParameterRowUploadFile(this, true),
        new ParameterRowHelp(this, true),
        new ParameterRowForum(this, true)
      };
      return hpars;
    }
    #endregion Methods 
    #region IDataControlTreeGrid3
    public string ParentNmobj { get => ParentKode(Nmobj); set { } }
    public static bool IsRootCondition(Rpt00datavolControl dc)
    {
      return (dc.Kdlevel == 1);
    }
    public Icon GetIcon()
    {
      return Ext.Net.Icon.Table;
    }
    public new string[] GetKeys()
    {
      return new string[] { "Nmobj", "Row_count"
        , "Status", "Statusicon", "Statusname", "Stricon", "Rating"
        , "Kdlevel", "Type" };
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmobj"), Width = 300, DataIndex = "Nmobj", Align = Ext.Net.TextAlign.Center });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Row_count"), Width = 100, DataIndex = "Row_count", Align = Ext.Net.TextAlign.Center });
      if (GetModePreviewIndex() == 1)
      {
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdlevel"), Width = 50, DataIndex = "Kdlevel", Align = Ext.Net.TextAlign.Center });
        Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Type"), Width = 50, DataIndex = "Type", Align = Ext.Net.TextAlign.Center });
      }
      TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 150, Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"<a style='visibility:{[ShowHidden(values.Type)]};' 
            onclick='parent.loadPageByID(""Pages"",""{Nmobj}"",""{Nmobj}"", ""{[GetURL(values.Nmobj)]}"");'>[lihat data]</a> 
      ";
      Columns.Add(col1);
      //col1 = new TreeGridColumn { Header = ConstantDict.Translate("Msg"), Width = 150, Align = Ext.Net.TextAlign.Center };
      //temps = LinkTableRows.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
      //col1.XTemplate.Html = @"<a style='visibility:{[ShowHidden(values.Type)]};' 
      //      onclick='parent.loadPageByID(""Pages"",""" + temps[0] + @""",""" + temps[0] + @""", """ + temps[1] + @""");'>[lihat diskusi]</a> 
      //";
      //Columns.Add(col1);
    }
    public string LinkTableRows
    {
      get
      {
        string dbname = SQLDataSource.GetOpDB(ConnectionString);
        return ConstantDict.Translate("LinkTableRows") + " " + Nmobj + ":" +
         string.Format(@"Page/HTML.aspx?app={0}&id={1}&idprev={2}&val={3}", GlobalAsp.GetSessionApp(),
         HttpUtility.HtmlEncode("F5F207DE-B959-4A5F-BDB5-E94622915952"), GlobalAsp.GetRequestId(),
         dbname + ".." + Nmobj);
      }
    }
    public new string[] GetScript()
    {
      string url = string.Format(@"Page/HTML.aspx?app={0}&id={1}&idprev={2}", GlobalAsp.GetSessionApp(),
         HttpUtility.HtmlEncode("F5F207DE-B959-4A5F-BDB5-E94622915952"), GlobalAsp.GetRequestId());
      ArrayList listScript = new ArrayList(base.GetScript());
      string script = @"
        var GetURL = function (val) {
            return '" + url + @"&val={0}'.format(val);
        };
      ";
      listScript.Add(script);
      string[] scripts = new string[listScript.Count];
      listScript.CopyTo(scripts);
      return scripts;
    }
    public Ext.Net.TreeNode CreateRoot(IList list, int typetree, bool withroot)
    {
      string delim_menu = GlobalAsp.DELIMITER_MENU;
      List<Rpt00datavolControl> localList = (List<Rpt00datavolControl>)list;
      List<Rpt00datavolControl> roots = localList.FindAll(i => IsRootCondition(i));

      Ext.Net.TreeNode root = new Ext.Net.TreeNode("Root", "Root", GetIcon());
      Ext.Net.TreeNodeCollection nodes = root.Nodes;
      foreach (Rpt00datavolControl ctrl in roots)
      {
        nodes.Add(CreateNodeWithOutChildren((List<Rpt00datavolControl>)list, ctrl, typetree, delim_menu));
      }
      return root;
    }
    public void LoadPages(IList inList, string nodeid, Ext.Net.TreeNodeCollection nodes, int typetree)
    {
      string delim_menu = ">";
      List<Rpt00datavolControl> list = (List<Rpt00datavolControl>)inList;
      if (list != null && list.Count > 0)
      {
        Rpt00datavolControl parent = list.Find(o => o.Nmobj.Equals(nodeid));
        if ((parent != null) && (parent.Type.Equals("H")))
        {
          List<Rpt00datavolControl> children = GetChildren(list, parent);
          foreach (Rpt00datavolControl ctrl in children)
          {
            nodes.Add(CreateNodeWithOutChildren(list, ctrl, typetree, delim_menu));
          }
        }
      }
    }
    public static List<Rpt00datavolControl> GetChildren(List<Rpt00datavolControl> domainset, Rpt00datavolControl parent)
    {
      List<Rpt00datavolControl> children = domainset.FindAll(o => o.Nmobj.StartsWith(parent.Nmobj) && (o.Kdlevel == parent.Kdlevel + 1));
      return children;
    }
    protected TreeNodeBase CreateNodeWithOutChildren(List<Rpt00datavolControl> list, Rpt00datavolControl parent, int typetree, string delim_menu)
    {
      TreeNodeBase treeNode;
      List<Rpt00datavolControl> children = GetChildren(list, parent);
      if (children != null && children.Count > 0)
      {
        treeNode = (AsyncTreeNode)ExtTreeNode.GetExtTreeNode(parent.Nmobj,
          ExtTreePanelUtil.GetUraian(parent.Nmobj, new string[] { "Nmobj", delim_menu }),
          typetree, GetKeys(), parent, true, Ext.Net.Icon.Folder);
      }
      else
      {
        treeNode = ExtTreeNode.GetExtTreeNode(parent.Nmobj,
          ExtTreePanelUtil.GetUraian(parent.Nmobj, new string[] { "Nmobj", delim_menu }),
          typetree, GetKeys(), parent, false, Ext.Net.Icon.Table);
      }
      return treeNode;
    }
    #endregion IDataControlTreeGrid3
    #region Sum All
    public static void Update(List<Rpt00datavolControl> domainset)
    {
      List<Rpt00datavolControl> roots = domainset.FindAll(i => IsRootCondition(i));
      if (roots.Count > 0)
      {
        foreach (Rpt00datavolControl r in roots)
        {
          UpdatedRecFields bobotProgress = SumChildren(domainset, r);
          r.SetValue(bobotProgress);
        }
      }
    }

    private static UpdatedRecFields SumChildren(List<Rpt00datavolControl> domainsetParent, Rpt00datavolControl parent)
    {
      UpdatedRecFields localBobotProgress = new UpdatedRecFields();
      List<Rpt00datavolControl> localDomainset = domainsetParent.FindAll(o => o.Nmobj.StartsWith(parent.Nmobj));

      List<Rpt00datavolControl> children = GetChildren(localDomainset, parent);
      int localNumChildren = 0;
      if ((children.Count > 0) && (parent.Type == "H"))
      {
        localBobotProgress.Nchild = children.Count;
        foreach (Rpt00datavolControl o in children)
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
      //Pagu = rec.Pagu;
      Row_count = rec.Row_count;
    }

    private class UpdatedRecFields
    {
      public int Nchild = 0;
      public decimal Pagu = 0;
      public int Row_count = 0;

      public void Sum(UpdatedRecFields rec)
      {
        Pagu += rec.Pagu;
        Row_count += rec.Row_count;
      }

      public void SetValue(Rpt00datavolControl rec)
      {
        //Pagu = rec.Pagu;
        Row_count = rec.Row_count;
      }
    }
    #endregion
  }
  #endregion Rpt00datavol
}

