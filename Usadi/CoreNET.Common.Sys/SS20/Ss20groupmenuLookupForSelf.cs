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
  #region CoreNET.Common.BO.Ss20groupmenuLookupForSelfControl, CoreNET.Common.BO
  /// <summary>
  /// 
  /// </summary>
  [Serializable]
  public class Ss20groupmenuLookupForSelfControl :  Ss20groupmenuLookupControl, IDataControlUIEntry, IHasJSScript
  {
    public Ss20groupmenuLookupForSelfControl()
    {
      XMLName = ConstantTablesSys.XMLSS20GROUPMENU;
    }
    ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if(cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
        cViewListProperties.IDProperty = "Kdmenu";
      }
      return cViewListProperties;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(Ss20groupmenuControl).IsInstanceOfType(bo))
      {
        Kdgroup = ((Ss20groupmenuControl)bo).Kdgroup;
        Nmgroup = ((Ss20groupmenuControl)bo).Nmgroup;

        Idapp = ((Ss20groupmenuControl)bo).Idapp;
        Kdapp = ((Ss20groupmenuControl)bo).Kdapp;
        Nmapp = ((Ss20groupmenuControl)bo).Nmapp;

      }
    }
    public new HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        Ss00appLookupControl.Instance.GetLookupParameterRowDetil(this, false).SetEnable(false)
      };
      return hpars;
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.LOOKUP_FOR_SELF);
      return list;
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection
      {
        Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Ext.Net.Icon), 5, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Kdmenu"), typeof(string), EditCmd, 15, HorizontalAlign.Left),
        Fields.Create(ConstantDict.GetColumnTitle("Nmmenu"), typeof(string), 50, HorizontalAlign.Left),
        Fields.Create(ConstantDict.GetColumnTitle("Kdlevel"), typeof(int), 7, HorizontalAlign.Center),
        Fields.Create(ConstantDict.GetColumnTitle("Type"), typeof(string), 7, HorizontalAlign.Center)
      };
      return columns;
    }

  }
  #endregion Ss20groupmenuLookupForSelf
}

