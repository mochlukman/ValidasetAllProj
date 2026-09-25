using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Ext.Net;
using CoreNET.Common.BO;

namespace CoreNET.Common.Base
{
  [Serializable]
  public class BaseDataControlMenu : BaseDataControlSys, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    public const string OLIST1LISTMENU = "1";
    public const string OLIST1MENUKKP = "2";
    public const string OLIST1SYSGETKEYS = "3";//"CoreNET.Common.BO.SysgetkeysUIControl, CoreNET.Common.Sys";
    public const string OLIST1SYSGETROWS = "4";//"CoreNET.Common.BO.SysgetrowsUIControl, CoreNET.Common.Sys";
    public const string OLIST1SYSGETCOLS = "5";//"CoreNET.Common.BO.SysgetcolsUIControl, CoreNET.Common.Sys";
    public const string OLIST1SYSGETTREECOLS = "6";//"CoreNET.Common.BO.SysgettreecolsUIControl, CoreNET.Common.Sys";
    public const string OLIST1SYSGETFILTERS = "7";//"CoreNET.Common.BO.SysgetfiltersUIControl, CoreNET.Common.Sys";
    public const string OLIST1STATUSFORM = "8";//"CoreNET.Common.BO.Ss01appmenuStatusFormControl, CoreNET.Common.Sys";

    #region Properties 

