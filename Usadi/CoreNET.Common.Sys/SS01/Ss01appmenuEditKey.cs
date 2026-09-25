using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuEditKeyControl
  /**
  * Used In : IDMENU = 637317A5-5DA3-4991-9CC2-468037264102 (Modul Developer 01.12.02.03.)
  * */
  [Serializable]
  public class Ss01appmenuEditKeyControl : Ss01appmenuEditConfigControl, IDataControlTreeGrid3, IHasJSScript, ICsv, IExtLoadCsv
  {
    public Ss01appmenuEditKeyControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IList View()
    {
      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = base.View(label);
      List<Ss01appmenuControl> ListData = new List<Ss01appmenuControl>();
      foreach (Ss01appmenuControl dc in list)
      {
        //Error View State
        //string cname = "CoreNET.Common.BO.Ss01appmenuconfigControl, CoreNET.Common.Sys";
        //string url = $"Page/PageForm.aspx?app={GlobalAsp.GetSessionApp()}&dc={cname}&val={dc.Idapp}";
        string idmenu = "E7040A01-3B04-45B9-9EFC-CAA6EAB6F4A6";
        dc.Url = $"Page/PageTabular.aspx?app={GlobalAsp.GetSessionApp()}&id={idmenu}&val={dc.Idmenu}";
        dc.UrlFull = dc.Url;
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
  }
  #endregion Ss01appmenuEditKeyControl
}

