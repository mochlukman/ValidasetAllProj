using System;
using System.Data;
using System.Web;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuStatus
  [Serializable]
  public class Ss01appmenuStatusControl : Ss01appmenuRegControl, IDataControlTreeGrid3, IHasJSScript, ICsv, IExtLoadCsv
  {
    public Ss01appmenuStatusControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      return cViewListProperties;
    }
    public new HashTableofParameterRow GetFilters()
    {
      Ss00appControl dc = new Ss00appControl();
      dc.Idapp = GlobalAsp.GetSessionApp();
      dc = Ss00appLookupControl.FindAndSetValuesIntoByIdapp(dc);
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev()) && (dc.Type.Equals("H"));
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      ParameterRow pr = Ss00appLookupControl.Instance.GetLookupParameterRowDetil(this, false).SetEnable(enableFilter);
      hpars.Add(pr);
      return hpars;
    }

    public new int Update()
    {
      int n = base.Update(BaseDataControl.STATUS);
      return n;
    }
  }
  #endregion Ss01appmenuStatus
}

