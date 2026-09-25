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
  public interface IExtFormEntry
  {
    Ext.Net.Component GetCenter();
    Ext.Net.Component GetNorth();
    Ext.Net.Component GetSouth();
    Ext.Net.Component GetEast();
    Ext.Net.Component GetWest();

    void BindData();
    void SaveData();
    void EditForm(IDataControlUI dc);
  }
}
