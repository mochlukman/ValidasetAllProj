using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgetcolsControl, CoreNET.Common.BO
  [Serializable]
  public class SysgetcolsControl : BaseDataControlExt, IDataControlUIEntry, IExtLoadCsv, IHasJSScript
  {
    #region Properties 
    public string Colalign { get; set; }
    public string Colname { get; set; }
    public string Colnameprev { get; set; }
    public string Coltype { get; set; }
    public short Colwidth { get; set; }
    public bool Coleditable { get; set; }
    public string Colavaillist { get; set; }
    public int Mode { get; set; }
    public int MaxMode { get; set; }
    public string Typename { get; set; }
    public string Idmenu { get; set; }
    public string Kdmenu { get; set; }
    public string Nmmenu { get; set; }
    #endregion Properties
    #region Methods 
    public SysgetcolsControl()
    {
      XMLName = "Sysgetcols";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = ViewListProperties.CreateDefaultProperties();
      cViewListProperties.TitleList = ConstantDict.Translate(XMLName);
      cViewListProperties.PrimaryKeys = new String[] { "Typename", "Mode", "Colname" };
      cViewListProperties.ReadOnlyFields = new String[] { "Typename", "Mode" };
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
      List<SysgetcolsControl> ListData = new List<SysgetcolsControl>();
      foreach (SysgetcolsControl dc in list)
      {
        ListData.Add(dc);
      }
      return ListData;
    }
    #endregion Methods 

  }
  #endregion Sysgetcols
}

