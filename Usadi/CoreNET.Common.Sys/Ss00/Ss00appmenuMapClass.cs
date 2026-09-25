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
  #region Ss00appmenuMapClass
  [Serializable]
  public class Ss00appmenuMapClassControl : Ss00appmenuSetURLControl, IDataControlTreeGrid3, IHasJSScript, ICsv
  {
    public Ss00appmenuMapClassControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPMENU;
      SetMaxModePreviewIndex(1);
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      return cViewListProperties;
    }
  }
  #endregion Ss00appmenuMapClass
}

