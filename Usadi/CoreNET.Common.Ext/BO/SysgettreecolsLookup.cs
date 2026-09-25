using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;


namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgettreecolsLookupControl, CoreNET.Common.BO
  [Serializable]
  public class SysgettreecolsLookupControl : SysgettreecolsControl, IDataControl
  {
    #region Singleton
    private static List<SysgettreecolsControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<SysgettreecolsControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        SysgettreecolsLookupControl dc = new SysgettreecolsLookupControl();
        _ListData = (List<SysgettreecolsControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static SysgettreecolsControl FindAndSetValuesInto(IDataControl dc)
    {
      SysgettreecolsControl founddc = null;
      List<SysgettreecolsControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (SysgettreecolsControl)_ListData.Find(o => o.Typename.Equals(dc.GetValue("Typename")) && o.Colname.Equals(dc.GetValue("Colname")));
        if (founddc != null)
        {
          dc.SetValue("Typename", founddc.Typename);
          dc.SetValue("Colname", founddc.Colname);
        }
      }
      return founddc;
    }
    public static List<SysgettreecolsControl> GetTreeColumns(IDataControl dc, int mode)
    {
      if (MasterAppConstants.Instance.StatusTesting)
      {
        SetListDataNull();
      }
      //SsappmenuControl dcmenu = new SsappmenuControl() { Idmenu = idmenu };
      //dcmenu = SsappmenuLookupControl.FindAndSetValuesInto(dcmenu);

      //string typename = (i == 1) ? dcmenu.Olist1 : dcmenu.Olistdetil1;
      List<SysgettreecolsControl> _ListData = GetListDataSingleton();
      string typename = UtilityBO.GetClassLibName(dc);
      _ListData = (List<SysgettreecolsControl>)GetListDataSingleton().FindAll(o => o.Typename.Equals(typename) && (o.Mode == mode));
      if (_ListData.Count == 0)
      {
        typename = UtilityBO.GetBaseClassLibName(dc);
        while ((_ListData.Count == 0) && (!typename.Contains("Base")))
        {
          _ListData = (List<SysgettreecolsControl>)GetListDataSingleton().FindAll(o => o.Typename.Equals(typename) && (o.Mode == mode));
          dc = UtilityBO.Create(typename);
          typename = UtilityBO.GetBaseClassLibName(dc);
        }
      }
      _ListData.Sort(delegate (SysgettreecolsControl x, SysgettreecolsControl y)
      {
        return x.Nourut.CompareTo(y.Nourut);
      });
      return _ListData;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Typename", "Colname" };
    }
    #endregion
    public SysgettreecolsLookupControl()
    {
      XMLName = "Sysgettreecols";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
    }
  }
  #endregion SysgettreecolsLookup
}

