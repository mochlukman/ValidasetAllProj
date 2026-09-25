using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;


namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgetcolsLookupControl, CoreNET.Common.BO
  [Serializable]
  public class SysgetcolsLookupControl :  SysgetcolsControl, IDataControl
  {
    #region Singleton
    private static List<SysgetcolsControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<SysgetcolsControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        SysgetcolsLookupControl dc = new SysgetcolsLookupControl();
        _ListData = (List<SysgetcolsControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static SysgetcolsControl FindAndSetValuesInto(IDataControl dc)
    {
      SysgetcolsControl founddc = null;
      List<SysgetcolsControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (SysgetcolsControl)_ListData.Find(o=>o.Typename.Equals(dc.GetValue("Typename")) && o.Colname.Equals(dc.GetValue("Colname")));
        if(founddc != null)
        {
          dc.SetValue("Typename", founddc.Typename);
          dc.SetValue("Colname", founddc.Colname);
        }
      }
      return founddc;
    }
    public static List<SysgetcolsControl> GetColumns(IDataControl dc, int mode)
    {
      //Pindahin ketika crud
      //if (MasterAppConstants.Instance.StatusTesting)
      //{
      //  SetListDataNull();
      //}
      //SsappmenuControl dcmenu = new SsappmenuControl() { Idmenu = idmenu };
      //dcmenu = SsappmenuLookupControl.FindAndSetValuesInto(dcmenu);

      //string typename = (i == 1) ? dcmenu.Olist1 : dcmenu.Olistdetil1;

      List<SysgetcolsControl> _ListData = GetListDataSingleton();
      string typename = UtilityBO.GetClassLibName(dc);
      _ListData = (List<SysgetcolsControl>)GetListDataSingleton().FindAll(o => o.Typename.Equals(typename) && (o.Mode == mode));
      if (_ListData.Count == 0)
      {
        typename = UtilityBO.GetBaseClassLibName(dc);
        while ((_ListData.Count == 0) && (!typename.Contains("Base")))
        {
          _ListData = (List<SysgetcolsControl>)GetListDataSingleton().FindAll(o => o.Typename.Equals(typename) && (o.Mode == mode));
          dc = UtilityBO.Create(typename);
          typename = UtilityBO.GetBaseClassLibName(dc);
        }
      }
      _ListData.Sort(delegate (SysgetcolsControl x, SysgetcolsControl y)
      {
        return x.Nourut.CompareTo(y.Nourut);
      });
      return _ListData;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Typename","Colname"};
    }
    #endregion
    public SysgetcolsLookupControl()
    {
      XMLName = "Sysgetcols";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
    }
  }
  #endregion SysgetcolsLookup
}

