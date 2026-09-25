using System;
using Ext.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreNET.Common.Base
{
  /// <summary>
  /// 
  /// </summary>
  public interface IExtDataControl : IDataControlUIEntry
  {
    Icon GetIcon();
    string GetIconText();
    void SetTotalFields(Toolbar tb);
    void SetTotal(Control seed);
  }
}
