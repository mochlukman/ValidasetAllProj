using CoreNET.Common.Base;
using System;
using System.Collections.Generic;


namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.SysgetkeysLookupControl, CoreNET.Common.BO
  [Serializable]
  public class SysgetkeysLookupControl : SysgetkeysControl, IDataControl
  {
    #region Singleton
    private static List<SysgetkeysControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<SysgetkeysControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        SysgetkeysLookupControl dc = new SysgetkeysLookupControl();
        _ListData = (List<SysgetkeysControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static SysgetkeysControl FindAndSetValuesInto(string typename, IDataControl dc)
    {
      SysgetkeysControl founddc = null;
      List<SysgetkeysControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (SysgetkeysControl)_ListData.Find(o => o.Typename.Equals(typename));
        if (founddc != null)
        {
          dc.SetValue("Typename", founddc.Typename);
          dc.SetValue("Cols", founddc.Cols);
          dc.SetValue("Csvcols", founddc.Csvcols);
          dc.SetValue("Loadcsvcols", founddc.Loadcsvcols);
          dc.SetValue("Loadconfig", founddc.Loadconfig);

        }
        else
        {
          //Insert Default dipindahkan ke BaseDataControlExt
        }
      }
      return founddc;
    }
    public static SysgetkeysControl FindByTypename(IDataControl dc)
    {
      if (MasterAppConstants.Instance.StatusTesting)
      {
        SetListDataNull();
      }

      string typename = UtilityBO.GetClassLibName(dc);
      SysgetkeysControl ctrl = new SysgetkeysControl()
      {
        Typename = typename
      };
      ctrl = FindAndSetValuesInto(typename, ctrl);
      if ((ctrl == null) || string.IsNullOrEmpty(ctrl.Cols))
      {
        typename = UtilityBO.GetBaseClassLibName(dc);
        while (((ctrl == null) || string.IsNullOrEmpty(ctrl.Cols)) && (!typename.Contains("Base")))
        {
          ctrl = new SysgetkeysControl()
          {
            Typename = typename
          };
          ctrl = FindAndSetValuesInto(typename, ctrl);
          dc = UtilityBO.Create(typename);
          typename = UtilityBO.GetBaseClassLibName(dc);
        }
      }
      return ctrl;
    }
    public static string[] GetCols(IDataControl dc)
    {
      SysgetkeysControl ctrl = FindByTypename(dc);
      if (ctrl != null)
      {
        string key = ctrl.Cols;
        string[] keys = key.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        return keys;
      }
      else
      {
        return new string[] { };
      }
    }
    public static string[] GetFields(IDataControl dc)
    {
      SysgetkeysControl ctrl = FindByTypename(dc);
      if (ctrl != null)
      {
        string key = ctrl.Cols;
        string[] keys = key.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        string[] fields = new string[keys.Length];
        for (int i = 0; i < keys.Length; i++)
        {
          string[] strs = keys[i].Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
          fields[i] = strs[0];
        }
        return keys;
      }
      else
      {
        return new string[] { };
      }
    }
    public static string[] GetCSVCols(IDataControl dc)
    {
      SysgetkeysControl ctrl = FindByTypename(dc);
      if (ctrl != null)
      {
        string key = ctrl.Csvcols;
        string[] keys = key.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        return keys;
      }
      else
      {
        return new string[] { };
      }
    }
    public static string[] GetLoadCSVCols(IDataControl dc)
    {
      SysgetkeysControl ctrl = FindByTypename(dc);
      if (ctrl != null)
      {
        string key = ctrl.Loadcsvcols;
        string[] keys = key.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        return keys;
      }
      else
      {
        return new string[] { };
      }
    }
    public static string GetLoadConfig(IDataControl dc)
    {
      SysgetkeysControl ctrl = FindByTypename(dc);
      if (ctrl != null)
      {
        string key = ctrl.Loadconfig;
        return key;
      }
      else
      {
        return string.Empty;
      }
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Typename" };
    }
    #endregion
    public SysgetkeysLookupControl()
    {
      XMLName = "Sysgetkeys";
      ModeDB = SQLDataSource.MODE_DB_CONFIG;
    }
  }
  #endregion SysgetkeysLookup
}

