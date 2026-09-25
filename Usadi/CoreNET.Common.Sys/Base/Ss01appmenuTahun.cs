using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;

namespace CoreNET.Common.BO
{
  #region Ss01appmenuTahun
  [Serializable]
  public class Ss01appmenuTahunControl : Ss01appmenuControl, IDataControlMenu, IDataControlMenuMapClass, IDataControlTreeGrid3, IHasJSScript, ICsv
  {
    public Ss01appmenuTahunControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
      SetMaxModePreviewIndex(1);
      Tahun = 0;
      DmtahunLookupControl.FindAndSetValuesInto(this);
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_EDIT;
      return cViewListProperties;
    }

    public new void SetPageKey()
    {
      Idapp = GlobalAsp.GetSessionApp();
    }
    public new IList View()
    {
      IList list = new List<Ss01appmenuControl>();
      if (Tahun != 0)
      {
        list = View(BaseDataControl.FILTER);
      }
      return list;
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = true;
      HashTableofParameterRow hpars = new HashTableofParameterRow
      {
        DmtahunUILookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter)
      };
      return hpars;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(DmtahunControl).IsInstanceOfType(bo))
      {
        Tahun = ((DmtahunControl)bo).Kdtahun;
        Kdtahun = Tahun;
        DmtahunLookupControl.FindAndSetValuesInto(this);
      }
    }
    public new void SetOtorisasiMenu(Page page, string idapp)
    {
      if (string.IsNullOrEmpty(idapp))
      {
        Tahun = DateTime.Now.Year;
      }
      else
      {
        Tahun = int.Parse(idapp);//idapp adalah tahun
      }
      Kdtahun = Tahun;
      Idapp = GlobalAsp.GetSessionApp();// idapp;

    }
    public new string GetAppName(string idapp)
    {
      if (string.IsNullOrEmpty(idapp))
      {
        Tahun = DateTime.Now.Year;
      }
      else
      {
        Tahun = int.Parse(idapp);//idapp adalah tahun
      }
      Kdtahun = Tahun;
      Ss00appControl dc = new Ss00appControl();
      Idapp = GlobalAsp.GetSessionApp();// idapp;
      SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      if (Tahun > 0)
      {
        return "Tahun " + Tahun;
      }
      else
      {
        return string.Empty;
      }
    }
    public new string GetAppTitle(string idapp)
    {
      Tahun = DateTime.Now.Year;
      if (!string.IsNullOrEmpty(idapp))
      {
        try
        {
          Tahun = int.Parse(idapp);//idapp adalah tahun
        }
        catch (Exception ex)
        {
          UtilityBO.Log(this, ex);
        }
      }
      Kdtahun = Tahun;
      Idapp = GlobalAsp.GetSessionApp();// idapp;
      SsappLookupControl.FindAndSetValuesIntoByIdapp(this);
      string title = Nmapp;
      if (MasterAppConstants.Instance.StatusTesting)
      {
        title = Urapp;
      }
      if (Tahun == 0)
      {
        return title;
      }
      else
      {
        return title + " Tahun " + Tahun;
      }
    }
  }
  #endregion Ss01appmenuTahun
}

