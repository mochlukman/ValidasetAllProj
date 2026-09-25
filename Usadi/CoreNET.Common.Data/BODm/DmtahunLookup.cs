using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;

namespace CoreNET.Common.BO
{
  #region DmtahunLookup
  [Serializable]
  public class DmtahunLookupControl :  DmtahunControl, IDataControl
  {
    #region Singleton
    private static List<DmtahunControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<DmtahunControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        DmtahunControl dc = new DmtahunControl();
        dc.SetPageKey();
        _ListData = (List<DmtahunControl>)dc.View();
      }
      return _ListData;
    }
    public static void FindAndSetValuesInto(IDataControl dc)
    {
      DmtahunControl founddc = (DmtahunControl)GetListDataSingleton().Find(o => o.Kdtahun.Equals(dc.GetValue("Kdtahun")));
      //DmtahunControl founddc = new DmtahunControl();// (DmtahunControl)GetListDataSingleton().Find(o => o.Kdtahun.Equals(dc.GetValue("Kdtahun")));
      //founddc.Kdtahun = (int)dc.GetValue("Tahun");
      //founddc = (DmtahunControl)founddc.Load(BaseDataControl.PK);
      if(founddc != null)
      {
        dc.SetValue("Kdtahun", founddc.Kdtahun);
        dc.SetValue("Nmtahun", founddc.Nmtahun);
      }
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Kdtahun","Nmtahun"};
    }
    #endregion
    public DmtahunLookupControl()
    {
      XMLName = "Dmtahun";
    }
  }
  #endregion DmtahunLookup
}

