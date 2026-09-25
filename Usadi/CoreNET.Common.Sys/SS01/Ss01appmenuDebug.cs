using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuDebug
  [Serializable]
  public class Ss01appmenuDebugControl : Ss01appmenuControl, IDataControlTreeGrid3, IDataControlMenu, IDataControlMenuMapClass, IHasJSScript, ICsv, IExtLoadCsv
  {

    public Ss01appmenuDebugControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      return cViewListProperties;
    }

    public new void SetPageKey()
    {
      Idapp = GlobalAsp.GetSessionApp();
    }

    public new HashTableofParameterRow GetFilters()
    {
      return ((BaseDataControlExt)this).GetFilters();
    }

    public new IList View()
    {
      return View(BaseDataControl.ALL);
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<Ss01appmenuControl> ListData = new List<Ss01appmenuControl>();
      foreach (Ss01appmenuControl dc in list)
      {
        SsappLookupControl.FindAndSetValuesIntoByIdapp(dc);
        switch (GlobalAsp.GetRequestDebug())
        {
          case OLIST1LISTMENU:
            dc.UrlFull = string.Format("Page/PageForm.aspx?app={0}&id={1}&idprev={2}",//&debug={3}", 
              Idapp, BOSysUtils.ID_LIST_MENU, dc.Idmenu, GlobalAsp.GetRequestDebug());
            break;
          case OLIST1MENUKKP:
            dc.UrlFull = string.Format("Page/PageTabular.aspx?app={0}&id={1}&idprev={2}&kode={3}&idx={4}",//&debug={3}", 
              Idapp, BOSysUtils.ID_MENU_KKP, dc.Idmenu, dc.Kddok, dc.Idxdok);
            break;
          case OLIST1STATUSFORM:
            dc.UrlFull = string.Format("Page/PageForm.aspx?app={0}&id={1}&idprev={2}",//&debug={3}", 
              Idapp, BOSysUtils.ID_MENU_STATUS, dc.Idmenu, GlobalAsp.GetRequestDebug());
            break;
          case OLIST1SYSGETKEYS:
            dc.UrlFull = string.Format("Page/PageForm.aspx?app={0}&id={1}&idprev={2}",//&debug={3}",
              Idapp, BOSysUtils.ID_KEYS, dc.Idmenu, GlobalAsp.GetRequestDebug());
            break;
          case OLIST1SYSGETROWS:
            dc.UrlFull = string.Format("Page/PageTabular.aspx?app={0}&id={1}&idprev={2}",//&debug={3}",
              Idapp, BOSysUtils.ID_FORM_ENTRY, dc.Idmenu, GlobalAsp.GetRequestDebug());
            break;
          case OLIST1SYSGETCOLS:
            dc.UrlFull = string.Format("Page/PageTabular.aspx?app={0}&id={1}&idprev={2}",//&debug={3}",
              Idapp, BOSysUtils.ID_COL_GRID, dc.Idmenu, GlobalAsp.GetRequestDebug());
            break;
          case OLIST1SYSGETTREECOLS:
            dc.UrlFull = string.Format("Page/PageTabular.aspx?app={0}&id={1}&idprev={2}",//&debug={3}",
              Idapp, BOSysUtils.ID_COL_TREE, dc.Idmenu, GlobalAsp.GetRequestDebug());
            break;
          case OLIST1SYSGETFILTERS:
            dc.UrlFull = string.Format("Page/PageTabular.aspx?app={0}&id={1}&idprev={2}",//&debug={3}",
              Idapp, BOSysUtils.ID_FILTERS, dc.Idmenu, GlobalAsp.GetRequestDebug());
            break;
        }
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }

  }
  #endregion Ss01appmenuDebug
}

