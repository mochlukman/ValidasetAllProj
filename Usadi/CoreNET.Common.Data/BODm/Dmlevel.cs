using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Dmlevel
  [Serializable]
  public class DmlevelControl : BaseDataControl, IDataControl
  {
    #region Properties 
    public bool Isroot { get; set; }
    public string Nmlevel { get; set; }
    public string Tablename { get; set; }
    #endregion Properties 
    #region Methods 
    public DmlevelControl()
    {
      XMLName = "Dmlevel";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
      ConnectionString = SQLDataSource.Instance.CS_Config;
    }
    public DmlevelControl(BaseBO bo)
    {
      XMLName = "Dmlevel";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
      Tablename = bo.XMLName.ToLower();
    }
    public new IList View()
    {
      List<DmlevelControl> list = DmlevelLookupControl.GetListDataSingleton();
      if (!string.IsNullOrEmpty(Tablename))
      {
        list = list.FindAll(o => Tablename.ToLower().Contains(o.Tablename.ToLower()));
      }
      else
      {
        list = list.FindAll(o => o.Tablename.Equals(string.Empty));
      }
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<DmlevelControl> ListData = new List<DmlevelControl>();
      foreach (DmlevelControl dc in list)
      {
        ListData.Add(dc);
      }
      return ListData;
    }
    #endregion Methods 
    #region IDataControlUIEntry
    public void SetEditable(bool editable)
    {
      Editable = editable;
    }
    public void AfterInsert()
    {
    }

    public void AfterUpdate()
    {
    }

    public void AfterDelete()
    {
    }
    public void FilterClick(string key)
    {
    }
    public string GetDestinationFile(string filename)
    {
      return Path;
    }

    public string GetURLFile(string filename, string versi)
    {
      return Path;
    }


    public Exception ValidateTotal()
    {
      return null;
    }

    #endregion Methods 
  }
  #endregion Dmlevel
}

