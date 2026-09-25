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
  #region CoreNET.Common.BO.Ss00appconfigUIControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appconfigUIControl :  Ss00appconfigRegControl, IDataControlUIEntry, IHasJSScript
  {
    public Ss00appconfigUIControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPCONFIG;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(Ss00appLookupControl.Instance.GetLookupParameterRowAll(this, false).SetEnable(false));
      return hpars;
    }

  }
  #endregion Ss00appconfigUIControl
}

