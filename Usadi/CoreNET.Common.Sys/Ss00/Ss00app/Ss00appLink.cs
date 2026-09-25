using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;
using CoreNET.Common.BO;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss00appLinkControl, CoreNET.Common.BO
  /**
   * Untuk struktur modul aplikasi di Portal
   * Hati2 extend kelas ini
   * */
  [Serializable]
  public class Ss00appLinkControl : Ss00appControl, IDataControlUIEntry, IDataControlMenu, IDataControlMenuMapClass, IHasJSScript
  {
    public Ss00appLinkControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APP;
      SetMaxModePreviewIndex(1);
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeToolbar = ViewListProperties.MODE_TOOLBAR_MINIMALIS;
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(Ss10userLoginControl).IsInstanceOfType(bo))
      {
        Idapp = GlobalAsp.GetRequestApp();
        SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      }
    }
    public new IList View()
    {
      QSSub = GlobalAsp.GetRequestSub();
      List<SsappControl> list = (List<SsappControl>)this.View(BaseDataControl.LOOKUP);
      //Ss00appControl dcRoot = (Ss00appControl)list.Find(o => o.Kdapp.Equals(Kdapp));
      //List<SsappControl> roots = list.FindAll(o => o.Kdapp.StartsWith(Kdapp)
      //&& (o.Kdlevel == dcRoot.Kdlevel + 1) && (o.Status != -1));// && (o.Status != -1)

      return list;
    }
    public new IList View(string label)
    {
      IList list = base.View(label);
      List<SsappControl> ListData = new List<SsappControl>();
      foreach (SsappControl dc in list)
      {
        //string prefix = GlobalAsp.GetConfigURLPrefix();
        //string url = MasterAppConstants.Instance.URLBase + prefix + dc.Url;
        ////url = url.Replace("MainMenu.aspx", "Login.aspx");//Ngga bisa passing userkey
        //if (MasterAppConstants.Instance.StatusTesting)
        //{
        //  url = MasterAppConstants.Instance.URLBase + prefix + dc.Url;
        //  url = "Login.aspx";
        //}
        //dc.UrlFull = string.Format(url + "?app={0}&sub={1}", dc.Idapp,this.QSSub);
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      Update(ListData);
      return ListData;
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdapp"), Width = 200, DataIndex = "Kdapp", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmapp"), Width = 500, DataIndex = "Nmapp", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nchild"), Width = 70, DataIndex = "Nchild", Align = Ext.Net.TextAlign.Center });
      Columns.Add(BaseDataControlExt.TreeColStatus);
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Progress"), Width = 70, DataIndex = "Progressstr", Align = Ext.Net.TextAlign.Center });
      TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 50, DataIndex = "Progressicon", Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"<a style='visibility:true;'>
        <img style='width: 16px; height: 16px;' title='Progress {Progressstr}%' src='../Res/Icons/{Progressicon}' alt='{Progressstr}'/></a>
      ";
      Columns.Add(col1);
      if (GetModePreviewIndex() == 1)
      {
        #region Command
        col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 200, Align = Ext.Net.TextAlign.Center };
        col1.XTemplate.Html = @"
        <a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Edit {Kdapp}"",""Edit {Kdapp}"", ""{[GetURLEdit(values.Kdapp)]}"");'>Edit</a>
        |<a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Config {Kdapp}"",""Config {Kdapp}"", ""{[GetURLConfig(values.Kdapp)]}"");'>Config</a>
        |<a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Login {Kdapp}"",""Login {Kdapp}"", ""{[GetURLLogin(values.Kdapp)]}"");'>Login</a>
        |<a style='visibility:true;'
          onclick = 'parent.loadPageByID(""Pages"",""Menu {Kdapp}"",""Menu {Kdapp}"", ""{[GetURLMenu(values.Kdapp)]}"");'>Menu</a>
        ";
        Columns.Add(col1);

        //col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 50, Align = Ext.Net.TextAlign.Center };
        //col1.XTemplate.Html = @"<a style='visibility:true;'
        //  onclick = 'parent.loadPageByID(""Pages"",""Login {Kdapp}"",""Login {Kdapp}"", ""{[GetURLLogin(values.Kdapp)]}"");'>Login</a><a style='visibility:true;'
        //";
        //Columns.Add(col1);

        //col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 50, Align = Ext.Net.TextAlign.Center };
        //col1.XTemplate.Html = @"<a style='visibility:true;'
        //  onclick = 'parent.loadPageByID(""Pages"",""Menu {Kdapp}"",""Menu {Kdapp}"", ""{[GetURLMenu(values.Kdapp)]}"");'>Menu</a><a style='visibility:true;'
        //";
        //Columns.Add(col1);
        #endregion
      }
    }
    public string[] GetScript()
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
        var GetURLLogin = function(val) {
            return 'Login.aspx?kdapp={0}&frame=1'.format(val);
        };
        var GetURLConfig = function(val) {
            return '" + URLCONFIG + @"'.format(val);
        };
        var GetURLMenu = function(val) {
            return '" + URLMENU + @"'.format(val);
        };
        var GetURLEdit = function(val) {
            return '" + URLEDIT + @"'.format(val);
        };
      ";
      return new string[] { script };
    }
    public string URLMENU
    {
      get
      {
        string url = string.Format("Page/PageTreeGrid.aspx?app={0}&i=1&id={1}",
           "{0}","");
        return url;
      }
    }
    public string URLEDIT
    {
      get
      {
        string url = string.Format("Page/PageForm.aspx?app={0}&i=1&id={1}",
           "{0}", "");
        return url;
      }
    }
    public string URLCONFIG
    {
      get
      {
        string url = string.Format("Page/PageTabular.aspx?app={0}&i=1&id={1}",
           "{0}", "");
        return url;
      }
    }

    public void GetDefaultURL(out string roleid, out string url)
    {
      //url = string.Format("Page/IndexMon.aspx?app={0}&id={1}&sub={2}", GlobalAsp.GetRequestApp(), "3714E4F3-76D0-44F5-8A9B-738ADEB8F86E", GlobalAsp.GetRequestSub());

      roleid = "01.";//Ngga ngaruh, diset default di IndexMon nya
      string idmenu = "3714E4F3-76D0-44F5-8A9B-738ADEB8F86E";
      string ses_name = "objlist1" + idmenu + ".";
      HttpContext.Current.Session[ses_name] = new Ss00appLinkControl();
      url = string.Format("Page/PageTreeGrid.aspx?passdc=1&app={0}&id={1}&sub={2}", GlobalAsp.GetRequestApp(), idmenu, GlobalAsp.GetRequestSub());
    }

    public IDataControlMenu FindObject(string roleid)
    {
      Ss00appLinkControl dc = new Ss00appLinkControl() { Kdapp = roleid };
      SsappControl dcfound = SsappLookupControl.FindAndSetValuesIntoByIdapp(dc);
      dc.CopyPropertyBOFrom(dcfound);
      return dc;
    }


  }
  #endregion Ss00appLink
}

