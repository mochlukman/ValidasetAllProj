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
  #region CoreNET.Common.BO.Ss00appLinkConfigControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appLinkConfigControl : Ss00appLinkControl, IDataControlMenu, IDataControlMenuMapClass, IHasJSScript
  {
    public Ss00appLinkConfigControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APP;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        Last_by = ((BaseBO)bo).Userid;
      }
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<SsappControl> ListData = new List<SsappControl>();
      foreach (SsappControl dc in list)
      {
        string url = "Page/PageTabular.aspx";//01.11.02.01.Konfigurasi Aplikasi Ini
        dc.UrlFull = string.Format(url + "?app={0}&id={1}", dc.Idapp, "C367A44A-6D7D-42BA-9A86-46324E0161A4");
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
  }
  #endregion Ss00appLinkConfig
}

