using CoreNET.Common.BO;
using Ext.Net;
using System;
using System.Collections.Generic;
using System.Web;

namespace CoreNET.Common.Base
{
  public class ExtTreePageUtils
  {
    public static void BuildTreeGrid(TreePanel tree)
    {
      BuildTreeGrid(tree, null, false);
    }
    public static void BuildTreeGrid(TreePanel tree, Ext.Net.Menu GridMenu, bool withHandler)
    {
      #region Inisialisasi
      int id = GlobalAsp.GetRequestI();
      IDataControlTreeGrid3 dc = (IDataControlTreeGrid3)UtilityUI.GetDataControl(id);
      #endregion
      #region Add TreeGrid
      if (withHandler)
      {
        bool master = (!string.IsNullOrEmpty(HttpContext.Current.Request["child"])) && (HttpContext.Current.Request["child"] != "0");
        DefaultSelectionModel selectionModel = new DefaultSelectionModel();
        selectionModel.Listeners.SelectionChange.Handler = @"
        if(#{TreeGrid1}.getSelectedNodes())
        {
          /*#{FPFormEntry1}.reset();*/
          var atrsel = #{TreeGrid1}.getSelectedNodes().attributes;
          CoreNET.Methods1.RefreshDetilInfo(Ext.encode(atrsel));" +
          ((master) ? "parent.CoreNET.RefreshDetilPage(Ext.encode(atrsel));NormalizeFrame();#{PanelFormEntry}.collapse();" : string.Empty) + @"
          if(!!#{TreePanel2})CopyIntoTreePanel2();
        }else
        {
          Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
        }";

        tree.SelectionModel.Add(selectionModel);
        if (typeof(TreeGrid).IsInstanceOfType(tree))
        {
          dc.SetTreeGridColumns(((TreeGrid)tree).Columns);
        }
        if (HttpContext.Current.Request["redirect"] == "1")
        {
          tree.Listeners.Click.Handler = "if (node.attributes.href) {e.stopEvent(); parent.loadNode(parent.Pages, node); }";
        }
      }
      #endregion

      #region SetTopBar & GridMenu
      Toolbar toolbar = new Toolbar() { ID = "TopBar1" };
      SetTopBar(toolbar);
      tree.TopBar.Add(toolbar);
      //if (TopBar1 != null)
      //{
      //  SetTopBar(TopBar1);
      //}
      if (GridMenu != null)
      {
        SetGridMenu(GridMenu);
        if (MasterAppConstants.Instance.ShowContextMenu)
        {
          tree.ContextMenuID = GridMenu.ID;
        }
      }
      #endregion
    }

    public static void SetGridMenu(Ext.Net.Menu GridMenu)
    {
      int id = GlobalAsp.GetRequestI();
      IDataControlUI dc = (IDataControlUI)UtilityUI.GetDataControl(id);

      if ((((BaseBO)dc).GetMaxModePreviewIndex()) > 0)
      {
        int mode = ((BaseBO)dc).GetModePreviewIndex();
        Ext.Net.MenuItem PreviewByIndex = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_PREVIEW_BY_INDEX") + " (" + mode + ")" };
        PreviewByIndex.Listeners.Click.Handler = @"
              ExpandFrame();
              CoreNET.Methods1.ChangeModePreview();
            ";
        GridMenu.Items.Add(PreviewByIndex);
      }

      Ext.Net.MenuItem ClearSelection = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_CLEAR_SELECTION") };
      ClearSelection.Listeners.Click.Handler = @"
              ExpandFrame();
              CoreNET.Methods2.TreeDeleteSelected();
              ClearDetilPage();
            ";
      GridMenu.Items.Add(ClearSelection);

      Ext.Net.MenuItem ViewInfo = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_VIEW_INFO") };
      ViewInfo.Listeners.Click.Handler = @"
        ExpandFrame();
        #{PanelFormEntry}.expand();
        if(#{TreeGrid1}.getSelectedNodes())
        {
          /*#{FPFormEntry1}.reset();*/
          var atrsel = #{TreeGrid1}.getSelectedNodes().attributes;
          var jsonsel = Ext.isArray(atrsel) ? Ext.encode(atrsel) : '[' + Ext.encode(atrsel) +']';
          CoreNET.Methods1.RefreshDetilInfo(jsonsel);
          if(!!#{TreePanel2})CopyIntoTreePanel2();
        }else
        {
          Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
        }
      ";
      GridMenu.Items.Add(ViewInfo);

      if ((HttpContext.Current.Request["child"] != null) && (int.Parse(HttpContext.Current.Request["child"]) > 0))
      {
        Ext.Net.MenuItem ViewChild = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_VIEW_CHILD") };
        ViewChild.Listeners.Click.Handler = "#{PanelFormEntry}.collapse();NormalizeFrame();";
        GridMenu.Items.Add(ViewChild);
      }
      if (((ViewListProperties)dc.GetProperties()).IsModeAdd())
      {
        Ext.Net.MenuItem MenuCopyBaris = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_COPY_ROW") };
        MenuCopyBaris.Listeners.Click.Handler = @"
            if(#{TreeGrid1}.getSelectedNodes())
            {
              ExpandFrame();
              /*#{FPFormEntry1}.reset();*/
              var atrsel = #{TreeGrid1}.getSelectedNodes().attributes;
              CoreNET.Methods1.btnCopyBarisClick(Ext.encode(atrsel));
              if(!!#{TreePanel2})CopyIntoTreePanel2();
            }else
            {
              Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
            }
        ";
        GridMenu.Items.Add(MenuCopyBaris);
      }
    }
    public static void SetTopBar(Toolbar TopBar1)
    {
      int id = GlobalAsp.GetRequestI();
      IDataControlUI dc = UtilityUI.GetDataControl(id);
      #region Add Tree Grid Toolbar
      if (((ViewListProperties)dc.GetProperties()).ModeToolbar != ViewListProperties.MODE_TOOLBAR_MINIMALIS)
      {
        Ext.Net.Button btnExpand = new Ext.Net.Button
        {
          ID = "BTN_EXPAND1"
        };
        btnExpand.Text = ConstantDictExt.Translate(btnExpand.ID);
        btnExpand.ToolTip = ConstantDictExt.TranslateTabTip(btnExpand);
        btnExpand.IconCls = "icon-expand-all2";
        btnExpand.Listeners.Click.Handler = @"
                  if(confirm('" + ConstantDictExt.Translate("LBL_CONFIRM_EXPAND") + @"'))
                  {
                    ExpandFrame();#{TreeGrid1}.expandAll();
                  }";
        TopBar1.Add(btnExpand);

        Ext.Net.Button btnCollapse = new Ext.Net.Button
        {
          ID = "BTN_COLLAPSE1"
        };
        btnCollapse.Text = ConstantDictExt.Translate(btnCollapse.ID);
        btnCollapse.ToolTip = ConstantDictExt.TranslateTabTip(btnCollapse);
        btnCollapse.IconCls = "icon-collapse-all2";
        btnCollapse.Listeners.Click.Handler = "ExpandFrame();#{TreeGrid1}.collapseAll();";
        TopBar1.Add(btnCollapse);
      }
      Ext.Net.Button btnRefresh = new Ext.Net.Button() { ID = GlobalAsp.BTN_REFRESH1, Icon = Icon.ArrowRefresh };
      btnRefresh.Text = ConstantDictExt.Translate(btnRefresh.ID);
      btnRefresh.ToolTip = ConstantDictExt.TranslateTabTip(btnRefresh);
      btnRefresh.Listeners.Click.Handler = @"ExpandFrame();refreshTree(); ClearDetilPage();";
      TopBar1.Add(btnRefresh);
      if (((ViewListProperties)dc.GetProperties()).IsModeAdd())
      {
        Ext.Net.Button btnAdd = new Ext.Net.Button() { ID = GlobalAsp.BTN_ADD1, Icon = Icon.Add };
        btnAdd.Text = ConstantDictExt.Translate(btnAdd.ID);
        btnAdd.ToolTip = ConstantDictExt.TranslateTabTip(btnAdd);
        string lblInfo = GlobalAsp.GetConfigLabelInfo();
        string msg = ConstantDictExt.Translate("LBL_EMPTY_FILTER");

        if (((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_FORM)
        {
          btnAdd.Listeners.Click.Handler = @"
            var valid = validateFilterWhenAdd();
            if(valid)
            {
              ExpandFrame();
              if(!!#{TreeGrid1}.getSelectedNodes()){
                  var sels = #{TreeGrid1}.getSelectedNodes().attributes;
                  CoreNET.Methods1.btnAddFromFormEntry(Ext.encode(sels));
                  if(!!#{TreePanel2})CopyIntoTreePanel2();
              }else{
                CoreNET.Methods1.btnAddFromFormEntry('');
              }
            }else{
              Ext.Msg.alert('" + lblInfo + @"','" + msg + @"');
            }
          ";
        }
        else if (((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_LOOKUP)
        {
          btnAdd.Listeners.Click.Handler = @"
            var valid = validateFilterWhenAdd();
            if(valid)
            {
              ExpandFrame();
              if(!!#{TreeGrid1}.getSelectedNodes()){
                var sels = #{TreeGrid1}.getSelectedNodes().attributes;
                CoreNET.Methods1.btnAddFromDlgLookup(Ext.encode(sels));
              }else{
                CoreNET.Methods1.btnAddFromDlgLookup('');
              }
            }else{
              Ext.Msg.alert('" + lblInfo + @"','" + msg + @"');
            }
          ";
        }
        else if (((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_LOOKUP_FORM)
        {
          btnAdd.Listeners.Click.Handler = @"
            var valid = validateFilterWhenAdd();
            if(valid)
            {
              if(!!#{TreeGrid1}.getSelectedNodes()){
                  var sels = #{TreeGrid1}.getSelectedNodes().attributes;
                  CoreNET.Methods1.btnAddDlgFormEntry(Ext.encode(sels));
                  if(!!#{TreePanel2})CopyIntoTreePanel2();
              }else{
                CoreNET.Methods1.btnAddDlgFormEntry('');
              }
            }else{
              Ext.Msg.alert('" + lblInfo + @"','" + msg + @"');
            }
          ";
        }
        TopBar1.Add(btnAdd);
      }

      if (((ViewListProperties)dc.GetProperties()).IsModeEdit())
      {
        Ext.Net.Button btnEdit = new Ext.Net.Button() { ID = GlobalAsp.BTN_EDIT1, Icon = Icon.Pencil };
        btnEdit.Text = ConstantDictExt.Translate(btnEdit.ID);
        btnEdit.ToolTip = ConstantDictExt.TranslateTabTip(btnEdit);

        btnEdit.Listeners.Click.Handler = @"
                    if(#{TreeGrid1}.getSelectedNodes())
                    {
                      ExpandFrame();
                      /*#{FPFormEntry1}.reset();*/
                      var atrsel = #{TreeGrid1}.getSelectedNodes().attributes;
                      CoreNET.Methods1.btnEditClick(Ext.encode(atrsel),'EditForm');
                      if(!!#{TreePanel2})CopyIntoTreePanel2();
                    }else
                    {
                      Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
                  }";
        TopBar1.Add(btnEdit);

      }

      if (((ViewListProperties)dc.GetProperties()).IsModeDelete())
      {
        Ext.Net.Button btnDel = new Ext.Net.Button() { ID = GlobalAsp.BTN_DEL1, Icon = Icon.Delete };
        btnDel.Text = ConstantDictExt.Translate(btnDel.ID);// (((ViewListProperties)dc.GetProperties()).LblBtnDel);
        btnDel.ToolTip = ConstantDictExt.TranslateTabTip(btnDel);
        btnDel.Listeners.Click.Handler = @"
            ExpandFrame();
            if(#{TreeGrid1}.getSelectedNodes())
            {
              Ext.Msg.confirm('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "', '" + ConstantDict.Translate("LBL_CONFIRM_DELETE") + @"', function (id, val) {
                if (id === 'yes') {
                  var atrsel = #{TreeGrid1}.getSelectedNodes().attributes;
                  CoreNET.Methods1.btnDeleteClick(Ext.encode(atrsel));
                }
              }, this);
            }else
            {
              Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
            }
          ";
        TopBar1.Add(btnDel);
      }

      //if (typeof(IExtLoadCsv).IsInstanceOfType(dc))
      if (((ViewListProperties)dc.GetProperties()).IsLoadCSV() && (HttpContext.Current.Request["enable"] != "0"))
      {
        Ext.Net.Button btnLoadCsv = new Ext.Net.Button() { ID = GlobalAsp.BTN_LOADCSV1, Icon = Icon.TableAdd };
        btnLoadCsv.Text = ConstantDictExt.Translate(btnLoadCsv.ID);
        btnLoadCsv.ToolTip = ConstantDictExt.TranslateTabTip(btnLoadCsv);
        btnLoadCsv.Listeners.Click.Handler =
          @"loadingmasklink();ExpandFrame();CoreNET.Methods1.btnLoadCsv();";
        TopBar1.Add(btnLoadCsv);
      }

      if (((ViewListProperties)dc.GetProperties()).IsToolbarPrint())
      {
        #region Menu Print
        Ext.Net.Button btnPrint = new Ext.Net.Button() { ID = GlobalAsp.BTN_PRINT1, Icon = Icon.Printer };
        btnPrint.Text = ConstantDictExt.Translate(btnPrint.ID);
        btnPrint.ToolTip = ConstantDictExt.TranslateTabTip(btnPrint);
        Ext.Net.Menu menuPrint = new Ext.Net.Menu();

        if (typeof(IPrintable).IsInstanceOfType(dc))
        {
          List<RptPars> Rpts = ((IPrintable)dc).GetReports();

          for (int i = 0; i < Rpts.Count; i++)
          {
            Ext.Net.MenuItem btnPrint2 = new Ext.Net.MenuItem() { ID = "BtnPrint" + UtilityBO.IntToStr(i, 3), Icon = Icon.Printer };
            btnPrint2.Text = ConstantDictExt.Translate(Rpts[i].Title);
            btnPrint2.ToolTip = ConstantDictExt.TranslateTabTip(btnPrint);
            btnPrint2.Listeners.Click.Handler = string.Format(@"
                loadingmasklink();ExpandFrame();CoreNET.Methods1.btnPrintClick('" + Rpts[i].Title + @"',{0});
            ", i);
            menuPrint.Add(btnPrint2);
          }
          menuPrint.Add(new MenuSeparator());
        }

        Ext.Net.MenuItem btnCsv = new Ext.Net.MenuItem() { ID = GlobalAsp.BTN_CSV1, Icon = Icon.PageExcel };
        //Ext.Net.Button btnCsv = new Ext.Net.Button() { ID = GlobalAsp.BTN_CSV1, Icon = Icon.PageExcel };
        btnCsv.Text = ConstantDictExt.Translate(btnCsv.ID);
        btnCsv.ToolTip = ConstantDictExt.TranslateTabTip(btnCsv);
        btnCsv.Listeners.Click.Handler =
            @"loadingmasklink();ExpandFrame();CoreNET.Methods1.btnSaveCsvClick();";
        if (typeof(ICsv).IsInstanceOfType(dc))
        {
          //TopBar1.Add(btnCsv);
          menuPrint.Add(btnCsv);
        }
        if (menuPrint.Items.Count > 0)
        {
          btnPrint.Menu.Add(menuPrint);
          TopBar1.Add(btnPrint);
        }

        Ext.Net.MenuItem btnWord = new Ext.Net.MenuItem { ID = GlobalAsp.BTN_WORD1, Icon = Icon.PageWord };
        //Ext.Net.Button btnWord = new Ext.Net.Button() { ID = GlobalAsp.BTN_WORD1, Icon = Icon.PageWord };
        btnWord.Text = ConstantDictExt.Translate(btnWord.ID);
        btnWord.ToolTip = ConstantDictExt.TranslateTabTip(btnWord);
        btnWord.Listeners.Click.Handler =
            @"loadingmasklink();ExpandFrame();CoreNET.Methods1.btnSaveWordClick();";
        if (typeof(IWord).IsInstanceOfType(dc))
        {
          //TopBar1.Add(btnWord);
          menuPrint.Add(btnWord);
        }
        #endregion Menu Print
      }
      if (((ViewListProperties)dc.GetProperties()).IsToolbarNormal())
      {
        #region Preview
        Ext.Net.Button btnSetting = new Ext.Net.Button() { ID = GlobalAsp.BTN_SETTING1, Icon = Icon.ShapesMany };
        btnSetting.Text = ConstantDictExt.Translate(btnSetting.ID);
        btnSetting.ToolTip = ConstantDictExt.TranslateTabTip(btnSetting);
        Ext.Net.Menu menuSetting = new Ext.Net.Menu();
        btnSetting.Menu.Add(menuSetting);
        TopBar1.Add(btnSetting);

        if (typeof(IPageMaster).IsInstanceOfType(dc))
        {
          Ext.Net.Button btnPreview0 = new Ext.Net.Button() { ID = GlobalAsp.BTN_MASTER };
          if (UtilityUI.GetSessionExtPage() == UtilityUI.PAGE_MASTER)
          {
            btnPreview0.Icon = Icon.Application;
          }
          else
          {
            btnPreview0.Icon = Icon.ApplicationSplit;
          }
          btnPreview0.Text = ConstantDictExt.Translate(btnPreview0.ID);
          btnPreview0.ToolTip = ConstantDictExt.TranslateTabTip(btnPreview0);
          btnPreview0.Listeners.Click.Handler =
              @"
              var ismaster=false;
              if(!!parent){
                if(!!parent.CoreNET.RefreshDetilPage){
                  ismaster=true;
                }
              }
              if(ismaster){
                parent.CoreNET.RedirectSingle();
              }else{
                CoreNET.Methods1.SetMasterDetilPage();
              }
            ";
          //TopBar1.Add(btnPreview0);
          menuSetting.Add(btnPreview0);
        }

        Ext.Net.MenuItem btnPreview = new Ext.Net.MenuItem() { ID = GlobalAsp.BTN_PREVIEW_TABLE, Icon = Icon.TableGo };
        //Ext.Net.Button btnPreview = new Ext.Net.Button() { ID = GlobalAsp.BTN_PREVIEW_TABLE, Icon = Icon.TableGo };
        btnPreview.Text = ConstantDictExt.Translate(btnPreview.ID);
        btnPreview.ToolTip = ConstantDictExt.TranslateTabTip(btnPreview);
        btnPreview.Listeners.Click.Handler =
                  @"ExpandFrame();CoreNET.RefreshPreview();";
        //TopBar1.Add(btnPreview);
        menuSetting.Add(btnPreview);

        Ext.Net.MenuItem btnPreview2 = new Ext.Net.MenuItem() { ID = GlobalAsp.BTN_PREVIEW_TREE2, Icon = Icon.ReportMagnify };
        //Ext.Net.Button btnPreview2 = new Ext.Net.Button() { ID = GlobalAsp.BTN_PREVIEW_TREE2, Icon = Icon.ReportMagnify };
        btnPreview2.Text = ConstantDictExt.Translate(btnPreview2.ID);
        btnPreview2.ToolTip = ConstantDictExt.TranslateTabTip(btnPreview2);
        btnPreview2.Listeners.Click.Handler =
            @"ExpandFrame();CoreNET.RefreshPreview2();";
        //TopBar1.Add(btnPreview2);
        menuSetting.Add(btnPreview2);
        #region WindowForum
        Ext.Net.MenuItem btnForum = new Ext.Net.MenuItem() { ID = GlobalAsp.BTN_FORUM1, Icon = Icon.EmailStar };
        //Ext.Net.Button btnForum = new Ext.Net.Button() { ID = GlobalAsp.BTN_FORUM1, Icon = Icon.EmailStar };
        btnForum.Text = ConstantDictExt.Translate(btnForum.ID);
        btnForum.ToolTip = ConstantDictExt.TranslateTabTip(btnForum);
        btnForum.Listeners.Click.Handler = "ExpandFrame();CoreNET.Methods1.btnForumClick()";
        //TopBar1.Add(btnForum);
        menuSetting.Add(btnForum);
        #endregion
        #region WindowHelp
        Ext.Net.MenuItem btnHelp = new Ext.Net.MenuItem() { ID = GlobalAsp.BTN_HELP1, Icon = Icon.PageWorld };
        //Ext.Net.Button btnHelp = new Ext.Net.Button() { ID = GlobalAsp.BTN_HELP1, Icon = Icon.PageWorld };
        btnHelp.Text = ConstantDictExt.Translate(btnHelp.ID);
        btnHelp.ToolTip = ConstantDictExt.TranslateTabTip(btnHelp);
        btnHelp.Listeners.Click.Handler = "ExpandFrame();CoreNET.Methods1.btnHelpClick()";
        //TopBar1.Add(btnHelp);
        menuSetting.Add(btnHelp);
        #endregion
        #endregion Preview
      }
      #region Debugging
      if (MasterAppConstants.Instance.StatusTesting)
      {
        Ext.Net.Button btnMenu = new Ext.Net.Button() { ID = GlobalAsp.BTN_MENU1, Icon = Icon.Wrench };
        btnMenu.Text = ConstantDictExt.Translate(btnMenu.ID);
        btnMenu.ToolTip = ConstantDictExt.TranslateTabTip(btnMenu);

        Ext.Net.Menu menuDebug = new Ext.Net.Menu() { };
        btnMenu.Menu.Add(menuDebug);

        Ext.Net.MenuItem mnInfo = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_INFO1, Icon = Icon.PageLightning };
        mnInfo.Text = ConstantDictExt.Translate(mnInfo.ID);
        mnInfo.Listeners.Click.Handler = "CoreNET.Methods1.btnDebugClick()";
        menuDebug.Items.Add(mnInfo);
        menuDebug.Items.Add(new Ext.Net.MenuSeparator());

        Ext.Net.MenuItem mnError = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_ERROR1, Icon = Icon.EmailStar };
        mnError.Text = ConstantDictExt.Translate(mnError.ID);
        mnError.Listeners.Click.Handler = "CoreNET.Methods1.btnErrorClick()";
        menuDebug.Items.Add(mnError);
        menuDebug.Items.Add(new Ext.Net.MenuSeparator());

        Ext.Net.MenuItem mntree = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_TREE1, Text = "Info", Icon = Icon.ApplicationSideTree };
        mntree.Text = ConstantDictExt.Translate(mntree.ID);
        mntree.Listeners.Click.Handler = "CoreNET.Methods1.btnAdminClick(2)";
        menuDebug.Items.Add(mntree);

        Ext.Net.MenuItem mnfilter = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_FILTER1, Text = "Info", Icon = Icon.ApplicationFormMagnify };
        mnfilter.Text = ConstantDictExt.Translate(mnfilter.ID);
        mnfilter.Listeners.Click.Handler = "CoreNET.Methods1.btnAdminClick(3)";
        menuDebug.Items.Add(mnfilter);

        Ext.Net.MenuItem mncols = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_COLS1, Text = "Info", Icon = Icon.ApplicationViewColumns };
        mncols.Text = ConstantDictExt.Translate(mncols.ID);
        mncols.Listeners.Click.Handler = "CoreNET.Methods1.btnAdminClick(4)";
        menuDebug.Items.Add(mncols);

        Ext.Net.MenuItem mnconfig = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_CONFIG1, Icon = Icon.ApplicationForm };
        mnconfig.Text = ConstantDictExt.Translate(mnconfig.ID);
        mnconfig.Listeners.Click.Handler = "CoreNET.Methods1.btnAdminClick(5)";
        menuDebug.Items.Add(mnconfig);

        TopBar1.Add(btnMenu);
      }
      #endregion
      bool root = !string.IsNullOrEmpty(HttpContext.Current.Request["root"]) && (HttpContext.Current.Request["root"].Contains("1"));
      if (!root)
      {
        //update 24/06/2019 dikomen
        //update 08/07/2019 diunkomen, buat aktivkan refreshparent di Transaksi Pesanan, Primos
        string scriptRefreshParent = string.Empty;
        if (!string.IsNullOrEmpty(HttpContext.Current.Request["target"])//Ketika Dialog Lookup Tutup
          || ((ViewListProperties)dc.GetProperties()).RefreshParent)//Ketika RefreshParent true
        {
          scriptRefreshParent = "parent.refreshDataWithSelection();";
        }

        Ext.Net.Button btnClose = new Ext.Net.Button() { ID = GlobalAsp.BTN_CLOSE1, Icon = Icon.DoorOpen };
        btnClose.Text = ConstantDictExt.Translate(btnClose.ID);
        btnClose.Listeners.Click.Handler = @"
          if (!!parent.CoreNET.Methods1) { parent.CoreNET.Methods1.btnCloseDialogClick();}
          if (!!parent.WindowLookup1) { parent.WindowLookup1.hide(this); }
          " + scriptRefreshParent + @"
        ";
        TopBar1.Add(btnClose);
      }

      Ext.Net.Label lblWait = new Ext.Net.Label() { ID = "lblWait" };
      lblWait.Html = @"<div id='loading-mask' style='display:none'><div id='loading'><div class='loading-indicator'>Loading..</div></div></div>";
      TopBar1.Add(lblWait);
      #endregion
    }
    public static Ext.Net.TextAlign GetAlign(string colalign)
    {
      switch (colalign)
      {
        case "left": return Ext.Net.TextAlign.Left;
        case "center": return Ext.Net.TextAlign.Center;
        case "right": return Ext.Net.TextAlign.Right;
      }
      return Ext.Net.TextAlign.Left;
    }
    public static void SetDefaultTreeGrid(TreeGridColumnCollection Columns, IDataControl ctrl, int modePreviewIndex)
    {
      List<SysgettreecolsControl> list = SysgettreecolsLookupControl.GetTreeColumns(ctrl, modePreviewIndex);
      if (list.Count > 0)
      {
        TreeGridColumn col1 = null;
        SysgettreecolsControl dc = null;
        for (int i = 0; i < list.Count; i++)
        {
          try
          {

            dc = list[i];
            int mode = dc.Mode;
            int colwidth = dc.Colwidth;
            string colname = dc.Colname;
            string colalign = dc.Colalign;
            string coltype = dc.Coltype;

            switch (coltype)
            {
              case "decimal":
                col1 = new TreeGridColumn { Header = ConstantDict.Translate(colname), Width = 100, DataIndex = colname, Align = Ext.Net.TextAlign.Right };
                col1.XTemplate.Html = "<a style='visibility:visible;'>{[formatdec(values." + colname + @")]}</a>";
                Columns.Add(col1);
                break;
              case "money":
                col1 = new TreeGridColumn { Header = ConstantDict.Translate(colname), Width = 100, DataIndex = colname, Align = Ext.Net.TextAlign.Right };
                col1.XTemplate.Html = "<a style='visibility:visible;'>{[formatmoney(values." + colname + @")]}</a>";
                Columns.Add(col1);
                break;
              case "link":
                col1 = new TreeGridColumn { Header = ConstantDict.Translate(colname), Width = 50, DataIndex = "Statusicon", Align = Ext.Net.TextAlign.Center };
                col1.XTemplate.Html = @"<a style='visibility:true;'
                onclick = 'parent.loadPageByID(""Pages"",""{" + colname + @"}"",""{" + colname + @"}"", ""{[GetURLEntry(values." + colname + @")]}"");' >{" + colname + @"}</a>
              ";
                Columns.Add(col1);
                break;
              case "icon":
                switch (colname)
                {
                  case "Stricon":
                    col1 = new TreeGridColumn { Header = ConstantDict.Translate(colname), Width = 50, DataIndex = "Statusicon", Align = Ext.Net.TextAlign.Center };
                    col1.XTemplate.Html = @"<a style='visibility:true;'>
                    <img style='width: 16px; height: 16px;' title='{Stricon}' src='../Res/Icons/{Stricon}.png' alt='{Stricon}'/></a>
                  ";
                    break;
                  case "Statusicon":
                    col1 = new TreeGridColumn { Header = ConstantDict.Translate(colname), Width = 50, DataIndex = "Statusicon", Align = Ext.Net.TextAlign.Center };
                    col1.XTemplate.Html = @"<a style='visibility:true;'>
                    <img style='width: 16px; height: 16px;' title='{Statusname}' src='../Res/Icons/{Statusicon}' alt='{Statusicon}'/></a>
                  ";
                    break;
                  case "Progressicon":
                    col1 = new TreeGridColumn { Header = ConstantDict.Translate(colname), Width = 50, DataIndex = "Progressicon", Align = Ext.Net.TextAlign.Center };
                    col1.XTemplate.Html = @"<a style='visibility:true;'>
                    <img style='width: 16px; height: 16px;' title='Progress {Progress}%' src='../Res/Icons/{Progressicon}' alt='{Progressicon}'/></a>
                  ";
                    break;
                }
                if (col1 != null)
                {
                  Columns.Add(col1);
                }
                break;
              default:
                Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate(colname), Width = colwidth, DataIndex = colname, Align = GetAlign(colalign) });
                break;
            }
            //col1 = new TreeGridColumn { Header = ConstantDict.Translate("Kdmenu"), Width = 200, DataIndex = "Kdmenu", Align = Ext.Net.TextAlign.Left };
            //col1.XTemplate.Html = @"<a style='visibility:true;'
            //  onclick = 'parent.loadPageByID(""Pages"",""{Kdmenu}"",""{Kdmenu}"", ""{[GetURLEntry(values.Kdmenu)]}"");' >{Kdmenu}</a>
            //";
            //Columns.Add(col1);
          }
          catch (Exception ex)
          {
            UtilityBO.Log(dc, ex);
          }
        }
      }
    }
  }
}
