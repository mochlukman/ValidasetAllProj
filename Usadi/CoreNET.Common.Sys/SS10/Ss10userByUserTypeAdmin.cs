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
  #region CoreNET.Common.BO.Ss10userByUserTypeAdminControl, CoreNET.Common.BO
  [Serializable]
  public class Ss10userByUserTypeAdminControl : Ss10userByUserTypeControl, IDataControlUIEntry, IHasJSScript
  {
    public Ss10userByUserTypeAdminControl()
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
      Usertype = GlobalAsp.USER_ADMIN;
      Useruraian = "Administrator";
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = false;
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowSelect(ConstantDict.GetColumnTitleEntry("Status"),
          GetList(new Ss10usertypesLookupControl()), "Usertype=Uturaian", 30).SetAllowRefresh(false).SetEnable(enableFilter));
      return hpars;
    }

  }
  #endregion Ss10userByUserTypeAdmin
}

