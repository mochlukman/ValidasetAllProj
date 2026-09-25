using System;
using System.Data;
using System.Collections;
using System.Web;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuApp
  /**
   * Used In : IDMENU = 9F5AC892-6B0C-4807-89AD-3BC739CE034B (Modul Developer 01.11.09.)
   * */
  [Serializable]
  public class Ss01appmenuAppControl : Ss01appmenuRegControl, IDataControlTreeGrid3, IHasJSScript, ICsv, IExtLoadCsv
  {
    public Ss01appmenuAppControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      cViewListProperties.ReadOnlyFields = new string[] { "Idapp", "Idmenu" };
      return cViewListProperties;
    }

    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(Ss00appkkpControl).IsInstanceOfType(bo))//Set when select in Lookup
      {
        Idapp = ((Ss00appkkpControl)bo).Idapp;
        SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      }
    }

    public new HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }

    public new void SetTreeGridColumns(TreeGridColumnCollection Columns)
    {
      ExtTreePageUtils.SetDefaultTreeGrid(Columns, this, ModePreviewIndex);
      #region Command
      TreeGridColumn col1 = new TreeGridColumn { Header = ConstantDict.Translate(string.Empty), Width = 250, Align = Ext.Net.TextAlign.Center };
      col1.XTemplate.Html = @"<a style='visibility:true;'
        onclick = 'parent.loadPageByID(""Pages"",""KKP"",""KKP"", ""{[GetURLKkp(values.Idmenu)]}"");'>KKP</a><a style='visibility:true;'
      ";
      Columns.Add(col1);
      #endregion
    }

  }
  #endregion Ss01appmenuApp
}

