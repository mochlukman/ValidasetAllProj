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
  #region Dmstatus
  [Serializable]
  public class DmstatusUIControl : DmstatusControl, IDataControlUIEntry, IHasJSScript
  {
    public DataControlFieldCollection columns = null;
    public void SetColumnsNull()
    {
      columns = null;
    }
    #region Methods 
    public DmstatusUIControl()
    {
      XMLName = ConstantTablesSys.XMLDMSTATUS;
    }
    public string GetDefaultDebug()
    {
      return BaseDataControlUI.GetStaticDefaultDebug(this);
    }

    public IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = ViewListProperties.CreateDefaultProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Idstatus" };
      cViewListProperties.ReadOnlyFields = new String[] { };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL;
      return cViewListProperties;
    }
    public bool IsEditable()
    {
      return Editable;
    }
    public DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection();
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Idstatus"), typeof(string), 30, HorizontalAlign.Left));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Nmstatus"), typeof(string), 50, HorizontalAlign.Left));
      return columns;
    }
    public DataControlFieldCollection GetColumnsFilter()
    {
      return GetColumns();
    }
    public HashTableofParameterRow GetEntries()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitleEntry("Status"), true, 90));
      hpars.Add(new ParameterRowTextBox(this, ConstantDict.GetColumnTitle("Nmstatus"), true, 90));
      return hpars;
    }
    public HashTableofParameterRow GetFilters()
    {
      HashTableofParameterRow hpars = new HashTableofParameterRow();
      return hpars;
    }

    public new IList View()
    {
      IList list = ((BaseDataControl)this).View(BaseDataControl.LOOKUP);
      return list;
    }

    #endregion Methods 
  }
  #endregion Dmstatus
}

