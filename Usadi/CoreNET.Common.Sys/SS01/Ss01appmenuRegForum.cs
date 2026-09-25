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
  #region CoreNET.Common.BO.Ss01appmenuRegForumControl, CoreNET.Common.Sys
  [Serializable]
  public class Ss01appmenuRegForumControl :  Ss01appmenuRegControl, IDataControlUIEntry, IHasJSScript
  {
    public Ss01appmenuRegForumControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public override HashTableofParameterRow GetEntries()
    {
      hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowForum(this, true));
      return hpars;
    }
  }
  #endregion Ss01appmenuRegForumControl
}

