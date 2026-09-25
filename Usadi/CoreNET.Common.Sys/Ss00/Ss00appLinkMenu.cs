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
  #region CoreNET.Common.BO.Ss00appLinkMenuControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appLinkMenuControl : Ss00appLinkControl, IDataControlMenu, IDataControlMenuMapClass, IHasJSScript
  {
    public Ss00appLinkMenuControl()
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
        string url = "IndexMenu.aspx";//01.12.02.01.Register Menu
        dc.UrlFull = string.Format(url + "?app={0}", dc.Idapp);
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
  }
  #endregion Ss00appLinkMenu
}

