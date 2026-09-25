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
  #region CoreNET.Common.BO.Ss10userByUserTypeControl, CoreNET.Common.BO
  [Serializable]
  public class Ss10userByUserTypeControl : Ss10userControl, IDataControlUIEntry, IHasJSScript
  {
    public Ss10userByUserTypeControl()
    {
      XMLName = ConstantTablesSys.XMLSS10USER;
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

    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          GetList(new Ss10usertypesLookupControl()), "Usertype=Uturaian", 30).SetAllowRefresh(false).SetEnable(enableFilter));
      return hpars;
    }

    public new IList View()
    {
      IList list = ((BaseDataControlUI)this).View(BaseDataControl.ALL);
      return list;
    }

  }
  #endregion Ss10userByUserType
}

