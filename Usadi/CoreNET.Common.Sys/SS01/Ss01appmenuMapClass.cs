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
  #region Ss01appmenuMapClass
  [Serializable]
  public class Ss01appmenuMapClassControl : Ss01appmenuSetURLControl, IDataControlTreeGrid3, IHasJSScript, ICsv
  {
    public Ss01appmenuMapClassControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
      //SetMaxModePreviewIndex(1);
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      return cViewListProperties;
    }
  }
  #endregion Ss01appmenuMapClass
}

