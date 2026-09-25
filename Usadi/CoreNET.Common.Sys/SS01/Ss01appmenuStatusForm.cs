using CoreNET.Common.Base;
using System;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuStatusForm
  [Serializable]
  public class Ss01appmenuStatusFormControl : Ss01appmenuStatusControl, IDataControlTreeGrid3, IHasJSScript, ICsv, IExtLoadCsv
  {
    public Ss01appmenuStatusFormControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
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
      try
      {
        Idmenu = GlobalAsp.GetRequestIdPrev();
        if (!string.IsNullOrEmpty(Idmenu))
        {
          Load(BaseDataControl.ID);
        }
      }
      catch (Exception ex)
      {
        UtilityBO.Log(this, ex);
      }
    }

    public new HashTableofParameterRow GetFilters()
    {
      Ss00appControl dc = new Ss00appControl
      {
        Idapp = GlobalAsp.GetSessionApp()
      };
      dc = Ss00appLookupControl.FindAndSetValuesIntoByIdapp(dc);
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev()) && (dc.Type.Equals("H"));
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      ParameterRow pr = Ss00appLookupControl.Instance.GetLookupParameterRowDetil(this, false).SetEnable(enableFilter);
      hpars.Add(pr);
      pr = Ss01appmenuLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter);
      hpars.Add(pr);
      return hpars;
    }
  }
  #endregion Ss01appmenuStatusForm
}

