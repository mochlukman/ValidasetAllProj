using System;
using System.Collections;
using System.Text;
using Ext.Net;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
  /// <summary>
  /// 
  /// </summary>
  public class ExtFields : Fields
  {
    public static MyBoundField GetRatingField()
    {
      return Create(ConstantDictExt.GetColumnTitle("Rating"), typeof(Ext.Net.RatingColumn));
    }
    public static MyBoundField GetStatusField()
    {
      return Fields.Create(ConstantDict.GetColumnTitle("Statusicon"), typeof(Icon), 5, HorizontalAlign.Center);
    }

  }
}
