using CoreNET.Common.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoreNET.Common.BO
{
  #region DmstatusLookup
  [Serializable]
  public class DmstatusLookupControl : DmstatusControl, IDataControl
  {
    #region Singleton
    private static DmstatusLookupControl _Instance = null;
    public static DmstatusLookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new DmstatusLookupControl();
        }
        return _Instance;
      }
    }

    private static List<DmstatusControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<DmstatusControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        DmstatusControl dc = new DmstatusControl();
        _ListData = (List<DmstatusControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }

    private static Hashtable HStatus = new Hashtable();
    public static List<DmstatusControl> GetListDataSingleton(string label)
    {
      DmstatusControl dc = new DmstatusControl();
      if ((HStatus[label] == null) || (MasterAppConstants.Instance.StatusTesting))
      {
        List<DmstatusControl> list = (List<DmstatusControl>)dc.View(label);
        HStatus[label] = list;
      }
      return (List<DmstatusControl>)HStatus[label];
    }
    public static DmstatusControl FindAndSetValuesInto(IDataControl dc)
    {
      DmstatusControl founddc = (DmstatusControl)GetListDataSingleton().Find(o => o.Idstatus.Equals(dc.GetValue("Status")));
      if (founddc != null)
      {
        dc.SetValue("Statusname", founddc.Nmstatus);
        dc.SetValue("Statusicon", founddc.Statusicon);
        dc.SetValue("Stricon", founddc.Statusicon + "=" + founddc.Ketstatus);
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Idstatus", "Nmstatus" };
    }
    #endregion
    public DmstatusLookupControl()
    {
      XMLName = "Dmstatus";
    }
    public string GetFieldValueMap()
    {
      return "Idstatus=Nmstatus";
    }
    //public static DmstatusControl FindAndSetValuesInto(BaseBO dc)
    //{
    //  DmstatusControl founddc = null;
    //  List<DmstatusControl> _ListData = GetListDataSingleton();
    //  if (_ListData != null)
    //  {
    //    founddc = (DmstatusLookupControl)_ListData.Find(o => o.Idstatus.Equals(dc.Status));
    //    if (founddc != null)
    //    {
    //      dc.Statusname = founddc.Nmstatus;
    //      dc.Statusicon = founddc.Statusicon;
    //    }
    //  }
    //  return founddc;
    //}
  }
  #endregion DmstatusLookup
}

