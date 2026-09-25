using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss00appEditRegUserControl, CoreNET.Common.Sys
  /**
   * Struktur Menu dan Monitoring di Modul Testing
   * as DCMenu where Idapp='68065186-FF1B-4EB3-8A6B-6D1FDCFD3BAD'
   * */
  [Serializable]
  public class Ss00appEditRegUserControl : Ss00appLinkControl, IDataControlTreeGrid3, IHasJSScript
  {
    public Ss00appEditRegUserControl()
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
        //string url = $"App_Portal/RegisterLogin.aspx?mode=1&app={GlobalAsp.GetSessionApp()}&dc={cname}&val={dc.Idapp}";
        string idmenu = "99f8b51c-87f3-4210-b36f-941cc2aa24e6";
        dc.Url = $"../App_Portal/RegisterLogin.aspx?mode=1&app={GlobalAsp.GetSessionApp()}&id={idmenu}&val={dc.Idapp}";
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

