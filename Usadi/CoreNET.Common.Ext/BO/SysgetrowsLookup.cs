using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;


namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgetrowsLookupControl, CoreNET.Common.BO
  [Serializable]
  public class SysgetrowsLookupControl : SysgetrowsControl, IDataControl
  {
    #region Singleton
    private static List<SysgetrowsControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<SysgetrowsControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        SysgetrowsLookupControl dc = new SysgetrowsLookupControl();
        _ListData = (List<SysgetrowsControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static SysgetrowsControl FindAndSetValuesInto(IDataControl dc)
    {
      SysgetrowsControl founddc = null;
      List<SysgetrowsControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (SysgetrowsControl)_ListData.Find(o => o.Typename.Equals(dc.GetValue("Typename")) && o.Rowname.Equals(dc.GetValue("Rowname")));
        if (founddc != null)
        {
          dc.SetValue("Typename", founddc.Typename);
          dc.SetValue("Rowname", founddc.Rowname);
        }
      }
      return founddc;
    }
    public static List<SysgetrowsControl> GetRows(IDataControl dc)
    {
      if (MasterAppConstants.Instance.StatusTesting)
      {
        SetListDataNull();
      }
      //SsappmenuControl dcmenu = new SsappmenuControl() { Idmenu = idmenu };
      //dcmenu = SsappmenuLookupControl.FindAndSetValuesInto(dcmenu);

      //string typename = (i == 1) ? dcmenu.Olist1 : dcmenu.Olistdetil1;
      List<SysgetrowsControl> _ListData = GetListDataSingleton();
      string typename = UtilityBO.GetClassLibName(dc);
      _ListData = (List<SysgetrowsControl>)GetListDataSingleton().FindAll(o => o.Typename.Equals(typename));
      if (_ListData.Count == 0)
      {
        typename = UtilityBO.GetBaseClassLibName(dc);
        while ((_ListData.Count == 0) && (!typename.Contains("Base")))
        {
          _ListData = (List<SysgetrowsControl>)GetListDataSingleton().FindAll(o => o.Typename.Equals(typename));
          dc = UtilityBO.Create(typename);
          typename = UtilityBO.GetBaseClassLibName(dc);
        }
      }
      _ListData.Sort(delegate (SysgetrowsControl x, SysgetrowsControl y)
      {
        return x.Nourut.CompareTo(y.Nourut);
      });
      return _ListData;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Typename", "Rowname" };
    }
    #endregion
    public SysgetrowsLookupControl()
    {
      XMLName = "Sysgetrows";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
    }
  }
  #endregion SysgetrowsLookup
}

