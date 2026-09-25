using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Ss00appmenuEditConfigControl
  /**
  * Used In : IDMENU = 637317A5-5DA3-4991-9CC2-468037264102 (Modul Developer 01.12.02.03.)
  * */
  [Serializable]
  public class Ss00appmenuEditConfigControl : Ss00appmenuRegControl, IDataControlTreeGrid3, IHasJSScript, ICsv, IExtLoadCsv
  {
    public Ss00appmenuEditConfigControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      return cViewListProperties;
    }
    public new IList View()
    {
      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = base.View(label);
      List<Ss00appmenuControl> ListData = new List<Ss00appmenuControl>();
      foreach (Ss00appmenuControl dc in list)
      {
        //Error View State
        //string cname = "CoreNET.Common.BO.Ss00appmenuconfigControl, CoreNET.Common.Sys";
        //string url = $"Page/PageForm.aspx?app={GlobalAsp.GetSessionApp()}&dc={cname}&val={dc.Idapp}";
        string idmenu = "F73C792F-FB69-4C7B-BD46-8BC40EF3E966";
        dc.Url = $"Page/PageForm.aspx?app={GlobalAsp.GetSessionApp()}&id={idmenu}&val={dc.Idapp}";
        dc.UrlFull = dc.Url;
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
  }
  #endregion Ss00appmenuStatus
}