    public string Grpack { get; set; }
    public string Kdpack { get; set; }
    public string Nmpack { get; set; }
    public int Kdtahun { get; set; }
    public string Nmtahun { get; set; }
    public string Nodok { get; set; }
    public string Idmenu { get; set; }
    public string Kdmenu { get; set; }
    public string Kddok { get; set; }
    public string Idxdok { get; set; }
    public string Nmmenu { get; set; }
    public string Olist1 { get; set; }
    public string Olistdetil1 { get; set; }
    public string Olistdetil2 { get; set; }
    public string Olistdetil3 { get; set; }
    public string Olistdetil4 { get; set; }
    public string Olookuplist1 { get; set; }
    public string Olookuplistdetil1 { get; set; }
    public string Olookuplistdetil2 { get; set; }
    public string Olookuplistdetil3 { get; set; }
    public string Olookuplistdetil4 { get; set; }
    public new string GetDefaultDebug()
    {
      return Debug;
    }
    public static string GetStaticDefaultDebug(BaseDataControlMenu dc)
    {
      string msg = string.Empty;
      try
      {
        msg += @"
          [BaseDataControlMenu]
          <ul>
            <li>Server = " + SQLDataSource.GetOpDB(dc.ConnectionString) + @"</li>
            <li>Idapp = " + GlobalAsp.GetSessionApp() + @"</li>
            <li>Kdapp = " + dc.Kdapp + @"</li>
            <li>Nmapp = " + dc.Nmapp + @"</li>
            <li>AppID = " + GlobalAsp.GetSessionAppValue(MasterAppConstants.APPID) + @"</li>
            <li>URLBase = " + GlobalAsp.GetSessionAppValue(MasterAppConstants.URLBASE) + @"</li>
            <li>MenuDC = " + GlobalAsp.GetSessionAppValue(MasterAppConstants.MENUDC) + @"</li>
            <li>UserDC = " + GlobalAsp.GetSessionAppValue(MasterAppConstants.USERDC) + @"</li>
            <li>IdapPproperty = " + GlobalAsp.GetSessionAppValue(MasterAppConstants.IDAPP_PROPERTY) + @"</li>
            <li>NmapPproperty = " + GlobalAsp.GetSessionAppValue(MasterAppConstants.NMAPP_PROPERTY) + @"</li>
            <li>url = " + HttpContext.Current.Request.UrlReferrer.OriginalString + @"</li>
            <li>i = " + GlobalAsp.GetRequestI() + @"</li>
            <li>id = " + GlobalAsp.GetRequestId() + @"</li>
            <li>kdgroup = " + GlobalAsp.GetSessionGroup().GetGroupID() + @"</li>
            <li>kdmenu = " + GlobalAsp.GetRequestId() + @"</li>
            <li>" + GlobalAsp.GetSessionUser().GetUserDebugInfo() + @"</li>
            <li>type=" + dc.GetType().AssemblyQualifiedName + @"</li>
            <li>IDataControl dc = new " + dc.GetType().FullName + @"();</li>    
            <li>user = " + GlobalAsp.GetSessionUser().GetUserID() + @"</li>
            <li>UserType = " + GlobalAsp.GetSessionUser().GetUserType() + @"</li>
          </ul>
          [/BaseDataControlMenu]<br/>
        ";

      }
      catch (Exception ex)
      {
        msg += "<br/>[BaseDataControlMenu]<br/>" + ex.Message + "<br/>[/BaseDataControlMenu]<br/>";
      }
      return msg + BaseDataControlUI.GetStaticDefaultDebug(dc);
    }
    public new string Debug
    {
      get
      {
        return _Debug + GetStaticDefaultDebug(this);
      }
      set
      {
        _Debug = value;
      }
    }
    #endregion Properties 
    #region Properties Group
    public string Kdgroup { get; set; }
    public string Nmgroup { get; set; }
    public bool Stdelete { get; set; }
    public bool Stfilter { get; set; }
    public bool Stinsert { get; set; }
    public bool Stload { get; set; }
    public bool Stupdate { get; set; }
    #endregion
    public new string[] GetKeys()
    {
      return new string[] { "Idapp","Kdapp","Nmapp","Urapp"
        ,"Idmenu","Kdmenu"
        ,"Olist1","Olistdetil1","Olistdetil2","Olistdetil3","Olistdetil4"
        ,"Stinsert","Stupdate","Stdelete","Stload","Stfilter"
        ,"Nchild","Nfiles","Progress","Tahun"
        , "Status", "Statusicon", "Statusname", "Stricon", "Rating"
        ,"Last_by","Last_date"
        ,"Url","Nmmenu","UrlFull","Kdlevel","Type" };
    }
    public new string[] GetCsvColumns(int mode)
    {
      return new string[] { "Kdmenu", "Nmmenu", "Url", "Kdlevel", "Type" };
      //return new string[] { "Kdmenu", "Kddok", "Idxdok", "Nmmenu", "Url", "Olist1", "Olistdetil1", "Olistdetil2", "Olistdetil3", "Olistdetil4", "Kdlevel", "Type" };
    }
    public new string[] GetLoadCsvColumns()
    {
      return new string[] { "Kdmenu", "Nmmenu", "Url", "Kdlevel", "Type" };
      //return new string[] { "Kdmenu", "Kddok", "Idxdok", "Nmmenu", "Url", "Olist1", "Olistdetil1", "Olistdetil2", "Olistdetil3", "Olistdetil4", "Kdlevel", "Type" };
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      ExtTreePageUtils.SetDefaultTreeGrid(Columns, this, ModePreviewIndex);
      #region Command
      TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 250, Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"<a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""URL"",""URL"", ""{[GetURLEditor(values.Idmenu)]}"");'>URL</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Link"",""Link"", ""{[GetURLMapClass(values.Idmenu)]}"");'>Link</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Entry"",""Entry"", ""{[GetURLEntry(values.Idmenu)]}"");'>Entry</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Grid"",""Grid"", ""{[GetURLColGrid(values.Idmenu)]}"");'>Grid</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Tree"",""Tree"", ""{[GetURLColTree(values.Idmenu)]}"");'>Tree</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Filter"",""Filter"", ""{[GetURLFilter(values.Idmenu)]}"");'>Filter</a> | <a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""Csv"",""Csv"", ""{[GetURLCsv(values.Idmenu)]}"");'>CSV</a>
      ";
      Columns.Add(col1);
      #endregion
    }
    public new string[] GetScript()
    {
      ArrayList listScript = new ArrayList(base.GetScript());
      string script = @"
        var GetURLForum = function (val) {
            return '" + URLForum + @"&id={0}&error=1'.format(val);
        };
        var GetURLListForum = function (val) {
            return '" + URLListForum + @"&idprev={0}'.format(val);
        };
        var GetURLEntry = function (val) {
            return '" + URLEntry + @"&idprev={0}'.format(val);
        };
        var GetURLColGrid = function(val) {
            return '" + URLGrid + @"&idprev={0}'.format(val);
        };
        var GetURLColTree = function(val) {
            return '" + URLTree + @"&idprev={0}'.format(val);
        };
        var GetURLFilter = function(val) {
            return '" + URLFilter + @"&idprev={0}'.format(val);
        };
        var GetURLCsv = function(val) {
            return '" + URLCsv + @"&idprev={0}'.format(val);
        };
        var GetURLKkp = function(val) {
            return '" + URLKKP + @"&idprev={0}'.format(val);
        };
      ";
      listScript.Add(script);
      string[] scripts = new string[listScript.Count];
      listScript.CopyTo(scripts);
      return scripts;
    }
    #region Property Link
    public string LinkLink
    {
      get
      {
        return ConstantDict.Translate("LinkURL") + ":" + Url;
      }
    }
    public string URLKKP
    {
      get
      {
        string url = string.Format("Page/PageTabular.aspx?app={0}&id={1}&idprev={2}",
           GlobalAsp.GetSessionApp(), BOSysUtils.ID_MENU_KKP, GlobalAsp.GetRequestId());
        return url;
      }
    }
    public string URLListForum
    {
      get
      {
        string url = string.Format("Page/PageTabular.aspx?app={0}&id={1}",
           GlobalAsp.GetSessionApp(), BOSysUtils.ID_LIST_FORUM);
        return url;
      }
    }
    public string URLForum
    {
      get
      {
        string url = string.Format("Page/Comment.aspx?app={0}",
           GlobalAsp.GetSessionApp());
        return url;
      }
    }
    public string URLEntry
    {
      get
      {
        string url = string.Format("Page/PageForm.aspx?app={0}&id={1}",
           GlobalAsp.GetSessionApp(), BOSysUtils.ID_FORM_ENTRY, GlobalAsp.GetRequestId());
        return url;
      }
    }
    public string URLGrid
    {
      get
      {
        string url = string.Format("Page/PageTabular.aspx?app={0}&id={1}",
           GlobalAsp.GetSessionApp(), BOSysUtils.ID_COL_GRID, GlobalAsp.GetRequestId());
        return url;
      }
    }
    public string URLTree
    {
      get
      {
        string url = string.Format("Page/PageTabular.aspx?app={0}&id={1}",
           GlobalAsp.GetSessionApp(), BOSysUtils.ID_COL_TREE, GlobalAsp.GetRequestId());
        return url;
      }
    }
    public string URLFilter
    {
      get
      {
        string url = string.Format("Page/PageTabular.aspx?app={0}&id={1}",
           GlobalAsp.GetSessionApp(), BOSysUtils.ID_FILTERS, GlobalAsp.GetRequestId());
        return url;
      }
    }
    public string URLCsv
    {
      get
      {
        string url = string.Format("Page/PageTabular.aspx?app={0}&id={1}",
           GlobalAsp.GetSessionApp(), BOSysUtils.ID_KEYS, GlobalAsp.GetRequestId());
        return url;
      }
    }
    public string LinkEntry
    {
      get
      {
        return ConstantDict.Translate("LinkEntry") + ":" + URLEntry + "&val=" + Idmenu;
      }
    }
    public void SetAboutValue(Page page, string id, string value)
    {
      HtmlGenericControl ctrl = (HtmlGenericControl)page.FindControl(id);
      if (ctrl != null)
      {
        ctrl.InnerText = value;
      }
    }
    public string GetOList1()
    {
      return Olist1;
    }

    public string GetOListDetil1()
    {
      return Olistdetil1;
    }

    public string GetOListDetil2()
    {
      return Olistdetil2;
    }

    public string GetOListDetil3()
    {
      return Olistdetil3;
    }

    public string GetOListDetil4()
    {
      return Olistdetil4;
    }
    public string GetLookupOList1()
    {
      return Olookuplist1;
    }

    public string GetLookupOListDetil1()
    {
      return Olookuplistdetil1;
    }

    public string GetLookupOListDetil2()
    {
      return Olookuplistdetil2;
    }

    public string GetLookupOListDetil3()
    {
      return Olookuplistdetil3;
    }

    public string GetLookupOListDetil4()
    {
      return Olookuplistdetil4;
    }
    #endregion

  }
}
