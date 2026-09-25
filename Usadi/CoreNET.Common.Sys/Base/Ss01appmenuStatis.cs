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
  #region Ss01appmenuStatis
  [Serializable]
  public class Ss01appmenuStatisControl : Ss01appmenuControl, IDataControlMenu, IDataControlMenuMapClass, IDataControlTreeGrid3, IHasJSScript, ICsv
  {
    public Ss01appmenuStatisControl()
    {
      XMLName = ConstantTablesSys.XMLSS01APPMENU;
      SetMaxModePreviewIndex(1);
      Kdtahun = DateTime.Now.Year;
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
      return View(BaseDataControl.ALL);
    }
    public new IList View(string label)
    {
      string dataurl = GlobalAsp.GetDataURL();//http://localhost/SmartDocs/
      IList list = ((BaseDataControl)this).View(label);
      List<Ss01appmenuControl> ListData = new List<Ss01appmenuControl>();
      foreach (Ss01appmenuControl dc in list)
      {
        dc.UrlFull = dc.Url;
        DmstatusLookupControl.FindAndSetValuesInto(dc);
        ListData.Add(dc);
      }
      return ListData;
    }
    public new HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }
  }
  #endregion Ss01appmenuStatis
}

