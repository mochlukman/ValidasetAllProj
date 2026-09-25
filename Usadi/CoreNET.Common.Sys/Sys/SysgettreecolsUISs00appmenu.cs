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
  #region CoreNET.Common.BO.SysgettreecolsUISs00appmenuControl, CoreNET.Common.BO
  [Serializable]
  public class SysgettreecolsUISs00appmenuControl :  SysgettreecolsUIControl, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    public SysgettreecolsUISs00appmenuControl()
    {
      XMLName = ConstantTablesSys.XMLSYSGETTREECOLS;
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
      hpars.Add(Ss00appLookupControl.Instance.GetLookupParameterRowAll(this, false).SetEnable(enableFilter));
      hpars.Add(Ss00appmenuLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter));
      return hpars;
    }
  }
  #endregion SysgettreecolsSs00appmenu
}

