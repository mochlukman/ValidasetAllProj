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
  #region CoreNET.Common.BO.Ss01appmenuFilterControl, CoreNET.Common.BO
  [Serializable]
  public class Ss01appmenuFilterControl : Ss01appmenuRegControl, IDataControlTreeGrid3, IHasJSScript
  {
    public Ss01appmenuFilterControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      if (GlobalAsp.GetSessionUser().GetUserType() == GlobalAsp.USER_ADMIN)
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL_LOADCSV;
      }
      else
      {
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT_DEL;
      }
      return cViewListProperties;
    }
    public new void SetPageKey()
    {
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlMenu).IsInstanceOfType(bo))
      {
        Kdapp = GlobalAsp.GetRequestKode();
        SsappLookupControl.FindAndSetValuesIntoByKdapp(this);
      }
    }

    public new IList View()
    {
      IList list = base.View(BaseDataControl.ALL);
      return list;
    }
    public new int Delete()
    {
      int n = -1;
      if (GlobalAsp.GetSessionUser().GetUserType() == GlobalAsp.USER_ADMIN)
      {
        n = base.Delete(BaseDataControl.DEFAULT);
      }
      else
      {
        Status = -1;
        n = base.Update(BaseDataControl.STATUS);
      }
      return n;
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

  }
  #endregion Ss01appmenuFilter
}

