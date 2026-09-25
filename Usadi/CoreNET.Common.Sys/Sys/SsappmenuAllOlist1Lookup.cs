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
  #region CoreNET.Common.BO.SsappmenuallLookupControl, CoreNET.Common.BO
  [Serializable]
  public class SsappmenuAllOlist1LookupControl : SsappmenuAllOlist1Control, IDataControlLookup, IHasJSScript
  {
    public SsappmenuAllOlist1LookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS00APPMENU;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.CanModeTree = false;
      return cViewListProperties;
    }
    public new IList View()
    {
      IList list = base.View(BaseDataControl.UNION+"Olist1");
      return list;
    }

    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      SsappmenuAllOlist1LookupControl dclookup = new SsappmenuAllOlist1LookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys = new String[] { "Typename" };
      string[] targets = new String[] { "Typename=Olist1" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 70, 0, 0 }, targets)
      {
        Label = title,
        VisibleControls = new bool[] { true, true, !entry },
        AllowRefresh = !entry,
        DCLookup = dclookup,
        IsTree = false,
        SelectionType = "D"
      };
      return par;
    }
    public string GetFieldValueMap()
    {
      return "Typename=Typename";
    }
  }
  #endregion SsappmenuLookup
}

