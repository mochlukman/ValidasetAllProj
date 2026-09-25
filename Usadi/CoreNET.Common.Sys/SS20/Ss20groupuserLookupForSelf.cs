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
  #region CoreNET.Common.BO.Ss20groupuserLookupForSelfControl, CoreNET.Common.BO
  [Serializable]
  public class Ss20groupuserLookupForSelfControl :  Ss20groupuserControl, IDataControlUIEntry, IHasJSScript
  {
    public Ss20groupuserLookupForSelfControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPUSER;
    }
    ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if(cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
        cViewListProperties.ReadOnlyFields = new string[] { "Kdgroup"};
      }
      return cViewListProperties;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(Ss20groupuserControl).IsInstanceOfType(bo))
      {
        Kdgroup = ((Ss20groupuserControl)bo).Kdgroup;
        Nmgroup = ((Ss20groupuserControl)bo).Nmgroup;
      }
    }

    public new IList View()
    {
      IList list = this.View(BaseDataControl.LOOKUP_FOR_SELF);
      return list;
    }

    public new HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }


  }
  #endregion Ss20groupuserLookupForSelf
}

