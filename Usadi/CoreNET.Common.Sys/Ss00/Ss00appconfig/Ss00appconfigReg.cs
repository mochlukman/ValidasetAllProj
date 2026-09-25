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
  #region CoreNET.Common.BO.Ss00appconfigRegControl, CoreNET.Common.BO
  [Serializable]
  public class Ss00appconfigRegControl :  Ss00appconfigControl, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    public Ss00appconfigRegControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPCONFIG;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL_LOADCSV;
      return cViewListProperties;
    }
    //public override DataControlFieldCollection GetColumns()
    //{
    //  DataControlFieldCollection columns = new DataControlFieldCollection();
    //  columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Par"), typeof(string), 20, HorizontalAlign.Left));
    //  columns.Add(Fields.Create(ConstantDict.GetColumnTitle("#"), typeof(CommandColumn), 20, HorizontalAlign.Center));
    //  columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Value"), typeof(string), 70, HorizontalAlign.Left).SetEditable(true));
    //  return columns;
    //}

    //public new HashTableofParameterRow GetFilters()
    //{
    //  bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
    //  HashTableofParameterRow hpars = new HashTableofParameterRow();
    //  hpars.Add(Ss00appLookupControl.Instance.GetLookupParameterRowAll(this, false).SetEnable(true));
    //  return hpars;
    //}
    public new string[] GetCsvColumns(int mode)
    {
      return new string[] { "Par", "Value"};
    }
    public new string[] GetLoadCsvColumns()
    {
      return new string[] { "Par", "Value" };
    }
  }
  #endregion Ss00appconfigReg
}

