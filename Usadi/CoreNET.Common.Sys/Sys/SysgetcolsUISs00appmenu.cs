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
  #region CoreNET.Common.BO.SysgetcolsSs00appmenuControl, CoreNET.Common.BO
  [Serializable]
  public class SysgetcolsUISs00appmenuControl :  SysgetcolsUIControl, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    public SysgetcolsUISs00appmenuControl()
    {
      XMLName = ConstantTablesSys.XMLSYSGETCOLS;
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
  #endregion SysgetcolsSs00appmenu
}

