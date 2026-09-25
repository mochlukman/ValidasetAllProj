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
  #region CoreNET.Common.BO.SsappmenuallLookupControl, CoreNET.Common.BO
  [Serializable]
  public class SsappmenuAllOlist1Control : Ss00appmenuControl, IDataControlTreeGrid3, IHasJSScript, ICsv, IExtLoadCsv
  {
    public SsappmenuAllOlist1Control()
    {
      XMLName = ConstantTablesSys.XMLSS00APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.CanModeTree = false;
      return cViewListProperties;
    }

    public new IList View()
    {
      IList list = base.View(BaseDataControl.UNION + "Olist1");
      return list;
    }
    public new void SetFilterKey(BaseBO bo)
    {
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev());
      HashTableofParameterRow hpars = BaseDataControlExt.GetFilters(this, enableFilter);
      return hpars;
    }
    public new DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection cols = BaseDataControlExt.GetColumn(this, ModePreviewIndex);
      return cols;
    }
    public new HashTableofParameterRow GetEntries()
    {
      bool enable = true;
      HashTableofParameterRow hpars = BaseDataControlExt.GetEntries(this, enable);
      return hpars;
    }

  }
  #endregion SsappmenuLookup
}

