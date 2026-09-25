using CoreNET.Common.BO;
using Ext.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

namespace CoreNET.Common.Base
{
  public class ExtFormDetail
  {
    //public static PanelBase GetPanelDetil(Toolbar TBDialog1)
    //{
    //  return GetPanelDetil(TBDialog1, true);
    //}
    public static void SetToolbarForm(Toolbar TBDialog1, bool visibleAdd)//deprecated
    {
      SetToolbarForm(TBDialog1);
    }
    public static void SetToolbarForm(Toolbar TBDialog1)
    {
      #region Inisialisasi
      int id = GlobalAsp.GetRequestI();
      IDataControlUIEntry dc = (IDataControlUIEntry)UtilityUI.GetDataControl(id);
      #endregion
      #region Add Form Entry Toolbar
      if (HttpContext.Current.Request["tb"] == "0")
      {
        TBDialog1.Hidden = true;
      }
      else
      {
        if (TBDialog1 != null)
        {
          bool visibleDel = UtilityUI.GetSessionMenuPage().Equals(UtilityUI.PAGE_FORM) &&
            ((ViewListProperties)dc.GetProperties()).IsModeDelete();

          bool visibleAdd = UtilityUI.GetSessionMenuPage().Equals(UtilityUI.PAGE_FORM) &&
            ((ViewListProperties)dc.GetProperties()).IsModeAdd();

          bool visiblePrint = UtilityUI.GetSessionMenuPage().Equals(UtilityUI.PAGE_FORM) &&
            ((ViewListProperties)dc.GetProperties()).IsToolbarPrint();

          bool tbprint = (((ViewListProperties)dc.GetProperties()).ModeToolbar == ViewListProperties.MODE_TOOLBAR_PRINT);

          bool visibleEdit = UtilityUI.GetSessionExtPage().Equals(UtilityUI.PAGE_FORM) &&
            (((ViewListProperties)dc.GetProperties()).IsModeEdit() ||
            (((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_LOOKUP_FORM));
          //bool entrystyleform = (((ViewListProperties)dc.GetProperties()).EntryStyle == ViewListProperties.ENTRY_STYLE_FORM)
          //  || UtilityUI.GetSessionMenuPage().Equals(UtilityUI.PAGE_FORM);
          Ext.Net.Button btnAdd = new Ext.Net.Button() { ID = GlobalAsp.BTN_ADD2, Icon = Icon.Add };
          btnAdd.Text = ConstantDictExt.Translate(btnAdd.ID);
          btnAdd.ToolTip = ConstantDictExt.TranslateTabTip(btnAdd);
          btnAdd.Listeners.Click.Handler = @"
              if(!!#{GridPanel1}){
                var sels = (#{GridPanel1}.getRowsValues({selectedOnly : true}))
                CoreNET.Methods1.btnAddFromFormEntry(sels);
              }else{
                CoreNET.Methods1.btnAddFromFormEntry('');
              }
          ";
          if (visiblePrint)
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

            //if (((ViewListProperties)dc.GetProperties()).IsModePrint())
            if (menuPrint.Items.Count > 0)
            {
              btnPrint.Menu.Add(menuPrint);
              TBDialog1.Add(btnPrint);
            }
            //btnPrint.Disabled = !((ViewListProperties)dc.GetProperties()).IsModePrint();
            #endregion Menu Print
          }
          if (visibleAdd)
          {
            if (((ViewListProperties)dc.GetProperties()).IsModeAdd())
            {
              TBDialog1.Add(btnAdd);
            }
            btnAdd.Disabled = !((ViewListProperties)dc.GetProperties()).IsModeAdd();
          }
          if (visibleDel)
          {
            Ext.Net.Button btnDel = new Ext.Net.Button() { ID = GlobalAsp.BTN_DEL2, Icon = Icon.Delete };
            btnDel.Text = ConstantDictExt.Translate(btnDel.ID);// (((ViewListProperties)dc.GetProperties()).LblBtnDel);
            btnDel.ToolTip = ConstantDictExt.TranslateTabTip(btnDel);
            //var sels = #{FPFormEntry1}.getForm().getValues();//return value by ID not DataIndex
            //var sels = #{FPFormEntry1}.getForm().getFieldValues();
            btnDel.Listeners.Click.Handler = @"
              Ext.Msg.confirm('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "', '" + ConstantDict.Translate("LBL_CONFIRM_DELETE") + @"', function (id, val) {
                if (id === 'yes') {
                    var sels = #{FPFormEntry1}.getForm().getValues();
                    CoreNET.Methods1.btnDeleteClick(sels);
                }
              }, this);
              CoreNET.Methods1.btnCancelClick();
            ";
            TBDialog1.Add(btnDel);
          }

          Ext.Net.Button btnEdit = new Ext.Net.Button() { ID = GlobalAsp.BTN_EDIT2, Icon = Icon.Pencil };
          btnEdit.Text = ConstantDictExt.Translate(btnEdit.ID);
          btnEdit.ToolTip = ConstantDictExt.TranslateTabTip(btnEdit);
          btnEdit.Listeners.Click.Handler = @"
            if(!!#{GridPanel1})
            {
              if (#{GridPanel1}.getSelectionModel().hasSelection())
              {
                var sels = (#{GridPanel1}.getRowsValues({selectedOnly : true}))
                CoreNET.Methods1.btnEditClick(Ext.encode(sels),'EditForm');
              }else
              {
                Ext.Msg.alert('" + ConstantDictExt.Translate(GlobalAsp.LBL_INFO) + "','" + ConstantDictExt.Translate("LBL_ERROR_DATA_NOT_SELECTED") + @"');
              }
            }else
            {
             CoreNET.Methods1.btnEditClick('','EditForm');
            }
          ";
          if (visibleEdit)
          {
            if (((ViewListProperties)dc.GetProperties()).IsModeEdit())
            {
              TBDialog1.Add(btnEdit);
            }
            btnEdit.Disabled = !((ViewListProperties)dc.GetProperties()).IsModeEdit();
          }

          //btnEdit.Disabled = (X.IsAjaxRequest) ? btnEdit.Disabled : !((ViewListProperties)dc.GetProperties()).IsModeEdit();
          //string url = HttpContext.Current.Request.UrlReferrer.OriginalString;

          if (!tbprint)
          {

            Ext.Net.Button btnSave = new Ext.Net.Button() { ID = GlobalAsp.BTN_SAVE1, Icon = Icon.Disk };
            btnSave.Text = ConstantDictExt.Translate(btnSave.ID);

            //update 24/06/2019 dikomen
            //update 08/07/2019 diunkomen, buat aktivkan refreshparent di Transaksi Pesanan, Primos
            //Khusus ENTRY_STYLE_LOOKUP_FORM
            //ENTRY_STYLE_LOOKUP dan ENTRY_STYLE_LOOKUP_ENTRY ngga akan masuk sini
            string scriptRefreshParent = string.Empty;
            string scriptRefreshMasterPage = string.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Current.Request["target"])//Ketika Dialog Lookup Tutup
              || ((ViewListProperties)dc.GetProperties()).RefreshParent)//Ketika RefreshParent true
            {
              scriptRefreshParent = "parent.refreshDataWithSelection();";
            }
            if (((ViewListProperties)dc.GetProperties()).RefreshMaster)//Ketika RefreshMaster true
            {
              scriptRefreshMasterPage = "refreshMasterPage();";
            }
            //" + scriptRefreshParent + @" Ini ketika tutup
            btnSave.Listeners.Click.Handler = @"
              CoreNET.Methods1.btnSaveForm();
              if(!!parent.CoreNET.RefreshDetilPage){
                parent.CoreNET.RefreshDetilPage(#{FPFormEntry1}.getForm().getValues());//record beda dgn gridpanel
              }
              " + scriptRefreshMasterPage + @"
            ";
            TBDialog1.Add(btnSave);
            //btnSave.Disabled = (X.IsAjaxRequest) ? btnSave.Disabled : true;

            string scriptCloseParent = string.Empty;
            if (UtilityUI.GetSessionExtPage().Equals(UtilityUI.PAGE_FORM))
            {
              //kasusnya PageForm sebagai detil
              scriptRefreshParent = @"
              if(!!parent.Methods1_WindowView1){
                " + scriptRefreshParent + @"
              }
            ";

              //*InsertPack dipindahin di btnsave*/
              scriptCloseParent = @"
              if(!!parent)
              {
                if(!!parent.Methods1_WindowView1){
                  parent.Methods1_WindowView1.hide();
                }
              }
            ";
            }
            Ext.Net.Button btnBack = new Ext.Net.Button() { ID = GlobalAsp.BTN_BACK1, Icon = Icon.Reload };
            btnBack.Text = ConstantDictExt.Translate(btnBack.ID);
            btnBack.Listeners.Click.Handler = @"
            if(!!parent.Methods1_WindowDetil1){
              CoreNET.Methods1.btnCancelClick();
              //alert('tes1');
              //parent.CoreNET.Methods1.btnCancelClick();//ini masuk
              //parent.Methods1_WindowDetil1.CoreNET.Methods1.btnCancelClick();
            }else{
              //alert('tes2');
              CoreNET.Methods1.btnCancelClick();
            }
            //scriptCloseParent hanya ada kalau PAGE_FORM
            " + scriptCloseParent + @"
          ";
            //if(!!parent){
            //  //Ini kasusnya dimana?dikomen dulu ya
            //  //if(!!parent.Methods1_WindowDetil1){
            //  //  parent.Methods1_WindowDetil1.hide();
            //  //}
            //" + scriptCloseParent + @"
            //}else{
            //  if(!!Methods1_WindowDetil1){
            //    Methods1_WindowDetil1.hide();
            //  }
            //  //Ini kasusnya dimana?dikomen dulu ya
            //  //if(!!Methods1_WindowView1){
            //  //  Methods1_WindowView1.hide();
            //  //}
            //}

            TBDialog1.Add(btnBack);
            //Untuk PageForm yang langsung diakses dari MainMenu: UtilityUI.GetSessionMenuPage().Equals(UtilityUI.PAGE_FORM)
            //No BtnBack
            //if (!UtilityUI.GetSessionMenuPage().Equals(UtilityUI.PAGE_FORM))
            //{
            //  TBDialog1.Add(btnBack);
            //}
            //btnBack.Disabled = (X.IsAjaxRequest) ? btnBack.Disabled : false;
          }
          //@todo Apakah bikin error viewstate?
          int NFormGroup = ((ViewListProperties)dc.GetProperties()).FormGroupLabels.Length;
          if (NFormGroup > 0)// (((BaseBO)dc).Ngroup > 0)
          {
            Ext.Net.Button btnExpand = new Ext.Net.Button
            {
              ID = "BTN_EXPAND_FORM1"
            };
            btnExpand.Text = ConstantDictExt.Translate("BTN_EXPAND1");
            btnExpand.ToolTip = ConstantDictExt.TranslateTabTip(btnExpand);
            btnExpand.Icon = Icon.ApplicationSplit;
            //btnExpand.IconCls = "icon-expand-all2";
            //btnExpand.Style.Add("background-image", "url(~/Res/Img/expand-all.gif) !important;");
            btnExpand.Listeners.Click.Handler = @"
              CoreNET.Methods1.btnExpandClick();
            ";
            TBDialog1.Add(btnExpand);

            Ext.Net.Button btnCollapse = new Ext.Net.Button
            {
              ID = "BTN_COLLAPSE_FORM1"
            };
            btnCollapse.Text = ConstantDictExt.Translate("BTN_COLLAPSE1");
            btnCollapse.ToolTip = ConstantDictExt.TranslateTabTip(btnCollapse);
            btnCollapse.Icon = Icon.ApplicationViewList;
            //btnCollapse.IconCls = "icon-collapse-all2";
            //btnCollapse.Style.Add("background-image", "url(~/Res/Img/collapse-all.gif) !important;");
            btnCollapse.Listeners.Click.Handler = @"
              CoreNET.Methods1.btnCollapseClick();
            ";
            TBDialog1.Add(btnCollapse);
          }
          #region Debugging
          if (MasterAppConstants.Instance.StatusTesting)
          {
            Ext.Net.Button btnMenu = new Ext.Net.Button() { ID = GlobalAsp.BTN_MENU2, Icon = Icon.Wrench };
            btnMenu.Text = ConstantDictExt.Translate(btnMenu.ID);
            btnMenu.ToolTip = ConstantDictExt.TranslateTabTip(btnMenu);

            Ext.Net.Menu menuDebug = new Ext.Net.Menu() { };
            btnMenu.Menu.Add(menuDebug);

            Ext.Net.MenuItem mnInfo = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_INFO2, Icon = Icon.PageLightning };
            mnInfo.Text = ConstantDictExt.Translate(mnInfo.ID);
            mnInfo.Listeners.Click.Handler = "CoreNET.Methods1.btnDebugClick()";
            menuDebug.Items.Add(mnInfo);
            menuDebug.Items.Add(new Ext.Net.MenuSeparator());

            Ext.Net.MenuItem mnentry = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_ENTRY2, Icon = Icon.ApplicationForm };
            mnentry.Text = ConstantDictExt.Translate(mnentry.ID);
            mnentry.Listeners.Click.Handler = "CoreNET.Methods1.btnAdminClick(0)";
            menuDebug.Items.Add(mnentry);

            Ext.Net.MenuItem mnconfig = new Ext.Net.MenuItem() { ID = GlobalAsp.MENU_CONFIG2, Icon = Icon.ApplicationForm };
            mnconfig.Text = ConstantDictExt.Translate(mnconfig.ID);
            mnconfig.Listeners.Click.Handler = "CoreNET.Methods1.btnAdminClick(5)";
            menuDebug.Items.Add(mnconfig);

            TBDialog1.Add(btnMenu);
          }
          #endregion

        }
      }
      #endregion
      #region HiddenToolbar
      ////kan ada tombol expand collapse. lebih estetis klo ngga dihide
      //if ((UtilityUI.GetSessionMenuPage() == UtilityUI.PAGE_FORM)
      //  && (((ViewListProperties)dc.GetProperties()).ModeEditable == ViewListProperties.MODE_EDITABLE_READONLY))
      //{
      //  TBDialog1.Hidden = true;
      //}
      #endregion
    }
    public static PanelBase GetPanelDetil()
    {
      #region Inisialisasi
      int id = GlobalAsp.GetRequestI();
      IDataControlUIEntry dc = (IDataControlUIEntry)UtilityUI.GetDataControl(id);
      #endregion
      #region Add Form Entry
      FormPanel FPFormEntry1 = new FormPanel
      {
        ID = "FPFormEntry1",
        Region = Region.Center,
        Padding = 5,
        Margins = "5 5 5 5",
        AutoScroll = false,
        Split = true,
        Height = 600,
        ForceLayout = true,
        MonitorResize = true,
        DefaultAnchor = "100%"
      };
      //AnchorVertical = "100%",
      //AutoDoLayout = false,
      //AutoHeight = true,
      //DefaultAnchor = "100%",
      //ForceLayout = true
      //AutoDoLayout = true,
      int ncf = 0;
      SortedDictionary<string, FormPanel> Groups = new SortedDictionary<string, FormPanel>();
      HashTableofParameterRow hps = dc.GetEntries();
      string[] keys = hps.GetOrderKeys();
      ArrayList arkeys = new ArrayList();
      foreach (string key in keys)
      {
        string[] fields = key.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        arkeys.AddRange(fields);
        ParameterRow pr = (ParameterRow)hps[key];
        pr.Editable = false;
        if ((pr.ModeEdit != ParameterRow.MODE_HTML) && (pr.ModeEdit != ParameterRow.MODE_FORUM)
          && (pr.ModeEdit != ParameterRow.MODE_FILEUPLOAD))
        {
          CoreNETCompositeField cf = new CoreNETCompositeField(pr, ExtGridPanelFilter.GRID);
          ncf++;
          if (pr.Group == null)
          {
            FPFormEntry1.Items.Add(cf);
          }
          else
          {
            FormPanel pnlGroup = null;
            if (!Groups.ContainsKey(pr.Group))
            {
              pnlGroup = new FormPanel()
              {
                Collapsible = true,
                AnchorHorizontal = "100%"
                ,
                Padding = 5,
                Margins = "5 5 5 5"
              };
              pnlGroup.Title = pr.Group;
              Groups[pr.Group] = pnlGroup;
            }
            else
            {
              pnlGroup = (FormPanel)Groups[pr.Group];
            }
            pnlGroup.Items.Add(cf);
          }
        }
      }
      int i = 0;
      foreach (string key in Groups.Keys)
      {
        FormPanel pnlGroup = (FormPanel)Groups[key];
        if (pnlGroup.Items.Count > 0)
        {
          FPFormEntry1.Items.Add(pnlGroup);
          pnlGroup.Collapsed = (i++ != 0);
        }
      }
      ((BaseBO)dc).Ngroup = Groups.Count;
      string[] pks = ((ViewListProperties)dc.GetProperties()).PrimaryKeys;
      foreach (string key in pks)
      {
        if (!arkeys.Contains(key))
        {
          TextField textField = new TextField
          {
            ID = "par" + key,
            Hidden = true,
            DataIndex = key,
            AnchorHorizontal = "100%"
          };
          FPFormEntry1.Items.Add(textField);
        }
      }
      TabPanel tabPanel = new TabPanel()
      {
        ID = "TabPanel1",
        Region = Region.Center,
        AutoHeight = false,
        Split = true,
        //Layout = "Fit",
        DefaultAnchor = "100%"
      };
      tabPanel.AnchorVertical = "100%";
      tabPanel.EnableTabScroll = true;
      UtilityExt.SetNonIFrameAutoLoad(tabPanel.AutoLoad);
      int count = AddTab(dc, tabPanel, FPFormEntry1, ncf);
      if (FPFormEntry1.Items.Count == count)//Auto Edit
      {
        dc.SetEditable(true);
      }
      #endregion
      #region Construct FormEntry Final
      if (count == 0)
      {
        return FPFormEntry1;
      }
      else
      {
        if (tabPanel.Items.Count > 1)
        {
          return tabPanel;
        }
        else
        {
          ((Ext.Net.Panel)tabPanel.Items[0]).Title = string.Empty;
          return (PanelBase)tabPanel.Items[0];
        }
      }
      #endregion
    }
    private static int AddTab(IDataControlUIEntry dc, TabPanel tabPanel, FormPanel FPFormEntry1, int ncf)
    {
      return AddTab(dc, tabPanel, FPFormEntry1, ncf, false);//Request["enable"] == "1"
    }
    private static int AddTab(IDataControlUIEntry dc, TabPanel tabPanel, FormPanel FPFormEntry1, int ncf, bool enable)
    {
      HashTableofParameterRow hps = dc.GetEntries();
      string[] keys = hps.GetOrderKeys();

      int count = 0;
      foreach (string key in keys)
      {
        ParameterRow pr = (ParameterRow)hps[key];
        if ((pr.ModeEdit == ParameterRow.MODE_HTML) || (pr.ModeEdit == ParameterRow.MODE_FILEUPLOAD) || (pr.ModeEdit == ParameterRow.MODE_FORUM))
        {
          ++count;
          if (count == 1)
          {
            if (ncf > 0)
            {
              tabPanel.Items.Add(FPFormEntry1);
              FPFormEntry1.Title = "Form";
              //FPFormEntry1.AutoLoad.TriggerEvent = "show";
            }
          }
          string isTextLookup = "&lookup=0";
          if (pr.DCLookup != null)
          {
            isTextLookup = "&lookup=1";
          }

          Ext.Net.Panel pnl = new Ext.Net.Panel() { Region = Region.Center };
          pnl.ID = "Panel" + pr.Name;
          pnl.ToolTip = pr.Hint;
          pnl.Title = pr.Label;//
          string strEnable = pr.Enable ? "1" : "0";
          string url = string.Empty;
          string parentsave = ((ViewListProperties)(dc.GetProperties())).RefreshParentForEditor ? "1" : "0";
          if (pr.ModeEdit == ParameterRow.MODE_HTML)
          {
            #region MODE_HTML
            url = UtilityExt.ValidateURL(null, string.Format("~/Page/TextEditor.aspx?local=" + HttpContext.Current.Request["local"]
              + "&app=" + GlobalAsp.GetRequestApp()
              + "&id=" + GlobalAsp.GetRequestId()
              + "&i=" + GlobalAsp.GetRequestI()
              + "&val=" + GlobalAsp.GetRequestVal()
              + "&tb=1" //+ strEnableNgga bisa on/off
              + "&type=" + NotesControl.DESCRIPTION
              + "&parentsave=" + parentsave
              + isTextLookup
              ));
            #endregion
          }
          else if (pr.ModeEdit == ParameterRow.MODE_FILEUPLOAD)
          {
            #region MODE_FILEUPLOAD
            url = "~/Page/FileBrowser.aspx?local=" + HttpContext.Current.Request["local"]
            + "&app=" + GlobalAsp.GetRequestApp()
            + "&roleid=" + HttpContext.Current.Request["roleid"]
            + "&procid=" + HttpContext.Current.Request["procid"]
            + "&id=" + GlobalAsp.GetRequestId()
            + "&i=" + GlobalAsp.GetRequestI()
              + "&val=" + GlobalAsp.GetRequestVal()
            + "&title=Template&current=1&pname=Path"
            + "&kkp=" + HttpContext.Current.Request["kkp"]
            + "&tbfolder=0&lcfolder=1";// &uploadEnabled=" + strEnable;
            #endregion
          }
          else if (pr.ModeEdit == ParameterRow.MODE_FORUM)
          {
            #region MODE_FORUM
            url = UtilityExt.ValidateURL(null, string.Format("~/Page/Comment.aspx?local=" + HttpContext.Current.Request["local"]
              + "&app=" + GlobalAsp.GetRequestApp()
              + "&id=" + GlobalAsp.GetRequestId()
              + "&i=" + GlobalAsp.GetRequestI()
              + "&val=" + GlobalAsp.GetRequestVal()
              //+ "&tb=" + strEnable//Ngga bisa on/off
              + "&type=" + NotesControl.FORUM
              + "&parentsave=" + parentsave
              ));
            #endregion
          }
          pnl.ItemID = url;
          pnl.AutoLoad.Url = url;
          UtilityExt.SetIFrameAutoLoad(pnl.AutoLoad);
          pnl.Listeners.Activate.Handler = "this.loadContent();";
          pnl.AutoLoad.TriggerEvent = "show";
          pnl.Layout = "Fit";
          pnl.Height = new System.Web.UI.WebControls.Unit(400);
          pnl.Plugins.Add(new PanelResizer() { MinHeight = 400 });
          tabPanel.Add(pnl);
        }
      }
      return count;
    }
  }
}
