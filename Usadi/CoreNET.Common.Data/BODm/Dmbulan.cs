using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region Dmbulan
  [Serializable]
  public class DmbulanControl : BaseDataControl, IDataControl
  {
    #region Properties 
    public string Kdbulan { get; set; }
    public string Nmbulan { get; set; }
    #endregion Properties 

    #region Methods 
    public DmbulanControl()
    {
      XMLName = "Dmbulan";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
      ConnectionString = SQLDataSource.Instance.CS_Config;
    }
    public void SetFilterKey(BaseBO bo)
    {
      if (typeof(IDataControlAppuser).IsInstanceOfType(bo))
      {
        Last_by = ((IDataControlAppuser)bo).GetUserID();
      }
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<DmbulanControl> ListData = new List<DmbulanControl>();
      foreach(DmbulanControl dc in list)
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
  #endregion Dmbulan
}

