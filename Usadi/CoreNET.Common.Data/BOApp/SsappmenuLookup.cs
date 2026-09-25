using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region SsappmenuLookup
  [Serializable]
  public class SsappmenuLookupControl :  SsappmenuControl, IDataControl
  {
    #region Singleton
    private static List<SsappmenuControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<SsappmenuControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        SsappmenuLookupControl dc = new SsappmenuLookupControl();
        _ListData = (List<SsappmenuControl>)dc.View(BaseDataControl.ALL);
      }
      return _ListData;
    }
    public static SsappmenuControl FindAndSetValuesInto(IDataControl dc)
    {
      SsappmenuControl founddc = null;
      List<SsappmenuControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (SsappmenuControl)_ListData.Find(o=>o.Idmenu.Equals(dc.GetValue("Idmenu")));
        if(founddc != null)
        {
          dc.SetValue("Idmenu", founddc.Idmenu);
          dc.SetValue("Kdmenu", founddc.Kdmenu);
          dc.SetValue("Nmmenu", founddc.Nmmenu);
          if (dc.GetProperty("Olist1") != null)
          {
            dc.SetValue("Olist1", founddc.Olist1);
            dc.SetValue("Olistdetil1", founddc.Olistdetil1);
          }
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Idapp","Kdmenu","Nmmenu"};
    }
    #endregion
    public SsappmenuLookupControl()
    {
      XMLName = "Ssappmenu";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
    }
  }
  #endregion SsappmenuLookup
}

