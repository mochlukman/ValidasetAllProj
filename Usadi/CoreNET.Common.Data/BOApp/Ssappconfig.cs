using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SsappconfigControl, CoreNET.Common.BO
  [Serializable]
  public class SsappconfigControl : BaseDataControl, IDataControl
  {
    #region Properties 
    public new string Idapp { get; set; }
    public string Par { get; set; }
    public string Value { get; set; }
    #endregion Properties 

    #region Methods 
    public SsappconfigControl()
    {
      XMLName = "Ssappconfig";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
      ConnectionString = SQLDataSource.Instance.CS_Config;
    }
    public new IList View()
    {
      IList list = this.View(BaseDataControl.ALL);
      return list;
    }
    public new IList View(string label)
    {
      IList list = ((BaseDataControl)this).View(label);
      List<SsappconfigControl> ListData = new List<SsappconfigControl>();
      foreach(SsappconfigControl dc in list)
      {
        ListData.Add(dc);
      }
      return ListData;
    }
    public new string[] GetKeys()
    {
      return new string[] { "Idapp", "Par", "Value", "Kdlevel", "Type" };
    }
    #endregion Methods 
  }
  #endregion Ssappconfig
}

