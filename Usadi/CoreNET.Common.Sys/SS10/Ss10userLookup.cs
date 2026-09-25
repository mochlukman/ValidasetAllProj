using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;
using CoreNET.Common.BO;

namespace CoreNET.Common.BO
{
  #region CoreNET.Common.BO.Ss10userLookupControl, CoreNET.Common.BO
  [Serializable]
  public class Ss10userLookupControl : Ss10userControl, IDataControlLookup, IHasJSScript
  {
    #region Singleton
    private static Ss10userLookupControl _Instance = null;
    public static Ss10userLookupControl Instance
    {
      get
      {
        if (_Instance == null)
        {
          _Instance = new Ss10userLookupControl();
        }
        return _Instance;
      }
    }
    private static List<Ss10userControl> _ListData = null;
    public static void SetListDataNull()
    {
      _ListData = null;
    }
    public static List<Ss10userControl> GetListDataSingleton()
    {
      if (_ListData == null)
      {
        Ss10userLookupControl dc = new Ss10userLookupControl();
        dc.SetPageKey();
        _ListData = (List<Ss10userControl>)dc.View(BaseDataControl.LOOKUP);
      }
      return _ListData;
    }
    public static Ss10userControl FindAndSetValuesInto(IDataControlUI dc)
    {
      Ss10userControl founddc = null;
      List<Ss10userControl> _ListData = GetListDataSingleton();
      if (_ListData != null)
      {
        founddc = (Ss10userControl)_ListData.Find(o => o.Userid.Equals(dc.GetValue("Userid")));
        if (founddc != null)
        {
          if (typeof(Ss10userControl).IsInstanceOfType(dc))
          {
            founddc.CopyPropertyBOTo(dc);
          }
          else
          {
            dc.SetValue("Userid", founddc.Userid);
          }
        }
      }
      return founddc;
    }
    public static string[] GetFieldValueProps()
    {
      return new string[] { "Userid" };
    }
    #endregion
    public Ss10userLookupControl()
    {
      XMLName = ConstantTablesSys.XMLSS10USER;
    }
    public new IProperties GetProperties()
    {
      ViewListProperties cViewListProperties = (ViewListProperties)base.GetProperties();
      return cViewListProperties;
    }
    public override DataControlFieldCollection GetColumns()
    {
      DataControlFieldCollection columns = new DataControlFieldCollection();
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Userid"), typeof(string), 30, HorizontalAlign.Left).SetEditable(false));
      columns.Add(Fields.Create(ConstantDict.GetColumnTitle("Usernama"), typeof(string), 50, HorizontalAlign.Left).SetEditable(false));
      return columns;
    }
    public ParameterRow GetLookupParameterRow(IDataControl callerCtr, bool entry)
    {
      Ss10userLookupControl dclookup = new Ss10userLookupControl();
      string title = ConstantDict.Translate(dclookup.XMLName);
      string[] keys = new String[] { "Userid", "Usernama" };
      string[] targets = new String[] { "Userid", "Usernama" };
      ParameterRowLookup2 par = new ParameterRowLookup2(callerCtr, keys, new int[] { 20, 75, 0 }, targets)
      {
        Label = title,
        VisibleControls = new bool[] { true, true, !entry },
        AllowRefresh = !entry,
        DCLookup = dclookup,
        IsTree = false,
        SelectionType = "D"
      };
      return par;
    }
    public string GetFieldValueMap()
    {
      return "Userid=Userid";
    }
  }
  #endregion Ss10userLookup
}

