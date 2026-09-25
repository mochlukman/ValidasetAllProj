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
  #region CoreNET.Common.BO.Ss01appmenuAppControl, CoreNET.Common.BO
  [Serializable]
  public class Ss01appmenuAppLookupControl :  Ss01appmenuControl, IDataControlUIEntry, IHasJSScript
  {
    public Ss01appmenuAppLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public new HashTableofParameterRow GetFilters()
    {
      Idapp = GlobalAsp.GetSessionApp();
      SsappControl dc = SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev()) && (dc.Type.Equals("H"));
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      string[] keys = new string[] { "Idapp", "Nmapp" };
      ParameterRow pr = Ss00appLookupControl.Instance.GetLookupParameterRowDetil(this, false, keys, keys).SetAllowRefresh(true).SetEnable(enableFilter);
      hpars.Add(pr);
      return hpars;
    }
  }
  #endregion Ss01appmenuApp
}

