using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;


namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgetfiltersControl, CoreNET.Common.BO
  [Serializable]
  public class SysgetfiltersControl : BaseDataControlExt, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Properties 
    public string Rowavaillist { get; set; }
    public bool Roweditable { get; set; }
    public bool Rowenable { get; set; }
    public string Rowname { get; set; }
    public string Rownameprev { get; set; }
    public string Rowtype { get; set; }
    public short Rowwidth { get; set; }
    public string Typename { get; set; }
    public string Idmenu { get; set; }
    public string Kdmenu { get; set; }
    public string Nmmenu { get; set; }
    #endregion Properties 
    #region Methods 
    public SysgetfiltersControl()
    {
      XMLName = "Sysgetfilters";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Typename", "Rowname" };
      cViewListProperties.ReadOnlyFields = new String[] { "Typename" };
      cViewListProperties.SortFields = new string[] { "Nourut" };
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
      List<SysgetfiltersControl> ListData = new List<SysgetfiltersControl>();
      foreach(SysgetfiltersControl dc in list)
      {
        ListData.Add(dc);
      }
      return ListData;
    }
    #endregion Methods 
  }
  #endregion Sysgetfilters
}

