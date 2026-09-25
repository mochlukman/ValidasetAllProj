using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss00appMonControl, CoreNET.Common.BO
  /**
   * Used In : IDMENU = DE9C8CE2-3486-42C5-BF82-61ED430B874B (Modul Developer 01.11.)
   * */
  [Serializable]
  public class Ss00appMonControl : Ss00appLinkControl, IDataControlTreeGrid3, IHasJSScript
  {
    public Ss00appMonControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APP;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      cViewListProperties.IDKey = "Idapp2";//Buat menyimpan data
      return cViewListProperties;
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (!string.IsNullOrEmpty(GlobalAsp.GetRequestVal()))
      {
        Idapp2 = GlobalAsp.GetRequestVal();
        SsappControl dc = new SsappControl() { Idapp = Idapp2 };
        SsappLookupControl.FindAndSetValuesIntoByIdapp(dc);
        Kdapp2 = dc.Kdapp;
        Nmapp2 = dc.Nmapp;
      }
      else
      {
        base.SetFilterKey(bo);
      }
    }
    public new IList View()
    {
      IList list = View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = base.View(label);
      List<SsappControl> ListData = new List<SsappControl>();
      foreach (SsappControl dc in list)
      {
        //Error View State
        //string cname = "CoreNET.Common.BO.Ss01appmenuMonControl, CoreNET.Common.Sys";
        //string url = $"Page/PageTreeGrid.aspx?app={GlobalAsp.GetSessionApp()}&dc={cname}&val={dc.Idapp}";
        string idmenu = "0758E97F-32E1-44F6-A1B3-B552296266D6";
        dc.Url = $"Page/PageTreeGrid.aspx?app={GlobalAsp.GetSessionApp()}&id={idmenu}&val={dc.Idapp}";
        dc.UrlFull = dc.Url;
        dc.Idapp2 = dc.Idapp;
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        if (MasterAppConstants.Instance.StatusAdmin)
        {
          ListData.Add(dc);
        }
        else
        {
          if (dc.Last_by.Equals(GlobalAsp.GetSessionUser().GetUserID()))
          {
            dc.Type = "D";
            dc.Kdlevel = 1;
            ListData.Add(dc);
          }
        }
      }
      return ListData;
    }

  }
  #endregion Ss00appMon
}

