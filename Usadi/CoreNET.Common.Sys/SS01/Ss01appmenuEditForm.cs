using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuEditFormControl
  /**
  * Used In : IDMENU = D1413E80-951B-4176-BE72-6BF2B6607EC0 (Modul Developer 01.12.02.03.)
  * */
  [Serializable]
  public class Ss01appmenuEditFormControl : Ss01appmenuEditConfigControl, IDataControlTreeGrid3, IHasJSScript, ICsv, IExtLoadCsv
  {
    public Ss01appmenuEditFormControl()
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
        string idmenu = "D1413E80-951B-4176-BE72-6BF2B6607EC0";
        dc.Url = $"Page/PageTabular.aspx?app={GlobalAsp.GetSessionApp()}&id={idmenu}&val={dc.Idmenu}";
        dc.UrlFull = dc.Url;
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
  }
  #endregion Ss01appmenuEditFormControl
}

