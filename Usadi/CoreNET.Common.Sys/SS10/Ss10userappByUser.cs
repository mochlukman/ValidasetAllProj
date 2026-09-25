using Ext.Net;
using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss10userappByUserControl, CoreNET.Common.BO
  [Serializable]
  public class Ss10userappByUserControl : Ss10userappControl, IDataControlUIEntry, IHasJSScript
  {
    public Ss10userappByUserControl()
    {
      XMLName = ConstantTablesSys.XMLSS10USERAPP;
    }

    private ViewListProperties cViewListProperties = null;
    public new IProperties GetProperties()
    {
      if (cViewListProperties == null)
      {
        cViewListProperties = (ViewListProperties)base.GetProperties();
        cViewListProperties.IDProperty = "Kdapp";
        cViewListProperties.ReadOnlyFields = new String[] { "Userid" };
        cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_DEL;
        cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_TREE;
        cViewListProperties.LookupDC = "CoreNET.Common.BO.Ss00appLookupControl, CoreNET.Common.Sys";
        cViewListProperties.LookupLabelQuery = BaseDataControl.LOOKUP + "ByUserid";
      }
      return cViewListProperties;
    }
    public new HashTableofParameterRow GetFilters()
    {
      bool enableFilter = string.IsNullOrEmpty(GlobalAsp.GetRequestIdPrev());
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(Ss10userLookupControl.Instance.GetLookupParameterRow(this, false).SetEnable(enableFilter));
      return hpars;
    }
    public new void SetFilterKey(BaseBO bo)
    {
      if (typeof(Ss10userControl).IsInstanceOfType(bo))
      {
        Userid = ((Ss10userControl)bo).Userid;
        Usernama = ((Ss10userControl)bo).Usernama;
      }
      else if (typeof(Ss00appControl).IsInstanceOfType(bo))
      {
        Idapp = ((Ss00appControl)bo).Idapp;
      }

    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.USER);
      return list;
    }
    public new DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection
      {
        Fields.Create(ConstantDict.GetColumnTitle("Kdapp"), typeof(string), 15, HorizontalAlign.Left),
        Fields.Create(ConstantDict.GetColumnTitle("Nmapp"), typeof(string), 45, HorizontalAlign.Left),
      };
      if (ModePreviewIndex == 1)
      {
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Kdlevel"), typeof(int), 7, HorizontalAlign.Center).SetEditable(false));
        columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Type"), typeof(string), 7, HorizontalAlign.Center).SetEditable(false));
      }
      return columns;
    }
  }
  #endregion Ss10userappByUser
}

