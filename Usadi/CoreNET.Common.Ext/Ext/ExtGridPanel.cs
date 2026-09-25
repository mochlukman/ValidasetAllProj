using CoreNET.Common.BO;
using Ext.Net;
using Ext.Net.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
  public class ExtGridPanel : GridPanel
  {
    public const int EDIT_COMMAND_COLUMN_WIDTH = 50;//20;
    public const int VIEW_COMMAND_COLUMN_WIDTH = 60;//20;
    public const int WIDTH_TREE_MENU = 320;

    public const int MODE_PAGE_NORMAL = 0;
    public const int MODE_PAGE_GRIDLOOKUP = 1;

    public static bool IsModeInserting()
    {
      return UtilityExt.IsModeAdd();
      //if (HttpContext.Current.Session[ExtGridPanel.SESSION_MODE] == null)
      //{
      //  return false;
      //}
      //else
      //{
      //  return ((string)HttpContext.Current.Session[ExtGridPanel.SESSION_MODE]).Equals(ExtGridPanel.MODE_INSERT);
      //}
    }

    public static string SESSION_MODE => "session_mode" + GlobalAsp.GetRequestId();

    protected int Id;
    public ExtGridPanel()
    {

    }
    public ExtGridPanel(int id, IDataControlUI dc, int modepage)
    {
      if (modepage == MODE_PAGE_GRIDLOOKUP)
      {
        SettingExtGridPanel(id, dc, modepage, false, MODE_MULTI_SELECT, true);
      }
      else
      {
        SettingExtGridPanel(id, dc, modepage, false, MODE_NORMAL, true);
      }
    }
    public ExtGridPanel(int id, IDataControlUI dc, string title)
      : this(id, dc, null, title)
    {

    }
    public ExtGridPanel(int id, IDataControlUI dc, Ext.Net.Menu GridMenu, string title)
    {
      bool master = (HttpContext.Current.Request["child"] != null)
        && (HttpContext.Current.Request["child"] != "0");

      int mode = ((ViewListProperties)dc.GetProperties()).AllowMultiDelete ? MODE_MULTI_SELECT : MODE_NORMAL;
      SettingExtGridPanel(id, dc, MODE_PAGE_NORMAL, master, mode, (GridMenu != null));

      if (GridMenu != null)
      {
        ExtGridPanel.SetGridMenu(GridMenu);
      }
      Title = title;
    }
    public ExtGridPanel(int id, IDataControlUI dc, bool master, bool edit)
    {
      int mode = ((ViewListProperties)dc.GetProperties()).AllowMultiDelete ? MODE_MULTI_SELECT : MODE_NORMAL;
      SettingExtGridPanel(id, dc, MODE_PAGE_NORMAL, master, mode, true);
    }
    public void SettingExtGridPanel(int id, IDataControlUI dc, int modepage, bool master, int mode, bool contextmenu)
    {
      Id = id;
      ID = "GridPanel1";
      StoreID = "Store1";
      /*Comment if you want to show context menu from browser, eg inspect*/
      bool noContextMenu = ((mode == MODE_MULTI_SELECT) || (mode == MODE_SINGLE_SELECT));
      if ((!noContextMenu) && MasterAppConstants.Instance.ShowContextMenu && contextmenu)
      {
        ContextMenuID = "GridMenu";
      }
      Region = Region.Center;
      Height = new Unit(300);
      Width = new Unit(240);
      Header = false;
      Border = false;
      StripeRows = true;
      TrackMouseOver = true;
      AutoWidth = true;
      LoadMask.ShowMask = true;

      DataControlFieldCollection cols = new DataControlFieldCollection();
      if (dc.GetType().BaseType.Equals(typeof(BaseDataControlExt)))
      {
        cols = ((BaseDataControlExt)dc).GetColumns();
      }
      if ((cols == null) || (cols.Count == 0))
      {
        cols = dc.GetColumns();
        //@todo Insert ke database
      }
      if (modepage == MODE_PAGE_NORMAL)
      {
        String res = (string)HttpContext.Current.Session["ScreenResolution"];
        if (res == null)
        {
          res = "1024x768";
        }
        string[] ress = res.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);

        SetColumnCollection(this, dc, cols, int.Parse(ress[0]) - WIDTH_TREE_MENU - 50, mode);
      }
      else
      {
        SetColumnCollection(this, dc, cols, 700, mode);
      }
      if (typeof(IHasMenuCommand).IsInstanceOfType(dc))
      {
        ((IHasMenuCommand)dc).SetMenuCommand(ColumnModel.Columns);
      }
      #region Setting Mode
      #region Command
      int entrystyle = ((ViewListProperties)dc.GetProperties()).EntryStyle;
      bool canredirect = ((ViewListProperties)dc.GetProperties()).CanRedirect;

      int width = EDIT_COMMAND_COLUMN_WIDTH;
      ImageCommandColumn col = new ImageCommandColumn() { Width = new Unit(width, UnitType.Pixel), Align = Alignment.Center };
      ImageCommand cmd = null;
      switch (mode)
      {
        case MODE_EDIT:
          cmd = new ImageCommand() { CommandName = "Edit", Icon = Icon.TableEdit };
          cmd.ToolTip.Text = ConstantDict.Translate("LBL_EDIT");
          col.Commands.Add(cmd);
          break;
        case MODE_SINGLE_SELECT:
          //Di aspxnya aja
          //cmd = new ImageCommand() { CommandName = "Select", Icon = Icon.Accept };
          //cmd.ToolTip.Text = ConstantDict.Translate("LBL_SELECT");
          //col.Commands.Add(cmd);

          break;
        case MODE_MULTI_SELECT:

          break;
        case 12://Redirect
          if (canredirect)
          {
            cmd = new ImageCommand() { CommandName = "View", Icon = Icon.TableGo, Text = "View" };
            cmd.ToolTip.Text = "View";
            col.Commands.Add(cmd);
          }
          break;
      }
      col.Width = new Unit(col.Commands.Count * EDIT_COMMAND_COLUMN_WIDTH, UnitType.Pixel);
      ColumnModel.Columns.Add(col);

      Plugins.Add(BuildGridFilters(dc, id.ToString()));
      #endregion

      #region SelectionModel
      string[] pks = ((ViewListProperties)dc.GetProperties()).PrimaryKeys;
      if (mode == MODE_MULTI_SELECT)
      {
        CheckboxSelectionModel checkSelectionModel = new CheckboxSelectionModel();
        SelectionModel.Add(checkSelectionModel);
      }
      else
      {
        #region RowSelectionModel
        RowSelectionModel rowSelectionModel = new RowSelectionModel { SingleSelect = true };
        rowSelectionModel.Listeners.BeforeRowSelect.Handler = @"
            if (!!#{GridPanel1}.getSelectionModel().getSelected()) {
              #{GridPanel1}.getSelectionModel().clearSelections();
            }
          ";
        rowSelectionModel.Listeners.RowSelect.Handler = @"
            if (!!#{GridPanel1}.getSelectionModel().getSelected()) {
              /*#{FPFormEntry1}.reset();
              #{FPFormEntry1}.getForm().loadRecord(record);*/
              CoreNET.Methods1.RefreshDetilInfo(Ext.encode(record.data));" +
            ((master) ? @"
                  if(!!parent.CoreNET.RefreshDetilPage){
                    parent.CoreNET.RefreshDetilPage(Ext.encode(record.data));
                    NormalizeFrame();
                    #{PanelFormEntry}.collapse();
                  }
                  "
            : string.Empty) +
            @"
            }
          ";
        SelectionModel.Add(rowSelectionModel);
        #endregion
      }
      #endregion
      #region Command Listener
      string PagingToolbarID = "Paging" + ID;
      if (modepage == MODE_PAGE_NORMAL)
      {
        Listeners.Command.Handler = @"
          if(command.indexOf('Edit') != -1){
            /*if(!! #{FPFormEntry1})
            {
              #{FPFormEntry1}.reset();
              #{FPFormEntry1}.getForm().loadRecord(record);
            }*/
            CoreNET.Methods1.btnEditClick(Ext.encode(record.data), command);
            #{PanelFormEntry}.expand();
          }else if(command.indexOf('View') != -1)
          {
            CoreNET.Methods1.btnViewClick(record.data, command);
          }else if(command.indexOf('Detil') != -1)
          {
            #{PanelFormEntry}.expand();
            ExpandFrame();
            CoreNET.Methods1.RefreshDetilInfo(Ext.encode(record.data));
          }else if(command.indexOf('Link') != -1)
          {
            CoreNET.Methods1.btnViewClick(record.data, command);
          }else if(command.indexOf('Select') != -1)
          {
            parent.CoreNET.Select(Ext.encode(record.data), command);
            parent.WindowLookup1.close();
          }else if(command == 'showhint')
          {
            alert('Tes');
          }
          else
          {
          }
          ExpandFrame();
        ";
      }
      if (modepage == MODE_PAGE_NORMAL)
      {
        Listeners.AfterEdit.Fn = "afterEdit"; //Handler =
        Listeners.KeyDown.Fn = "startEditing"; //Handler =
        //"#{GridPanel1}.getRowEditor().startEditing(rowIndex)";//record.index
      }
      #endregion
      #endregion
      View.Add(new LockingGridView());

      Toolbar tb = new Toolbar();
      Ext.Net.Panel label = new Ext.Net.Panel() { ID = "Signature" };
      label.Html = @"<a onclick=""alert('Core.NET 2017')""><a>";
      tb.Items.Add(label);

      Ext.Net.Button btnSign = new Ext.Net.Button() { ID = idbtn };
      btnSign.OnClientClick = @"alert('contact: dev.usadi@gmail.com')";
      tb.Items.Add(btnSign);
      BottomBar.Add(tb);
    }

    private static string idbtn = Guid.NewGuid().ToString().Replace("-", "");//Diencript ya
    public void Validate(Page page)
    {
      Ext.Net.Button btn = ControlUtils.FindControl<Ext.Net.Button>(this, idbtn);
      if (btn == null)
      {
        string script = "alert('')";
        page.ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), script, true);
      }
    }
    #region constant Handler
    public const int EDIT_HANDLER = 1;
    public const int ADD_HANDLER = 2;
    public const int DELETE_HANDLER = 3;
    public const int TYPE_PAGE_TOOLBAR_25 = 1;
    public const int TYPE_PAGE_TOOLBAR_10 = 2;
    #endregion
    public static void SetGridMenu(Ext.Net.Menu GridMenu)
    {
      int id = GlobalAsp.GetRequestI();
      IDataControlUI dc = (IDataControlUI)UtilityUI.GetDataControl(id);

      if ((((BaseBO)dc).GetMaxModePreviewIndex()) > 0)
      {
        int mode = ((BaseBO)dc).GetModePreviewIndex();
        Ext.Net.MenuItem PreviewByIndex = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_PREVIEW_BY_INDEX") + " (" + mode + ")" };
        PreviewByIndex.Listeners.Click.Handler = @"
              CoreNET.Methods1.ChangeModePreview();
            ";
        GridMenu.Items.Add(PreviewByIndex);
      }

      Ext.Net.MenuItem ClearSelection = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_CLEAR_SELECTION") };
      ClearSelection.Listeners.Click.Handler = @"
              CoreNET.Methods1.GridDeleteSelected();
              ExpandFrame();
              ClearDetilPage();
            ";
      GridMenu.Items.Add(ClearSelection);

      Ext.Net.MenuItem ViewInfo = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_VIEW_INFO") };
      ViewInfo.Listeners.Click.Handler = @"
        #{PanelFormEntry}.expand();
        ExpandFrame();
        if (!!#{GridPanel1}.getSelectionModel().getSelected()) {
          var sels = (#{GridPanel1}.getRowsValues({selectedOnly : true}))
          CoreNET.Methods1.RefreshDetilInfo(Ext.encode(sels));
        }else
        {
          Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
        }
      ";
      GridMenu.Items.Add(ViewInfo);

      if ((HttpContext.Current.Request["child"] != null) && (int.Parse(HttpContext.Current.Request["child"]) > 0))
      {
        Ext.Net.MenuItem ViewChild = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_VIEW_CHILD") };
        ViewChild.Listeners.Click.Handler = "NormalizeFrame();";
        GridMenu.Items.Add(ViewChild);
      }
      if (((ViewListProperties)dc.GetProperties()).IsModeAdd())
      {
        Ext.Net.MenuItem MenuCopyBaris = new Ext.Net.MenuItem() { Text = ConstantDictExt.Translate("MENU_COPY_ROW") };
        MenuCopyBaris.Listeners.Click.Handler = @"
            ExpandFrame();
            if(!!#{GridPanel1}.getSelectionModel().getSelected())
            {
              var sels = (#{GridPanel1}.getRowsValues({selectedOnly : true}))
              CoreNET.Methods1.btnCopyBarisClick(Ext.encode(sels));
            }else
            {
              Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
            }
        ";
        GridMenu.Items.Add(MenuCopyBaris);
      }
    }
    public static void SetRegisteredIcons(ResourceManager ResourceManager1)
    {
      ResourceManager1.RegisterIcon(Icon.Accept);
      ResourceManager1.RegisterIcon(Icon.Book);
      ResourceManager1.RegisterIcon(Icon.BookGo);
      ResourceManager1.RegisterIcon(Icon.Calculator);
      ResourceManager1.RegisterIcon(Icon.Cancel);
      ResourceManager1.RegisterIcon(Icon.Cart);
      ResourceManager1.RegisterIcon(Icon.CartAdd);
      ResourceManager1.RegisterIcon(Icon.CartFull);
      ResourceManager1.RegisterIcon(Icon.CartGo);
      ResourceManager1.RegisterIcon(Icon.CartPut);
      ResourceManager1.RegisterIcon(Icon.Comment);
      ResourceManager1.RegisterIcon(Icon.BookGo);
      ResourceManager1.RegisterIcon(Icon.BulletYellow);
      ResourceManager1.RegisterIcon(Icon.BulletGreen);
      ResourceManager1.RegisterIcon(Icon.BulletBlue);
      ResourceManager1.RegisterIcon(Icon.BulletRed);
      ResourceManager1.RegisterIcon(Icon.Exclamation);
      ResourceManager1.RegisterIcon(Icon.Folder);
      ResourceManager1.RegisterIcon(Icon.House);
      ResourceManager1.RegisterIcon(Icon.HouseStar);
      ResourceManager1.RegisterIcon(Icon.HouseGo);
      ResourceManager1.RegisterIcon(Icon.Laptop);
      ResourceManager1.RegisterIcon(Icon.LaptopAdd);
      ResourceManager1.RegisterIcon(Icon.LaptopGo);
      ResourceManager1.RegisterIcon(Icon.LorryDelete);
      ResourceManager1.RegisterIcon(Icon.LorryError);
      ResourceManager1.RegisterIcon(Icon.LorryGo);
      ResourceManager1.RegisterIcon(Icon.Lorry);
      ResourceManager1.RegisterIcon(Icon.MoneyDelete);
      ResourceManager1.RegisterIcon(Icon.Money);
      ResourceManager1.RegisterIcon(Icon.MoneyAdd);
      ResourceManager1.RegisterIcon(Icon.Note);
      ResourceManager1.RegisterIcon(Icon.Package);
      ResourceManager1.RegisterIcon(Icon.PackageAdd);
      ResourceManager1.RegisterIcon(Icon.PackageGo);
      ResourceManager1.RegisterIcon(Icon.Pencil);
      ResourceManager1.RegisterIcon(Icon.Table);
      ResourceManager1.RegisterIcon(Icon.TableAdd);
      ResourceManager1.RegisterIcon(Icon.TableGo);
      ResourceManager1.RegisterIcon(Icon.UserGo);
      ResourceManager1.RegisterIcon(Icon.UserHome);
    }
    public static PagingToolbar BuildPagingToolbarEmpty(IDataControlUI dc, int type)
    {
      PagingToolbar TopBar1 = new PagingToolbar
      {
        ID = "TopBar1"
      };
      if (type == 1)
      {
        TopBar1.PageSize = ((ViewListProperties)dc.GetProperties()).PageSize;
      }
      else
      {
        TopBar1.PageSize = 5;
      }
      ComboBox cbPageSize = new ComboBox() { Width = new Unit(50, UnitType.Pixel) };
      cbPageSize.Items.Add(new Ext.Net.ListItem("5"));
      cbPageSize.Items.Add(new Ext.Net.ListItem("10"));
      cbPageSize.Items.Add(new Ext.Net.ListItem("15"));
      cbPageSize.Items.Add(new Ext.Net.ListItem("20"));
      cbPageSize.Items.Add(new Ext.Net.ListItem("50"));
      cbPageSize.Items.Add(new Ext.Net.ListItem("100"));
      cbPageSize.Items.Add(new Ext.Net.ListItem("150"));
      cbPageSize.Items.Add(new Ext.Net.ListItem("200"));
      cbPageSize.Items.Add(new Ext.Net.ListItem("300"));
      cbPageSize.Items.Add(new Ext.Net.ListItem("500"));
      cbPageSize.Text = TopBar1.PageSize.ToString();
      cbPageSize.Listeners.Select.Handler = @"#{TopBar1}.pageSize = parseInt(this.getValue()); #{TopBar1}.doLoad();";


      TopBar1.Items.Add(new Ext.Net.Label() { Text = "Page Size:" });
      TopBar1.Items.Add(new ToolbarSpacer());
      TopBar1.Items.Add(cbPageSize);
      return TopBar1;
    }
    public static PagingToolbar BuildPagingToolbarDefault(int type)
    {
      int id = GlobalAsp.GetRequestI();
      IDataControlUI dc = (IDataControlUI)UtilityUI.GetDataControl(id);
      PagingToolbar TopBar1 = BuildPagingToolbarEmpty(dc, type);
      bool tbprint = (((ViewListProperties)dc.GetProperties()).ModeToolbar == ViewListProperties.MODE_TOOLBAR_PRINT);

        string lblInfo = GlobalAsp.GetConfigLabelInfo();
      string msg = ConstantDictExt.Translate("LBL_EMPTY_FILTER");
      if (((ViewListProperties)dc.GetProperties()).IsModeAdd() && (HttpContext.Current.Request["enable"] != "0"))
      {
        Ext.Net.Button btnAdd = new Ext.Net.Button() { ID = GlobalAsp.BTN_ADD1, Icon = Icon.Add };
        btnAdd.Text = ConstantDictExt.Translate(btnAdd.ID);
        btnAdd.ToolTip = ConstantDictExt.TranslateTabTip(btnAdd);
        if (((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_FORM)
        {
          #region btnAddListener
          btnAdd.Listeners.Click.Handler = @"
            ExpandFrame();
            var valid = validateFilterWhenAdd();
            if(valid)
            {
              var sels = (#{GridPanel1}.getRowsValues({selectedOnly : true}))
              CoreNET.Methods1.btnAddFromFormEntry(sels);
            }else{
              Ext.Msg.alert('" + lblInfo + @"','" + msg + @"');
            }
          ";
          #endregion
        }
        else if ((((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_LOOKUP)
          || (((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_TREE)
          || (((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_LOOKUP_ENTRY)
          || (((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_TREE_ENTRY))
        {
          btnAdd.Listeners.Click.Handler = @"
            ExpandFrame();
            var valid = validateFilterWhenAdd();
            if(valid)
            {
              var sels = (#{GridPanel1}.getRowsValues({selectedOnly : true}))
              CoreNET.Methods1.btnAddFromDlgLookup(sels);
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
              var sels = (#{GridPanel1}.getRowsValues({selectedOnly : true}))
              CoreNET.Methods1.btnAddDlgFormEntry(sels);
            }else{
              Ext.Msg.alert('" + lblInfo + @"','" + msg + @"');
            }
          ";
        }
        if (HttpContext.Current.Request["hide"] != "1")
        {
          TopBar1.Add(btnAdd);
        }
      }
      if (((ViewListProperties)dc.GetProperties()).IsModeEdit())
      {
        /*Tetap perlu meskipun tombol edit bisa di form entry dan disetiap baris data */
        Ext.Net.Button btnEdit = new Ext.Net.Button() { ID = GlobalAsp.BTN_EDIT1, Text = "Edit", Icon = Icon.Pencil };
        btnEdit.Text = ConstantDictExt.Translate(btnEdit.ID);
        btnEdit.ToolTip = ConstantDictExt.TranslateTabTip(btnEdit);
        btnEdit.Listeners.Click.Handler = @"
                if(#{GridPanel1}.getSelectionModel().hasSelection())
                {
                  ExpandFrame();
                  var sels = (#{GridPanel1}.getRowsValues({selectedOnly : true}))
                  CoreNET.Methods1.btnEditClick(sels,'EditForm');
                }else
                {
                 Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
                }";
        TopBar1.Add(btnEdit);
      }
      if (((ViewListProperties)dc.GetProperties()).IsModeDelete() && (HttpContext.Current.Request["enable"] != "0"))
      {
        Ext.Net.Button btnDel = new Ext.Net.Button() { ID = GlobalAsp.BTN_DEL1, Icon = Icon.Delete };
        btnDel.Text = ConstantDictExt.Translate(((ViewListProperties)dc.GetProperties()).LblBtnDel);
        btnDel.ToolTip = ConstantDictExt.TranslateTabTip(btnDel);
        btnDel.Listeners.Click.Handler = @"
            ExpandFrame();
            if (!!#{GridPanel1}.getSelectionModel().getSelected()) {
              Ext.Msg.confirm('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "', '" + ((ViewListProperties)dc.GetProperties()).MsgConfirmDelete + @"', function (id, val) {
                if (id === 'yes') {
                    var sels = (#{GridPanel1}.getRowsValues({selectedOnly : true}))
                    CoreNET.Methods1.btnDeleteClick(sels);
                }
              }, this);
            }else
            {
              Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
            }
        ";
        TopBar1.Add(btnDel);
      }

      if (UtilityUI.GetSessionExtPage().Equals(UtilityUI.GRID_BALANCE))
      {
        return TopBar1;
      }

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
                loadingmasklink();ExpandFrame();CoreNET.Methods1.btnPrintClick('"+ Rpts[i].Title + @"',{0});
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
          Ext.Net.MenuItem btnPreview0 = new Ext.Net.MenuItem() { ID = GlobalAsp.BTN_MASTER };
          //Ext.Net.Button btnPreview0 = new Ext.Net.Button() { ID = GlobalAsp.BTN_MASTER };
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

        if (typeof(IDataControlTreeGrid3).IsInstanceOfType(dc) && ((ViewListProperties)dc.GetProperties()).CanModeTree)
        {
          Ext.Net.MenuItem btnPreview = new Ext.Net.MenuItem() { ID = GlobalAsp.BTN_PREVIEW_TREE, Icon = Icon.ReportMagnify };
          //Ext.Net.Button btnPreview = new Ext.Net.Button() { ID = GlobalAsp.BTN_PREVIEW_TREE, Icon = Icon.ReportMagnify };
          btnPreview.Text = ConstantDictExt.Translate(btnPreview.ID);
          btnPreview.ToolTip = ConstantDictExt.TranslateTabTip(btnPreview);
          btnPreview.Listeners.Click.Handler =
              @"ExpandFrame();CoreNET.RefreshPreview();";
          //TopBar1.Add(btnPreview);
          menuSetting.Add(btnPreview);
        }
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

        Ext.Net.MenuItem mngrid = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_GRID1, Text = "Info", Icon = Icon.Table };
        mngrid.Text = ConstantDictExt.Translate(mngrid.ID);
        mngrid.Listeners.Click.Handler = "CoreNET.Methods1.btnAdminClick(1)";
        menuDebug.Items.Add(mngrid);

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
        ////operasi ini ketika tombol save, karena layout atas bawah
        //string scriptRefreshMasterPage = string.Empty;
        //if (((ViewListProperties)dc.GetProperties()).RefreshMaster)//Ketika RefreshMaster true
        //{
        //  scriptRefreshMasterPage = "refreshMasterPage();";
        //}

        Ext.Net.Button btnClose = new Ext.Net.Button() { ID = GlobalAsp.BTN_CLOSE1, Icon = Icon.DoorOpen };
        btnClose.Text = ConstantDictExt.Translate(btnClose.ID);
        //if (!!parent.CoreNET.Methods1) { parent.CoreNET.Methods1.btnCloseDialogClick();}
        //" + scriptRefreshMasterPage + @"
        //if (!!parent.Methods1_WindowView1) { parent.Methods1_WindowView1.close(); }
        btnClose.Listeners.Click.Handler = @"
          if (!!parent.CoreNET.Methods1) { parent.CoreNET.Methods1.btnCloseDialogClick();}
          if (!!parent.WindowLookup1) { parent.WindowLookup1.hide(this); }
          " + scriptRefreshParent + @"
        ";
        TopBar1.Add(btnClose);
      }

      return TopBar1;
    }
    public const int MODE_NORMAL = 0;
    public const int MODE_EDIT = 1;
    public const int MODE_SINGLE_SELECT = 2;
    public const int MODE_MULTI_SELECT = 4;
    public static void SetColumnCollection(GridPanel gridPanel, IDataControlUI dc, DataControlFieldCollection cols
      , int totalwidth, int mode)
    {
      string[] sortFields = ((ViewListProperties)dc.GetProperties()).SortFields;
      ArrayList array = new ArrayList(sortFields);
      ColumnCollection Columns = gridPanel.ColumnModel.Columns;
      int width = totalwidth;

      //ini logicnya gmn ya?
      //if ((mode == MODE_EDIT) && (mode == MODE_SINGLE_SELECT) && (mode == MODE_MULTI_SELECT))
      //
      if (mode == MODE_MULTI_SELECT)
      {
        width = totalwidth - EDIT_COMMAND_COLUMN_WIDTH;
      }
      bool canredirect = ((ViewListProperties)dc.GetProperties()).CanRedirect;
      if (canredirect)
      {
        width = width - VIEW_COMMAND_COLUMN_WIDTH;
      }

      int totwid = 0;
      for (int i = 0; i < cols.Count; i++)
      {
        if (cols[i].ItemStyle.Width.Value != 0)
        {
          totwid += (int)(cols[i].ItemStyle.Width.Value * width / 100);
        }
      }

      for (int i = 0; i < cols.Count; i++)
      {
        if (UtilityUI.GetSessionExtPage().Equals(UtilityUI.PAGE_JURNAL))
        {
          ((MyBoundField)cols[i]).Editable = false;
        }
        string coltype = cols[i].FooterText.ToLower();
        if (coltype.Contains("rownumberer"))
        {
          RowNumbererColumn col = new RowNumbererColumn() { Locked = true };
          col.Locked = true;
          col.Sortable = true;
          col.Fixed = true;
          col.Width = 50;
          Columns.Add(col);//klo ratusan, jd ketutup
        }
        else if (coltype.Contains("rating"))
        {
          RatingColumn col = new RatingColumn() { Align = Alignment.Center };
          col.Header = "#";
          col.DataIndex = "Rating";
          col.Width = new Unit(10 * width / 100, UnitType.Pixel);
          col.Align = Alignment.Center;
          col.MaxRating = 3;
          col.Locked = ((MyBoundField)cols[i]).Locked;
          Columns.Add(col);
        }
        else if (coltype.Contains("datetime"))
        {
          #region DateColumn
          DateColumn c = new DateColumn
          {
            //c.ColumnID = cols[i].HeaderText;//filter jadi gak jalan
            Header = cols[i].HeaderText,
            Tooltip = cols[i].HeaderText,
            MenuDisabled = false,
            Align = Alignment.Center
          };
          if (cols[i].ItemStyle.Width.Value == 0)
          {
            c.Width = new Unit(width - totwid, UnitType.Pixel);
          }
          else
          {
            c.Width = new Unit(cols[i].ItemStyle.Width.Value * width / 100, UnitType.Pixel);
          }
          if (cols[i].GetType() == typeof(MyBoundField))
          {
            c.Sortable = true;
            c.DataIndex = ((MyBoundField)cols[i]).DataField;
            c.Locked = ((MyBoundField)cols[i]).Locked;
            c.Format = "dd-MM-yyyy";// ((MyBoundField)cols[i]).DataFormatString;
            if (((MyBoundField)cols[i]).Editable)
            {
              DateField datefield = new DateField
              {
                AllowBlank = false
              };
              c.Editor.Add(datefield);
            }
            if (((MyBoundField)cols[i]).IsCommand)
            {
              ImageCommand col = (ImageCommand)((MyBoundField)cols[i]).Command;
              c.Commands.Add(col);
              c.PrepareCommand.Fn = "prepare" + c.DataIndex + "Command";
            }
          }
          else if (cols[i].GetType() == typeof(ButtonField))
          {
            c.DataIndex = ((ButtonField)cols[i]).DataTextField;
            c.Format = ((ButtonField)cols[i]).DataTextFormatString;
          }
          Columns.Add(c);
          #endregion
        }
        else if (coltype.ToLower().Contains("bool"))
        {
          #region BoolColumn

          //BooleanColumn c = new BooleanColumn (STABIL, tapi renderer ngga jalan.Editing udah pake checkbox)
          //CheckColumn c = new CheckColumn();
          //CheckboxColumn c = new CheckboxColumn();
          //c.ColumnID = cols[i].HeaderText;//filter jadi gak jalan
          Column c = new Column
          {
            Header = cols[i].HeaderText,
            Tooltip = cols[i].HeaderText,
            MenuDisabled = false,
            Align = Alignment.Center
          };
          if (cols[i].ItemStyle.Width.Value == 0)
          {
            c.Width = new Unit(width - totwid, UnitType.Pixel);
          }
          else
          {
            c.Width = new Unit(cols[i].ItemStyle.Width.Value * width / 100, UnitType.Pixel);
          }
          if (cols[i].GetType() == typeof(MyBoundField))
          {
            c.Sortable = true;
            c.DataIndex = ((MyBoundField)cols[i]).DataField;
            c.Locked = ((MyBoundField)cols[i]).Locked;
            if (((MyBoundField)cols[i]).Editable)
            {
              Checkbox checkbox = new Checkbox();
              c.Editor.Add(checkbox);
            }
            if (((MyBoundField)cols[i]).IsCommand)
            {
              ImageCommand col = (ImageCommand)((MyBoundField)cols[i]).Command;
              c.Commands.Add(col);
              c.PrepareCommand.Fn = "prepare" + c.DataIndex + "Command";
            }
          }
          else if (cols[i].GetType() == typeof(ButtonField))
          {
            c.DataIndex = ((ButtonField)cols[i]).DataTextField;
          }
          Renderer r = new Renderer
          {
            Fn = "checkify"
          };
          c.Renderer = r;
          Columns.Add(c);
          #endregion
        }
        else if (coltype.Contains("decimal") || coltype.Contains("money"))
        {
          #region NumberColumn
          NumberColumn c = new NumberColumn
          {
            //c.ColumnID = cols[i].HeaderText;//filter jadi gak jalan
            Header = cols[i].HeaderText,
            Tooltip = cols[i].HeaderText,
            MenuDisabled = false,
            Align = Alignment.Right
          };
          if (coltype.Contains("decimal"))
          {
            c.Format = "0,000.00";
          }
          else//money
          {
            c.Format = "0,000";
          }
          if (cols[i].ItemStyle.Width.Value == 0)
          {
            c.Width = new Unit(width - totwid, UnitType.Pixel);
          }
          else
          {
            c.Width = new Unit(cols[i].ItemStyle.Width.Value * width / 100, UnitType.Pixel);
          }
          if (cols[i].GetType() == typeof(MyBoundField))
          {
            c.Sortable = true;
            c.DataIndex = ((MyBoundField)cols[i]).DataField;
            c.Locked = ((MyBoundField)cols[i]).Locked;
            if (((MyBoundField)cols[i]).Editable)
            {
              NumberField editor = new NumberField();
              c.Editor.Add(editor);
            }
            if (((MyBoundField)cols[i]).IsCommand)
            {
              ImageCommand col = (ImageCommand)((MyBoundField)cols[i]).Command;
              c.Commands.Add(col);
              c.PrepareCommand.Fn = "prepare" + c.DataIndex + "Command";
            }
          }
          else if (cols[i].GetType() == typeof(ButtonField))
          {
            c.DataIndex = ((ButtonField)cols[i]).DataTextField;
          }

          Renderer r = new Renderer
          {
            Fn = "formatdec1"//tes ya
          };
          c.Renderer = r;
          Columns.Add(c);
          #endregion
        }
        else if (coltype.Contains("command"))
        {

          #region Command
          ImageCommandColumn col = new ImageCommandColumn() { Align = Alignment.Center };
          col.Icons.Add(Icon.Button);
          col.Align = Alignment.Center;
          col.Width = new Unit(cols[i].ItemStyle.Width.Value * width / 100, UnitType.Pixel);
          col.Tooltip = ConstantDict.Translate(((MyBoundField)cols[i]).DataField + "Tip");
          col.Header = "<img title='" + col.Tooltip + "' src='../Res/Icons/kotakbiru.png'>";
          ImageCommand[] cmds = (ImageCommand[])((MyBoundField)cols[i]).Command;

          if (cmds == null)
          {
            col.PrepareCommands.Fn = "prepareCommands";
          }
          else
          {
            col.Commands.AddRange(cmds);
          }
          Columns.Add(col);
          #endregion
        }
        else if (coltype.Contains("column"))
        {
          Column c = (Column)((MyBoundField)cols[i]).Command;
          Columns.Add(c);
        }
        else
        {
          #region DefaultColumn
          Column c = new Column();
          //c.ColumnID = cols[i].HeaderText;//filter jadi gak jalan
          string idproperty = ((ViewListProperties)dc.GetProperties()).IDProperty;
          c.Header = cols[i].HeaderText;
          c.Tooltip = cols[i].HeaderText;
          c.MenuDisabled = false;
          c.Align = (cols[i].ItemStyle.HorizontalAlign == HorizontalAlign.Left) ? Alignment.Left : (cols[i].ItemStyle.HorizontalAlign == HorizontalAlign.Center) ? Alignment.Center : Alignment.Right;
          if (cols[i].ItemStyle.Width.Value == 0)
          {
            c.Width = new Unit(width - totwid, UnitType.Pixel);
          }
          else
          {
            c.Width = new Unit(cols[i].ItemStyle.Width.Value * width / 100, UnitType.Pixel);
          }
          if (cols[i].GetType() == typeof(MyBoundField))
          {
            c.Sortable = true;
            c.DataIndex = ((MyBoundField)cols[i]).DataField;
            c.AutoDataBind = true;
            c.Locked = ((MyBoundField)cols[i]).Locked;
            #region Editor
            if (((MyBoundField)cols[i]).Editable)
            {
              if (((MyBoundField)cols[i]).AvailableValues != null)
              {
                ComboBox comboBox = new ComboBox
                {
                  ID = "Editor" + c.DataIndex,
                  EmptyText = "Select",
                  AutoFocus = false,
                  ForceSelection = false,
                  AutoScroll = false,
                  SelectOnFocus = false,
                  TypeAhead = true,
                  Mode = DataLoadMode.Local,
                  TriggerAction = TriggerAction.All
                };
                comboBox.ForceSelection = true;
                comboBox.AnchorHorizontal = "95%";
                IList values = ((MyBoundField)cols[i]).AvailableValues;
                string[] fields = ((MyBoundField)cols[i]).KeyFields;
                comboBox.ValueField = fields[0];
                comboBox.DisplayField = fields[1];
                ExtStore store = new ExtStore("listvalues" + c.DataIndex, fields, values);
                comboBox.Store.Add(store);
                //if (pr.Parent != "")
                //{
                //  store.BaseParams.Add(new Ext.Net.Parameter() { Name = pr.Parent, Mode = ParameterMode.Raw, Value = "#{CB" + pr.Parent + "}.getValue()" });
                //}
                //store.RefreshData += new Store.AjaxRefreshDataEventHandler(store_RefreshData);
                //comboBox.Store.Add(store);
                //comboBox.Width = pr.Width * (ExtWindows.DEFAULT_WIDTH - ExtWindows.DEFAULT_LABEL_WIDTH) / 100;
                //if (pr.AllowRefresh)
                //{
                //  comboBox.Listeners.Select.Handler = (pr.Child == "") ? "" : "#{CB" + pr.Child + "}.clearValue();#{CB" + pr.Child + "}.store.reload();";
                //}
                c.Editor.Add(comboBox);
              }
              else
              {
                TextField editor = new TextField();
                c.Editor.Add(editor);
              }
            }
            #endregion
            if (((MyBoundField)cols[i]).IsCommand)
            {
              ImageCommand col = (ImageCommand)((MyBoundField)cols[i]).Command;
              c.Commands.Add(col);
              if (!col.CommandName.Equals("EditForm"))
              {
                c.PrepareCommand.Fn = "prepare" + c.DataIndex + "Command";
              }
            }
          }
          else if (cols[i].GetType() == typeof(ButtonField))
          {
            c.DataIndex = ((ButtonField)cols[i]).DataTextField;
          }

          Renderer r = new Renderer();

          if (string.IsNullOrEmpty(((MyBoundField)cols[i]).Fn))
          {
            if ((cols[i].GetType() == typeof(MyBoundField)) && ((MyBoundField)cols[i]).IsLink)
            {
              r.Fn = "linkify";
            }
            else if (coltype.ToLower().Contains("icon"))
            {
              c.Tooltip = ConstantDict.Translate(((MyBoundField)cols[i]).DataField + "Tip");
              r.Fn = "iconifygrid";
            }
            else if (coltype.ToLower().Contains("image"))
            {
              r.Fn = "imagify";
            }
            else if (coltype.Contains("decimal"))
            {
              r.Fn = "formatdec";
            }
            else if (coltype.Contains("bool"))
            {
              r.Fn = "checkify";
            }
          }
          else
          {
            r.Fn = ((MyBoundField)cols[i]).Fn;
          }
          c.Renderer = r;
          c.Hidden = !((MyBoundField)cols[i]).Visible;

          Columns.Add(c);
          #endregion
        }
      }
    }
    public static GridFilters BuildGridFilters(IDataControlUI cCtrl, string id)
    {
      GridFilters gridFilters = new GridFilters
      {
        ID = "gf" + id,
        Local = true
      };
      DataControlFieldCollection cols = new DataControlFieldCollection();
      cols = cCtrl.GetColumns();
      if (cols.Count == 0)
      {
        if (typeof(BaseDataControlExt).IsInstanceOfType(cCtrl))
        {
          cols = ((BaseDataControlExt)cCtrl).GetColumns();
        }
        //@todo Insert ke database
      }
      for (int i = 0; i < cols.Count; i++)
      {
        string footertext = "";
        string datafield = "";
        if (cols[i].GetType() == typeof(MyBoundField))
        {
          MyBoundField col = (MyBoundField)cols[i];
          footertext = col.FooterText;
          datafield = col.DataField;
        }
        else if (cols[i].GetType() == typeof(ButtonField))
        {
          ButtonField col = (ButtonField)cols[i];
          footertext = col.FooterText;
          datafield = col.DataTextField;
        }
        if (datafield == "Type")
        {
          gridFilters.Filters.Add(new ListFilter() { DataIndex = "Type", Options = new string[] { "H", "D" } });
        }
        else
        {
          if ((footertext == typeof(int).Name) || (footertext == typeof(long).Name) || (footertext == typeof(Decimal).Name))
          {
            gridFilters.Filters.Add(new NumericFilter() { DataIndex = datafield });
          }
          else if (footertext == typeof(DateTime).Name)
          {
            gridFilters.Filters.Add(new DateFilter() { DataIndex = datafield });
          }
          else if (footertext == typeof(bool).Name)
          {
            gridFilters.Filters.Add(new BooleanFilter() { DataIndex = datafield });
          }
          else
          {
            gridFilters.Filters.Add(new StringFilter() { DataIndex = datafield });
          }
        }
      }
      return gridFilters;
    }
  }
}
