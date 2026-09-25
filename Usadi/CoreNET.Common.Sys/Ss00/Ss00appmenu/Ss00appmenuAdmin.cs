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
  #region CoreNET.Common.BO.Ss00appmenuAdminControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appmenuAdminControl : Ss00appmenuControl, IDataControlMenu, IDataControlTreeGrid3, IHasJSScript
  {
    public Ss00appmenuAdminControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
    }
    public new void SetFilterKey(BaseBO bo)
    {
      base.SetFilterKey(bo);
    }

    public new HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }
    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Kdmenu"), Width = 300, DataIndex = "Kdmenu", Align = Ext.Net.TextAlign.Left });
      Columns.Add(new TreeGridColumn { Header = ConstantDict.Translate("Nmapp"), Width = 500, DataIndex = "Nmmenu", Align = Ext.Net.TextAlign.Left });
      TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate("Statusicon"), Width = 50, DataIndex = "Statusicon", Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"<a style='visibility:true;'>
                    <img style='width: 16px; height: 16px;' title='{Statusname}' src='../Res/Icons/{Statusicon}' alt='{Statusicon}'/></a>
                  ";
      Columns.Add(col1);
    }
    #endregion Ss00appmenuAdmin
  }
}

