using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss00appEditUserControl, CoreNET.Common.Sys
  /**
   * Struktur Menu dan Monitoring di Modul Testing
   * as DCMenu where Idapp='68065186-FF1B-4EB3-8A6B-6D1FDCFD3BAD'
   * */
  [Serializable]
  public class Ss00appEditUserControl : Ss00appLinkControl, IDataControlTreeGrid3, IHasJSScript
  {
    public Ss00appEditUserControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APP;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
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
      List<SsappControl> ListData = new List<SsappControl>();
      foreach (SsappControl dc in list)
      {
        //Error View State
        //string cname = "CoreNET.Common.BO.Ss10userappControl, CoreNET.Common.Sys";
        //string url = $"Page/PageTabular.aspx?app={GlobalAsp.GetSessionApp()}&dc={cname}&val={dc.Idapp}";
        string idmenu = "A7C71368-37B6-4086-9DDE-8FB1153C5DE1";
        dc.Url = $"Page/PageTabular.aspx?app={GlobalAsp.GetSessionApp()}&id={idmenu}&val={dc.Idapp}";
        dc.UrlFull = dc.Url;
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        if (MasterAppConstants.Instance.StatusAdmin)
        {
          ListData.Add(dc);
        }
        else
        {
          if (dc.Last_by.Equals(GlobalAsp.GetSessionUser().GetUserID()))
          {
            dc.Type = "D";
            dc.Kdlevel = 1;
            ListData.Add(dc);
          }
        }
      }
      return ListData;
    }
  }
  #endregion Ss00appLink
}

