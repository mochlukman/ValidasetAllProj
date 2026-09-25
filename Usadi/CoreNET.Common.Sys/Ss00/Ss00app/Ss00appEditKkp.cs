using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss00appMonKkpControl, CoreNET.Common.BO
  /**
   * Struktur Menu dan Monitoring di Modul Testing
   * as DCMenu where Idapp='68065186-FF1B-4EB3-8A6B-6D1FDCFD3BAD'
   * */
  [Serializable]
  public class Ss00appMonKkpControl : Ss00appLinkControl, IDataControlTreeGrid3, IHasJSScript
  {
    public Ss00appMonKkpControl()
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
        //string cname = "CoreNET.Common.BO.Ss01appmenuMonControl, CoreNET.Common.Sys";
        //string url = $"Page/PageTreeGrid.aspx?app={GlobalAsp.GetSessionApp()}&dc={cname}&val={dc.Idapp}";
        string idmenu = "9F5AC892-6B0C-4807-89AD-3BC739CE034B";
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

