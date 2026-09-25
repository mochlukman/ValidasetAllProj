using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using Ext.Net;
using Ext.Net.Utilities;
using CoreNET.Common.Base;

namespace CoreNET.Common.Base
{
  public class ExtGridFilter : FormPanel
  {

    #region Property Target
    private int _Target;
    public int Target
    {
      get { return _Target; }
      set { _Target = value; }
    }
    #endregion

    #region Property RowCount
    private int _RowCount;
    public int RowCount
    {
      get { return _RowCount; }
      set { _RowCount = value; }
    }
    #endregion

    HashTableofParameterRow hps = null;
    public ExtGridFilter(int id, int target)
      : this(id, UtilityUI.GetDataControl(id), target)
    {
    }
    public ExtGridFilter(int id, IDataControlUI dc, int target)
    {
      Target = target;
      MonitorResize = true;

      hps = dc.GetFilters();

      if (dc != null)
      {
        string name = dc.GetType().Name;
        if (name.Contains("Lookup"))
        {
          //Ini kenapa ya?
          //hps.SetEnable(false);
        }
      }

      RowCount = hps.Count;
      this.Height = new Unit(RowCount * 25 + 40, UnitType.Pixel);
      //this.AutoHeight = true;
      AutoDoLayout = true;
      if (hps.Count > 0)
      {
        this.ID = "FormFilter1";
        this.Padding = 5;
        this.Region = Region.North;
        this.Collapsible = true;
        string[] keys = hps.GetOrderKeys();
        foreach (string key in keys)
        {
          this.Add(new CoreNETCompositeField((ParameterRow)hps[key], GlobalAsp.FILTER, target));
        }
      }
      //Nilainya ngga keset
      //bool hasPrev = !string.IsNullOrEmpty(GlobalExt.GetRequestIdPrev());
      //this.Collapsed = hasPrev;
    }
    //public ExtGridFilter(int id, IDataControlUI dc, int target)
    //{
    //  //AutoScroll = true;
    //  hps = dc.GetFilters();
    //  RowCount = hps.Count;
    //  //this.Height = new Unit(RowCount * 25 + 20, UnitType.Pixel);
    //  this.AutoHeight = true;
    //  if (hps.Count > 0)
    //  {
    //    this.ID = "FormFilter1";
    //    this.Padding = 5;
    //    this.Region = Region.North;
    //    string[] keys = hps.GetOrderKeys();
    //    foreach (string key in keys)
    //    {
    //      this.Add(new CoreNETCompositeField((ParameterRow)hps[key], ConstantApp.FILTER, target));
    //    }
    //  }
    //}
  }
}
