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
  #region CoreNET.Common.BO.Ss01appmenuRegFileControl, CoreNET.Common.Sys
  [Serializable]
  public class Ss01appmenuRegFileControl :  Ss01appmenuRegControl, IDataControlUIEntry, IHasJSScript
  {
    public Ss01appmenuRegFileControl()
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
      hpars.Add(new ParameterRowUploadFile(this, true));
      return hpars;
    }
  }
  #endregion Ss01appmenuRegFileControl
}

