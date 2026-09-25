using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;


namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgetkeysControl, CoreNET.Common.BO
  [Serializable]
  public class SysgetkeysControl : BaseDataControlExt, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Properties 
    public string Csvcols { get; set; }
    public string Cols { get; set; }
    public string Loadcsvcols { get; set; }
    public string Loadconfig { get; set; }
    public string Typename { get; set; }
    public string Idmenu { get; set; }
    public string Kdmenu { get; set; }
    public string Nmmenu { get; set; }
    #endregion Properties 
    #region Methods 
    public SysgetkeysControl()
    {
      XMLName = "Sysgetkeys";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = ViewListProperties.CreateDefaultProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Typename" };
      cViewListProperties.ReadOnlyFields = new String[] { "Typename" };
      cViewListProperties.EntryStyle = ViewListProperties.ENTRY_STYLE_FORM;
      cViewListProperties.ModeEditable = ViewListProperties.MODE_EDITABLE_ADD_EDIT_DEL_LOADCSV;
      return cViewListProperties;
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<SysgetkeysControl> ListData = new List<SysgetkeysControl>();
      foreach(SysgetkeysControl dc in list)
      {
        ListData.Add(dc);
      }
      return ListData;
    }
    #endregion Methods 
  }
  #endregion Sysgetkeys
}

