using CoreNET.Common.Base;
using System;
using System.Web;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuSetURL
  [Serializable]
  public class Ss01appmenuSetURLControl : Ss01appmenuRegControl, IDataControlTreeGrid3, IHasJSScript, ICsv
  {
    public Ss01appmenuSetURLControl()
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
      string idapp = GlobalAsp.GetRequestApp();
      if (string.IsNullOrEmpty(idapp))
      {
        idapp = GlobalAsp.GetSessionApp();
      }
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev()) && (idapp.Equals(MasterAppConstants.Instance.MasterAppID));
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      ParameterRow pr = Ss00appLookupControl.Instance.GetLookupParameterRowAll(this, false).SetEnable(enableFilter);
      hpars.Add(pr);
      pr = Ss01appmenuLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter);
      hpars.Add(pr);
      return hpars;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
      }
      else
      {
        try
        {
          string idmenu = HttpContext.Current.Request["val"];
          if (string.IsNullOrEmpty(idmenu))
          {
            idmenu = GlobalAsp.GetRequestIdPrev();
          }
          Idmenu = idmenu;
          Load(BaseDataControl.ID);

          string idapp = GlobalAsp.GetRequestApp();
          if (string.IsNullOrEmpty(idapp))
          {
            idapp = GlobalAsp.GetSessionApp();
          }
          SsappControl dc = new SsappControl() { Idapp = idapp };
          dc = SsappLookupControl.FindAndSetValuesIntoByIdapp(dc);
          Kdapp = dc.Kdapp;
          Nmapp = dc.Nmapp;
          //SsappmenuControl dc = new SsappmenuControl() { Idmenu = idmenu };
          //dc = SsappmenuLookupControl.FindAndSetValuesInto(dc);
          //Kdmenu = dc.Kdmenu;
          //Nmmenu = dc.Nmmenu;
        }
        catch (Exception ex)
        {
          UtilityBO.Log(this, ex);
        }
      }
    }

  }
  #endregion Ss01appmenuSetURL
}

