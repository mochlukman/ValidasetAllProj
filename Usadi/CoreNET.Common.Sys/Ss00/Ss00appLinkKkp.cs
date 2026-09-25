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
  #region CoreNET.Common.BO.Ss00appLinkKkpControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appLinkKkpControl : Ss00appLinkControl, IDataControlMenu, IDataControlMenuMapClass, IHasJSScript
  {
    public Ss00appLinkKkpControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APP;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
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
        string url = "Page/PageMasterDetil.aspx";//01.11.03. APP Versi/KKP
        dc.UrlFull = string.Format(url + "?app={0}&val={1}&id={2}&kode={3}&idx={3}", GlobalAsp.GetSessionApp(), dc.Idapp, "9F5AC892-6B0C-4807-89AD-3BC739CE034B", dc.Kdapp);
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
  }
  #endregion Ss00appLinkKkp
}

