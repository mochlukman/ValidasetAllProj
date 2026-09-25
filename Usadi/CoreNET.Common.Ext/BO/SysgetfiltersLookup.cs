using System;
using System.Collections;
using System.Collections.Generic;
using CoreNET.Common.Base;


namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgetfiltersLookupControl, CoreNET.Common.BO
  [Serializable]
  public class SysgetfiltersLookupControl :  SysgetfiltersControl, IDataControl
  {
    #region Singleton
    private static List<SysgetfiltersControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<SysgetfiltersControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        SysgetfiltersLookupControl dc = new SysgetfiltersLookupControl();
        _ListData = (List<SysgetfiltersControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static SysgetfiltersControl FindAndSetValuesInto(IDataControl dc)
    {
      SysgetfiltersControl founddc = null;
      List<SysgetfiltersControl> _ListData = GetListDataSingleton();
      if(_ListData != null)
      {
        founddc = (SysgetfiltersControl)_ListData.Find(o=>o.Typename.Equals(dc.GetValue("Typename")) && o.Rowname.Equals(dc.GetValue("Rowname")));
        if(founddc != null)
        {
          dc.SetValue("Typename", founddc.Typename);
          dc.SetValue("Rowname", founddc.Rowname);
        }
      }
      return founddc;
    }
    public static List<SysgetfiltersControl> GetFilters(IDataControl dc)
    {
      if (MasterAppConstants.Instance.StatusTesting)
      {
        SetListDataNull();
      }
      //SsappmenuControl dcmenu = new SsappmenuControl() { Idmenu = idmenu };
      //dcmenu = SsappmenuLookupControl.FindAndSetValuesInto(dcmenu);

      //string typename = (i == 1) ? dcmenu.Olist1 : dcmenu.Olistdetil1;
      string typename = UtilityBO.GetClassLibName(dc);
      List<SysgetfiltersControl> _ListData = GetListDataSingleton();
      _ListData = (List<SysgetfiltersControl>)_ListData.FindAll(o => o.Typename.Equals(typename));
      _ListData.Sort(delegate (SysgetfiltersControl x, SysgetfiltersControl y)
      {
        return x.Nourut.CompareTo(y.Nourut);
      });
      return _ListData;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[]{ "Typename","Rowname"};
    }
    #endregion
    public SysgetfiltersLookupControl()
    {
      XMLName = "Sysgetfilters";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
    }
  }
  #endregion SysgetfiltersLookup
}

