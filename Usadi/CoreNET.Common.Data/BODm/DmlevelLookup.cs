using CoreNET.Common.Base;
using System;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region DmlevelLookup
  [Serializable]
  public class DmlevelLookupControl : DmlevelControl, IDataControl
  {
    #region Singleton
    private static List<DmlevelControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<DmlevelControl> GetListDataSingleton()
    {
      if ((_ListData == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        DmlevelControl dc = new DmlevelControl();
        _ListData = (List<DmlevelControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static List<DmlevelControl> GetData(string tablename)
    {
      return GetListDataSingleton().FindAll(o => o.Tablename.Equals(tablename));
    }
    public static DmlevelControl FindAndSetValuesInto(DmlevelControl dc)
    {
      DmlevelControl founddc = (DmlevelControl)GetListDataSingleton().Find(o => o.Tablename.Equals(dc.Tablename) && o.Kdlevel.Equals(dc.Kdlevel));
      if (founddc != null)
      {
        dc.SetValue("Nmlevel", founddc.Nmlevel);
        //dc.SetValue("Ketlevel", founddc.Ketlevel);
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Kdlevel", "Nmlevel" };
    }
    #endregion
    public DmlevelLookupControl()
    {
      XMLName = "Dmlevel";
    }
    public string GetFieldValueMap()
    {
      return "Idlevel=Nmlevel";
    }
  }
  #endregion DmlevelLookup
}

