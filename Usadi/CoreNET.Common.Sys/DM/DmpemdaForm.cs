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
  #region CoreNET.Common.BO.DmpemdaFormControl, CoreNET.Common.BO
  [Serializable]
  public class DmpemdaFormControl :  DmpemdaControl, IDataControlUIEntry, IHasJSScript
  {
    public DmpemdaFormControl()
    {
      XMLName = ConstantTablesSys.XMLDMPEMDA;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(DmpemdaLookupControl.Instance.GetLookupParameterRow(this, false));
      return hpars;
    }

    //public new IList View()
    //{
    //  Load(BaseDataControl.PK);
    //  IList list = this.View(BaseDataControl.DEFAULT);
    //  return list;
    //}

  }
  #endregion DmpemdaForm
}

