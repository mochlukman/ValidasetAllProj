using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region DmbulanLookup
  [Serializable]
  public class DmbulanLookupControl :  DmbulanControl, IDataControl
  {
    #region Singleton
    private static List<DmbulanControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<DmbulanControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        DmbulanControl dc = new DmbulanControl();
        dc.SetPageKey();
        _ListData = (List<DmbulanControl>)dc.View();
      }
      return _ListData;
    }
    public static void FindAndSetValuesInto(IDataControl dc)
    {
      DmbulanControl founddc = (DmbulanControl)GetListDataSingleton().Find(o => o.Kdbulan.Equals(dc.GetValue("Kdbulan")));
      //DmbulanControl founddc = new DmbulanControl();
      //founddc.Kdbulan = (string)dc.GetValue("Kdbulan");
      //founddc = (DmbulanControl)founddc.Load(BaseDataControl.PK);
      if(founddc != null)
      {
        dc.SetValue("Kdbulan", founddc.Kdbulan);
        dc.SetValue("Nmbulan", founddc.Nmbulan);
      }
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Kdbulan","Nmbulan"};
    }
    #endregion
    public DmbulanLookupControl()
    {
      XMLName = "Dmbulan";
    }
    public string GetFieldValueMap()
    {
      return "Bulan=Nmbulan";
    }
  }
  #endregion DmbulanLookup
}

