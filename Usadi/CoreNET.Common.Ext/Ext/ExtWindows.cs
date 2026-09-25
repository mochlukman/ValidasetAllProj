using CoreNET.Common.BO;
using Ext.Net;
using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
  public class ExtWindows
  {

    #region Property RES
    public static int DEFAULT_WIDTH_RES
    {
      get
      {
        string res = (string)HttpContext.Current.Session[GlobalAsp.SESSION_SCREEN_RES];
        string[] strs = res.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
        return int.Parse(strs[0]);
      }
    }
    public static int DEFAULT_HEIGHT_RES
    {
      get
      {
        string res = (string)HttpContext.Current.Session[GlobalAsp.SESSION_SCREEN_RES];
        string[] strs = res.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
        return int.Parse(strs[1]);
      }
    }
    #endregion

    public const int MAX_WIDTH = 900;
    public const int DEFAULT_WIDTH = 700;
    public const int DEFAULT_HEIGHT = 450;
    public const int DEFAULT_LABEL_WIDTH = 120;
    public const int MODE_FILTER = 1;
    public const int MODE_ENTRY = 2;
    public static void CreateWindowFormEntry(Window wWindow, IExtFormEntry FormEntry)
    {
      wWindow.AutoDoLayout = true;
      wWindow.Maximized = true;
      Ext.Net.Component c = FormEntry.GetCenter();
      if (c != null)
      {
        wWindow.Add(c);
      }

      c = FormEntry.GetNorth();
      if (c != null)
      {
        wWindow.Add(c);
      }

      c = FormEntry.GetSouth();
      if (c != null)
      {
        wWindow.Add(c);
      }

      c = FormEntry.GetWest();
      if (c != null)
      {
        wWindow.Add(c);
      }

      c = FormEntry.GetEast();
      if (c != null)
      {
        wWindow.Add(c);
      }
    }
    public static Window CreateWindow(string id, string title)
    {
      Window winLookup = new Window
      {
        ID = "Window" + id,
        Height = ExtWindows.DEFAULT_HEIGHT,
        Width = ExtWindows.DEFAULT_WIDTH,
        Title = title,
        Layout = "Fit"
      };
      winLookup.Height = DEFAULT_HEIGHT;
      winLookup.Width = DEFAULT_WIDTH;
      winLookup.Resizable = true;
      winLookup.Maximizable = true;
      winLookup.Modal = true;
      winLookup.Padding = 5;
      winLookup.Hidden = true;
      winLookup.Listeners.Close.Handler = @"if (!!parent)
            {
              if (!!parent.NormalizeFrame)
              {
                parent.NormalizeFrame();
              }
            }";


      return winLookup;
    }
    public static void SetWindow(Window winLookup, string id, string title)
    {
      winLookup.ID = "Window" + id;
      winLookup.Height = ExtWindows.DEFAULT_HEIGHT;
      winLookup.Width = ExtWindows.DEFAULT_WIDTH;
      winLookup.Title = title;
      winLookup.Layout = "Fit";
      winLookup.Height = DEFAULT_HEIGHT;
      winLookup.Width = DEFAULT_WIDTH;
      winLookup.Resizable = true;
      winLookup.Maximizable = true;
      winLookup.Modal = true;
      winLookup.Padding = 5;
      winLookup.Hidden = true;
      //      winLookup.Listeners.BeforeShow.Handler = @"if (!!parent) 
      //      {
      //        if(!!parent.MyMethods){
      //          if (location.search.indexOf('child') !== -1) {
      //            parent.MyMethods.ExpandTopFrame();
      //          } else {
      //            parent.MyMethods.ExpandBotFrame();
      //          }
      //        }
      //      }";
      winLookup.Listeners.Close.Handler = @"if (!!parent)
      {
        if (!!parent.MyMethods)
        {
          parent.MyMethods.NormalizeFrame();
        }
      }";

    }
    public static Hashtable CreateLookupFilterWindow(Page page, int id, int target)
    {
      IDataControlUI cCtrl = UtilityUI.GetDataControl(id);
      return CreateLookupFilterWindow(page, id, cCtrl, target, false);
    }
    public static Hashtable CreateLookupFilterWindow(Page page, int id, IDataControlUI cCtrl, int target, bool lookup)
    {
      HashTableofParameterRow hps = cCtrl.GetFilters();
      string[] keys = hps.GetOrderKeys();
      Hashtable ws = new Hashtable();
      foreach (string key in keys)
      {
        ParameterRow pr = (ParameterRow)hps[key];
        if (pr.ModeEdit == ParameterRow.MODE_LOOKUP2)
        {
          ws.Add(pr.Name, CreateLookupWindow(page, MODE_FILTER, pr, target, lookup));//ExtGridPanelFilter.GRID/ExtGridPanelFilter.TREE
        }
      }
      return ws;
    }
    public static Hashtable CreateLookupEntryWindow(Page page, int id)
    {
      IDataControlUIEntry cCtrl = (IDataControlUIEntry)UtilityUI.GetDataControl(id);
      HashTableofParameterRow hps = cCtrl.GetEntries();
      string[] keys = hps.GetOrderKeys();
      Hashtable ws = new Hashtable();
      foreach (string key in keys)
      {
        ParameterRow pr = (ParameterRow)hps[key];
        if (pr.ModeEdit == ParameterRow.MODE_LOOKUP2)
        {
          ws.Add(pr.Name, CreateLookupWindow(page, MODE_ENTRY, pr, ExtGridPanelFilter.ENTRY));
        }
      }
      return ws;
    }
    public static Window CreateLookupWindow(Page page, int mode, ParameterRow pr, int target)
    {
      return CreateLookupWindow(page, mode, pr, target, false);
    }
    public static Window CreateLookupWindow(Page page, int mode, ParameterRow pr, int target, bool lookup)
    {
      string key = pr.Param3.Name;
      string title = pr.Label;
      Window winLookup = CreateWindow((mode == MODE_FILTER) ? "Filter" : "Entry" + key,
        ConstantDict.Translate("BTN_SELECT1") + " " + title);

      string strlookup = lookup ? "&lookup=1" : "&lookup=0";
      //form=FPFormEntry1
      //digunakan di FilterTabular.aspx dan FilterTreePanel.aspx untuk mendapatkan komponen form entry
      //yg dipassing untuk reset form
      if (pr.IsTree)
      {
        winLookup.AutoLoad.Url = UtilityExt.ValidateURL(winLookup, string.Format("~/Lookup/FilterTreePanel.aspx?mode=" + mode + strlookup + "&app=" + GlobalAsp.GetRequestApp() + "&roleid=" + HttpContext.Current.Request["roleid"] + "&id=" + GlobalAsp.GetRequestId() + "&kkp=" + HttpContext.Current.Request["kkp"] + "&winid=" + winLookup.ID + "&key=" + pr.Name + "&target=" + target + "&form=FPFormEntry1&kode=" + HttpContext.Current.Request["kode"] + "&i=" + GlobalAsp.GetRequestI() + "&label=" + HttpContext.Current.Request["label"] + "&level=" + (HttpContext.Current.Request["level"] == null ? "" : HttpContext.Current.Request["level"])));
      }
      else
      {
        winLookup.AutoLoad.Url = UtilityExt.ValidateURL(winLookup, string.Format("~/Lookup/FilterTabular.aspx?mode=" + mode + strlookup + "&app=" + GlobalAsp.GetRequestApp() + "&roleid=" + HttpContext.Current.Request["roleid"] + "&id=" + GlobalAsp.GetRequestId() + "&kkp=" + HttpContext.Current.Request["kkp"] + "&winid=" + winLookup.ID + "&key=" + pr.Name + "&target=" + target + "&form=FPFormEntry1&kode=" + HttpContext.Current.Request["kode"] + "&i=" + GlobalAsp.GetRequestI() + "&label=" + HttpContext.Current.Request["label"] + "&level=" + (HttpContext.Current.Request["level"] == null ? "" : HttpContext.Current.Request["level"])));
      }
      winLookup.AutoLoad.Mode = LoadMode.IFrame;
      //      winLookup.Listeners.BeforeShow.Handler = @"if (!!parent) 
      //      {
      //        if(!!parent.MyMethods){
      //          if (location.search.indexOf('child') !== -1) {
      //            parent.MyMethods.ExpandTopFrame();
      //          } else {
      //            parent.MyMethods.ExpandBotFrame();
      //          }
      //        }
      //      }";
      winLookup.Listeners.Close.Handler = @"if (!!parent)
      {
        if (!!parent.MyMethods)
        {
          parent.MyMethods.NormalizeFrame();
        }
      }";
      //winLookup.Render(page.Form);//bikin error
      return winLookup;
    }
    //public static Window CreateLookupWindow(Page page, int mode, string key, string title, int target, string dclookup, string labelquery)
    //{
    //  Window winLookup = CreateWindow((mode == MODE_FILTER) ? "Filter" : "Entry" + key, ((ConstantDictExt.IDLOCALE == ConstantDictExt.EN) ? "Select " : "Pilih ") + title);
    //  winLookup.AutoLoad.Url = UtilityExt.ValidateURL(winLookup, string.Format("~/Lookup/FilterTreePanel.aspx?mode=" + mode + "&app=" + GlobalAsp.GetRequestApp() + "&roleid=" + HttpContext.Current.Request["roleid"] + "&id=" + GlobalAsp.GetRequestId() + "&kkp=" + HttpContext.Current.Request["kkp"] + "&winid=" + winLookup.ID + "&key=" + key + "&target=" + target + "&form=" + string.Empty + "&kode=" + HttpContext.Current.Request["kode"] + "&i=" + GlobalAsp.GetRequestI() + "&label=" + HttpContext.Current.Request["label"] + "&level=" + (HttpContext.Current.Request["level"] == null ? "" : HttpContext.Current.Request["level"]) + "&DCLookup=" + dclookup + "&LabelQuery=" + labelquery));
    //  winLookup.AutoLoad.Mode = LoadMode.IFrame;
    //  //      winLookup.Listeners.BeforeShow.Handler = @"if (!!parent) 
    //  //      {
    //  //        if(!!parent.MyMethods){
    //  //          if (location.search.indexOf('child') !== -1) {
    //  //            parent.MyMethods.ExpandTopFrame();
    //  //          } else {
    //  //            parent.MyMethods.ExpandBotFrame();
    //  //          }
    //  //        }
    //  //      }";
    //  winLookup.Listeners.Close.Handler = @"if (!!parent)
    //  {
    //    if (!!parent.MyMethods)
    //    {
    //      parent.MyMethods.NormalizeFrame();
    //    }
    //  }";
    //  //winLookup.Render(page.Form);//bikin error
    //  return winLookup;
    //}
  }
  public class WindowLookup1 : Window
  {
    public WindowLookup1()
    {
      string strid = "WindowLookup";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = false;
      Maximizable = false;
      Hidden = true;
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
      //      Listeners.BeforeShow.Handler = @"if (!!parent) 
      //      {
      //        if(!!parent.MyMethods){
      //          if (location.search.indexOf('child') !== -1) {
      //            parent.MyMethods.ExpandTopFrame();
      //          } else {
      //            parent.MyMethods.ExpandBotFrame();
      //          }
      //        }
      //      }";
      Listeners.Close.Handler = @"if (!!parent)
      {
        if (!!parent.MyMethods)
        {
          parent.MyMethods.NormalizeFrame();
        }
      }";
    }
  }
  public class WindowSearch : Window
  {

    #region Property EnableEdit
    private bool _EnableEdit;
    public bool EnableEdit
    {
      get => _EnableEdit;
      set => _EnableEdit = value;
    }
    #endregion

    public WindowSearch()
    {
      string strid = "WindowSearch";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = true;
      Hidden = true;
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
      Listeners.Close.Handler = "parent.NormalizeFrame();";
    }

    public new void Render(Control ctrl)
    {
      string strenable = (EnableEdit) ? "1" : "0";
      AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/Page/PageTabular.aspx?app=" +
        GlobalAsp.GetRequestApp() +
        "&roleid=" + HttpContext.Current.Request["roleid"] +
        "&id=" + GlobalAsp.GetRequestId() + "&i=" + GlobalAsp.GetRequestI()
        + "&label=" + HttpContext.Current.Request["label"]
        + "&enable=" + strenable
        + "&passlist=1"));
      base.Render(ctrl);
    }
  }
  public class WindowHelp : Window
  {
    public WindowHelp()
    {
      string strid = "WindowHelp";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = true;
      Hidden = true;
      int id = GlobalAsp.GetRequestI();
      string url = string.Empty;
      if (MasterAppConstants.Instance.StatusTesting)
      {
        url = UtilityExt.ValidateURL(null, string.Format("~/Page/TextEditor.aspx?local=" + HttpContext.Current.Request["local"]
          + "&app=" + GlobalAsp.GetRequestApp()
          + "&id=" + GlobalAsp.GetRequestId()
          + "&i=" + GlobalAsp.GetRequestI()
          + "&tb=1"
          + "&mode=help"
          + "&type=" + NotesControl.RELEASE_NOTES
          + "&parentsave=0"
          ));
      }
      else
      {
        url = UtilityExt.ValidateURL(null, string.Format("~/Page/TextEditor.aspx?local=" + HttpContext.Current.Request["local"]
          + "&app=" + GlobalAsp.GetRequestApp()
          + "&id=" + GlobalAsp.GetRequestId()
          + "&i=" + GlobalAsp.GetRequestI()
          + "&tb=0"
          + "&type=" + NotesControl.RELEASE_NOTES
          + "&parentsave=0"
          ));
      }
      AutoLoad.Url = UtilityExt.ValidateURL(this, url);
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }
  }
  public class WindowPrint : Window
  {
    public WindowPrint(int n)
    {
      string strid = "WindowPrint";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = true;
      Hidden = true;

      string url = string.Empty;
      int id = GlobalAsp.GetRequestI();
      IDataControlUI dc = UtilityUI.GetDataControl(id);
      if (typeof(IPrintable).IsInstanceOfType(dc))
      {
        IPrintable dcprint = (IPrintable)dc;
        url = dcprint.GetURLReport(n);
      }
      if (string.IsNullOrEmpty(url))
      {
        url = string.Format("~/Page/TextEditor.aspx?app=" + GlobalAsp.GetRequestApp() + "&id=" + GlobalAsp.GetRequestId() + "&i=" + "&type=" + NotesControl.HTMLREPORT
        + GlobalAsp.GetRequestI() + "&mode=print&tb=1&title=Print&pname=Html");
        AutoLoad.Url = UtilityExt.ValidateURL(this, url);
        UtilityExt.SetIFrameAutoLoad(AutoLoad);
        Listeners.Activate.Handler = "this.loadContent();";
      }
      else
      {
        LoadContent(url);
        //Listeners.Activate.Handler = "open('" + url + "');";
      }
    }
  }
  public class WindowLoadCSV : Window
  {
    public WindowLoadCSV(string mode, string title)
    {
      string strid = "WindowLoadCSV";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Layout = "Layout";
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = false;
      Hidden = true;
      Title = title;
      int id = GlobalAsp.GetRequestI();
      IDataControl dc = UtilityUI.GetDataControl(id);
      string url = string.Format(@"~/Page/CSVUpload.aspx?app={0}&i={1}&mode={2}&title={3}"
        , GlobalAsp.GetSessionApp(), id, mode, title);
      if (MasterAppConstants.Instance.StatusTesting || (dc != null))
      {
        AutoLoad.Url = UtilityExt.ValidateURL(this, url + "&tb=1");
      }
      else
      {
        AutoLoad.Url = UtilityExt.ValidateURL(this, url + "&tb=0");
      }
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }
    public WindowLoadCSV(string mode, string title, string url, int w, int h)
    {
      string strid = "WindowLoadCSV";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel((w > 0) ? w : ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel((h > 0) ? h : ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = false;
      Hidden = true;
      Title = title;
      int i = GlobalAsp.GetRequestI();
      string id = GlobalAsp.GetRequestId();

      string qs = "?";
      if (url.Contains("?"))
      {
        qs = "&";
      }

      if (MasterAppConstants.Instance.StatusTesting)
      {
        AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format(url + qs + "i=" + i + "&id=" + id + "&mode=" + mode + "&tb=1&title=" + title));
      }
      else
      {
        AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format(url + qs + "i=" + i + "&id=" + id + "&mode=" + mode + "&tb=0&title=" + title));
      }
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }
  }
  public class WindowDebug : Window
  {
    public WindowDebug(int i, string key)
    {
      string strid = "WindowDebug";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = true;
      Hidden = true;
      int id = GlobalAsp.GetRequestI();
      AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/Page/TextEditor.aspx?app=" + GlobalAsp.GetRequestApp() + "&debug=1" + "&key=" + key + "&id=" + GlobalAsp.GetRequestId() + "&i=" +
      GlobalAsp.GetRequestI() + "&mode=0&tb=0&title=Debug&pname=Debug"));
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }
    public WindowDebug(string msg)
    {
      string strid = "WindowDebug";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = true;
      Hidden = true;
      int id = GlobalAsp.GetRequestI();
      //string sesname = Guid.NewGuid().ToString();
      string sesname = GlobalAsp.SESSION_APP_PARAMS;
      HttpContext.Current.Session[sesname] = msg;
      AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/Page/TextEditor.aspx?app=" + GlobalAsp.GetRequestApp() + "&msg=" + sesname));
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }

    public static void ShowMessage(Page page, string msg)
    {
      WindowDebug WindowDebug1 = new WindowDebug(msg)
      {
        ID = "Error" + Guid.NewGuid().ToString().Replace("-", "")
      };
      WindowDebug1.Render(page.Form);
      WindowDebug1.LoadContent();
      WindowDebug1.Show();

    }
  }
  public class WindowForum : Window
  {
    public WindowForum()
    {
      Configure(false);
    }
    public WindowForum(bool error)
    {
      Configure(error);
    }
    public void Configure(bool error)
    {
      string strid = "WindowForum";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = true;
      Hidden = true;
      int id = GlobalAsp.GetRequestI();
      string strErr = (error) ? "&error=1" : string.Empty;
      AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/Page/Comment.aspx?app=" + GlobalAsp.GetRequestApp()
        + "&id=" + GlobalAsp.GetRequestId() + "&i=" +
        GlobalAsp.GetRequestI() + "&type=1&tb=1" + strErr));
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }
  }
  public class WindowAdmin : Window
  {
    public WindowAdmin(int idx, string id)
    {
      string strid = "WindowAdmin" + idx;
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = true;
      Hidden = true;
      AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/Page/PageTabular.aspx?debug=" + GlobalAsp.GetRequestDebug() + "&app=" + GlobalAsp.GetRequestApp() + "&id=" + id + "&idprev=" + GlobalAsp.GetRequestId() + "&i=" + GlobalAsp.GetRequestI()));
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }
    public WindowAdmin(int idx, string id, bool isform)
    {
      string strid = "WindowAdmin" + idx;
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = true;
      Hidden = true;
      AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/Page/PageForm.aspx?debug=" + GlobalAsp.GetRequestDebug() + "&app=" + GlobalAsp.GetRequestApp() + "&id=" + id + "&idprev=" + GlobalAsp.GetRequestId() + "&i=1"));
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }
  }
  public class WindowUrl : Window
  {
    public WindowUrl()
    {
      string strid = "WindowUrl";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = true;
      Maximizable = true;
      Hidden = true;
    }
    public void SetURL(string url)
    {
      AutoLoad.Url = url;
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }
  }
  public class WindowLookupText : Window
  {
    public WindowLookupText()
    {
      string strid = "WindowLookup1Text";
      ID = strid;
      Title = GlobalAsp.GetConfigLabelInfo();
      Width = Unit.Pixel(ExtWindows.DEFAULT_WIDTH);
      Height = Unit.Pixel(ExtWindows.DEFAULT_HEIGHT);
      Modal = true;
      Resizable = false;
      Maximizable = false;
      Hidden = true;
      int id = GlobalAsp.GetRequestI();
      AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/Lookup/FilterTextEditor.aspx?"
        + "app=" + GlobalAsp.GetRequestApp()
        + "&roleid=" + HttpContext.Current.Request["roleid"]
        + "&id=" + GlobalAsp.GetRequestId()
        + "&winid=WindowLookup1Text"
        + "&i=" + GlobalAsp.GetRequestI()
        + "&tb=0"
        + "&title=Pilih Uraian&pname=" + HttpContext.Current.Request["pname"]
      ));
      //AutoLoad.Url = UtilityExt.ValidateURL(this, string.Format("~/Lookup/FilterTreePanel.aspx?mode=" + mode
      //  + "&winid=" + winLookup.ID 
      //  + "&target=" + target 
      //  + "&form=" + formPanel.ID 
      //  + "&kode=" + HttpContext.Current.Request["kode"] 
      //  + "&i=" + ConstantApp.REQUEST_I 
      //  + "&label=" + HttpContext.Current.Request["label"] 
      //  + "&level=" + (HttpContext.Current.Request["level"] == null ? "" : HttpContext.Current.Request["level"])
      //  ));
      UtilityExt.SetIFrameAutoLoad(AutoLoad);
      Listeners.Activate.Handler = "this.loadContent();";
    }
  }
}
